
    //数据项
    $dom.load.css([$dom.path() + 'Viewport/Components/Styles/dataitem.css'])
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