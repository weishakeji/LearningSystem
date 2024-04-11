$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('id'),
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            outlines: [],     //所有章节数据
            defaultProps: {
                children: 'children',
                label: 'Ol_Name',
                value: 'Ol_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            olSelects: [],      //选择中的章节项

            form: {
                'orgid': -1, 'sbjid': -1, 'couid': '', 'olid': '',
                'type': '', 'use': '', 'error': '', 'wrong': '', 'search': '', 'size': 20, 'index': 1
            },

            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: true,
            loadingid: 0,
            loading_init: true
        },
        updated: function () {
            this.$mathjax();
        },
        mounted: function () {
            var th = this;
            th.form.couid = this.couid;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (organ, types) {
                //获取结果
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
                //获取章节
                th.getOutlineTree();
                th.handleCurrentChange(1);
            })).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading_init = false);
        },
        created: function () { },
        computed: {},
        watch: {
            'olSelects': function (nv, ov) {
                console.log(nv);
                if (nv.length > 0) this.form.olid = nv[nv.length - 1];
                else
                    this.form.olid = '';
                //关闭级联菜单的浮动层
                this.$refs["outlines"].dropDownVisible = false;
            }
        },
        methods: {
            //获取课程章节的数据
            getOutlineTree: function () {
                var th = this;
                th.loading = true;
                $api.put('Outline/Tree', { 'couid': th.couid, 'isuse': true }).then(function (req) {
                    if (req.data.success) {
                        th.outlines = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.loading = true;
                var loading = this.$fulloading();
                $api.put("Question/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        for (var i = 0; i < result.length; i++) {
                            result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }
                        th.datas = result;
                        th.$nextTick(function () {
                            loading.close();
                        });
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //刷新行数据，
            freshrow: function (id) {
                if (id == null || id == '') return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Qus_ID == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('Question/ForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        result.Qus_Title = result.Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        let index = th.datas.findIndex(item => item.Qus_ID == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                var loading = this.$fulloading();
                var quesid = datas.split(',');
                var form = { 'qusid': quesid };
                //要删除的试题,当删除后要重新统计章节、课程、专业下的试题数，所以需要提交更多id
                var ques = th.getques_selected(quesid);
                form['olid'] = th.getques_keys(ques, 'Ol_ID'); //章节id
                $api.delete('Question/Delete', form).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.getOutlineTree();
                        th.handleCurrentChange();
                        th.$nextTick(function () {
                            loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //获取选中的（要删除的）试题
            getques_selected: function (ids) {
                var arr = [];
                for (let i = 0; i < ids.length; i++) {
                    const id = ids[i];
                    var q = this.datas.find(el => el.Qus_ID == id);
                    if (q != null) arr.push(q);
                }
                return arr;
            },
            //获取试题的章节
            getques_keys: function (ques, key) {
                var arr = [];
                for (let i = 0; i < ques.length; i++)
                    arr.push(ques[i][key]);
                return Array.from(new Set(arr));
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                var url = "../Question/Export";
                url = $api.url.set(url, { 'couid': this.couid });
                this.$refs.btngroup.pagebox(url, title, null, 800, 600, { 'ico': 'e73e' });
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                var url = "../Question/Import";
                url = $api.url.set(url, { 'couid': this.couid });
                this.$refs.btngroup.pagebox(url, title, null, 900, 650, { 'ico': 'e69f' });
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Qus_ID;
                $api.post('Question/ChangeUse', { 'id': row.Qus_ID, 'use': row.Qus_IsUse }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err, '错误'))
                    .finally(() => th.loadingid = 0);
            },
            //批量修改状态
            batchState: function (use) {
                use = Boolean(use);
                var th = this;
                this.$confirm('批量更改当前页面的试题为“' + (use ? '启用' : '禁用') + '”, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var ids = '';
                    for (var i = 0; i < th.datas.length; i++) {
                        ids += th.datas[i].Qus_ID;
                        if (i < th.datas.length - 1) ids += ',';
                    }
                    var loading = this.$fulloading();
                    $api.post('Question/ChangeUse', { 'id': ids, 'use': use }).then(function (req) {
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!',
                                center: true
                            });
                            th.handleCurrentChange();
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => alert(err, '错误'))
                        .finally(() => {
                            th.$nextTick(function () {
                                loading.close();
                            });
                        });
                }).catch(() => { });
            }
        }
    });
    //显示章节名称
    Vue.component('outline_name', {
        props: ["olid"],
        data: function () {
            return {
                oultine: {},
                loading: false
            }
        },
        watch: {
            'olid': {
                handler: function (nv) {
                    if (nv == null || nv == '' || Number(nv) <= 0) return;
                    var th = this;
                    th.loading = true;
                    $api.cache('Outline/ForID', { 'id': nv }).then(function (req) {
                        if (req.data.success) {
                            th.oultine = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {

        },
        template: `<span>
        <loading v-if="loading"></loading>
        <template v-else>{{oultine.Ol_Name}}</template>
        </span>`
    });
    //章节下的试题数
    Vue.component('outline_ques_count', {
        props: ["outline"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'outline': {
                handler: function (nv) {
                    if (nv == null) return;
                    var th = this;
                    th.loading = true;
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': nv.Ol_ID, 'type': '', 'use': '' })
                        .then(function (req) {
                            if (req.data.success) {
                                th.count = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {

        },
        template: `<span class="ques_count">
        <loading v-if="loading"></loading>
        <template v-else-if="count>0">({{count}})</template>
        </span>`
    });
}, ['../Question/Components/ques_type.js']);
