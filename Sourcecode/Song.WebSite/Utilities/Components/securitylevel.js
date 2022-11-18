// 密码安全等校验
$dom.load.css(['/Utilities/Components/Styles/securitylevel.css']);
Vue.component('securitylevel', {
    //password:需要验证的密码
    //minlength:密码最小长度
    //level:限定的安全等，低于这个等级则报错
    props: ['password','minlength','level'],
    data: function () {
        return {
          
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
    template: `<div class="securitylevel">

    </div>`
});
