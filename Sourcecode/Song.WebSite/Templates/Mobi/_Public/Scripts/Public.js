(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/Vant/Vant.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        '/Utilities/Fonts/icon.css'
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = new Array();
        //加载Vant
        arr2.push('/Utilities/Vant/vant.min.js');
        //mathjax，解析latex公式
        arr2.push('/Utilities/MathJax/globalVariable.js');
        arr2.push('/Utilities/MathJax/tex-mml-chtml.js');
        //加载vue组件
        //arr2.push($dom.path() + 'Components/footer_menu.js');
        //arr2.push($dom.path() + 'Components/aside_menu.js');
        //通用组件，用于获取学员登录，机构信息等
        //arr2.push($dom.path() + 'Components/generic.js');
        window.$dom.load.js(arr2, f);
    };
    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
        var route = $dom.route().toLowerCase();
        //如果设备不是手机端，转向web端页面
        if (!($dom.ismobi() || $dom.isWeixinApp() || $dom.ispad()) && route.indexOf('/mobi/') > -1) {
            var search = window.location.search;
            var href = route.replace('/mobi/', '/web/');
            var pathname = window.location.pathname;
            var dot = pathname.indexOf('.') > -1 ? pathname.substring(pathname.lastIndexOf('.')) : '';
            window.navigateTo(href + dot + search);
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
        window.setTimeout(function () {
            //关闭按钮的事件
            $dom('button.el-button--close').click(function () {
                if (window.top.$pagebox) window.top.$pagebox.shut($dom.trim(window.name));
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
                'signin': '/mobi/sign/in',      //登录地址
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
                return vant.Dialog ? vant.Dialog.alert({ message: txt, title: title }) : window.alert_base(txt);
            } else {
                return Vue.prototype.$alert ? Vue.prototype.$alert(txt, title) : window.alert_base(txt);
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
    //重构一些方法
    //页面跳转
    window.navigateTo = function (url) {
        //如果处在微信小程序中
        if ($dom.isWeixinApp()) {
            //alert(wx);
            //wx.navigateTo({ url: url });
            window.location.href = url;
        } else {
            window.location.href = url;
        }
    }

})();


