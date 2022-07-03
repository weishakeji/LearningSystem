//支付接口的类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/interface_type.css']);
Vue.component('interface_type', {
     //接口的对象实体
    props: ['entity'],
    data: function () {
        return {
            //导航
            navigation: [
                { id: 'alipaywap', name: '支付宝手机支付', icon: '&#xe602',  scene: 'alipay,h5'},
                { id: 'alipayweb', name: '支付宝网页直付', icon: '&#xe602',  scene: 'alipay,native' },
                { id: 'weixinpubpay', name: '微信公众号支付', icon: '&#xe832', scene: 'weixin,public' },            
                { id: 'weixinnativepay', name: '微信扫码支付', icon: '&#xe832',  scene: 'weixin,native'},
                { id: 'WeixinAppPay', name: '微信小程序支付', icon: '&#xe832',  scene: 'weixin,mini'},
                { id: 'WeixinH5Pay', name: '微信Html5支付', icon: '&#xe832',  scene: 'weixin,h5'},
            ],
        
        }
    },
    watch: {
        
    },
    mounted: function () {

    },
    methods: {     
        
    },
    template: `<dl class="interface_type">   
        <dt>支付方式</dt>     
        <dd v-for="(item,i) in navigation">
            <icon v-html="item.icon"></icon>
            <span>{{item.name}}</span>
        </dd>
    </dl> `
});