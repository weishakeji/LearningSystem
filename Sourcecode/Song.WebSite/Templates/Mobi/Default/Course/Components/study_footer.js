//底部按钮组件
Vue.component('study_footer', {
    props: ['course'],
    data: function () {
        return {
            menus: [
                { label: '章节列表', id: 'outline', icon: 'e841', show: true },
                { label: '视频', id: 'video', icon: 'e761', show: true },
                { label: '交流', id: 'message', icon: 'e817', show: false },
                { label: '学习内容', id: 'content', icon: 'e813', show: true },
                { label: '附件', id: 'accessory', icon: 'e853', show: true },
                { label: '返回课程', id: 'goback', icon: 'f007c', show: true }
            ],
            loading: true //预载中             
        }
    },
    computed: {},
    created: function () {

    },
    methods: {
        //事件
        click: function (item) {
            switch (item.id) {
                //显示章节树形菜单
                case 'outline':
                    var tree = this.$parent.$refs['outline_tree'];
                    tree.show();
                    break;
                case 'goback':
                    document.location.href = 'detail.' + this.course.Cou_ID;
                    break;
                default:
                    vapp.contextShow = item.id;
                    break;
            }
            //console.log(this.course);
        }
    },
    template: `<footer id="nav_menu">
            <div v-for='item in menus' :id='item.id' v-if='item.show' v-on:click='click(item)'>
                <icon :style="'font-size:'+item.size+'px;'" 
                v-html="'&#x'+item.icon" v-if="!!item.icon && item.icon!=''"></icon>
                <icon v-else :style="'font-size:21px;'" v-html="'&#x'+deficon"></icon>   
                <span>{{item.label}}</span>
            </div>
        </footer>`
});