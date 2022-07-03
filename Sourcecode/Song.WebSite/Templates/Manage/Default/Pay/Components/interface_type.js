//支付接口的类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/interface_type.css']);
Vue.component('interface_type', {
    //接口的对象实体
    props: ['entity', 'layout'],
    data: function () {
        return {
            //导航
            navigation: [
                { pattern: 'alipaywap', name: '支付宝手机支付', icon: '&#xe602', scene: 'alipay,h5' },
                { pattern: 'alipayweb', name: '支付宝网页直付', icon: '&#xe602', scene: 'alipay,native' },
                { pattern: 'weixinpubpay', name: '微信公众号支付', icon: '&#xe832', scene: 'weixin,public' },
                { pattern: 'weixinnativepay', name: '微信扫码支付', icon: '&#xe832', scene: 'weixin,native' },
                { pattern: 'WeixinAppPay', name: '微信小程序支付', icon: '&#xe832', scene: 'weixin,mini' },
                { pattern: 'WeixinH5Pay', name: '微信Html5支付', icon: '&#xe832', scene: 'weixin,h5' },
            ],
            //当前导航项
            //current:{}
        }
    },
    watch: {
        'entity': {
            handler: function (nv, ov) {
                if (nv == null) return;
                var current = this.navigation.find(item => item.name == nv.Pai_Pattern);
                if (current != null && this.layout_value == 'list') this.gonavi(current);
            }, immediate: true
        }
    },
    computed: {
        //布局的值
        'layout_value': function () {
            if (this.layout == 'list') return 'list';
            return 'single';
        },
        //当前导航项
        'current': function () {
            var current = null;
            if (JSON.stringify(this.entity) != '{}' && this.entity != null) {
                current = this.navigation.find(item => item.name == this.entity.Pai_Pattern);
            }
            if (current == null) {
                var view = $dom('meta[view]').attr("view").toLowerCase();
                current = this.navigation.find(item => item.pattern == view);
            }          
            return current == null ? {} : current;
        }
    },
    mounted: function () {
console.log(this.layout_value);
    },
    methods: {
        //跳转
        gonavi: function (item) {
            var url = item.pattern.toLowerCase();
            url = $api.url.set(url, {
                'id': $api.querystring('id'),
                'scene': item.scene
            });
            window.location.href = url;
        },
        //是否是当前选项
        current: function (item) {
            var view = $dom('meta[view]').attr("view").toLowerCase();
            var pattern = item.pattern.toLowerCase();
            return view == pattern;
        }
    },
    template: `<div>
        <dl class="interface_type" v-if="layout_value=='list'">   
            <dt>支付方式</dt>     
            <dd v-for="(item,i) in navigation" @click="gonavi(item)" :class="{'current':current(item)}">
                <icon v-html="item.icon"></icon>
                <span>{{item.name}}</span>
            </dd>
        </dl> 
        <span v-if="layout_value=='single'">
            <el-tag type="success" size="medium"> {{current.name}}</el-tag>
       
        </span>
    </div>`
});