/*jslint browser: true*/
/*global jQuery */
(function ($) {
    "use strict";

    var InspectR = {
        start: function (config) {
            config = $.extend({}, InspectR.defaults, config);

            var viewModel = new InspectRViewModel(config);
            ko.applyBindings(viewModel, config.rootNode);

            var Router = Backbone.Router.extend({
                routes: {
                    "session/:id": "loadSession",
                    "*uniquekey": "start"
                },
                start: viewModel.start,
                loadSession: viewModel.loadSession
            });

            var router = new Router;

            $.connection.hub.start({
                // transport: 'auto'
            }, function () {
                Backbone.history.start({});
            });
        },
        defaults: {
            // inspectorKey: ''
            // rootNode:'#inspectr'
        }
    };

    var InspectRViewModel = function (config) {
        var self = this;

        self.inspectorKey = config.inspector;

        // router.on('route:loadSession', self.loadSession);
        // router.on('route:start', self.start);

        self.hub = $.connection.inspectRHub;
        var client = self.hub.client;
        var server = self.hub.server;

        self.NewTitle = ko.observable();
        self.IsEditingTitle = ko.observable(false);
        self.UserProfile = ko.observable();
        self.Requests = ko.mapping.fromJS([]);
        self.Inspector = ko.observable();
        self.SupportedContentTypes = _.keys(CodeMirror.mimeModes).sort();

        self.RequestList = ko.computed(function () {
            return self.Requests();
        });

        self.start = function (uniquekey) {
            if (uniquekey && uniquekey.length > 0) {
                self.inspectorKey = uniquekey;
            }
            
            self.updateUserProfile();

            self.startInspect();
        };

        self.startInspect = function () {
            self.Inspector(null);
            self.Requests([]);
            server.startInspect(self.inspectorKey)
                .done(function (result) {
                    if (result) {
                        self.Inspector(ko.mapping.fromJS(result));
                        self.loadRecentRequests();
                    }
                });
        };

        self.loadRecentRequests = function () {
            server.getRecentRequests(self.inspectorKey)
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
            server.clearRecentRequests(self.inspectorKey);
            self.Requests.removeAll();
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

    $.inspectR = InspectR;
}(jQuery));
