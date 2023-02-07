$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        
            datas: [],
            form: {
                orgid: '',
                search: '',
                isuse: null
            },
            defaultProps: {
                children: 'children',
                label: 'Sbj_Name'
            },
            expanded: [],        //专业树形默认展开的节点
            expanded_storage: 'subject_for_admin_tree',  //用于记录展开节点的storage名称
            filterText: '',      //查询过虑树形的字符
            total: 0,     //当前机构下的专业总数

            loading: false,
            loadingid: -1,
            loading_sumbit: false,   //提交时的预载           
            loading_init: true
        },
        mounted: function () {

        },
        created: function () {
            var th=this;
            $api.get('Organization/Current').then(function (req) {
                vapp.loading_init = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.form.orgid = th.organ.Org_ID;
                    //机构配置信息
                    th.config = $api.organ(vapp.organ).config;
                    th.getTreeData();
                    th.getTotal();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
            $api.storage(this.expanded_storage, null);
        },
        computed: {
        },
        watch: {
            filterText: function (val) {
                this.$refs.tree.filter(val);
            }
        },
        methods: {
            //所取专业的数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                $api.get('Subject/Tree', th.form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                        //获取默认展开的节点
                        var arr = $api.storage(th.expanded_storage);
                        if ($api.getType(arr) == 'Array') {
                            th.expanded = arr;
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //获取专业总数
            getTotal: function () {
                var th = this;
                $api.get('Subject/CountOfChildren',
                    { 'orgid': vapp.organ.Org_ID, 'sbjid': 0, 'use': null })
                    .then(function (req) {
                        if (req.data.success) {
                            th.total = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        console.error(err);
                    });
            },
            handleDragStart(node, ev) {
                //console.log('drag start', node);
            },
            handleDragEnter(draggingNode, dropNode, ev) {
                //console.log('tree drag enter: ', dropNode.label);
            },
            handleDragLeave(draggingNode, dropNode, ev) {
                //console.log('tree drag leave: ', dropNode.label);
            },
            handleDragOver(draggingNode, dropNode, ev) {
                //console.log('tree drag over: ', dropNode.label);
            },
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                //console.log('tree drag end: ', dropNode && dropNode, dropType);
                var th = this;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.datas);
                $api.post('Subject/ModifyTaxis', { 'list': arr }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.fresh_cache();
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            handleDrop(draggingNode, dropNode, dropType, ev) {
                //console.log('tree drop: ', dropNode.label, dropType);
            },
            allowDrop(draggingNode, dropNode, type) {
                return true;
            },
            allowDrag(draggingNode) {
                return true;
            },
            //节点展开事件
            nodeexpand: function (data, node, tree) {
                this.expanded.push(data.Sbj_ID);
                $api.storage(this.expanded_storage, this.expanded);
            },
            //节点折叠事件
            nodecollapse: function (data, node, tree) {
                var index = this.expanded.indexOf(data.Sbj_ID);
                if (index > -1) {
                    this.expanded.splice(index, 1);
                    $api.storage(this.expanded_storage, this.expanded);
                }
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                console.log(txt.length);
                if (txt == '') return true;
                return data.Sbj_Name.toLowerCase().indexOf(txt) !== -1;
            },
            //编辑当前专业
            modify: function (row) {
                this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey]);
            },
            //修改状态
            changeState: function (data, field) {
                data[field] = !data[field];
                var th = this;
                this.loadingid = data.Sbj_ID;
                $api.post('Subject/Modify', { 'entity': data }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        vapp.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                        th.fresh_cache();
                        $api.cache('Subject/ForID:clear', { 'id': data.Sbj_ID });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = -1;
                });
            },
            //将树形数据转到数据列表，用于递交到服务端更改专业的排序
            tree2array: function (datas) {
                var list = [];
                list = toarray(datas, 0, 1, list);
                return list;
                function toarray(arr, pid, level, list) {
                    for (let i = 0; i < arr.length; i++) {
                        const d = arr[i];
                        var obj = {
                            'Sbj_ID': d.Sbj_ID,
                            'Sbj_PID': pid,
                            'Sbj_Tax': i + 1,
                            'Sbj_Level': level
                        }
                        list.push(obj);
                        if (d.children && d.children.length > 0) {
                            list = toarray(d.children, d.Sbj_ID, ++level, list);
                        }
                    }
                    return list;
                }
            },
            remove: function (node, data) {
                console.log(node);
                console.log(data);
                if (!!data.sbjCount && data.sbjCount > 0) {
                    var msg = '当前专业“' + data.Sbj_Name + '”下共有 <b>' + data.sbjCount + '</b> 个子专业，请先删除子专业。'
                    this.$alert(msg, '不可删除', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true,
                        callback: () => { }
                    });
                    return;
                }
                if (!!data.courseCount && data.courseCount > 0) {
                    var msg = '当前专业“' + data.Sbj_Name + '”下有 <b>' + data.courseCount + '</b> 门课程，请删除课程后再删除当前专业。'
                    this.$alert(msg, '不可删除', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true,
                        callback: () => { }
                    });
                    return;
                }
                var th = this;
                th.loading_sumbit = true;
                $api.delete('Subject/Delete', { 'id': data.Sbj_ID }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.fresh_cache();
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //鼠标滑过Logo，显示大图
            hoverlogo: function (data) {
                if (data == null) return;
                var img = $dom("#logo_" + data.Sbj_ID);
                if (img == null || img.length < 1) return;
                img.show();
                var icon = img.prev().offset();
                var offset = $dom('.el-tree').offset();
                img.css("top", icon.top - offset.top + 22 + 'px');
                img.css("left", icon.left - offset.left + 8 + 'px');
            },
            //鼠标滑离图片，隐藏预览
            mouseleave: function () {
                var img = $dom(".logoSmall");
                img.hide();
            },
            //操作成功的刷新
            fresh_operateSuccess: function () {
                this.fresh_cache();
                this.getTreeData();
            },
            //当专业数据更改时，刷新缓存数据
            fresh_cache: function () {
                $api.cache('Subject/TreeFront:update', { 'orgid': this.organ.Org_ID });                
            }
        }
    });

    //专业所的课程数，包括子级专业
    Vue.component('course_count', {
        props: ["sbjid", "subject"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'sbjid': {
                handler: function (nv, ov) {
                    this.getcount();
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getcount: function () {
                var th = this;
                th.loading = true;
                $api.get('Subject/CountOfCourse', { 'sbjid': th.sbjid, 'use': null }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.count = req.data.result;
                        th.subject.courseCount = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        },
        template: `<span title="课程数">
            <span class="el-icon-loading" v-if="loading"></span>
            <icon course :class="{'count':true,'zero':count<=0}">{{count}}</icon>
        </span> `
    });

    //专业下的所有子专业数
    Vue.component('sbj_count', {
        props: ["sbjid", "subject"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'subject': {
                handler: function (nv, ov) {
                    this.count = this.calcChild(nv);
                    this.loading = false;
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            calcChild: function (sbj) {
                if (!sbj.children) return 0;
                var count = sbj.children.length;
                for (var i = 0; i < sbj.children.length; i++) {
                    count += this.calcChild(sbj.children[i]);
                }
                return count;
            }
        },
        template: `<span title="子级专业数" v-if="count>0">
            <span class="el-icon-loading" v-if="loading"></span>
            <span class="sbjcount">({{count}})</span>
        </span> `
    });
});
