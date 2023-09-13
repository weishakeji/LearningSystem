//顶部的选项卡
Vue.component('exam_tabs', {
    //account:学员账号，如果登录,
    //loaded:初始数据是否加载完成，例如是否已经判断过登录状态
    props: ["account", "loaded"],
    data: function () {
        return {
            //选项卡，search为搜索字符
            tabs: [
                { 'name': '我的考试', 'tag': 'my_exam', 'icon': 'e811', 'curr': true, 'search': '', 'login': true },
                { 'name': '所有考试', 'tag': 'all_exam', 'icon': 'e810', 'curr': false, 'search': '', 'login': false },
                { 'name': '成绩查看', 'tag': 'score_exam', 'icon': 'e6ef', 'curr': false, 'search': '', 'login': true },
            ],
            tabmenu: ''     //当前选项卡
        }
    },
    watch: {
        'loaded': {
            handler: function (nv, ov) {
                if (nv != true || this.tabmenu != '') return;
                let def = this.tabs.find(t => t.login == this.islogin);
                if (def != null) {
                    this.tabmenu = def.tag;
                    this.$emit('change', this.tabmenu, {});
                }
            }, immediate: true
        },
        'tabmenu': {
            handler: function (nv, ov) {
                let tag = this.tabs.find(t => t.tag == nv);
                if (tag != null)
                    this.search(tag);

            }, immediate: false
        }
    },
    computed: {
        //是否登录
        islogin: t => !$api.isnull(t.account)
    },
    mounted: function () {

    },
    methods: {
        //选项卡点击事件
        clickEvent: function (tab, item) {
            this.tabmenu = tab;
            this.$emit('change', tab, item);
        },
        //搜索事件
        search: function (item) {
            var existEvent = this.$listeners['search'];
            if (existEvent) this.$emit('search', item.tag, item.search);
        },
        //当搜索框内容变更多时触，如果为空，则触发搜索事件
        changesearch: function (item) {
            item.search = $api.trim(item.search);
            if (item.search == '') this.search(item);
        }
    },
    template: ` <div class="menu-box" :login="islogin">
            <div v-for="(item,index) in tabs" :current="item.tag==tabmenu" @click="clickEvent(item.tag,item)">
                <icon v-html="'&#x'+item.icon"></icon>
                <span>{{item.name}}</span>
            </div>           
        </div>
  `
});