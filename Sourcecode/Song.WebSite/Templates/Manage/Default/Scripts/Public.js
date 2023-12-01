
(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        '/Utilities/Fonts/icon.css'
    ]);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = new Array();
        //加载ElementUI
        arr2.push('/Utilities/ElementUi/index.js');
        arr2.push('/Utilities/Components/btngroup.js');
        //加载Sortable拖动
        arr2.push('/Utilities/Scripts/Sortable.min.js');
        arr2.push('/Utilities/Scripts/vuedraggable.min.js');
        //加载图标选择组件
        arr2.push('/Utilities/Components/icons.js');
        //图片上传组件
        arr2.push('/Utilities/Components/upload-img.js');
        arr2.push('/Utilities/Components/upload-file.js');
        //编辑器
        arr2.push('/Utilities/TinyMCE/tinymce.js');
        arr2.push('/Utilities/TinyMCE/tinymce.vue.js');
        //查询面板
        arr2.push('/Utilities/Components/query_panel.js');
        $dom.load.js(arr2, f);
    };
    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
        var func = null, jsfile = null;
        for (let i = 0; i < arguments.length; i++) {
            if (arguments[i].constructor === Function) func = arguments[i];
            if (arguments[i] instanceof Array) jsfile = arguments[i];
        }
        $dom.ready(function () {
            $dom.corejs(function () {
                $components(function () {
                    window.$init_func();
                    if (jsfile != null) {
                        //如果引用的js不是绝对路径，则默认取当前默认库的根路径
                        for (var i = 0; i < jsfile.length; i++) {
                            if (jsfile[i].substring(0, 1) == "/") continue;
                            if (jsfile[i].length >= 7 && jsfile[i].substring(0, 7).toLowerCase() == "http://") continue;
                            if (jsfile[i].length >= 8 && jsfile[i].substring(0, 8).toLowerCase() == "https://") continue;
                            jsfile[i] = webdom.pagepath() + jsfile[i];
                        }
                        window.$dom.load.js(jsfile, func);
                    } else if (func != null) func();
                });
            });

        });
    };
    //加载完成后的初始化方法
    window.$init_func = function () {
        //设置ElementUI的一些参数
        Vue.prototype.$ELEMENT = { size: 'small', zIndex: 3000 };
        //关闭按钮的事件
        window.closebtn_event_count = 100;
        window.closebtn_event = window.setInterval(function () {
            if (window.closebtn_event_count-- < 0) window.clearInterval(window.closebtn_event);
            let btns = $dom('button.el-button--close:not([event_close])');
            btns.each(function () {
                let btn = $dom(this);
                if (btn.attr('event_close') == null || btn.attr('event_close') == '') {
                    btn.attr('event_close', true);
                    btn.click(function () {
                        window.top.$pagebox.shut($dom.trim(window.name));
                    });
                }
            });
        }, 300);
        //全屏的预载效果
        Vue.prototype.$fulloading = function () {
            return this.$loading({
                lock: true,
                text: '正在处理...',
                spinner: 'el-icon-loading',
                background: 'rgba(255, 255, 255, 0.5)'
            });
        };
        //将查询结果高亮显示
        Vue.prototype.showsearch = function (txt, search) {
            if (txt == null || txt == '') return '';
            if (search == null || search == '') return txt;
            var regExp = new RegExp('(' + search + ')', 'ig');
            return txt.replace(regExp, `<red>$1</red>`);
        };
        //重构alert
        window.alert_base = window.alert;
        window.alert = function (txt) {
            //手机端
            if (webdom.ismobi()) {
                vant.Dialog ? vant.Dialog.alert({ message: txt }) : window.alert_base(txt);
            } else {
                Vue.prototype.$alert ? Vue.prototype.$alert(txt) : window.alert_base(txt);
            }
        };
    }
})();


