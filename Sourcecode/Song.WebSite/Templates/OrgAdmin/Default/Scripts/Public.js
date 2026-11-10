
(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/Fonts/icon.css',
        '/Utilities/Fonts/SvgIcons/svg.css',
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        //$dom.path() + 'styles/dropmenu.css',
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = [];
        //加载ElementUI
        arr2.push('/Utilities/ElementUi/index.js');
        //加载Sortable拖动
        arr2.push('/Utilities/Scripts/Sortable.min.js');
        arr2.push('/Utilities/Scripts/vuedraggable.min.js');
        window.$dom.componentjs(arr2, f);
    };
    //一些自定义组件
    window.$customize_componentjs = function (jsfile) {
        let arr2 = [];
        let webpath = $dom.path();
        //
        arr2.push('/Utilities/Components/btngroup.js');
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
        //查询面板与控件面板
        arr2.push('/Utilities/Components/query_panel.js');
        arr2.push('/Utilities/Components/panel.js');
        //专业
        arr2.push('/Utilities/Components/sbj_cascader.js');
        //日期区间选择器
        arr2.push('/Utilities/Components/date_range.js');
        return jsfile.concat(arr2);
    };
    //加载组件所需的javascript文件
    window.$ctrljs = function (f) {
        $dom.corejs(function () {
            var arr = ['pagebox', 'treemenu', 'dropmenu', 'tabs', 'verticalbar', 'timer', 'skins', 'login'];
            for (var t in arr) arr[t] = $dom.path() + 'Panel/Scripts/' + arr[t] + '.js';
            $dom.load.js(arr, f);
        }, ['/Utilities/Panel/Scripts/ctrls.js']);
    };
    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
       //如果参数没有按顺序传，自动调整，例如原本第一个是方法，第二个是资源路径，调用时写反了也可以
        var func = null, jsfile = [], cssfile = [];
        func = Array.from(arguments).find(v => typeof v === 'function') || null;
        //加载js与css
        for (let i = 0; i < arguments.length; i++) {
            const item = arguments[i];
            if (typeof item === 'string') {
                if (/\.js$/i.test(item)) jsfile.push(item);
                if (/\.css$/i.test(item)) cssfile.push(item);
            } else if (Array.isArray(item)) {
                jsfile.push(...item.filter(s => /\.js$/i.test(s)));
                cssfile.push(...item.filter(s => /\.css$/i.test(s)));
            }
        }
        $dom.load.css(cssfile);
        $dom.ready(function () {
            $dom.corejs(function () {
                $components(function () {
                    window.$init_load(() => $dom.componentjs(window.$customize_componentjs(jsfile), func));
                    //$dom.componentjs(window.$customize_componentjs(jsfile), window.$init_load(func));
                    window.$init_func();
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
            return txt.replace(regExp, (match, p1) => '<red>' + p1 + '</red>');
        };
    };
    //初始加载
    window.$init_load = function (func) {
        $api.bat(
            $api.cache('Platform/PlatInfo:60'),
            $api.get('Organization/Current'),
            $api.cache('Question/Types:99999')      //试题类型
        ).then(([platinfo, org, questypes]) => {
            //平台信息
            window.platinfo = platinfo.data.result;
            //机构信息与机构配置
            window.org = org.data.result;
            window.config = $api.organ(window.org).config;
            document.title += ' - ' + window.org.Org_PlatformName;
            //试题类型
            window.questypes = questypes.data.result;
        }).catch(err => console.error(err)).finally(() => func());
    };
})();
