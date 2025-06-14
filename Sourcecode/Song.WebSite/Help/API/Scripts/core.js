
(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/Fonts/icon.css',
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        '/Utilities/prism/prism.css',        //高亮显示
        '/Utilities/jsontreejs/jsonTree.css',
        '/Utilities/jsontreejs/themes/light/jsontree.js.light.theme.css'
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = [
            '/Utilities/ElementUi/index.js',
            '/Utilities/jsontreejs/jsonTree.js',
            '/Utilities/prism/prism.js', //高亮显示
           ];
        window.$dom.load.js(arr2, f);
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
        //Vue.prototype.$ELEMENT = { size: 'small', zIndex: 3000 };
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
            return txt.replace(regExp, (match, p1) => '<red>' + p1 + '</red>');
        };
    };
})();
