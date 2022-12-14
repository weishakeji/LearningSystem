
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项    


            form: { 'orgid': '', 'use': null, 'search': '', 'index': 1, 'size': 10 },
            sorts: [],           //学员组
            total: 1, //总记录数
            totalpages: 1, //总页数

            checkAll: false,
            checkedSorts: [],       //已经选中的分组
            drawer_selected: false,  //显示选中分组信息的面板


            loading: false,
            loading_export:false,       //生成的预载

            files: [],
            filepanel:false      //显示文件列表的面板
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Organization/Current').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.config = $api.organ(th.organ).config;
                    th.form.orgid = th.organ.Org_ID;
                    th.getStudentSorts(1);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });

            this.getFiles();
        },
        watch: {
            //选择当前页的所有
            'checkAll': function (nv, ov) {
                for (var i = 0; i < this.sorts.length; i++) {
                    const element = this.sorts[i];
                    //计算当前项是否存在于被选中的数组
                    var index = this.checkedSorts.findIndex(m => {
                        return element.Sts_ID == m.Sts_ID;
                    });
                    //如果全选
                    if (nv) {
                        if (index < 0) {
                            element.selected = true;
                            this.$set(this.checkedSorts, this.checkedSorts.length, element);
                        }
                    } else {
                        if (index > 0) {
                            element.selected = false;
                            this.$delete(this.checkedSorts, index);
                        }
                    }
                }
                console.log(nv);
                console.log(this.checkedSorts);
            }
        },
        methods: {
            //获取学员组
            getStudentSorts: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 25);
                th.form.size = th.form.size <= 2 ? 2 : th.form.size;
                th.form.size = th.form.size >= 50 ? 50 : th.form.size;
                th.loading = true;
                $api.get("Account/SortPager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        for (var i = 0; i < result.length; i++) {
                            const element = result[i];
                            var index = th.checkedSorts.findIndex(m => {
                                return element.Sts_ID == m.Sts_ID;
                            });
                            result[i].selected = index >= 0;
                        }

                        th.sorts = result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //全选,nv:为true全选,false取消全选
            selectAll: function (nv) {
                for (var i = 0; i < this.sorts.length; i++) {
                    const element = this.sorts[i];
                    //计算当前项是否存在于被选中的数组
                    var index = this.checkedSorts.findIndex(m => {
                        return element.Sts_ID == m.Sts_ID;
                    });
                    //如果全选
                    if (nv) {
                        if (index < 0) {
                            element.selected = true;
                            this.$set(this.checkedSorts, this.checkedSorts.length, element);
                        }
                    } else {
                        if (index >= 0) {
                            element.selected = false;
                            this.$delete(this.checkedSorts, index);
                        }
                    }
                }
            },
            //反选
            selectReverse: function () {
                for (var i = 0; i < this.sorts.length; i++) {
                    const element = this.sorts[i];
                    //计算当前项是否存在于被选中的数组
                    var index = this.checkedSorts.findIndex(m => {
                        return element.Sts_ID == m.Sts_ID;
                    });
                    if (index < 0) {
                        element.selected = true;
                        this.$set(this.checkedSorts, this.checkedSorts.length, element);
                    } else {
                        element.selected = false;
                        this.$delete(this.checkedSorts, index);
                    }
                }
            },
            //选中单个项
            selectSingle: function (item) {
                if (item.selected) {
                    this.$set(this.checkedSorts, this.checkedSorts.length, item);
                } else {
                    var index = this.checkedSorts.findIndex(m => {
                        return item.Sts_ID == m.Sts_ID;
                    });
                    this.$delete(this.checkedSorts, index);
                }
            },
            //取消选择
            selectCancel: function (item) {
                var index = this.checkedSorts.findIndex(m => {
                    return item.Sts_ID == m.Sts_ID;
                });
                this.$delete(this.checkedSorts, index);
                for (var i = 0; i < this.sorts.length; i++) {
                    if (this.sorts[i].Sts_ID == item.Sts_ID) {
                        this.sorts[i].selected = false;
                        this.$set(this.sorts, i, this.sorts[i]);
                        break;
                    }
                }
            },
            btnOutput: function () {
                var total = 0, count = 0;
                var msg = '', sorts = '';
                if (this.checkedSorts.length > 0) {
                    this.checkedSorts.map(function (v) {
                        total += v.count;
                        sorts += v.Sts_ID + ',';
                    });
                    count = this.checkedSorts.length;
                    msg = '选中' + count + '个学员组，共' + total + '学员，是否导出?';
                } else {
                    msg = '没有选择学员组，将导出未分组的学员，是否导出?';
                }
                this.$confirm(msg, '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.excelOutput(sorts);
                }).catch(() => { });
            },
            //导出
            excelOutput: function (sorts) {
                var th = this;
                //创建生成Excel
                th.loading_export = true;
                $api.get('Account/ExcelOutputForSort', { 'orgid': th.organ.Org_ID, 'sorts': sorts }).then(function (req) {
                    th.loading_export = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            message: '成功生成Excel文件！',
                            type: 'success',
                            position: 'top-right',
                            duration: 2000
                        });
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }                  
                }).catch(function (err) {
                    th.loading_export = false;
                    console.error(err);                   
                });
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                $api.get('Account/ExcelFiles', { 'path': 'AccountsToExcelForSort' }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                        th.loading = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                this.loading = true;
                $api.delete('Account/ExcelDelete', { 'filename': file, 'path': 'AccountsToExcelForSort' }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success',
                            position: 'bottom-right',
                            duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.loadDatas', true);
            }
        },
    });
    //分组下的学员数
    Vue.component('student_count', {
        props: ["sort"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'sort': {
                handler: function (nv, ov) {
                    this.getcount(nv, nv.Sts_ID);
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getcount: function (item, sortid) {
                var th = this;
                th.loading = true;
                $api.cache('Account/SortOfNumber', { 'sortid': sortid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.count = req.data.result;
                        item.count = th.count;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        },
        template: `<span title="学员数"  :class="{'count':true,'zero':count<=0}">
                <span class="el-icon-loading" v-if="loading"></span>
                <el-tooltip v-else effect="light" content="当前组的学员数" placement="right">
                    <span>( {{count}} )</span>
                </el-tooltip>      
             </span> `
    });
});
