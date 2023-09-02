$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            stat: {},       //机构的各种统计数据
            loading_init: true,
            loading_stat: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, org) {
                //获取结果           
                th.platinfo = platinfo.data.result;
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                th.getStatistics(th.org);
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
            //获取统计数据
            getStatistics: function (org) {
                var th = this;
                th.loading_stat = true;
                $api.get('Organization/Statistics', { 'orgid': org.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.stat = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_stat = false);
            }
        }
    });
    //头部的数据块
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
                return 'background: linear-gradient(to right, ' + left + ', ' + right + ');';
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
    //数据项
    Vue.component('dataitem', {
        props: ["title", "ico", "iconsize", "color"],
        data: function () {
            return {}
        },
        watch: {},
        computed: {
            //左侧图标
            'icon': function () {
                if ($api.isnull(this.ico)) return '&#xe729';
                return '&#x' + this.ico;
            },
            //背景图标的字号
            'icon_size': function () {
                let fontsize = 20;
                if (!$api.isnull(this.iconsize) && !(isNaN(Number(this.iconsize))))
                    fontsize = Number(this.iconsize);
                return 'font-size: ' + fontsize + 'px;';
            },
            //颜色
            "icon_color": function () {
                if ($api.isnull(this.color)) return '';
                return 'color: ' + this.color + ';';
            }
        },
        mounted: function () {
        },
        methods: {
        },
        template: `<div class="dataitem">        
                <icon v-html="icon"  :style="icon_size + icon_color"></icon>
                <div class="area">                    
                    <div class="contx">
                        <slot></slot>
                    </div>
                    <div class="tit">{{title}}</div>
                </div>
        </div>`
    });
});
