$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
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
            //是否折叠
            fold: true,

            loading: true,
            loadingid: -1,
            loading_sumbit: false,   //提交时的预载           
            loading_init: true
        },
        mounted: function () {
            this.$refs.btngroup.addbtn({
                text: '更新数据', tips: '更新课程数、试题数、试卷数的统计数据',
                id: 'update_data', type: 'warning',
                icon: 'e651'
            });
            this.$refs.btngroup.addbtn({
                text: '展开/折叠', tips: '展开树形或折叠树形',
                id: 'fold_open', type: 'primary',
                icon: 'e6ea'
            });
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.org = req.data.result;
                    th.form.orgid = th.org.Org_ID;
                    //机构配置信息
                    th.config = $api.organ(th.org).config;
                    th.getTreeData();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
            //$api.storage(this.expanded_storage, null);
        },
        computed: {
        },
        watch: {
            //过滤树形数据
            filterText: function (val) {
                this.$refs.tree.filter(val);
                this.fold = true;
            },
            //树形是否折叠
            fold: function (nv, ov) {
                if (nv) this.unFoldAll2(this.datas);
                else this.collapseAll2(this.datas);
            }
        },
        methods: {
            //所取专业的数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                th.total = 0;
                $api.get('Subject/Tree', th.form).then(function (req) {
                    if (req.data.success) {
                        th.datas = th.clacCount(req.data.result);
                        //获取默认展开的节点
                        var arr = $api.storage(th.expanded_storage);
                        if ($api.getType(arr) == 'Array') {
                            th.expanded = arr;
                        }
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //计算课程数，ques数，test数
            clacCount: function (datas) {
                this.calcSerial(datas);
                datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_CourseCount', 'CourseCount'));
                datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_QuesCount', 'QuesCount'));
                datas.forEach(d => this.ergodic_clacCount(d, 'Sbj_TestCount', 'TestCount'));
                return datas;
            },
            //遍历计算各个专业的课程数，包括当前专业的子专业
            //field:要计算的字段
            //result:计算结果的字段名，主要为了保留field原始值，方便恢复
            ergodic_clacCount: function (sbj, field, result) {
                let count = sbj[field];
                if (sbj.children && sbj.children.length > 0) {
                    let datas = sbj.children;
                    for (let i = 0; i < datas.length; i++)
                        count += this.ergodic_clacCount(datas[i], field, result);
                }
                sbj[result] = count;
                return count;
            },
            //计算序号
            calcSerial: function (item, lvl) {
                var childarr = Array.isArray(item) ? item : (item.children ? item.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].serial = (lvl ? lvl : '') + (i + 1) + '.';
                    childarr[i]['CourseCount'] = 0;
                    childarr[i]['QuesCount'] = 0;
                    childarr[i]['TestCount'] = 0;
                    childarr[i]['calcChild'] = this.calcChild(childarr[i]);
                    this.total++;
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            calcChild: function (sbj) {
                if (!sbj.children) return 0;
                var count = sbj.children.length;
                for (var i = 0; i < sbj.children.length; i++) {
                    count += this.calcChild(sbj.children[i]);
                }
                return count;
            },
            //拖动改变顺序
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                //console.log('tree drag end: ', dropNode && dropNode, dropType);
                var th = this;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.datas);
                $api.post('Subject/ModifyTaxis', { 'list': arr }).then(function (req) {

                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.fresh_cache();
                        th.clacCount(th.datas);
                        //th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_sumbit = false);
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
            // 全部展开
            unFoldAll2: function (data) {
                let self = this;
                data.forEach((el) => {
                    self.$refs.tree.store.nodesMap[el.Sbj_ID].expanded = true;
                    el.children && el.children.length > 0 ? self.unFoldAll2(el.children) : ""; // 子级递归
                });
            },
            // 全部折叠
            collapseAll2: function (data) {
                let self = this;
                data.forEach((el) => {
                    self.$refs.tree.store.nodesMap[el.Sbj_ID].expanded = false;
                    el.children && el.children.length > 0 ? self.collapseAll2(el.children) : ""; // 子级递归
                });
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
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
                        th.$message({
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
                    alert(err, '错误');
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
                        if (d.children && d.children.length > 0)
                            list = toarray(d.children, d.Sbj_ID, ++level, list);
                    }
                    return list;
                }
            },
            //删除节点
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
                        th.$message({
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
            //当专业数据更改时，刷新缓存数据
            fresh_cache: function () {
                $api.cache('Subject/TreeFront:update', { 'orgid': this.org.Org_ID });
            },
            //更新统计数据，包括课程数、试题数、试卷数
            update_statdata: function () {
                this.$confirm('此操作将重新计算专业的课程数、试题数、试卷数，, 是否继续?', '更新数据', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    var loading = this.$fulloading();
                    $api.post('Subject/updatestatisticaldata', { 'orgid': th.org.Org_ID, 'sbjid': '' }).then(req => {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.getTreeData();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => {
                            th.$nextTick(function () {
                                loading.close();
                            });
                        });
                }).catch(() => { });
            }

        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });
});
