/*jslint browser: true*/
/*global ko:true */
(function ($, ko) {
    "use strict";

    ko.bindingHandlers['codeMirror'] = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var property = ko.utils.unwrapObservable(valueAccessor());
            // var mode = ko.utils.unwrapObservable(config.mode);

            var mode = ko.utils.unwrapObservable(viewModel[property + 'CodeMirrorMode']);
            
            if (element.codeMirror) {
                element.codeMirror.toTextArea();
                delete element.codeMirror;
            }
            
            // var editorElement = $(element).find('textarea').get(0);
            var myCodeMirror = CodeMirror.fromTextArea(element, {
                lineNumbers: true,
                readOnly: true,
                mode: mode
            });
            element.codeMirror = myCodeMirror;
            viewModel[property + 'CodeMirror'] = myCodeMirror;

            // myCodeMirror.autoFormatRange({ line: 0, ch: 0 }, { line: myCodeMirror.lineCount, ch: 0 });
        }
    };
}(jQuery, ko));
