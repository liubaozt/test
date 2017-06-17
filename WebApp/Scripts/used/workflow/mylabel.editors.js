(function ($) {
    var mylabel = $.mylabel;

    $.extend(true, mylabel.editors, {
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
        dialogEditor: function (arg) {
            var _props, _k, _div, _src, _r;
            this.init = function (props, k, div, src, r) {
                _props = props; _k = k; _div = div; _src = src; _r = r;
                //                alert("k=" + _k);
                //                alert("div=" + _div);
                //                alert("propsk=" + props[_k]);
                var sle = $('<select id="drop" style="width:100%;"/>').val(props[_k].value).change(function () {
                    props[_k].value = $(this).val();
                }).appendTo('#' + _div);

                var dialogId = sle[0].id + "Dialog"
                var dialogDiv = $('<div id="' + dialogId + '"></div>').appendTo('#' + _div);
                var btnDialog = $('<button  style="height:21px;"></button>').appendTo('#' + _div);
                btnDialog.click(function () {
                    AppTreeDialogButtonClick("#" + sle[0].id, "tree1000", "1000", '/BusinessCommon/Department/Select', "≤ø√≈—°‘Ò", '/DropList/DepartmentDropList');
                });
                
                if (typeof arg === 'string') {
                    $.ajax({
                        type: "GET",
                        url: arg,
                        success: function (data) {
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

        }
    });

})(jQuery);