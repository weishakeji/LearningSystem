
// 页面底部的导航栏
Vue.component('footer_menu', {
    props: ['organ'],
    data: function () {
        return {
            default_menus: [
                { name: '首页', url: '/mobi/', icon: 'a020', img: '', size: 22, show: true },
                { name: '课程中心', url: '/mobi/course/index', icon: 'e765', img: '', size: 25, show: true },
                { name: '我的课程', url: '/mobi/Account/MyCourse', icon: 'e813', img: '', size: 23, show: true },
                { name: '考试', url: '/mobi/exam/index', icon: 'e810', img: '', size: 24, show: true },
                { name: '我的..', url: '', icon: 'e804', img: '', size: 26, show: true, evt: this.btnMymenu }
            ],
            deficon: 'e72f',    //默认图标
            customer_meus: []        //自定义菜单
        }
    },
    watch: {
        'organ': function (nv, ov) {
            this.getMenus();
        }
    },
    computed: {
        //合并默认菜单和自定义菜单
        'menus': function () {
            var arr = [];
            for (var i = 0; i < this.default_menus.length; i++)
                arr.push(this.default_menus[i]);
            for (var i = 0; i < this.customer_meus.length; i++) {
                var item = this.nodeconvert(this.customer_meus[i]);
                arr.push(item);
            }
            return arr;
        }
    },
    mounted: function () { },
    methods: {
        //获取菜单
        getMenus: function () {
            var th = this;
            var orgid = !!this.organ.Org_ID ? this.organ.Org_ID : 0;
            $api.cache('Navig/Mobi', { 'orgid': orgid, 'type': 'foot' }).then(function (req) {
                if (req.data.success) {
                    th.customer_meus = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                //console.error(err);
            });
        },
        nodeconvert: function (obj) {
            var result = '';
            if (typeof (obj) != 'string')
                result = JSON.stringify(obj);
            //result = result.replace(/MM_WinID/g, "id");
            result = result.replace(/Nav_Name/g, "name");
            result = result.replace(/Nav_Url/g, "url");
            result = result.replace(/Nav_Logo/g, "img");
            result = result.replace(/Nav_Event/g, "evt");
            result = result.replace(/Nav_Target/g, "target");
            return JSON.parse(result);
        },
        btnMymenu: function (item) {
            //左侧主菜单滑出
            window.vapp.$refs.aside_menu.show = true;
        },
        btnDefault: function (item) {
            if (!item.url || item.url == '') return;
            window.navigateTo(item.url);
        }
    },
    template: `<footer id="nav_menu">
       <div v-for="item in menus" @click="!!item.evt ? item.evt(item) : btnDefault(item)" v-if="item.show">
            <img :src="item.img" v-if="item.img!=''"/>
            <template v-else>
                <icon :style="'font-size:'+item.size+'px;'" v-html="'&#x'+item.icon" v-if="!!item.icon && item.icon!=''"></icon>
                <icon v-else :style="'font-size:21px;'" v-html="'&#x'+deficon"></icon>            
            </template>
            <span>{{item.name}}</span>
       </div>
    </footer>`
});
