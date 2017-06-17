(function ($) {
    var myflow = $.myflow;

    $.extend(true, myflow.editors, {
        inputEditor: function () {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                $('<input style="width:100%;"/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                $('#' + _div).data('editor', this);
            }
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            }
        },
        selectEditor: function (arg) {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;

                var sle = $('<select  style="width:100%;"/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                if (typeof arg === 'string') {
                    $.ajax({
                        type: "GET",
                        url: arg,
                        success: function (data) {
                            sle.append('<option value=" ">' + ' ' + '</option>');
                            var opts = eval(data);
                            if (opts && opts.length) {
                                for (var idx = 0; idx < opts.length; idx++) {
                                    sle.append('<option value="' + opts[idx].Value + '">' + opts[idx].Text + '</option>');
                                }
                                sle.val(_props[_k].value);
                            }
                        }
                    });
                } else {
                    sle.append('<option value=" ">' + ' ' + '</option>');
                    for (var idx = 0; idx < arg.length; idx++) {
                        sle.append('<option value="' + arg[idx].Value + '">' + arg[idx].Text + '</option>');
                    }
                    sle.val(_props[_k].value);
                }

                $('#' + _div).data('editor', this);
            };
            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    _props[_k].value = $(this).val();
                });
            };
        },
        dropMultipleEditor: function (arg) {
            var _props, _k, _div, _src, _r;
            var sle;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;
                sle = $('<input id="workFlowDropMultiple' + _div + '" style="width:100%;"/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                    //props[_k].value = $('#' + sle[0].id).val();

                }).appendTo('#' + _div);

                //var sss = Object.keys(props[_k]);
                //alert(sss.join("\n"));

                var dialogDiv = $("<div id='workFlowDropMultiple" + _div + "DropDiv'  class='ui-widget-content' style='z-index:999999;position: absolute;display:none; background-color: #fff;' />").appendTo('#' + _div);
                dialogDiv.width(sle.width() - 22);
                sle.click(function () {
                    dialogDiv.show();
                });



                if (typeof arg === 'string') {
                    if (props[_k].value) {
                        var ids = props[_k].value.split("[");
                        var idval;
                        if (ids.length > 1)
                            idval = ids[1].substring(0, ids[1].length - 1);
                        else
                            idval = ids[0];
                        arg += "&selectVal=" + idval + "&fieldIdObj=" + sle[0].id;
                    }
                    else {
                        arg += "&fieldIdObj=" + sle[0].id;
                    }
                    $.ajax({
                        type: "GET",
                        url: arg,
                        success: function (data) {
                            dialogDiv.html(data);
                        }
                    });
                } else {

                }

                $('#' + _div).data('editor', this);
            };

            this.destroy = function () {
                $('#' + _div + ' input').each(function () {
                    //_props[_k].value = $(this).val();
                    _props[_k].value = $('#' + sle[0].id).val();
                });
            };

        }
    });

})(jQuery);