$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
        },
        methods: {
        }
    });
    Vue.component('piece', {
        props: ["title",    //标题
            "ico",          //右侧大图标
            "iconsize",     //图标的大小，默认是100像素
            "leftcolor",        //左侧背景色，背景是过渡色
            "rightcolor"],      //右侧背景色
        data: function () {
            return {}
        },
        watch: {},
        computed: {
            //作为背景用的图标
            'background_ico': function () {
                if ($api.isnull(this.ico)) return '';
                return '&#x' + this.ico;
            },
            //背景图标的字号
            'background_size': function () {
                let fontsize = 100;
                if (!$api.isnull(this.iconsize) && !(isNaN(Number(this.iconsize))))
                    fontsize = Number(this.iconsize);
                return 'font-size: ' + fontsize + 'px';
            },
            //背景色
            "background_color": function () {
                if ($api.isnull(this.leftcolor) && $api.isnull(this.rightcolor)) return '';
                let left = $api.isnull(this.leftcolor) ? '#fff' : this.leftcolor;
                let right = $api.isnull(this.rightcolor) ? '#fff' : this.rightcolor;
                let c = 'background: linear-gradient(to right, ' + left + ', ' + right + ');';
                console.log(c);
                return c;
            }
        },
        mounted: function () {
        },
        methods: {
        },
        template: `<div class="piece" :style="background_color">        
                <icon class="bg" v-html="background_ico" v-if="background_ico!=''" :style="background_size"></icon>              
                <div class="area">
                    <div class="tit">{{title}}</div>
                    <div class="contx">
                        <slot></slot>
                    </div>
                </div>
        </div>`
    });
});
