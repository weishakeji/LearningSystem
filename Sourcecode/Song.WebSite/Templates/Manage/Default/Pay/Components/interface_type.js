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
                current = this.navigation.find(item => item.pattern.toLowerCase() == view);
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
        iscurrent: function (item) {
            var view = $dom('meta[view]').attr("view").toLowerCase();
            var pattern = item.pattern.toLowerCase();
            return view == pattern;
        },
        //获取支付类型，例如支付宝、微信
        getpattern: function (pattern) {
            if (pattern.indexOf('支付宝') > -1) return 'zhifubao';
            if (pattern.indexOf('微信') > -1) return 'weixin';
        }
    },
    template: `<div>
        <dl class="interface_type" v-if="layout_value=='list'">   
            <dt>请选择支付接口的类型</dt>     
            <dd v-for="(item,i) in navigation" :class="{'current':iscurrent(item)}">
                <el-button type="primary" plain @click="gonavi(item)" >
                    <icon v-html="item.icon" :class="getpattern(item.name)"></icon>
                    <span>{{item.name}}</span>
                </el-button>
            </dd>
        </dl> 
        <span v-if="layout_value=='single'">
            <el-tag type="success" size="medium"> {{current.name}}</el-tag>
       
        </span>
    </div>`
});