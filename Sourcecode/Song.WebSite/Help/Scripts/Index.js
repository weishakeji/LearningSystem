
window.vapp = new Vue({
    el: '#vapp',
    data: {
        //菜单项
        //type:节点类型，node为菜单项,link为外部链接
        //icon节点：i图标,s图标大小（即size)，l左移量(即left),t即top
        //color节点：颜色，f前景色，b背景色
        menus: [
            {
                name: '系统简述', type: 'node', hot: false, url: 'Contents/Overview.html',
                icon: { i: 'a051', s: 26, l: 0, t: -2 }, color: { f: '67C23A', b: '' }
            },
            {
                name: '使用教程', type: 'node', hot: false, url: 'Contents/Tutorials.html',
                icon: { i: 'a026', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            { type: 'line' },
            {
                name: '安装部署', type: 'node', url: 'Contents/Deployment.html',
                icon: { i: 'e79b', s: 23, l: 0, t: 2 }, color: { f: '', b: '' }
            },
            {
                name: '软件升级', type: 'node', url: 'Contents/Upgrade.html',
                icon: { i: 'e836', s: 30, l: -5, t: -2 }, color: { f: '', b: '' }
            },
            {
                name: '检测数据库', type: 'node', hot: true, url: 'datas/test.htm',
                icon: { i: 'a030', s: 20, l: 3, t: 1 }, color: { f: 'rgba(251, 118, 118,1)', b: '' }
            },
            { type: 'line' },
            {
                name: '源代码说明', type: 'node', url: 'Contents/Sourcecode.html',
                icon: { i: 'a034', s: 22, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '视图模版', type: 'node', url: 'Contents/Templates.html',
                icon: { i: 'a033', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: 'RESTful API', type: 'node', url: 'api/Index.htm',
                icon: { i: 'a01c', s: 22, l: 0, t: 0 }, color: { f: '', b: '' }
            },           
            {
                name: '数据库', type: 'node', url: 'database/index.htm',
                icon: { i: 'e6a4', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },           
            {
                name: '图标库', type: 'node', url: '../Utilities/Fonts/index.html',
                icon: { i: 'a007', s: 29, l: -4, t: -2 }, color: { f: '', b: '' }
            },
            {
                name: 'WebdeskUI', type: 'node', url: 'Webdeskui/index.html',
                icon: { i: 'a010', s: 22, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            { type: 'line' },
            {
                name: '版权信息修改', type: 'node', url: 'copyright.html',
                icon: { i: 'a027', s: 23, l: 0, t: 0 }, color: { f: '', b: '' }
            },
            {
                name: '开源协议', type: 'node', url: 'License.html',
                icon: { i: 'a037', s: 21, l: 3, t: 1 }, color: { f: '', b: '' }
            },

        ],
        showmenu: false,
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
        //第一次打开，获取索引值，用于触发默认菜单项
        let index = Number($api.hash());
        if (isNaN(index) || index < 0 || index > this.menus.length - 1) index = 0;
        for (let i = index; i < this.menus.length; i++) {
            if (this.menus[i].type == 'node') {
                this.current = this.menus[i];
                break;
            }
        }
    },
    methods: {
        //菜单项点击事件
        menuclk: function (item, index) {
            if (item.type != 'node') return;
            this.current = item;
            this.showmenu = false;
            window.history.pushState({}, '', $api.hash(index == 0 ? '' : String(index)));
        },
        //图标样式
        iconstyle: function (icon) {
            const left = 15, top = 10;
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
