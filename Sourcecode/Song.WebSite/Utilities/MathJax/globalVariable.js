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
    TypeSet: async function (elementId) {
        if (!window.MathJax) {
            return
        }
        window.MathJax.startup.promise = window.MathJax.startup.promise
            .then(() => {
                return window.MathJax.typesetPromise(elementId)
            })
            .catch((err) => console.log('Typeset failed: ' + err.message))
        return window.MathJax.startup.promise
    }
}
