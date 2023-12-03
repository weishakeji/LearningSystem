
(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/Fonts/icon.css',
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        //$dom.path() + 'styles/dropmenu.css',
        '/Utilities/katex/katex.min.css'
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = [];
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
        //TinyMCE编辑器
        arr2.push('/Utilities/TinyMCE/tinymce.js');
        arr2.push('/Utilities/TinyMCE/tinymce.vue.js');
        //mathjax，解析latex公式
        arr2.push('/Utilities/MathJax/tex-mml-chtml.js');
        arr2.push('/Utilities/MathJax/globalVariable.js');
        //头像组件
        arr2.push('/Utilities/Components/avatar.js');
        //加载状态组件
        arr2.push('/Utilities/Components/useicon.js');
        //查询面板
        arr2.push('/Utilities/Components/query_panel.js');
        //日期区间选择器
        arr2.push('/Utilities/Components/date_range.js');
        window.$dom.load.js(arr2, f);
    };
    //加载组件所需的javascript文件
    window.$ctrljs = function (f) {
        $dom.corejs(function () {
            var arr = ['pagebox', 'treemenu', 'dropmenu', 'tabs', 'verticalbar', 'timer', 'skins', 'login'];
            for (var t in arr) arr[t] = $dom.path() + 'Panel/Scripts/' + arr[t] + '.js';
            $dom.load.js(arr, f);
        },['/Utilities/Panel/Scripts/ctrls.js']);
    };
    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
        //如果参数没有按顺序传，自动调整，例如原本第一个是方法，第二个是资源路径，调用时写反了也可以
        var func = null, jsfile = [];
        for (let i = 0; i < arguments.length; i++) {
            if (arguments[i].constructor === Function) func = arguments[i];
            if (arguments[i] instanceof Array) {
                for (let j = 0; j < arguments[i].length; j++)
                    if (typeof arguments[i][j] === 'string') jsfile.push(arguments[i][j]);
            }
            if (typeof arguments[i] === 'string') jsfile.push(arguments[i]);
        }
        $dom.ready(function () {
            $dom.corejs(function () {
                $components(function () {
                    window.$init_func();
                    $dom.componentjs(jsfile, func);
                });
            });
        });
    };
    //加载完成后的初始化方法
    window.$init_func = function () {
        //设置ElementUI的一些参数
        Vue.prototype.$ELEMENT = { size: 'small', zIndex: 3000 };
        /*
        window.setTimeout(function () {
            //关闭按钮的事件
            $dom('button.el-button--close').click(function () {
                if (window.top.$pagebox) window.top.$pagebox.shut($dom.trim(window.name));
            });
        }, 1000);*/
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

        //渲染函数的方法，需要vue对象中updated中引用this.$mathjax()              
        //elements可以是一个DOM节点的数组(注意getXXXsByYYY的结果是collection，必须手动转为数组才行)
        Vue.prototype.$mathjax = function (elements) {
            // 判断是否初始配置，若⽆则配置
            if (window.globalVariable.isMathjaxConfig)
                window.globalVariable.initMathjaxConfig();
            window.globalVariable.TypeSet(elements);
        };
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
            if ($dom.ismobi()) {
                vant.Dialog ? vant.Dialog.alert({ message: txt }) : window.alert_base(txt);
            } else {
                Vue.prototype.$alert ? Vue.prototype.$alert(txt) : window.alert_base(txt);
            }
        };
    };
})();
