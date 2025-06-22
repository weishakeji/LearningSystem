(function () {
    //加载主要的Css文件
    $dom.load.css([
        '/Utilities/styles/public.css',
        $dom.path() + 'styles/public.css',
        //$dom.path() + 'styles/dropmenu.css',  
        '/Utilities/Fonts/icon.css',
        '/Utilities/Fonts/SvgIcons/svg.css',
    ], $dom.selfresource);


    //加载必要的资源完成
    //f:加载完成要执行的方法
    //source:要加载的资源
    window.$ready = function (f, source) {
        //要加载的js 
        var arr = ['vue.min', 'polyfill.min', 'axios_min', 'api'];
        var webpath = $dom.path();    //
        for (var t in arr) arr[t] = webpath + 'CourseChat/Scripts/' + arr[t] + '.js';

        $dom.load.js(arr, f);
    };
})();
