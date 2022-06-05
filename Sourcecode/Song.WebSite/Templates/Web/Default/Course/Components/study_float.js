//飘浮信息，防录屏
Vue.component('study_float', {
    props: ['account', 'tag'],
    data: function () {
        return {

        }
    },
    watch: {
        'islogin': function (nv, ov) {
            if (nv) {
                if (window.study_float) return;
                window.study_float = window.setInterval(function () {
                    var items = document.querySelectorAll(".study_float");
                    if (items == null || items.length < 1) return;
                    for (let i = 0; i < items.length; i++) {
                        const acc = items[i];
                        var parent=document.getElementById("contextArea");
                        var pHeight = parent.offsetHeight;
                        var pWidth = parent.offsetWidth;
                        if (pHeight == 0 || pWidth == 0) return;
                        //移动速度
                        var acctop = acc.getAttribute("acctop") != null ? Number(acc.getAttribute("acctop")) : Math.ceil(Math.random() * 100) / 10;
                        var accleft = acc.getAttribute("accleft") != null ? Number(acc.getAttribute("accleft")) : Math.ceil(Math.random() * 100) / 10;
                        //获取当前坐标
                        var top = Number(acc.style.top.replace('px', ''));
                        var left = Number(acc.style.left.replace('px', ''));
                        //转向            
                        if (top < 0 || top > pHeight - acc.offsetHeight) acctop = -acctop;
                        if (left < 0 || left > pWidth - acc.offsetWidth) accleft = -accleft;
                        //移动 
                        acc.style.top = (top < 0 ? 0 : (top > pHeight - acc.offsetHeight ? pHeight - acc.offsetHeight : top + acctop)) + "px";
                        acc.style.left = (left < 0 ? 0 : (left > pWidth - acc.offsetWidth ? pWidth - acc.offsetWidth : left + accleft)) + "px";
                        //记录信息到元素上
                        acc.setAttribute("acctop", acctop);
                        acc.setAttribute("accleft", accleft);
                    }
                }, 200);
            } else {
                clearInterval(window.study_float);
            }
        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_float.css';
        $dom.load.css([css]);
    },
    methods: {},
    template: `<div class="study_float" :tag="tag" title="仅自己可见" v-if="islogin">    
        {{account.Ac_Name}} {{account.Ac_MobiTel1}}
    </div>`
});