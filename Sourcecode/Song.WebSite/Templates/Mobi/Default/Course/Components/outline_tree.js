//附件menutree
$dom.load.css([$dom.pagepath() + 'Components/Styles/outline_tree.css']);
// 章节列表组件
Vue.component('outline_tree', {
    //章节列表，课程对象，菜单是否显示
    //studied:是否可以学习该章节
    //owned: 是否拥有该课程，例如学员组关联，购买
    //videolog:章节的学习记录
    props: ['outlines', 'course', 'studied', 'owned', 'videolog'],
    data: function () {
        return {
            current: {}, //当前章节对象		
            olid: '',		//当前章节id

            menushow: false,     //是否显示章节面板
            loading: true, //预载中
        }
    },
    watch: {
        //当章节id变化时
        olid: {
            handler(nv, ov) {
                if (nv == '' || nv == ov) return;
                var url = $api.setpara('olid', nv);
                history.pushState({}, null, url);
            },
            immediate: true
        },
        //章节数数据
        'outlines': {
            deep: true, immediate: true,
            handler: function (n, o) {
                if (this.outlines && this.outlines.length > 0)
                    this.init();
            }
        }

    },
    computed: {
        //课程是否存在
        couexist: function () {
            return JSON.stringify(this.course) != '{}' && this.course != null;
        },

    },
    created: function () {

    },
    methods: {
        //初始化的一些计算
        init: function () {
            //当前章节
            this.olid = $api.querystring("olid", '');
            var current = this.current;
            if (this.outlines && this.outlines.length > 0) {
                if (this.olid == '') {
                    for (var i = 0; i < this.outlines.length; i++) {
                        if (this.outlines[i].Ol_IsVideo) {
                            current = this.outlines[i];
                            this.olid = current.Ol_ID;
                            break;
                        }
                    }
                } else {
                    for (var i = 0; i < this.outlines.length; i++) {
                        if (this.outlines[i].Ol_ID == this.olid) {
                            current = this.outlines[i];
                            break;
                        }
                    }
                }
            }
            this.node_click_event(current);
        },
        //计算缩进
        padding: function (level) {
            return 'padding-left:' + (level * 20 + 15) + 'px';
        },
        //章节点击事件
        click: function (node) {
            //是否允许学习
            var allow = false;
            if (this.course.Cou_IsTry && node.Ol_IsFree) allow = true;
            if (this.owned || this.course.Cou_IsFree || this.course.Cou_IsLimitFree) allow = true;
            if (allow) {
                if (node.Ol_ID == this.olid) return;
                this.node_click_event(node);
            } else {
                var url = $api.url.set('/mobi/course/buy', {
                    'couid': this.course.Cou_ID,
                    'olid': this.olid,
                    'link': encodeURIComponent(window.location.href)
                });
                window.location.href = url;
            }
        },
        //节点的事件
        node_click_event: function (node) {
            this.olid = node.Ol_ID;
            this.current = node;
            this.$emit('change', node);
            this.menushow = false;
        },
        show: function () {
            this.menushow = true;
        }
    },
    template: `<van-popup id='menu' position="left"  v-model="menushow">
    <div class='cour-info'>
        <img :src='course.Cou_LogoSmall' v-if="couexist && course.Cou_LogoSmall !='' "/>
        <img src='/Utilities/Images/cou_nophoto.jpg' v-else/>
        <div class='cour-info-right'>
            <icon course>{{course.Cou_Name}}</icon>
            <icon subject>{{course.Sbj_Name}}</icon>
        </div>
    </div>
    <van-cell-group v-if='outlines && outlines.length>0'>
    <van-cell :current='o.Ol_ID==olid' v-for='o in outlines' :isvideo='o.Ol_IsVideo' :islive='o.Ol_IsLive'
      :olid='o.Ol_ID' :style='padding(o.Ol_Level)' v-on:click='click(o)'>
      <template #title>
      <span>{{o.Ol_XPath}}{{o.Ol_Name}}</span> 
            <template v-if="course.Cou_IsTry && o.Ol_IsFree">
                <van-tag type="success" v-if="o.Ol_IsVideo">免费</van-tag>
            </template>
            <template v-else>
                <span v-if="owned || course.Cou_IsFree || course.Cou_IsLimitFree">
                      <progress_video :videolog="videolog" :outline="o" text="学习" v-if="o.Ol_IsVideo">
                    </progress_video>
                </span>
                <template v-else>
                    <van-tag type="warning">购买</van-tag>
                </template>
            </template>            
    </van-cell>
  </van-cell-group>
  <div class='mui-table-view-cell' v-else style='color: azure;'> 当前课程没有章节 </div>
</van-popup>`
});