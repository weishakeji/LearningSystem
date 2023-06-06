
window.vapp = new Vue({
    el: '#vapp',
    data: {
        //菜单项
        //icon节点：i图标,s图标大小（即size)，l左移量(即left),t即top
        //color节点：颜色，f前景色，b背景色
        menus: [
            {
                name: '系统简述', type: 'node', hot: false, url: 'Contents/Overview.html',
                icon: { i: 'a051', s: 26, l: 0, t: -2 }, color: { f: '67C23A', b: '' }
            },
             {
                name: '使用教程', type: 'link', hot: false, url: 'http://www.weisha100.net/',
                icon: { i: 'a026', s: 23, l: 0, t: 0 }, color: { f: 'rgb(121, 187, 255)', b: '' }
            },
            { type: 'line' },
            {
                name: '安装部署', type: '', url: 'Contents/Deployment.html',
                icon: { i: 'a030', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '检测数据库', type: 'node', hot: true, url: 'datas/test.htm',
                icon: { i: 'e6a4', s: 23, l: 0, t: 0 }, color: { f: 'rgb(251 118 118)', b: 'rgb(249 236 234)' }
            },
            { type: 'line' },
            {
                name: '源代码（二次开发）说明', type: '', url: 'Contents/Sourcecode.html',
                icon: { i: 'a034', s: 24, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '模版使用说明', type: '', url: 'Contents/Templates.html',
                icon: { i: 'a033', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: 'RESTful API接口', type: 'link', url: 'api/',
                icon: { i: 'a01c', s: 22, l: 0, t: 0 }, color: { f: 'rgb(121, 187, 255)', b: 'rgb(245 241 227)' }
            },
            {
                name: '数据实体', type: 'link', url: 'datas/',
                icon: { i: 'e85a', s: 23, l: 0, t: 0 }, color: { f: 'rgb(121, 187, 255)', b: 'rgb(237 248 249)' }
            },
            { type: 'line' },
            {
                name: '图标库', type: 'node', url: '../Utilities/Fonts/index.html',
                icon: { i: 'e610', s: 25, l: 0, t: 0 }, color: { f: 'E6A23C', b: '' }
            },
            {
                name: '预载效果', type: 'node', url: '../Utilities/Fonts/loading.html',
                icon: { i: 'e601', s: 21, l: 0, t: 0 }, color: { f: 'E6A23C', b: '' }
            },
         
           
           
        ],
        //当前点击的对象
        current: {}
    },
    computed: {
        //右侧内容区的链接地址
        iframeurl: function () {
            if ($api.isnull(this.current)) return '';
            return this.current.url;
        },
    },
    watch: {

    },
    created: function () {
        if ($api.isnull(this.current)) {
            for (let i = 0; i < this.menus.length; i++) {
                if (this.menus[i].type == 'node') {
                    this.current = this.menus[i];
                    break;
                }
            }
        }
    },
    methods: {
        //菜单项点击事件
        menuclk: function (item) {
            if (item.type != 'node') return;
            this.current = item;
            console.log(item);
        },
        //图标样式
        iconstyle: function (icon) {
            const left = 8, top = 10;
            let size = 'font-size:' + icon.s + 'px;';
            return size + 'left:' + (left + icon.l) + 'px;top:' + (top + icon.t) + 'px';
        },
        //节点的样式
        nodestyle: function (item) {
            if (!item.color) return '';
            let fore = item.color.f ? item.color.f : '';
            let bg = item.color.b ? item.color.b : '';
            if (fore != '' && fore.indexOf('(') < 0) fore = '#' + fore;
            if (bg != '' && bg.indexOf('(') < 0) bg = '#' + bg;
            return 'color:' + fore + ';'
                + 'background-color:' + bg + ';';
        }
    },
    components: {
        //左侧菜单节点项
        'menunode': {
            props: ['item'],
            data: function () {
                return {
                    prev: '',
                    dot: '.',
                    aftet: ''
                }
            },
            created: function () {
                var num = String(Math.round(this.number * 100) / 100);
                if (num.indexOf('.') > -1) {
                    this.prev = num.substring(0, num.indexOf('.'));
                    this.after = num.substring(num.indexOf('.') + 1);
                } else {
                    this.prev = num;
                    this.dot = '&nbsp;';
                }
            },
            template: `<div class="score">
            <span class="prev">{{prev}}</span>
            <span class="dot" v-html="dot"></span>
            <span class="after">{{after}}</span>
            </div>`
        }
    }
});
