//课程学习界面的章节目录，内嵌引用咨询组件
//事件：
//int:初始化完成，返回 课程、章节
//change:更改章节时，返回 章节状态、章节
//switch:选项卡切换时，返回 选项卡tag和索引
$dom.load.css([$dom.pagepath() + 'Components/Styles/study_outline.css']);
Vue.component('study_outline', {
    props: ['account', 'owned', 'config'],
    data: function () {
        return {
            couid: $api.dot() != "" ? $api.dot() : $api.querystring("couid"),
            olid: $api.querystring("olid"),

            organ: {},
            config: {},      //当前机构配置项       

            course: {}, //当前课程
            outline: {}, //当前课程章节 
            outlines: [],

            playtime: 0,     //视频播放进度

            loading: true,
            tabs: [
                { 'name': '章节', 'tag': 'outline', 'icon': 'e841', 'show': true },
                { 'name': '交流', 'tag': 'chat', 'icon': 'e817', 'show': true }],
            tabActive: 'outline',
            tabindex: 0,


        }
    },
    watch: {
        'islogin': function (nv, ov) {

        },
        //机构配置项
        'config': {
            deep: true, immediate: true,
            handler: function (n, o) {
                if ($api.isnull(n)) return;
                for (let i = 0; i < this.tabs.length; i++) {
                    if (this.tabs[i].tag == 'chat') {
                        this.tabs[i].show = !(!!this.config.IsDisableChat ? this.config.IsDisableChat : false);
                    }
                }
            }
        }
    },
    computed: {
        //是否登录
        islogin: t => !$api.isnull(t.account),
        //课程是否存在
        couexist: t => { return !$api.isnull(t.course); }
    },
    mounted: function () {
        var th = this;
        th.loading = true;
        $api.bat(
            $api.cache("Outline/tree", { 'couid': th.couid, 'isuse': true }),
            $api.cache("Course/ForID", { id: th.couid })
        ).then(([ol, cur]) => {
            if (cur.data.success) {
                th.course = cur.data.result;
                document.title = th.course.Cou_Name;
            }
            if (ol.data.success) {
                th.outlines = ol.data.result;
                //生成多级序号                   
                (() => {
                    calcSerial(null, '');
                    function calcSerial(outline, lvl) {
                        var childarr = outline == null ? th.outlines : (outline.children ? outline.children : null);
                        if (childarr == null) return;
                        for (let i = 0; i < childarr.length; i++) {
                            childarr[i].serial = lvl + (i + 1) + '.';
                            calcSerial(childarr[i], childarr[i].serial);
                        }
                    }
                })();
                if (th.olid == '') {
                    th.olid = ol.data.result[0].Ol_ID;
                    th.outline = th.getOutline(th.olid, null);
                    if (!th.outline.Ol_IsVideo && !th.outline.Ol_IsLive && th.outline.Ol_QuesCount < 1)
                        th.outline = th.nextVideo(th.outline);
                    if (th.outline == null) th.outline = th.getOutline(th.olid, null);
                } else {
                    th.outline = th.getOutline(th.olid, null);
                }
                if (th.outline == null) {
                    th.$alert('当前章节不存在', '提示', {
                        confirmButtonText: '确定',
                        callback: action => {
                            var href = location.href;
                            href = $api.url.set(href, 'olid', '');
                            location.href = href;
                        }
                    });
                }
                //课程与章节加载完成
                th.$emit('init', th.course, th.outline);
                th.outlineClick(th.outline, null);
            } else {
                th.outlines = [];
                th.$emit('init', th.course, {});
                //if (!ol.data.success) throw "章节列表加载异常！详情：\r" + ol.data.message;
                //if (!cur.data.success) throw "课程信息加载异常！详情：\r" + cur.data.message;
            }
        }).catch(function (err) {
            th.outlines = [];
            console.error(err);
        }).finally(() => th.loading = false);
    },
    methods: {
        //选项卡的点击事件
        tabClick: function (item, index) {
            this.tabActive = item.tag;
            this.tabindex = index;
            this.$emit('switch', this.tabActive, this.tabindex);
        },
        //获取当前章节
        getOutline: function (id, outlines) {
            outlines = outlines ? outlines : this.outlines;
            var ol = null;
            for (var i = 0; i < outlines.length; i++) {
                if (outlines[i].Ol_ID == id) {
                    ol = outlines[i];
                    break;
                }
                if (ol == null && (outlines[i].children && outlines[i].children.length > 0)) {
                    ol = this.getOutline(id, outlines[i].children);
                }
            }
            return ol;
        },
        //章节树形列表中的点击事件
        outlineClick: function (outline) {
            var th = this;
            var olid = outline.Ol_ID;
            this.olid = olid;
            //设置当前节点的样式
            this.$nextTick(function () {
                var tree = this.$refs['outlines_tree'];
                tree.setCurrentKey(outline.Ol_ID);
            });
            th.outline = outline;            
            //
            th.loading = true;
            //获取章节相关信息
            $api.bat(
                $api.get('Outline/State', { 'olid': olid, 'acid': th.account.Ac_ID }),
                $api.cache("Outline/Info", { 'olid': olid })
            ).then(([state, info]) => {
                //获取结果
                var result = info.data.result;
                for (let key in state.data.result) {
                    result[key] = state.data.result[key];
                }
                th.state = result;
                if (!th.state.canStudy) {
                    th.$confirm('当前章节需要购买后学习, 是否继续?', '提示', {
                        confirmButtonText: '购买',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        var link = window.location.href;
                        link = link.substring(link.indexOf(window.location.pathname));
                        var url = $api.url.set('/mobi/course/buy', {
                            'couid': th.couid, 'olid': th.olid
                        });
                        window.location.href = url;
                    }).catch(() => { });
                    return;
                }
                var url = $api.setpara("olid", olid);
                history.pushState({}, null, url);
                th.$emit('change', th.state, outline);
                /*
                //获取当前章节的详细信息
                $api.get('Outline/ForID', { 'id': olid }).then(function (req) {
                    if (req.data.success) {
                        th.$emit('change', th.state, req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });*/
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
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
        nextVideo: function (outline) {
            let nextnode = this.nextnode(outline);
            if (nextnode != null && !nextnode.Ol_IsVideo) return this.nextVideo(nextnode);
            return nextnode;            
        },
    },
    template: `<div id="rightBox">          
        <div class="tabs">            
            <div v-if="tab.show" v-for="(tab,i) in tabs" @click="tabClick(tab,i)"
            :class="{'tabCurrent':tab.tag==tabActive}">
                <icon v-html="'&#x'+tab.icon"></icon>
                {{tab.name}}
            </div>      
        </div>     
       <el-tree :data="outlines" v-show="tabActive=='outline'" v-loading="loading" node-key="Ol_ID" ref="outlines_tree"
            default-expand-all :expand-on-click-node="false" :check-on-click-node="true" empty-text="没有章节"
            :props="{children: 'children',label: 'Ol_Name'}" @node-click="outlineClick">
            <span class="custom-tree-node" slot-scope="{ node, data }" :title="data.Ol_Name"
                :isvideo='data.Ol_IsVideo' :islive='data.Ol_IsLive'>
                <span>{{data.serial}}</span>
                <span>{{ data.Ol_Name }}</span>
            </span>           
        </el-tree>
        <study_chat v-if="tabActive=='chat'" :course="course" :account="account" :outline="outline" :playtime="playtime"></study_chat>
    </div>`
});