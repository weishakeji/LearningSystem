//支付接口的类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/interface_type.css']);
Vue.component('interface_type', {
    //entity:接口的对象实体
    //layout:布局方式，list为列表，single显示单个
    props: ['entity', 'layout'],
    data: function () {
        return {
            //导航，scene：使用场景
            navigation: [
                {
                    pattern: 'alipaywap', platform: 'mobi', name: '支付宝手机支付',
                    icon: '&#xe602', enable: false, scene: 'alipay,h5', tips: '手机端网页支付'
                },
                {
                    pattern: 'alipayweb', platform: 'web', name: '支付宝网页直付',
                    icon: '&#xe602', enable: false, scene: 'alipay,native', tips: '电脑端网页支付'
                },
                {
                    pattern: 'weixinpubpay', platform: 'mobi', name: '微信公众号支付',
                    icon: '&#xe832', enable: true, scene: 'weixin,public', tips: '在微信公众号中使用'
                },
                {
                    pattern: 'weixinnativepay', platform: 'web', name: '微信扫码支付',
                    icon: '&#xe832', enable: true, scene: 'weixin,native', tips: '电脑端网页中使用'
                },
                {
                    pattern: 'WeixinAppPay', platform: 'mobi', name: '微信小程序支付',
                    icon: '&#xe832', enable: false, scene: 'weixin,mini', tips: '在微信小程序中使用'
                },
                {
                    pattern: 'WeixinH5Pay', platform: 'mobi', name: '微信Html5支付',
                    icon: '&#xe832', enable: true, scene: 'weixin,h5', tips: '手机端网页支付'
                },
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
                if (current != null && this.layout_value == 'list') this.gonavi(current, true);
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
        //跳转,item是支付接口项,compel是否强制跳转，默认item.enable为false不跳转
        gonavi: function (item, compel) {
            if (!compel && !item.enable) return;
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
            <dd v-for="(item,i) in navigation" :class="{'current':iscurrent(item)}" @click="gonavi(item,false)" :disabled="!item.enable">
                <div>
                    <img :src="'/pay/images/'+item.name+'.png'"/>
                    <span>{{item.name}}</span>     
                </div> 
                <div v-html="item.tips">提示信息</div>
            </dd>
        </dl> 
        <span v-if="layout_value=='single'">
            <el-tag type="success" size="medium"> {{current.name}}</el-tag>       
        </span>
    </div>`
});