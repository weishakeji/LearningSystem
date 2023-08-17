
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
                name: '安装部署', type: 'node', url: 'Contents/Deployment.html',
                icon: { i: 'a030', s: 22, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '软件升级', type: 'node', url: 'Contents/Upgrade.html',
                icon: { i: 'e836', s: 30, l: -2, t: -2 }, color: { f: '', b: '' }
            },
            {
                name: '检测数据库', type: 'node', hot: true, url: 'datas/test.htm',
                icon: { i: 'e6a4', s: 22, l: 2, t: 0 }, color: { f: 'rgba(251, 118, 118,1)', b: '' }
            },
            { type: 'line' },
            {
                name: '源代码（二次开发）', type: 'node', url: 'Contents/Sourcecode.html',
                icon: { i: 'a034', s: 24, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '模版使用', type: 'node', url: 'Contents/Templates.html',
                icon: { i: 'a033', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: 'RESTful API接口', type: 'link', url: 'api/',
                icon: { i: 'a01c', s: 22, l: 0, t: 0 }, color: { f: 'rgb(121, 187, 255)', b: '' }
            },
            {
                name: '数据实体说明', type: 'link', url: 'datas/index.htm',
                icon: { i: 'e85a', s: 23, l: 0, t: 0 }, color: { f: 'rgb(121, 187, 255)', b: '' }
            },
            {
                name: '数据实体生成', type: 'node', url: 'Contents/DataEntity.html',
                icon: { i: 'e852', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '图标库', type: 'node', url: '../Utilities/Fonts/index.html',
                icon: { i: 'a007', s: 28, l: -3, t: 0 }, color: { f: '333', b: '' }
            },
            { type: 'line' },
            {
                name: '版权信息修改', type: 'node', url: 'copyright.html',
                icon: { i: 'a027', s: 23, l: 0, t: 0 }, color: { f: '333', b: '' }
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
    mounted: function () {
        let index = Number($api.hash());
        if (isNaN(index) || index < 0 || index > this.menus.length - 1) index = 0;
        for (let i = index; i < this.menus.length; i++) {
            if (this.menus[i].type == 'node') {
                this.current = this.menus[i];
                break;
            }
        }
        // 获取地址栏中的#值
        var hashValue =$api.hash(9);
        console.log(hashValue);
    },
    methods: {
        //菜单项点击事件
        menuclk: function (item, index) {
            if (item.type != 'node') return;
            this.current = item;
            //let url = $api.url.set(window.location.href, { 'index': index });
            window.history.pushState({}, '', $api.hash(index));
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

});
