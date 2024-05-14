
//数值的展示
$dom.load.css([$dom.path() + 'Viewport/Components/Styles/piece.css'])
//头部的数据块
Vue.component('piece', {
    props: ["title",    //标题
        "ico",          //右侧大图标
        "iconsize",     //图标的大小，默认是100像素
        "leftcolor",        //左侧背景色，背景是过渡色
        "rightcolor"         //右侧背景色
       
    ],
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
        },
        //字体颜色
        "font_color":function(){
            if ($api.isnull(this.leftcolor)) return '';        
            return 'color: ' + this.leftcolor + ';';
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
                    <div class="contx" :style="font_color">
                        <slot></slot>
                    </div>
                </div>
        </div>`
});