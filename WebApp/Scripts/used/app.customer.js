//对象序列化
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

//将json数据附加到select标签上
function AppAppendSelect(data, appendSelect, url) {
    $(appendSelect).empty();
    $.each(data, function (i, item) {
        $("<option></option>")
            .val(item["Value"])
            .text(item["Text"])
            .appendTo($(appendSelect));
    });
    $(appendSelect).options[0].selected = true;
    if (url) {
        UpdateSelectUrl(url, appendSelect);
    }
}

//将json数据附加到select标签上，第一行插入空行
function AppAppendSelect2(data, appendSelect, url) {
    $(appendSelect).empty();
    $("<option></option>")
        .val("")
        .text("")
        .appendTo($(appendSelect));
    $.each(data, function (i, item) {
        $("<option></option>")
        .val(item["Value"])
        .text(item["Text"])
        .appendTo($(appendSelect));
    });
    if (url) {
        UpdateSelectUrl(url, appendSelect);
    }
}

//更新下拉框的数据源url
function UpdateSelectUrl(url, appendSelect) {
    $(appendSelect + "Url").val(url);
}



//追加值到DropDown时判断是否含有该值，无则追加
function AddValueToDropDown(fieldIdObj, nodeId, nodeName) {
    var hasId = 0;
    $(fieldIdObj + " option").each(function () {
        if ($(this).val() == nodeId) {
            hasId = 1
            return false;
        }
    });
    if (hasId == 0) {
        $("<option></option>")
        .val(nodeId)
        .text(nodeName)
        .appendTo($(fieldIdObj));
    }
    $(fieldIdObj).val(nodeId);
}
//grid,追加值到DropDown时判断是否含有该值，无则追加
function AddValueToDropDown2(fieldIdObj, gridObj, nodeId, nodeName) {
    var hasId = 0;
    $(fieldIdObj + " option", gridObj).each(function () {
        if ($(this).val() == nodeId) {
            hasId = 1
            return false;
        }
    });
    if (hasId == 0) {
        $("<option></option>")
        .val(nodeId)
        .text(nodeName)
        .appendTo($(fieldIdObj));
    }
    $(fieldIdObj, gridObj).val(nodeId);
}

//弹出树形选择框
function AppTreeDialogButtonClick(fieldIdObj, treeIdObj, pageid, dialogUrl, dialogTitle, dropUrl, addFavoritUrl, replaceFavoritUrl) {
    $.ajax({
        type: "POST",
        url: dialogUrl,
        data: { pageId: pageid },
        datatype: "html",
        success: function (data) {
            $(fieldIdObj + "Dialog").html(data).dialog({
                title: dialogTitle,
                height: 350,
                width: 400,
                modal: true,
                resizable: true,
                buttons: {
                    "收藏": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            $.ajax({
                                url: addFavoritUrl,
                                async: false,
                                data: { pkValue: pid },
                                success: function (msg) {
                                    //对返回值的处理
                                    $.getJSON(dropUrl, function (data) {
                                        AppAppendSelect2(data, $(fieldIdObj), dropUrl);
                                        $(fieldIdObj).val(pid);
                                    });
                                }
                            });

                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj).change(); }, 800);
                        }
                    },
                    "替换": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            $.ajax({
                                url: replaceFavoritUrl,
                                async: false,
                                data: { pkValue: pid },
                                success: function (msg) {
                                    $.getJSON(dropUrl, function (data) {
                                        AppAppendSelect2(data, $(fieldIdObj), dropUrl);
                                        $(fieldIdObj).val(pid);
                                    });
                                }
                            });
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj).change(); }, 800);
                        }
                    },
                    "确定": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            AddValueToDropDown(fieldIdObj, pid, pname)
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj).change(); }, 800);
                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    //allFields.val("").removeClass("ui-state-error");
                }
            });
        }
    });
}

//弹出树形选择框2，主要对表格使用
function AppTreeDialogButtonClick2(fieldIdObj, gridObj, treeIdObj, pageid, dialogUrl, dialogTitle, dropUrl, addFavoritUrl, replaceFavoritUrl) {
    $.ajax({
        type: "POST",
        url: dialogUrl,
        data: { pageId: pageid },
        datatype: "html",
        success: function (data) {
            $('#grid' + pageid + "Dialog").html(data).dialog({
                title: dialogTitle,
                height: 350,
                width: 400,
                modal: true,
                resizable: true,
                buttons: {
                    "收藏": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            $.ajax({
                                url: addFavoritUrl,
                                async: false,
                                data: { pkValue: pid },
                                success: function (msg) {
                                    //对返回值的处理
                                }
                            });
                            $.getJSON(dropUrl, function (data) {
                                AppAppendSelect2(data, $(fieldIdObj, gridObj), dropUrl);
                                $(fieldIdObj, gridObj).val(pid);
                            });
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj, gridObj).change(); }, 800);
                        }
                    },
                    "替换": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            $.ajax({
                                url: replaceFavoritUrl,
                                async: false,
                                data: { pkValue: pid },
                                success: function (msg) {
                                    $.getJSON(dropUrl, function (data) {
                                        AppAppendSelect2(data, $(fieldIdObj, gridObj), dropUrl);
                                        $(fieldIdObj, gridObj).val(pid);
                                    });
                                }
                            });
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj, gridObj).change(); }, 800);
                        }
                    },
                    "确定": function () {
                        var bValid = true;
                        if (bValid) {
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getSelectedNodes();
                            var pid = nodes[0].id;
                            var pname = nodes[0].name;
                            var hasId = 0;
                            AddValueToDropDown2(fieldIdObj, gridObj, pid, pname)
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj, gridObj).change(); }, 800);
                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    //allFields.val("").removeClass("ui-state-error");
                }
            });
        }
    });
}

