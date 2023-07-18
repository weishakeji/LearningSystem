//底部按钮组件
$dom.load.css([$dom.pagepath() + 'Components/Styles/study_footer.css']);
Vue.component('study_footer', {
    props: ['course', 'state'],
    data: function () {
        return {
            menus: [
                { label: '章节列表', id: 'outline', icon: 'e841', disable: false, show: true },
                { label: '视频', id: 'video', icon: 'e761', disable: false, show: true },
                { label: '交流', id: 'message', icon: 'e817', disable: false, show: false },
                { label: '学习内容', id: 'content', icon: 'e813', disable: false, show: true },
                { label: '附件', id: 'accessory', icon: 'e853', disable: false, show: true },
                { label: '返回课程', id: 'goback', icon: 'f007c', disable: false, show: true }
            ],
            loading: true //预载中             
        }
    },
    watch: {
        'state': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                for (let i = 0; i < this.menus.length; i++) {
                    const el = this.menus[i];                  
                    if (el.id == 'video') {
                        el.disable = !nv.existVideo;                       
                        console.error(el);
                    }
                    this.$set(this.menus, el, i);
                }
            }, immediate: true, deep: true
        },
    },
    computed: {},
    created: function () {

    },
    methods: {
        //事件
        click: function (item) {
            if (item.disable) return;
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
            <div v-for='item in menus' :class='{"disable":item.disable}' :id='item.id' v-if='item.show' v-on:click='click(item)'>
                <icon :style="'font-size:'+item.size+'px;'" 
                v-html="'&#x'+item.icon" v-if="!!item.icon && item.icon!=''"></icon>
                <icon v-else :style="'font-size:21px;'" v-html="'&#x'+deficon"></icon>   
                <span>{{item.label}}</span>
            </div>
        </footer>`
});