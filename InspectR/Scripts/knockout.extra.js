/*jslint browser: true*/
/*global ko:true */
(function ($, ko) {
    "use strict";

    ko.bindingHandlers['syntaxhighlight'] = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            $(element).syntaxHighlight();
        }
    };
}(jQuery, ko));
