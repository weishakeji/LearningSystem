tinymce.PluginManager.add('kityformula-editor', function (editor, url) {

    var baseURL = tinymce.baseURL + '/plugins/kityformula-editor/kityFormula.html';

    editor.on('dblclick', function () {
        var sel = editor.selection.getContent();
        var path = /\<img(.*?)src="data:image\/png;base64,[A-Za-z0-9+/=]*"(.*?)data-latex="(.*?)" \/>/g;
        var path2 = /data-latex="(.*?)"/g;

        if (sel.search(path) == 0) {
            sel.replace(path2, function ($0, $1) {
                var param = encodeURIComponent($1);
                openDialog(param);
                return $0;
            });
        };
    });

    var openDialog = function (param) {
        return editor.windowManager.openUrl({
            title: '插入公式',
            size: 'large',
            width: 785,
            height: 475,
            url: param ? baseURL + "?c=" + param : baseURL,
            buttons: [
                {
                    type: 'cancel',
                    text: 'Close'
                },
                {
                    type: 'custom',
                    text: 'Save',
                    name: 'save',
                    primary: true
                },
            ],
            onAction: function (api, details) {
                switch (details.name) {
                    case 'save':
                        api.sendMessage("save");
                        break;
                    default:
                        break;
                };
            }
        });
    };

    editor.ui.registry.getAll().icons.formula || editor.ui.registry.addIcon('formula', '<svg viewBox="0 0 30 35" xmlns="http://www.w3.org/2000/svg" version="1.1" width="15" height="15"><path d="M0.672,33.603c-0.432,0-0.648,0-0.648-0.264c0-0.024,0-0.144,0.24-0.432l12.433-14.569L0,0.96c0-0.264,0-0.72,0.024-0.792   C0.096,0.024,0.12,0,0.672,0h28.371l2.904,6.745h-0.6C30.531,4.8,28.898,3.72,28.298,3.336c-1.896-1.2-3.984-1.608-5.28-1.8   c-0.216-0.048-2.4-0.384-5.617-0.384H4.248l11.185,15.289c0.168,0.24,0.168,0.312,0.168,0.36c0,0.12-0.048,0.192-0.216,0.384   L3.168,31.515h14.474c4.608,0,6.96-0.624,7.464-0.744c2.76-0.72,5.305-2.352,6.241-4.848h0.6l-2.904,7.681H0.672z" fill="black"></path></svg>');
    editor.ui.registry.addButton('kityformula-editor', {
        icon: 'formula',
        tooltip: '插入公式',
        onAction: function () {
            openDialog();
        }
    });
    editor.ui.registry.addMenuItem('kityformula-editor', {
        text: '公式',
        onAction: function () {
            openDialog();
        }
    });
    return {
        getMetadata: function () {
            return {
                name: "公式",
                url: "http://hgcserver.gitee.io",
            };
        }
    };
});