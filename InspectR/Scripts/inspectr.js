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
                    "*actions": "start"
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

        self.UserProfile = ko.observable();
        self.Requests = ko.mapping.fromJS([]);
        self.Inspector = ko.observable();
        self.SupportedContentTypes = _.keys(CodeMirror.mimeModes);

        self.RequestList = ko.computed(function () {
            return self.Requests();
        });

        $.connection.hub.reconnected(function () {
            server.startInspect(self.inspectorKey);
        });

        self.start = function () {
            server.startInspect(self.inspectorKey);

            server.getUserProfile()
                .done(self.updateUserProfile);

            server.getRecentRequests(self.inspectorKey)
                .done(function (result) {
                    if (result && result.length > 0) {
                        ko.mapping.fromJS(result, self.Requests);
                    }
                });
        };

        self.loadSession = function () {

        };

        self.updateUserProfile = function (profile) {
            self.UserProfile(profile);
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
