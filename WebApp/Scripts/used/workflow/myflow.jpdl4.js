(function ($) {
    var myflow = $.myflow;

    $.extend(true, myflow.config.rect, {
        attr: {
            r: 8,
            fill: '#F6F7FF',
            stroke: '#03689A',
            "stroke-width": 2
        }
    });

    $.extend(true, myflow.config.props.props, {
        name: { name: 'name', label: '名称', value: '流程管理', editor: function () { return new myflow.editors.inputEditor(); } }
    });

    

    $.extend(true, myflow.config.tools.states, {
        start: {
            showType: 'image',
            type: 'start',
            name: { text: '<<start>>' },
            text: { text: '开始' },
            img: { src: workflowImgPath + '/48/start_event_empty.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '开始' }
            }
        },
        end: { showType: 'image', type: 'end',
            name: { text: '<<end>>' },
            text: { text: '结束' },
            img: { src: workflowImgPath + '/48/end_event_terminate.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '结束' }
            }
        },
        fork: { showType: 'image', type: 'fork',
            name: { text: '<<fork>>' },
            text: { text: '分支' },
            img: { src: workflowImgPath + '/48/gateway_parallel.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '分支' }
            }
        },
        join: { showType: 'image', type: 'join',
            name: { text: '<<join>>' },
            text: { text: '合并' },
            img: { src: workflowImgPath + '/48/gateway_parallel.png', width: 48, height: 48 },
            attr: { width: 50, heigth: 50 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '合并' }
            }
        },
        task: { showType: 'text', type: 'task',
            name: { text: '<<task>>' },
            text: { text: '任务' },
            img: { src: workflowImgPath + '/48/task_empty.png', width: 48, height: 48 },
            props: {
                text: { name: 'text', label: '名称', value: '', editor: function () { return new myflow.editors.textEditor(); }, value: '任务' },
                department: { name: 'desc', label: '角色', value: '', editor: function () { return new myflow.editors.dropMultipleEditor(userGroupJson); } },
                assignee: { name: 'desc', label: '审批范围', value: '', editor: function () { return new myflow.editors.selectEditor(postJson); } }
            }
        }
    });
})(jQuery);