/**
link_embedded editable input.
Internally value stored as {name: "a name", url: "a url", embedded: "short text"}

@class link_embedded
@extends abstractinput
@final
@example
<a href="#" id="link_embedded" data-type="link_embedded" data-pk="1">awesome</a>
<script>
$(function(){
    $('#link').editable({
        url: '/post',
        title: 'Enter name, url and/or embedded',
        value: {
            name: "a name", 
            url: "a url", 
            embedded: "short text"
        }
    });
});
</script>
**/
(function ($) {
    "use strict";

    var Link_Embedded = function (options) {
        this.init('link_embedded', options, Link_Embedded.defaults);
    };

    //inherit from Abstract input
    $.fn.editableutils.inherit(Link_Embedded, $.fn.editabletypes.abstractinput);

    $.extend(Link_Embedded.prototype, {
        /**
        Renders input from tpl

        @method render() 
        **/
        render: function () {
            this.$input = this.$tpl.find(':input');
        },

        /**
        Default method to show value in element. Can be overwritten by display option.
        
        @method value2html(value, element) 
        **/
        value2html: function (value, element) {
            if (!value) {
                $(element).empty();
                return;
            }
            var html = $('<div>').text(value.name).html();
            $(element).html(html);
        },

        /**
        Gets value from element's html
        
        @method html2value(html) 
        **/
        html2value: function (html) {
            /*
              you may write parsing method to get value by element's html
              e.g. "a name, a url, a short text" => {name: "a name", url: "a url", embedded: "short text"}
              but for complex structures it's not recommended.
              Better set value directly via javascript, e.g. 
              editable({
                  value: {
                      name: "a name", 
                      url: "a url", 
                      embedded: "short text"
                  }
              });
            */
            return null;
        },

        /**
         Converts value to string. 
         It is used in internal comparing (not for sending to server).
         
         @method value2str(value)  
        **/
        value2str: function (value) {
            var str = '';
            if (value) {
                for (var k in value) {
                    str = str + k + ':' + value[k] + ';';
                }
            }
            return str;
        },

        /*
         Converts string to value. Used for reading value from 'data-value' attribute.
         
         @method str2value(str)  
        */
        str2value: function (str) {
            /*
            this is mainly for parsing value defined in data-value attribute. 
            If you will always set value by javascript, no need to overwrite it
            */
            return str;
        },

        /**
         Sets value of input.
         
         @method value2input(value) 
         @param {mixed} value
        **/
        value2input: function (value) {
            if (!value) {
                return;
            }
            this.$input.filter('[name="name"]').val(value.name);
            this.$input.filter('[name="url"]').val(value.url);
            this.$input.filter('[name="embedded"]').val(value.embedded);
        },

        /**
         Returns value of input.
         
         @method input2value() 
        **/
        input2value: function () {
            return {
                name: this.$input.filter('[name="name"]').val(),
                url: this.$input.filter('[name="url"]').val(),
                embedded: this.$input.filter('[name="embedded"]').val()
            };
        },

        /**
        Activates input: sets focus on the first field.
        
        @method activate() 
       **/
        activate: function () {
            this.$input.filter('[name="name"]').focus();
        },

        /**
         Attaches handler to submit form in case of 'showbuttons=false' mode
         
         @method autosubmit() 
        **/
        autosubmit: function () {
            this.$input.keydown(function (e) {
                if (e.which === 13) {
                    $(this).closest('form').submit();
                }
            });
        }
    });

    Link_Embedded.defaults = $.extend({}, $.fn.editabletypes.abstractinput.defaults, {
        tpl: '<div class="editable-address"><label><span>Name: </span><input type="text" name="name" class="input-small"></label></div>' +
             '<div class="editable-address"><label><span>Url: </span><input type="text" name="url" class="input-small"></label></div>' +
             '<div class="editable-address"><label><span>Embedded: </span><textarea rows="5" cols="30" name="embedded" class="input-small"></textarea></label></div>',

        inputclass: ''
    });

    $.fn.editabletypes.link_embedded = Link_Embedded;

}(window.jQuery));

/**
link editable input.
Internally value stored as {name: "a name", url: "a url", embedded: "short text"}

@class link
@extends abstractinput
@final
@example
<a href="#" id="link" data-type="link" data-pk="1">awesome</a>
<script>
$(function(){
    $('#link').editable({
        url: '/post',
        title: 'Enter name and url',
        value: {
            name: "a name", 
            url: "a url" 
        }
    });
});
</script>
**/
(function ($) {
    "use strict";

    var Link = function (options) {
        this.init('link', options, Link.defaults);
    };

    //inherit from Abstract input
    $.fn.editableutils.inherit(Link, $.fn.editabletypes.abstractinput);

    $.extend(Link.prototype, {
        /**
        Renders input from tpl

        @method render() 
        **/
        render: function () {
            this.$input = this.$tpl.find(':input');
        },

        /**
        Default method to show value in element. Can be overwritten by display option.
        
        @method value2html(value, element) 
        **/
        value2html: function (value, element) {
            if (!value) {
                $(element).empty();
                return;
            }
            var html = $('<div>').text(value.name).html();
            $(element).html(html);
        },

        /**
        Gets value from element's html
        
        @method html2value(html) 
        **/
        html2value: function (html) {
            /*
              you may write parsing method to get value by element's html
              e.g. "a name, a url" => { name: "a name", url: "a url" }
              but for complex structures it's not recommended.
              Better set value directly via javascript, e.g. 
              editable({
                  value: {
                      name: "a name", 
                      url: "a url" 
                  }
              });
            */
            return null;
        },

        /**
         Converts value to string. 
         It is used in internal comparing (not for sending to server).
         
         @method value2str(value)  
        **/
        value2str: function (value) {
            var str = '';
            if (value) {
                for (var k in value) {
                    str = str + k + ':' + value[k] + ';';
                }
            }
            return str;
        },

        /*
         Converts string to value. Used for reading value from 'data-value' attribute.
         
         @method str2value(str)  
        */
        str2value: function (str) {
            /*
            this is mainly for parsing value defined in data-value attribute. 
            If you will always set value by javascript, no need to overwrite it
            */
            return str;
        },

        /**
         Sets value of input.
         
         @method value2input(value) 
         @param {mixed} value
        **/
        value2input: function (value) {
            if (!value) {
                return;
            }
            this.$input.filter('[name="name"]').val(value.name);
            this.$input.filter('[name="url"]').val(value.url);
        },

        /**
         Returns value of input.
         
         @method input2value() 
        **/
        input2value: function () {
            return {
                name: this.$input.filter('[name="name"]').val(),
                url: this.$input.filter('[name="url"]').val()
            };
        },

        /**
        Activates input: sets focus on the first field.
        
        @method activate() 
       **/
        activate: function () {
            this.$input.filter('[name="name"]').focus();
        },

        /**
         Attaches handler to submit form in case of 'showbuttons=false' mode
         
         @method autosubmit() 
        **/
        autosubmit: function () {
            this.$input.keydown(function (e) {
                if (e.which === 13) {
                    $(this).closest('form').submit();
                }
            });
        }
    });

    Link.defaults = $.extend({}, $.fn.editabletypes.abstractinput.defaults, {
        tpl: '<div class="editable-address"><label><span>Name: </span><input type="text" name="name" class="input-small"></label></div>' +
             '<div class="editable-address"><label><span>Url: </span><input type="text" name="url" class="input-small"></label></div>',
        inputclass: ''
    });

    $.fn.editabletypes.link = Link;

}(window.jQuery));