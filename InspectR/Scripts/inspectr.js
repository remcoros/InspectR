/*jslint browser: true*/
/*global jQuery */
(function ($) {
    "use strict";

    // Bootstrapper
    var InspectR = {
        // Static start function, called from main html/view to start all things up.
        start: function (config) {
            config = $.extend({}, InspectR.defaults, config);

            // main router, taking care of things
            var router = InspectR.Router = new Router(config),
                viewModel = InspectR.ViewModel = new InspectRViewModel(router, config);

            ko.applyBindings(viewModel, config.rootNode);

            // start the signalr connection
            // when the connection is successful, start the router
            // $.connection.hub.logging = true;
            $.connection.hub.start({
                waitForPageLoad: false,
                transport: 'longPolling'
            }, function () {
                router.trigger('connection:start');
                Backbone.history.start({});
                config.onStart();
            });

            // Don't show any errors because of page unload
            $(window).bind('beforeunload', function () {
                viewModel.ignoreConnectionErrors = true;
            });
        },
        defaults: {
            // inspector: ''
            // rootNode:'#inspectr',
            onStart: function () { }
        }
    };

    // Backbone Router, takes care of history
    var Router = Backbone.Router.extend({
        routes: {
            "": "start",
            "*uniquekey": "loadInspector"
        }
    });

    // Main viewmodel
    var InspectRViewModel = function (router, options) {
        var self = this,
            hub = $.connection.inspectRHub,
            client = hub.client,
            server = hub.server;

        // mixin Events
        // _.extend(this, Backbone.Events);

        // public properties
        this.NewTitle = ko.observable();
        this.IsEditingTitle = ko.observable(false);
        this.UserProfile = ko.observable();
        this.Requests = ko.mapping.fromJS([]);
        this.Inspector = ko.observable();
        this.SupportedContentTypes = _.keys(CodeMirror.mimeModes).sort();
        this.Alerts = ko.observableArray([]);

        this.RequestList = ko.computed(function () {
            return self.Requests();
        });

        /** Routes */
        router.on('connection:start', function () { self.loadUserProfile(); });

        router.on('route:start', function () {
            self.loadInspector(options.inspector);
            // router.navigate(options.inspector, { trigger: true });
        });

        this.loadInspector = function (uniquekey) {
            // self.Inspector(null); // disable this to not flicker the screen
            self.Requests([]);
            if (self.Inspector()) {
                server.stopInspect(self.Inspector().Id());
            }
            server.startInspect(uniquekey)
                .done(function (result) {
                    if (result) {
                        self.Inspector(ko.mapping.fromJS(result));
                        self.loadRecentRequests();
                    }
                });
        };
        router.on('route:loadInspector', this.loadInspector);

        /** Methods */
        this.loadRecentRequests = function () {
            server.getRecentRequests(self.Inspector().UniqueKey())
                .done(function (result) {
                    if (result && result.length > 0) {
                        ko.mapping.fromJS(result, self.Requests);
                    }
                });
        };

        this.removeInspector = function (inspector) {
            if (self.Inspector().Id() == inspector.Id()) {
                throw "Cannot remove current inspector";
            }

            if (!confirm('Remove inspector: ' + inspector.UniqueKey())) {
                return;
            }

            server.removeInspectorFromUserProfile(inspector.Id())
                .done(function () {
                    self.UserProfile().Inspectors.remove(inspector);
                });
        };

        this.saveTitle = function () {
            server.setTitle(self.Inspector().Id(), self.NewTitle())
                .done(function () {
                    self.IsEditingTitle(false);
                    self.Inspector().Title(self.NewTitle());
                    self.loadUserProfile();
                });
        };

        this.loadUserProfile = function () {
            server.getUserProfile()
                .done(function (result) {
                    if (result) {
                        self.UserProfile(ko.mapping.fromJS(result));
                    }
                });
        };

        this.clearRecentRequests = function () {
            server.clearRecentRequests(self.Inspector().UniqueKey());
            self.Requests.removeAll();
        };

        /** Hub client functions */
        client.requestLogged = function (inspector, request) {
            if (inspector.Id != self.Inspector().Id()) {
                console.log('received request for non current inspector');
                return;
            }
            self.Requests.unshift(request);
        };

        /** Private functions */
        this.showAlert = function (title, content, type) {
            var alert = new Alert(title, content, type);
            alert.closed(function () {
                self.Alerts.remove(alert);
            });
            self.Alerts.push(alert);
            return alert;
        };

        this._connectionSlow = function () {
            if (self.ignoreConnectionErrors) {
                return;
            }
            self.showAlert('', 'The connection is having some issues. Try refreshing this page.', Alert.warning);
        };

        this._disconnected = function () {
            if (self.ignoreConnectionErrors) {
                return;
            }
            self.showAlert('', 'You are disconnected. Try refreshing this page.', Alert.error);
        };

        this._connectionError = function (data) {
            if (self.ignoreConnectionErrors) {
                return;
            }
            self.ignoreConnectionErrors = true;

            self.showAlert('', 'Errors occured in the connection. Try refreshing this page.', Alert.error)
                .closed(function () {
                    self.ignoreConnectionErrors = false;
                });
        };

        // Respond to connection errors
        $.connection.hub.connectionSlow(this._connectionSlow);
        $.connection.hub.disconnected(this._disconnected);
        $.connection.hub.error(this._connectionError);
    };

    // Alert model
    var Alert = function (title, content, type) {
        var self = this;
        this.Title = ko.observable(title || '');
        this.Content = ko.observable(content || '');
        this.Type = ko.observable(type ? type : '');

        this.close = function () {
            $(self).triggerHandler('close');
        };

        this.closed = function (callback) {
            $(self).bind('close', function (e) {
                callback.call(self);
            });
        };

        this.hideAfter = function (timeout) {
            setTimeout(this.close, timeout);
        };
    };
    Alert.warning = '';
    Alert.error = 'alert-error';
    Alert.success = 'alert-success';
    Alert.info = 'alert-info';

    $.inspectR = InspectR;
}(jQuery));
