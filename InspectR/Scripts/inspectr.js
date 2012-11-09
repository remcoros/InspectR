/*jslint browser: true*/
/*global jQuery */
(function ($) {
    "use strict";

    var InspectR = {
        start: function (config) {
            config = $.extend({}, InspectR.defaults, config);
            
            var viewModel = new InspectRViewModel(config);

            ko.applyBindings(viewModel, config.rootNode);

            $.connection.hub.start({
                // transport: 'auto'
            }, viewModel.start);
        },
        defaults: {
            // inspectorKey: ''
            // rootNode:'#inspectr'
        }
    };

    var InspectRViewModel = function (config) {
        var self = this;

        self.inspectorKey = config.inspector;

        self.hub = $.connection.inspectRHub;
        var client = self.hub.client;
        var server = self.hub.server;

        self.Requests = ko.observableArray([]);
        self.Inspector = ko.observable();
        self.RequestList = ko.computed(function () {
            return self.Requests();
        });
        
        self.start = function () {
            server.startInspect(self.inspectorKey)
                .done(function () {
                    server.getRecentRequests(self.inspectorKey)
                        .done(function (result) {
                            if (result && result.length > 0) {
                                self.Requests(result);
                            }
                        });
                });
        };

        self.clearRecentRequests = function () {
            server.clearRecentRequests(self.inspectorKey);
            self.Requests.removeAll();
        };


        self.formatDate = function (datestring) {
            var d = new Date(datestring);
            return d.getFullYear() + '-'
                + d.getMonth() + '-'
                + d.getDate();
        };
        
        self.formatTime = function (datestring) {
            var d = new Date(datestring);
            return d.getHours() + ':'
                + d.getMinutes() + ':'
                + d.getSeconds() + '.'
                + d.getMilliseconds();
        };
        
        client.requestLogged = function (inspector, request) {
            // console.log('http request');
            console.log(inspector, request);
            self.Requests.unshift(request);
        };
    };

    $.inspectR = InspectR;
}(jQuery));
