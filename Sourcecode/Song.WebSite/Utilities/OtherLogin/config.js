// 第三方登录的配置项
$dom.load.css(['/Utilities/OtherLogin/Styles/config.css']);
Vue.component('config', {
    props: ['selected'],
    data: function () {
        return {
            items: [
                { name: 'QQ登录', tag: 'qq', icon: 'e82a', size: 16, width: 700, height: 400, obj: {} },
                { name: '微信登录', tag: 'weixin', icon: 'e730', size: 18, width: 700, height: 500, obj: {} },
                { name: '金碟.云之家', tag: 'yunzhijia', icon: 'e726', size: 18, width: 600, height: 500, obj: {} },
                { name: '郑州工商学院', tag: 'zzgongshang', icon: 'a006', size: 18, width: 600, height: 400, obj: {} }
            ],
        }
    },
    watch: {},
    computed: {},
    created: function () { },
    methods: {
        //图标地址
        logosrc: function (item) {
            return '/Utilities/OtherLogin/Images/' + item.tag + '.png';
        },
        //图标
        icon: function (item) {
            return '<icon style="font-size:"' + item.size + 'px;">&#x' + item.icon + '</icon>';
        }
    },
    template: `<div>
        <template v-for="(item,index) in items">
            <slot name="item" :item="item" :index="index" :img="logosrc(item)" :icon="icon(item)"></slot>
            <slot name="btn" :item="item"></slot>
        </template>
    </div>`
});
