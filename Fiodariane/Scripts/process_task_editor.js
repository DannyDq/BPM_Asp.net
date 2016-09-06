/*
 * process_task_editor - jQuery plugin to support process and task creation.
 * 
 * Copyright (c) 2015 Jose Danado
 *
 * version: 1.0.0
 */

(function ($) {

    /*
     * Function to update process related values
    */
    $.fn.updateProcessValue = function (options) {
        var $this = this;
        var settings = $.extend({
            url: '',
            processID: 0,
            editor: '',
            validation: '',
            operation: null,
            task: 0
        }, options);
        $.ajax({
            type: "POST",
            url: settings.url,
            data: JSON.stringify({ Identifier: settings.processID, Value: settings.editor, Operation: settings.operation, TaskID: settings.task }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (handler, status, error) {
                settings.validation.text(error);
            },
            success: function (data) {
                if (data.code == 0) {
                    settings.validation.text(data.message);
                } else if (data.code == 2) {
                    location.reload(true);
                }
            }
        });
        return this;
    }

    /*
     * Initialise embedded links
     */
    $.fn.initialiseEmbeddedLinks = function (options) {
        var settings = $.extend({
            url: '',
            name: '',
            item_name: '',
            item_embedded: '',
            item_url: '',
            process_id: 0,
            task_id: 0,
            sysurl_id: 0,
            operation: '',
            type: 'link_embedded'
        }, options);
        $(this).editable({
            url: settings.url,
            type: settings.type,
            pk: settings.sysurl_id,
            params: function(params) {
                var data = { sysurl_id: settings.sysurl_id, task_id: settings.task_id, process_id: settings.process_id, operation: settings.operation, name: params.value.name, url: params.value.url, embedded: params.value.embedded };
                return data;
            },
            value: {
                name: settings.item_name,
                url: settings.item_url,
                embedded: settings.item_embedded
            },
            validate: function(value) {
                if (value.name == '') return settings.name + ' is required!';
            },
            display: function(value) {
                if (!value) {
                    $(this).empty();
                    return;
                }
                var txt = '';
                if((value.name == undefined) || (value.name == '') || (value.name == null)) { txt = 'Empty'; } else { txt = value.name; }
                var html = $('<div>').text(txt).html();
                $(this).html(html);
            }
        });
    }

    /*
     * Delete an embedded link
     */
    $.fn.deleteEmbeddedLink = function (options) {
        var settings = $.extend({
            url: '',
            sysurl_id: 0,
            sysurl_type: 'Input',
            task_id: 0,
            validation: '',
        }, options);
        var $this = $(this);

        $.ajax({
            type: "POST",
            url: settings.url,
            data: JSON.stringify({ OperationType: 'Delete', TaskID: settings.task_id, SysUrlID: settings.sysurl_id, SysUrlType: settings.sysurl_type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (handler, status, error) {
                settings.validation.text(error);
            },
            success: function (data) {
                if (data.code == 0) {
                    settings.validation.text(data.message);
                } else if (data.code == 1) {
                    $this.parent().remove();
                } else if (data.code == 2) {
                    location.reload(true);
                }
            }
        });

        return this;
    }

    /*
     * creates and adds an embedded link
     */
    $.fn.createEmbeddedLink = function (options) {
        var settings = $.extend({
            url: '',
            delete_url: '',
            process_id: 0,
            task_id: 0,
            operation: 'Input',
            data_type: 'link',
            data_pk: 0,
            data_name: '',
            data_content_name: '',
            data_content_url: '',
            data_content_embedded: ''
        }, options);
        var newnode = $('<div class="link_container">' +
            '<a href="#" class="task_link" data-pk="' + settings.data_pk +
            '" data-type="' + settings.data_type + '" data-url="' + settings.url + '" data-title="Enter ' + settings.data_name +
            '">' + settings.data_content_name + '</a>' +
            '<div class="task_delete btn btn-link">' +
            '<span class="glyphicon glyphicon-trash"></span>' +
            '<span class="sr-only">Delete</span>' +
            '</div>' +
            '<span class="field-validation-valid text-danger" data-valmsg-replace="true"></span>' +
            '</div>');
        newnode.children('.task_link').initialiseEmbeddedLinks({ url: settings.url, type: settings.data_type, name: settings.data_name, 
            item_name: settings.data_content_name, item_url: settings.data_content_url, item_embedded: settings.data_content_embedded, 
            process_id: settings.process_id, task_id: settings.task_id, sysurl_id: settings.data_pk, operation: settings.operation
        });

        newnode.children('.task_delete').click(function () {
            $(this).deleteEmbeddedLink({
                url: settings.delete_url,
                sysurl_id: $(this).siblings('a').attr('data-pk'), validation: $(this).siblings('span'),
                task_id: settings.task_id, sysurl_type: $(this).attr('data-type')
            });
        });
        $(this).before(newnode);
        return this;
    }

    /*
     * Delete an embedded link
     */
    $.fn.addEmbeddedLink = function (options) {
        var settings = $.extend({
            url: '',
            delete_url: '',
            process_id: 0,
            task_id: 0,
            sysurl_type: 'Input',
            data_name: 'Input',
            validation: ''
        }, options);
        var $this = $(this);

        $.ajax({
            type: "POST",
            url: settings.delete_url,
            data: JSON.stringify({ OperationType: 'Add', TaskID: settings.task_id, SysUrlID: 0, SysUrlType: settings.sysurl_type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (handler, status, error) {
                settings.validation.text(error);
            },
            success: function (data) {
                if (data.code == 0) {
                    settings.validation.text(data.message);
                } else if (data.code == 1) {
                    $this.createEmbeddedLink({ url: settings.url, delete_url: settings.delete_url, process_id: settings.process_id, 
                        task_id: settings.task_id, operation: settings.sysurl_type, data_type: 'link', data_pk: data.message.sysurl_id,
                        data_name: settings.data_name, data_content_name: '', data_content_url: '', data_content_embedded: '' });
                }
            }
        });

        return this;
    }

    /*
     * Initialise select links
     */
    $.fn.initialiseSelectLinks = function (options) {
        var settings = $.extend({
            task_id: 0,
            process_id: 0,
            operation: '',
            value: null,
            source: null
        }, options);
        $(this).editable({
            params: function(params) {
                var arr = [];
                $.each( params.value, function( key, value ) {
                    arr.push(value);
                });
                var data = { task_id: settings.task_id, process_id: settings.process_id, operation: settings.operation, values: arr };
                return data;
            },
            value: settings.value,
            source: settings.source
        });

        return this;
    }

    /*
     * Calculates the size of the image box
     */
    $.fn.calculatesBoxSize = function (options) {
        var settings = $.extend({}, options);
        var col = $('.mycol');
        return (col.width() + Number(col.css('padding-left').replace('px',''))) * ($('#tasks > .form-group:eq(0)').children().length - 1);
    }

    /*
     * Updates the size of the image box according to the number of columns
     */
    $.fn.updateBoxSize = function (options) {
        var settings = $.extend({}, options);
        if ($(this).length > 0) {
            var box_size = $(this).calculatesBoxSize();
            $(this).parent().css('width', box_size + 'px');
            $(this).parent().css('background-image', 'url(\'\')');
            $(this).parent().css('line-height', '10px');
            $(this).css('width', (box_size - $(this).parent().css('padding-left').replace('px', '') * 2) + 'px');
            $(this).parent().css('height', ($(this).height() + Number($(this).parent().css('padding-top').replace('px', '')) * 2) + 'px');
        }
        return this;
    }

    /*
     * Uploads a file to the server
     */

    $.fn.singleupload = function (options) {

        var $this = this;
        var inputfile = null;

        var settings = $.extend({
            action: '#',
            onSuccess: function (url, data) { },
            onError: function (code) { },
            OnProgress: function (loaded, total) {
                var percent = Math.round(loaded * 100 / total);
                $this.html(percent + '%');
            },
            width: function () { return 0; },
            name: 'img',
            processID: 0
        }, options);

        $('#' + settings.inputId).bind('change', function () {
            var fd = new FormData();
            fd.append($('#' + settings.inputId).get(0).files[0].name, $('#' + settings.inputId).get(0).files[0]);
            fd.append('processID', settings.processID);
            var xhr = new XMLHttpRequest();
            xhr.addEventListener("load", function (ev) {
                $this.html('');

                var res = eval("(" + ev.target.responseText + ")");

                if (res.code != 0) {
                    settings.onError(res.code);
                    return;
                }
                $this.css('background-image', 'url()');
                var review = ('<img src="' + res.url + '" />');
                $this.append(review);
                $this.children('img').css('width', (settings.width() - $this.css('padding-left').replace('px', '') * 2) + 'px');
                $this.css('height', 'auto');
                $this.css('width', settings.width());
                $this.css('margin', '0px');
                settings.onSuccess(res.url, res.data);
            },
            false);
            xhr.upload.addEventListener("progress", function (ev) {
                settings.OnProgress(ev.loaded, ev.total);
            }, false);
            xhr.open("POST", settings.action, true);
            xhr.send(fd);

        });

        return this;
    }

    /*
     * Delete a task
     */
    $.fn.deleteTask = function (options) {
        var settings = $.extend({
            url: '',
            task_id: 0,
            process_id: 0
        }, options);
        // Check if it is the last column
        var idx = $(this).index(".delete");
        var siblings_num = $(this).siblings(".delete").length;
        // Posts data
        $.ajax({
            type: "POST",
            url: settings.url,
            data: JSON.stringify({ ProcessID: settings.process_id, TaskID: settings.task_id, TaskType: (siblings_num > 0 ? 'Delete' : 'Clear') }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (handler, status, error) {
                alert(error);
            },
            success: function (data) {
                if (data.code == 1) {
                    $('#tasks > .form-group').each(function (index, elem) {
                        var child = $(this).children('.cloneme');
                        if (child.length >= idx)
                            child.eq(idx).remove();
                    });
                    $('#tasks > .form-group').updateLineWidth({ column: $('.mycol'), label: $('.control-label'), num_cols: ($('#tasks > .form-group:eq(0)').children().length - 1) });
                    $('#uploadbox > img').updateBoxSize();
                } else if (data.code == 2) {
                    $('#tasks > .form-group').each(function (index, elem) {
                        var child = $(this).children('.cloneme');
                        switch (index) {
                            case 1:
                                child.children('a').editable('setValue', {
                                    name: '',
                                    url: '',
                                    embedded: ''
                                });
                                break;
                            case 2:
                                child.children('select').children('option').removeAttr('selected');
                                break;
                            case 3:
                                child.children('select').children('option').removeAttr('selected');
                                break;
                            case 4:
                                child.children('select').children('option').removeAttr('selected');
                                break;
                            case 5:
                                child.children('select').children('option').removeAttr('selected');
                                break;
                            case 6:
                                var num_link_container = child.children('.link_container');
                                if (num_link_container.length > 0) {
                                    child.children('.link_container').remove();
                                }
                                break;
                            case 7:
                                var num_task_options = child.children('.task_options');
                                num_task_options.editable('setValue','');
                                break;
                            case 8:
                                var num_link_container = child.children('.link_container');
                                if (num_link_container.length > 0) {
                                    child.children('.link_container').remove();
                                }
                                break;
                            case 9:
                                var num_task_options = child.children('.task_options');
                                num_task_options.editable('setValue', '');
                                break;
                            case 10:
                                var num_link_container = child.children('.link_container');
                                if (num_link_container.length > 0) {
                                    child.children('.link_container').remove();
                                }
                                break;
                            case 11:
                                child.children('a').editable('setValue', {
                                    name: '',
                                    url: '',
                                    embedded: ''
                                });
                                break;
                            case 12:
                                child.children('a').editable('setValue', {
                                    name: '',
                                    url: '',
                                    embedded: ''
                                });
                                break;
                            default:
                                break;
                        }
                    });
                    formNode.updateLineWidth({ column: $('.mycol'), label: $('.control-label'), num_cols: ($('#tasks > .form-group:eq(0)').children().length - 1) });
                    $('#uploadbox > img').updateBoxSize();
                } else {
                    alert(data.message);
                }
            }
        });
        return this;
    }

    /*
     * Add a task
     */
    $.fn.addTask = function (options) {
        var settings = $.extend({
            url: '',
            url_embedded: '',
            url_delete: '',
            task_id: 0,
            process_id: 0,
            select_options: null
        }, options);
        $.ajax({
            type: "POST",
            url: settings.url,
            data: JSON.stringify({ ProcessID: settings.process_id, TaskID: settings.task_id, TaskType: 'Add' }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (handler, status, error) {
                alert(error);
            },
            success: function (data) {
                var formNode = $('#tasks > .form-group');
                formNode.each(function (index, elem) {
                    var mynode = $(this).children('.cloneme:last');
                    var newnode;
                    switch (index) {
                        case 0:
                            newnode = mynode.clone(true);
                            newnode.attr('data-pk', data.message.task_id);
                            break;
                        case 1:
                            newnode = mynode.clone();
                            newnode.children('.task_link').initialiseEmbeddedLinks({
                                url: settings.url_embedded, type: 'link_embedded',
                                name: 'Name', item_name: '', item_url: '', item_embedded: '', process_id: settings.process_id,
                                task_id: data.message.task_id, sysurl_id: data.message.sysurl_id, operation: 'Name'
                            });
                            break;
                        case 2:
                            newnode = mynode.clone(true);
                            newnode.children('select').children('option').removeAttr('selected');
                            newnode.attr('data-pk',data.message.task_id);
                            break;
                        case 3:
                            newnode = mynode.clone(true);
                            newnode.children('select').children('option').removeAttr('selected');
                            newnode.attr('data-pk',data.message.task_id);
                            break;
                        case 4:
                            newnode = mynode.clone(true);
                            newnode.children('select').children('option').removeAttr('selected');
                            newnode.attr('data-pk',data.message.task_id);
                            break;
                        case 5:
                            newnode = mynode.clone(true);
                            newnode.children('select').children('option').removeAttr('selected');
                            newnode.attr('data-pk',data.message.task_id);
                            break;
                        case 6:
                            newnode = mynode.clone();
                            var num_link_container = newnode.children('.link_container');
                            if (num_link_container.length > 0) {
                                newnode.children('.link_container').remove();
                            }
                            var addbtn = newnode.children('.task_add');
                            addbtn.click(function () {
                                $(this).addEmbeddedLink({
                                    url: settings.url_embedded, delete_url: settings.url_delete,
                                    process_id: settings.process_id, task_id: data.message.task_id, sysurl_type: 'Input',
                                    data_name: 'Input', validation: $(this).siblings('span')
                                });
                            });
                            break;
                        case 7:
                            newnode = mynode.clone();
                            var num_task_options = newnode.children('.task_options');
                            if (num_task_options.length > 0) {
                                num_task_options.initialiseSelectLinks({
                                    process_id: settings.process_id, task_id: data.message.task_id, operation: 'Origin',
                                    value: '', source: settings.select_options
                                });
                            }
                            break;
                        case 8:
                            newnode = mynode.clone();
                            var num_link_container = newnode.children('.link_container');
                            if (num_link_container.length > 0) {
                                newnode.children('.link_container').remove();
                            }
                            var addbtn = newnode.children('node_link');
                            addbtn.click(function () {
                                $(this).addEmbeddedLink({
                                    url: settings.url_embedded, delete_url: settings.url_delete,
                                    process_id: settings.process_id, task_id: data.message.task_id, sysurl_type: 'Output',
                                    data_name: 'Output', validation: $(this).siblings('span')
                                });
                            });
                            break;
                        case 9:
                            newnode = mynode.clone();
                            var num_task_options = newnode.children('.task_options');
                            if (num_task_options.length > 0) {
                                num_task_options.initialiseSelectLinks({
                                    process_id: settings.process_id, task_id: data.message.task_id, operation: 'Destination',
                                    value: '', source: settings.select_options
                                });
                            }
                            break;
                        case 10:
                            newnode = mynode.clone();
                            var num_link_container = newnode.children('.link_container');
                            if (num_link_container.length > 0) {
                                newnode.children('.link_container').remove();
                            }
                            var addbtn = newnode.children('node_link');
                            addbtn.click(function () {
                                $(this).addEmbeddedLink({
                                    url: settings.url_embedded, delete_url: settings.url_delete,
                                    process_id: settings.process_id, task_id: data.message.task_id, sysurl_type: 'DocRef',
                                    data_name: 'DocRef', validation: $(this).siblings('span')
                                });
                            });
                            break;
                        case 11:
                            newnode = mynode.clone();
                            newnode.children('.task_link').initialiseEmbeddedLinks({
                                url: settings.url_embedded, type: 'link_embedded',
                                name: 'Archive', item_name: '', item_url: '', item_embedded: '', process_id: settings.process_id,
                                task_id: data.message.task_id, sysurl_id: data.message.sysurl_id, operation: 'Archive'
                            });
                            break;
                        case 12:
                            newnode = mynode.clone();
                            newnode.children('.task_link').initialiseEmbeddedLinks({
                                url: settings.url_embedded, type: 'link_embedded',
                                name: 'KPI', item_name: '', item_url: '', item_embedded: '', process_id: settings.process_id,
                                task_id: data.message.task_id, sysurl_id: data.message.sysurl_id, operation: 'Kpi'
                            });
                            break;
                        default:
                            break;
                    }
                    mynode.after(newnode);
                });
                formNode.updateLineWidth({ column: $('.mycol'), label: $('.control-label'), num_cols: ($('#tasks > .form-group:eq(0)').children().length - 1) });
                $('#uploadbox > img').updateBoxSize();
            }
        });

        return this;
    }

    /*
     * Updates the width of a line
     */
    $.fn.updateLineWidth = function (options) {
        var settings = $.extend({
            column: null,
            label: null,
            num_cols: 0
        }, options);
        var col_width = (settings.column.width() + settings.column.css('padding-left').replace('px', '') * 2) * (settings.num_cols + 2);
        var lab_width = (settings.label.width() + settings.label.css('padding-left').replace('px', '') * 2);
        var line_width = $(this).width() + $(this).css('padding-left').replace('px','') * 2;
        if((col_width + lab_width) > line_width) {
            $(this).css('width', (col_width + lab_width) + 'px');
        }
    }


}(jQuery));