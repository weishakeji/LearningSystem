//menutree
$dom.load.css([$dom.pagepath() + 'Components/Styles/outline_tree.css']);
// 章节列表组件
//事件;
//show:菜单显示的事件，隐藏时也会触发
Vue.component('outline_tree', {
    //章节列表，课程对象，菜单是否显示
    //studied:是否可以学习该章节
    //owned: 是否拥有该课程，例如学员组关联，购买
    //videolog:章节的学习记录
    props: ['outlines', 'course', 'studied', 'owned', 'videolog'],
    data: function () {
        return {
            current: {}, //当前章节对象		
            olid: $api.querystring("olid", ''),		//当前章节id
            couid: $api.querystring("couid", ''),    //课程id

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
            }, immediate: true
        },
        //章节数数据
        'outlines': {
            deep: true, immediate: true,
            handler: function (n, o) {
                if (this.outlines && this.outlines.length > 0)
                    this.init();
            }
        },
        //菜单显示的变化
        'menushow': function (nv, ov) {
            this.$emit('show', nv);
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
            //当前章节，如果没有传入章节id，默认取第一个视频章节
            this.olid = $api.querystring("olid", '');
            var current = this.current;
            if (this.outlines && this.outlines.length > 0) {
                if (this.olid == '') {
                    for (var i = 0; i < this.outlines.length; i++) {
                        if (this.outlines[i].Ol_IsVideo || this.outlines[i].Ol_IsLive) {
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
            //如果没有取到视频章节，则取第一个
            if ($api.isnull(current) && this.outlines && this.outlines.length > 0)
                current = this.outlines[0];
            this.outlineClick(current);
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
                this.outlineClick(node);
            } else {
                var url = $api.url.set('/mobi/course/buy', {
                    'couid': this.course.Cou_ID,
                    'olid': this.olid,
                    'link': encodeURIComponent(window.location.href)
                });
                window.navigateTo(url);
            }
        },
        //节点的事件
        outlineClick: function (node) {
            this.olid = node.Ol_ID;
            this.current = node;
            this.$emit('change', node);
            this.menushow = false;
        },
        //当前节点的下一个节点
        nextnode: function (selfnode, tree) {
            if (tree == null) tree = this.outlines;
            //获取当前节点的兄弟节点（同一层级的节点）
            var getsiblings = function (tree, self) {
                for (const node of tree) {
                    if (node.Ol_ID === self.Ol_ID) return tree;
                    if (node.children) {
                        const siblings = getsiblings(node.children, self);
                        if (siblings.length > 0) return siblings;
                    }
                }
                return [];
            };
            //获取当前节点的同级节点的下一级
            var getsiblingnext = function (list, self) {
                for (let i = 0; i < list.length; i++) {
                    if (list[i].Ol_ID == self.Ol_ID) {
                        if (i < list.length - 1) {
                            return list[i + 1];
                        }
                    }
                }
                return null;
            };
            //当前节点的父级节点
            var getparent = function (tree, self) {
                for (const node of tree) {
                    if (node.children) {
                        if (node.children.some(child => child.Ol_ID === self.Ol_ID)) return node;
                        const parent = getparent(node.children, self);
                        if (parent) return parent;
                    }
                }
            };
            //如果有下级，先取下级第一个节点
            if (selfnode.children != null) return selfnode.children[0];
            //再取同级的下一个节点
            let siblings = getsiblings(tree, selfnode);
            let next = getsiblingnext(siblings, selfnode);
            if (next) return next;
            //没有下一个（即最后一个）则取父级的下一个
            let parent = getparent(tree, selfnode);
            while (parent) {
                let next = getsiblingnext(getsiblings(tree, parent), parent);
                if (next) return next;
                parent = getparent(tree, parent);
            }
            return null;
        },
        //下一个章节（视频章节）
        nextVideo: function (outline, state) {
            let nextnode = this.nextnode(outline);
            if (nextnode != null && !nextnode.Ol_IsVideo) return this.nextVideo(nextnode);
            return nextnode;
        },
        //播放下一个视频章节
        playnextVideo: function (outline) {
            let nextnode = this.nextVideo(outline);
            if (nextnode) this.outlineClick(nextnode);
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
      {{o.Ol_XPath}}<span>{{o.Ol_Name}}</span> 
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