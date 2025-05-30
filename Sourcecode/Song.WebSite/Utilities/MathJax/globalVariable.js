window.globalVariable = {
    isMathjaxConfig: false,//用于标识是否配置
    initMathjaxConfig: function () {
        if (!window.MathJax) {
            return;
        }
        window.MathJax.Hub.Config({
            showProcessingMessages: false, //关闭js加载过程信息
            messageStyle: "none", //不显示信息
            jax: ["input/TeX", "output/HTML-CSS"],
            tex2jax: {
                packages: ['base', 'ams'],  // 显式声明需要加载的 TeX 包
                inlineMath: [["$", "$"], ["\\(", "\\)"]], //行内公式选择符
                displayMath: [["$$", "$$"], ["\\[", "\\]"]], //段内公式选择符
                skipTags: ["script", "noscript", "style", "textarea", "pre", "code", "a"] //避开某些标签
            },
            "HTML-CSS": {
                availableFonts: ["STIX", "TeX"], //可选字体
                showMathMenu: false //关闭右击菜单显示
            }
        });
        isMathjaxConfig = true; //配置完成，改为true
    },
    //渲染函数，调用时会渲染指定节点elements，如果没有指定节点，渲染页面上所有公式
    //elements可以是一个DOM节点的数组(注意getXXXsByYYY的结果是collection，必须手动转为数组才行)
    TypeSet: async function (elements) {
        if (!window.MathJax) return;
        return window.MathJax.startup.promise.then(() => {
            return window.MathJax.typesetPromise(elements)
        }).catch((err) => console.log('Typeset failed: ' + err.message));
    }
}
