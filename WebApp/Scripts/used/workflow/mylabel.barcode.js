(function ($) {
    var mylabel = $.mylabel;

    $.extend(true, mylabel.config.rect, {
        attr: {
            r: 8,
            fill: '#F6F7FF',
            stroke: '#03689A',
            "stroke-width": 2
        }
    });

    $.extend(true, mylabel.config.props.props, {
        name: { name: 'name', label: '名称', value: '条码样式设置', editor: function () { return new mylabel.editors.inputEditor(); } }
    });



    $.extend(true, mylabel.config.tools.states, {
        rectangle: {
            showType: 'text',
            type: 'rectangle',
            name: { text: '<<rectangle>>' },
            text: { text: '' },
            img: { src: workflowImgPath + '/48/task_empty.png', width: 48, height: 48 },
            attr: { width: 200, heigth: 200 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new mylabel.editors.textEditor(); }, value: '' },
                x: { name: 'attr', label: 'x', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                y: { name: 'attr', label: 'y', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                width: { name: 'attr', label: '宽', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                heigth: { name: 'attr', label: '高', value: '', editor: function () { return new mylabel.editors.inputEditor(); } }
            }
        },
        textField: { showType: 'text', type: 'textField',
            name: { text: '<<textField>>' },
            text: { text: '字段' },
            img: { src: workflowImgPath + '/48/text.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new mylabel.editors.textEditor(); }, value: '字段' },
                refField: { name: 'desc', label: '关联字段', value: '', editor: function () { return new mylabel.editors.selectEditor(textFieldJson); } },
                x: { name: 'attr', label: 'x', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                y: { name: 'attr', label: 'y', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                width: { name: 'attr', label: '宽', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                heigth: { name: 'attr', label: '高', value: '', editor: function () { return new mylabel.editors.inputEditor(); } }
            }
        },
        barcode: { showType: 'text', type: 'barcode',
            name: { text: '<<barcode>>' },
            text: { text: '条码' },
            img: { src: workflowImgPath + '/48/barcode.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new mylabel.editors.textEditor(); }, value: '条码' },
                barcodeType: { name: 'desc', label: '条码类型', value: '', editor: function () { return new mylabel.editors.selectEditor(barcodeTypeJson); } },
                refField: { name: 'desc', label: '关联字段', value: '', editor: function () { return new mylabel.editors.selectEditor(barcodeFieldJson); } },
                barWidth: { name: 'attr', label: '条码宽度', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                x: { name: 'attr', label: 'x', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                y: { name: 'attr', label: 'y', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                width: { name: 'attr', label: '宽', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                heigth: { name: 'attr', label: '高', value: '', editor: function () { return new mylabel.editors.inputEditor(); } }
            }
        },
        imageField: { showType: 'text', type: 'imageField',
            name: { text: '<<imageField>>' },
            text: { text: '图片' },
            img: { src: workflowImgPath + '/48/picture.png', width: 48, height: 48, border: 1 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new mylabel.editors.textEditor(); }, value: '图片' },
                refField: { name: 'desc', label: '关联字段', value: '', editor: function () { return new mylabel.editors.selectEditor(imageFieldJson); } },
                x: { name: 'attr', label: 'x', value: '', editor: function () { return new mylabel.editors.inputEditor(); }},
                y: { name: 'attr', label: 'y', value: '', editor: function () { return new mylabel.editors.inputEditor(); }},
                width: { name: 'attr', label: '宽', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                heigth: { name: 'attr', label: '高', value: '', editor: function () { return new mylabel.editors.inputEditor(); } }

            }
        },
         staticField: { showType: 'text', type: 'staticField',
            name: { text: '<<staticField>>' },
            text: { text: '文本' },
            img: { src: workflowImgPath + '/48/text.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new mylabel.editors.textEditor(); }, value: '文本' },
                refField: { name: 'desc', label: '备注', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                x: { name: 'attr', label: 'x', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                y: { name: 'attr', label: 'y', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                width: { name: 'attr', label: '宽', value: '', editor: function () { return new mylabel.editors.inputEditor(); } },
                heigth: { name: 'attr', label: '高', value: '', editor: function () { return new mylabel.editors.inputEditor(); } }
            }
        }
    });
})(jQuery);