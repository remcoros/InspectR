/*jslint browser: true*/
/*global jQuery */
(function ($) {
    "use strict";

    // Bootstrapper
    var InspectR = {
        start: function (config) {
            config = $.extend({}, InspectR.defaults, config);
            var router = InspectR.Router = new Router(config);

            $.connection.hub.connectionSlow(function (data) {
                router.connectionSlow(this, data);
            });
            $.connection.hub.disconnected(function (data) {
                router.disconnected(this, data);
            });
            $.connection.hub.error(function (data) {
                router.connectionError(this, data);
            });

            $.connection.hub.start({
                transport: 'longPolling'
            }, function () {
                router.connected();
                Backbone.history.start({});
            });
        },
        defaults: {
            // inspector: ''
            // rootNode:'#inspectr'
        }
    };

    // Backbone Router, takes care of history
    var Router = Backbone.Router.extend({
        routes: {
            "": "start",
            "*uniquekey": "loadInspector"
        },
        viewModel: null,
        initialize: function (options) {
            // initialize and bind viewmodel
            var self = this;
            self.options = options;
            self.viewModel = new InspectRViewModel();
            ko.applyBindings(self.viewModel, options.rootNode);
        },
        start: function () {
            this.loadInspector(this.options.inspector);
        },
        loadInspector: function (uniquekey) {
            this.viewModel.startInspect(uniquekey);
        },
        connected: function () {
            // initial route
            this.viewModel.connected();
        },
        connectionSlow: function (connection) {
            this.viewModel.showAlert('', 'The connection is having some issues. Try refreshing this page.', Alert.warning);
        },
        disconnected: function (connection) {
            this.viewModel.showAlert('', 'You are disconnected. Try refreshing this page.', Alert.error);
        },
        connectionError: function (connection, data) {
            var self = this;
            if (self.ignoreConnectionErrors) {
                return;
            }
            self.ignoreConnectionErrors = true;
            self.viewModel.showAlert('', 'Errors occured in the connection. Try refreshing this page.', Alert.error)
                .closed(function () {
                    self.ignoreConnectionErrors = false;
                });
        }
    });

    // Main viewmodel
    var InspectRViewModel = function () {
        var self = this;

        self.hub = $.connection.inspectRHub;
        var client = self.hub.client;
        var server = self.hub.server;

        self.NewTitle = ko.observable();
        self.IsEditingTitle = ko.observable(false);
        self.UserProfile = ko.observable();
        self.Requests = ko.mapping.fromJS([]);
        self.Inspector = ko.observable();
        self.SupportedContentTypes = _.keys(CodeMirror.mimeModes).sort();
        self.Alerts = ko.observableArray([]);

        self.RequestList = ko.computed(function () {
            return self.Requests();
        });

        self.connected = function () {
            self.updateUserProfile();
        };

        self.startInspect = function (uniquekey) {
            // self.Inspector(null); // disable this to not flicker the screen
            self.Requests([]);
            server.startInspect(uniquekey)
                .done(function (result) {
                    if (result) {
                        self.Inspector(ko.mapping.fromJS(result));
                        self.loadRecentRequests();
                    }
                });
        };

        self.loadRecentRequests = function () {
            server.getRecentRequests(self.Inspector().UniqueKey)
                .done(function (result) {
                    if (result && result.length > 0) {
                        ko.mapping.fromJS(result, self.Requests);
                    }
                });
        };

        self.removeInspector = function (inspector) {
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

        self.loadSession = function () {

        };

        self.saveTitle = function () {
            server.setTitle(self.Inspector().Id(), self.NewTitle())
                .done(function () {
                    self.IsEditingTitle(false);
                    self.Inspector().Title(self.NewTitle());
                    self.updateUserProfile();
                });
        };

        self.updateUserProfile = function () {
            server.getUserProfile()
                .done(function (result) {
                    if (result) {
                        self.UserProfile(ko.mapping.fromJS(result));
                    }
                });
        };

        self.clearRecentRequests = function () {
            server.clearRecentRequests(self.Inspector().UniqueKey);
            self.Requests.removeAll();
        };

        self.showAlert = function (title, content, type) {
            var alert = new Alert(title, content, type);
            alert.closed(function () {
                self.Alerts.remove(alert);
            });
            self.Alerts.push(alert);
            return alert;
        };

        self.P = function (property, data) {
            if (!ko.isObservable(data[property])) {
                data[property] = ko.observable();
            }
            return data[property];
        };

        client.requestLogged = function (inspector, request) {
            // console.log('http request');
            // console.log(inspector, request);
            self.Requests.unshift(request);
        };
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
