/*jslint browser: true*/
/*global ko:true */
(function ($, ko) {
    "use strict";

    Date.prototype.getMonthName = function (lang) {
        lang = lang && (lang in Date.locale) ? lang : 'en';
        return Date.locale[lang].month_names[this.getMonth()];
    };

    Date.prototype.getMonthNameShort = function (lang) {
        lang = lang && (lang in Date.locale) ? lang : 'en';
        return Date.locale[lang].month_names_short[this.getMonth()];
    };

    Date.prototype.getDayName = function (lang) {
        lang = lang && (lang in Date.locale) ? lang : 'en';
        return Date.locale[lang].day_names[this.getDay()];
    };

    Date.prototype.getDayNameShort = function (lang) {
        lang = lang && (lang in Date.locale) ? lang : 'en';
        return Date.locale[lang].day_names_short[this.getDay()];
    };

    Date.prototype.format = function (format, lang) {
        format = format || 'yyyy-MM-dd HH:mm.fff';
        lang = lang || (navigator.language ? navigator.language : navigator.userLanguage);

        var d = this;
        var date = format;

        // year
        $.each(['yyyy', 'yyy', 'yy', 'y'], function () {
            date = date.replace(this, d.getFullYear(lang));
        });

        // month
        date = date.replace('MMMM', '{L1}');
        date = date.replace('MMM', '{L2}');
        var month = d.getMonth();
        date = date.replace('MM', month < 10 ? '0' + month : month);
        date = date.replace('M', month);

        // day of week
        date = date.replace('dddd', '{L3}');
        date = date.replace('ddd', '{L4}');

        // day of month
        var day = d.getDate();
        date = date.replace('dd', day < 10 ? '0' + day : day);
        date = date.replace('d', day);

        // hour
        var hours = d.getHours();
        var suffix = (hours >= 12) ? 'PM' : 'AM';
        var hoursAmpm = (hours > 12) ? hours - 12 : hours;
        hoursAmpm = (hours == '00') ? 12 : hours;

        date = date.replace('HH', hours < 10 ? '0' + hours : hours);
        date = date.replace('H', hours);
        date = date.replace('hh', hoursAmpm < 10 ? '0' + hoursAmpm : hoursAmpm);
        date = date.replace('h', hoursAmpm);

        date = date.replace('tt', suffix);
        date = date.replace('t', suffix[0]);

        // minutes
        var minutes = d.getMinutes();
        date = date.replace('mm', minutes < 10 ? '0' + minutes : minutes);
        date = date.replace('m', minutes);

        // seconds
        var seconds = d.getSeconds();
        date = date.replace('ss', seconds < 10 ? '0' + seconds : seconds);
        date = date.replace('s', seconds);

        // milliseconds
        date = date.replace('fff', d.getMilliseconds());

        date = date.replace('{L1}', d.getMonthName(lang));
        date = date.replace('{L2}', d.getMonthNameShort(lang));
        date = date.replace('{L3}', d.getDayName(lang));
        date = date.replace('{L4}', d.getDayNameShort(lang));
        return date;
    };

    Date.locale = {
        en: {
            month_names: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            month_names_short: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            day_names: ['Monday', 'Thuesday', 'Wednesday', 'Thursday', 'Friday', 'Saterday', 'Sunday'],
            day_names_short: ['Mon', 'Thue', 'Wed', 'Thur', 'Fri', 'Sat', 'Sun']
        },
        nl: {
            month_names: ['Januari', 'Februari', 'Maart', 'April', 'Mei', 'Juni', 'Juli', 'Augustus', 'September', 'Oktober', 'November', 'December'],
            month_names_short: ['Jan', 'Feb', 'Mrt', 'Apr', 'Mei', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dec'],
            day_names: ['Maandag', 'Dinsdag', 'Woensdag', 'Donderdag', 'Vrijdag', 'Zaterdag', 'Zondag'],
            day_names_short: ['Ma', 'Di', 'Wo', 'Do', 'Vr', 'Za', 'Zo']
        }
    };

    String.prototype.fromJsonDate = function () {
        // converts microsoft json date to js Date. credits Jabbr.net
        return eval(this.replace(/\/Date\((\d+)(\+|\-)?.*\)\//gi, "new Date($1)"));
    };
    
    ko.bindingHandlers['dateformat'] = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor(),
                format = 'yyyy-MM-dd HH:mm.fff',
                lang = navigator.language ? navigator.language : navigator.userLanguage,
                cfg, date;
            
            if (ko.isObservable(value) || !value.date) {
                date = ko.utils.unwrapObservable(value);
            } else {
                cfg = value;
                format = ko.utils.unwrapObservable(cfg.format) || format;
                lang = ko.utils.unwrapObservable(cfg.lang) || lang;
                date = ko.utils.unwrapObservable(cfg.date);
            }
            
            if (Object.prototype.toString.call(date) !== '[object Date]') {
                if (date.indexOf('Date') != -1) {
                    date = date.fromJsonDate();
                } else {
                    date = new Date(date);
                }
            }
            
            var dateformatted = Date.prototype.format.call(date, format, lang);
            ko.bindingHandlers['text'].update(element, function () { return dateformatted; }, allBindingsAccessor, viewModel, bindingContext);
        }
    };

    var formatCodeMirror = function (cm) {
        var totalLines = cm.lineCount();
        cm.autoFormatRange(
            { line: 0, ch: 0 },
            { line: totalLines - 1, ch: cm.getLine(totalLines - 1).length }
        );
        cm.setSelection({ line: 0, ch: 0 }, { line: 0, ch: 0 });
        
        //CodeMirror.commands["selectAll"](cm);
        //cm.autoFormatRange(cm.getCursor(true), cm.getCursor(false));
        //cm.setSelection({ line: 0, ch: 0 }, { line: 0, ch: 0 });
        //// cm.setCursor(0, 0);
    };

    ko.bindingHandlers['withCodeMirror'] = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var name = ko.utils.unwrapObservable(valueAccessor());

            var vm = {
                codeMirror: null,
                LineWrapping: ko.observable(false),
                formatCodeMirror: function () {
                    formatCodeMirror(vm.codeMirror);
                }
            };

            vm.LineWrapping.subscribe(function (newval) {
                vm.codeMirror.setOption('lineWrapping', newval);
                vm.codeMirror.setValue(vm.codeMirror.getValue());
            });
            
            bindingContext[name] = vm;
            ko.applyBindingsToDescendants(bindingContext, element);

            return { controlsDescendantBindings: true };
        }
    };

    ko.bindingHandlers['codeMirror'] = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var config = ko.utils.unwrapObservable(valueAccessor()),
                name = ko.utils.unwrapObservable(config.name);

            var content = ko.utils.unwrapObservable(config.text);
            $(element).val(content);
            var codeMirror = CodeMirror.fromTextArea(element, {
                lineWrapping: false,
                lineNumbers: true,
                readOnly: true
            });
            bindingContext[name].codeMirror = codeMirror;

            ko.computed(function () {
                var contentType = ko.utils.unwrapObservable(config.contentType);
                codeMirror.setOption('mode', contentType);
                formatCodeMirror(codeMirror);
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var config = ko.utils.unwrapObservable(valueAccessor());

            ko.bindingHandlers['text'].update(element, function () { return config.text; }, allBindingsAccessor, viewModel, bindingContext);
        }
    };
}(jQuery, ko));
