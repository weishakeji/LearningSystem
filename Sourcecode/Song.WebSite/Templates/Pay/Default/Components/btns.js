
// 按钮组(返回按钮)
Vue.component('btns', {
    props: [],
    data: function () {
        return {
          
        }
    },
    watch: {},
    computed: {
       
    },
    mounted: function () { },
    methods: {       
    }, 
    template: `<div class='btns'>
            <a href="#" style="font-size: 22px;" onclick="window.history.back(-1);">
                <icon>&#xe727</icon>
            </a>
            <a href="/">
                <icon>&#xa020</icon>
            </a>
        </div>`
});
