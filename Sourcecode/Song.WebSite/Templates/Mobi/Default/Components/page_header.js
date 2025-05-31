
//顶部导航
Vue.component('page_header', {
    props: ['title', 'icon', 'logo', 'fresh', 'home', 'menu'],
    data: function () {
        return {

        }
    },
    watch: {
        'title': {
            handler(nv, ov) {
                document.title = nv;
            },
            immediate: true
        }
    },
    computed: {
        //是否登录
        islogin: t => !$api.isnull(t.account),
        //是否显示按钮
        showbtn: t => {
            return t.fresh || t.home || t.menu;
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Components/Styles/page_header.css']);
    },
    methods: {
        //刷新页面
        btnFresh: () => window.location.reload(),
        //返回上一页
        btnback: () => window.history.go(-1),
        //返回主页
        btnHome: () => window.navigateTo("/mobi/"),
        //菜单按钮,如果有则触发
        btnMenu: function () { this.$emit('menu'); }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `<div  class="page_header">
           <icon class="goback" @click="btnback">&#xe72a</icon>         
           <div class="header_title"> 
                <img class="logo" :src="logo" v-if="logo">
                <span :icon="icon" v-else v-html="title"></span>  
                <slot></slot>
           </div>          
           <div v-if="showbtn" class="page_header_btns">
                <icon class="fresh" @click="btnFresh" v-if="fresh">&#xe694</icon>     
                <icon class="home" @click="btnHome" v-if="home">&#xa020</icon>  
                <icon class="menu" @click="btnMenu" v-if="menu">&#xa01a</icon>    
           </div>  
        </div>`
});