//弹出树形多选框
function AppTreeDialogButtonClick3(fieldIdObj, treeIdObj, pageid, dialogUrl, dialogTitle, dropUrl, addFavoritUrl, replaceFavoritUrl) {
    $.ajax({
        type: "POST",
        url: dialogUrl,
        data: { pageId: pageid },
        datatype: "html",
        success: function (data) {
            $(fieldIdObj + "Dialog").html(data).dialog({
                title: dialogTitle,
                height: 350,
                width: 400,
                modal: true,
                resizable: true,
                buttons: {
                    "确定": function () {
                        var bValid = true;
                        if (bValid) {
                            var valstring = '';
                            var displaystring = '';
                            var treeObj = $.fn.zTree.getZTreeObj(treeIdObj);
                            var nodes = treeObj.getCheckedNodes(true);
                            $.each(nodes, function (n, vlaue) {
                                var node = nodes[n];
                                $.each(node, function (i, name) {
                                    if (i == 'id') {
                                        valstring = valstring + name + ',';
                                    }
                                    if (i == 'name') {
                                        displaystring = displaystring + name + ',';
                                    }
                                });
                            });
                            $(fieldIdObj + "Display").val(displaystring.substring(0, displaystring.length - 1));
                            $(fieldIdObj).val(valstring.substring(0, valstring.length - 1));
                            $(this).dialog("close");
                            setTimeout(function () { $(fieldIdObj).change(); }, 800);
                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    //allFields.val("").removeClass("ui-state-error");
                }
            });
        }
    });
}

//弹出多选的下拉框
function AppDropMultipleDiv(fieldIdObj, pageid, dropUrl,selectval) {
    $.ajax({
        type: "POST",
        url: dropUrl,
        data: { pageId: pageid, showCheckbox: 'true', selectVal: selectval, fieldIdObj: fieldIdObj },
        datatype: "html",
        success: function (data) {
            var offset = $(fieldIdObj + "Display").offset();
            $(fieldIdObj + "DropDiv").css({ left: offset.left - 210, top: offset.top - 60 });
            $(fieldIdObj + "DropDiv").width($(fieldIdObj + "Display").width()+2);
            $(fieldIdObj + "DropDiv").html(data);

        }
    });
}

///弹出消息框
function AppMessage(pageId, title, message, messageType, callback) {
    var data = "";
    if (messageType == "success") {
        data = '<p style="float:left"><span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 20px 0;"></span>' + message + '</p>';
    }
    else if (messageType == "error") {
        data = '<p style="float:left"><span class="ui-icon ui-icon-circle-close" style="float:left; margin:0 7px 20px 0;"></span>' + message + '</p>';
    }
    else if (messageType == "warning") {
        data = '<p style="float:left"><span class="ui-icon ui-icon-stop" style="float:left; margin:0 7px 20px 0;"></span>' + message + '</p>';
    }
    else {
        data = '<p style="float:left"><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>' + message + '</p>';
    }
    $('#MessageDialog' + pageId).html(data).dialog({
        title: title,
        height: 180,
        modal: true,
        resizable: false,
        buttons: {
            "确定": function () {
                $(this).dialog("close");
                callback();
            }
        }
    });
}

//客户端校验输入的数据
function CheckInputData(e, obj, charType, isPositive, maxLength) {
    if (charType == "Numerical") {
        if ($.browser.msie) {
            if ((e.keyCode > 47 && e.keyCode < 58) || (e.keyCode > 95 && e.keyCode < 106) || e.keyCode == 8 || e.keyCode == 110 || e.keyCode == 190 || e.keyCode == 189 || e.keyCode == 109) {
                if (isPositive) {
                    if (e.keyCode == 189 || e.keyCode == 109)
                        return false;
                }
            }
            else {
                return false;
            }
        } else {
            if ((e.which > 47 && e.which < 58) || (e.which > 95 && e.which < 106) || e.which == 8 || e.keyCode == 17 || e.which == 110 || e.which == 190 || e.which == 109) {
                if (isPositive) {
                    if (e.keyCode == 17 || e.which == 109)
                        return false;
                }
            }
            else {
                return false;
            }
        }
    }
    else if (charType == "Int") {
        if ($.browser.msie) {
            if ((e.keyCode > 47 && e.keyCode < 58) || (e.keyCode > 95 && e.keyCode < 106) || e.keyCode == 8 || e.keyCode == 189 || e.keyCode == 109) {
                if (isPositive) {
                    if (e.keyCode == 189 || e.keyCode == 109)
                        return false;
                }
            }
            else {
                return false;
            }
        } else {
            if ((e.which > 47 && e.which < 58) || (e.which > 95 && e.which < 106) || e.which == 8 || e.keyCode == 17 || e.which == 109) {
                if (isPositive) {
                    if (e.keyCode == 17 || e.which == 109)
                        return false;
                }
            }
            else {
                return false;
            }
        }
    }
    if (maxLength > 0) {
        if ($(obj).val().length >= maxLength) {
            if ($.browser.msie) {
                if (e.keyCode != 8) {
                    return false;
                }
            } else {
                if (e.which != 8) {
                    return false;
                }
            }
        }
    }
    return true;
}

//下载文件
function IEdownloadFile(fileName, contentOrPath) {
    var ifr = document.createElement('iframe');
    ifr.style.display = 'none';
    ifr.src = contentOrPath;
    document.body.appendChild(ifr);
    // 保存页面 -> 保存文件
    ifr.contentWindow.document.execCommand('SaveAs', false, fileName);
    document.body.removeChild(ifr);
}





