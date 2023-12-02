(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        $dom.path() + 'styles/dropmenu.css',
        //'/Utilities/katex/katex.min.css',      
        '/Utilities/Fonts/icon.css'
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        //电脑端拖动与手式拖动的js库,以及其它
        var arr2 = ['Sortable.min', 'vuedraggable.min', 'hammer.min', 'vue-touch'];
        for (var t in arr2) arr2[t] = '/Utilities/Scripts/' + arr2[t] + '.js';
        let webpath = $dom.path();    //
        //加载ElementUI
        arr2.push('/Utilities/ElementUi/index.js');
        arr2.push('/Utilities/Components/btngroup.js');
        arr2.push(webpath + 'scripts/dropmenu.js');
        arr2.push('/Utilities/TinyMCE/tinymce.js');
        //页面的头部和底部
        arr2.push(webpath + 'Components/page_header.js');
        arr2.push(webpath + 'Components/page_footer.js');
        arr2.push(webpath + 'Components/course.js');
        //mathjax，解析latex公式
        arr2.push('/Utilities/MathJax/tex-mml-chtml.js');
        arr2.push('/Utilities/MathJax/globalVariable.js');
        //未登录的样式
        arr2.push(webpath + 'Components/nologin.js');
        window.$dom.load.js(arr2, f);
    };
    //加载组件所需的javascript文件
    $dom.ctrljs = function (f) {
        $dom.corejs(function () {
            var arr = ['pagebox', 'treemenu', 'tabs', 'verticalbar', 'timer', 'skins', 'login'];
            for (var t in arr) arr[t] = '/Utilities/Panel/Scripts/' + arr[t] + '.js';
            window.$dom.load.js(arr, f);
        }, '/Utilities/Panel/Scripts/ctrls.js');
    };
    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
        var route = $dom.route().toLowerCase();
        //如果设备是手机端，转向手机页面
        if (($dom.ismobi() || $dom.isWeixinApp() || $dom.ispad()) && route.indexOf('/web/') > -1) {
            var search = window.location.search;
            var href = route.replace('/web/', '/mobi/');
            var pathname = window.location.pathname;
            var dot = pathname.indexOf('.') > -1 ? pathname.substring(pathname.lastIndexOf('.')) : '';
            window.location.href = href + dot + search;
            return;
        }
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
            //$dom.corejs(function () {
            $dom.ctrljs(function () {
                $components(function () {
                    window.$init_func();
                    $dom.componentjs(jsfile, func);
                });
            });
            //});
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
        //常用地址
        Vue.prototype.commonaddr = function (key) {
            var urls = {
                'signin': '/web/sign/in',      //登录地址
                'myself': '/mobi/account/myself'        //个人中心
            };
            if (urls[key] == undefined) return '';
            return $api.url.set(urls[key], {
                'referrer': encodeURIComponent(location.href)
            });
        };
        //重构alert
        window.alert_base = window.alert;
        window.alert = function (txt, title) {
            //手机端
            if ($dom.ismobi()) {
                vant.Dialog ? vant.Dialog.alert({ message: txt, title: title }) : window.alert_base(txt);
            } else {
                Vue.prototype.$alert ? Vue.prototype.$alert(txt, title) : window.alert_base(txt);
            }
        };
        //重构确认
        window.confirm_base = window.confirm;
        window.confirm = function (title, msg, evtConfirm, evtCancel) {
            //手机端
            if ($dom.ismobi()) {
                if (vant.Dialog) {
                    vant.Dialog.confirm({ title: title, message: msg, })
                        .then(evtConfirm != null ? evtConfirm : () => { })
                        .catch(evtCancel != null ? evtCancel : () => { });
                }
            } else {
                if (Vue.prototype.$confirm) {
                    Vue.prototype.$confirm(msg, title, {
                        dangerouslyUseHTMLString: true, type: 'warning',
                        confirmButtonText: '确定', cancelButtonText: '取消'
                    }).then(evtConfirm != null ? evtConfirm : () => { })
                        .catch(evtCancel != null ? evtCancel : () => { });
                }
            }
        };
    };
})();
