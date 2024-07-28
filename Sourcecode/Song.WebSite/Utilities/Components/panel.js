//控件面板
$dom.load.css(['/Utilities/Components/Styles/panel.css']);
Vue.component('panel', {
    //disabled: 是否禁用 
    props: ['disabled'],
    data: function () {
        return {           
            def: {
               
            },
            loading: false
        }
    },
    watch: {
      
    },
    computed: {
       

    },
    created: function () {
      
    },
    methods: {
      
    },
    template: `<div :class="{'ws_panel':true,'disabled':!disabled,'ws_disabled':!disabled}">      
        <slot></slot>      
    </div>`
});
