(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/ElementUi/index.css',
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        '/Utilities/Fonts/icon.css',
        '/Utilities/Fonts/SvgIcons/svg.css',
    ], $dom.selfresource);
    //加载相关组件
    window.$components = function (f) {
        var arr2 = new Array();
        //加载ElementUI
        arr2.push('/Utilities/ElementUi/index.js');
        //加载Sortable拖动
        arr2.push('/Utilities/Scripts/Sortable.min.js');
        arr2.push('/Utilities/Scripts/vuedraggable.min.js');
        //编辑器
        arr2.push('/Utilities/TinyMCE/tinymce.js');
        arr2.push('/Utilities/TinyMCE/tinymce.vue.js');
        window.$dom.componentjs(arr2, f);
    };
    window.$customize_componentjs = function (jsfile) {
        let arr = [];
        let webpath = $dom.path();    //
        arr.push('/Utilities/Components/btngroup.js');
        //加载图标选择组件
        arr.push('/Utilities/Components/icons.js');
        //图片上传组件
        arr.push('/Utilities/Components/upload-img.js');
        arr.push('/Utilities/Components/upload-file.js');
        return jsfile.concat(arr);
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
                    window.$init_load(() => $dom.componentjs(window.$customize_componentjs(jsfile), func));
                    window.$init_func();                    
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
            return txt.replace(regExp, function (match, p1) {
                return '<red>' + p1 + '</red>';
            });
        };
    };
    //初始加载
    window.$init_load = function (func) {
        $api.bat(
            $api.cache('Platform/PlatInfo:60'),
            $api.get('Organization/Current')
        ).then(([platinfo, org]) => {
            //平台信息
            window.platinfo = platinfo.data.result;
            //机构信息与机构配置
            window.org = org.data.result;
            window.config = $api.organ(window.org).config;
            document.title += ' - ' + window.org.Org_PlatformName;
        }).catch(err => console.error(err)).finally(() => func());
    };
})();

