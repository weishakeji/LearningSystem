$ready(function () {
    window.vue = new Vue({
        el: '#vapp',
        data: {
            form: {
                orgid: '', titid: '',
                search: '', phone: '', acc: '',
                size: 20,
                index: 1
            },
            organ: {},       //当前机构
            titles: [],          //职称

            loading: false,
            loadingid: 0,        //当前操作中的对象id
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        mounted: function () {

        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.form.orgid = th.organ.Org_ID;
                    $api.get('Teacher/Titles', { 'orgid': th.organ.Org_ID, 'name': '', 'use': '' }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.titles = req.data.result;
                            th.handleCurrentChange(1);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading = false;
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });

        },
        computed: {

        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('Teacher/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        window.vue.handleCurrentChange();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 67);
                th.loading = true;
                $api.get("Teacher/Pager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.accounts = d.data.result;
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
            //在列中显示信息，包含检索
            showInfo: function (txt) {
                if (txt != '' && this.form.search != '') {
                    var regExp = new RegExp(this.form.search, 'g');
                    txt = txt.replace(regExp, `<red>${this.form.search}</red>`);
                }
                return txt;
            },
            //更改使用状态，包括使用、审核
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Th_ID;
                var form = { 'id': row.Th_ID, 'use': row.Th_IsUse, 'pass': row.Th_IsPass };
                $api.post('Teacher/ModifyState', form).then(function (req) {
                    if (req.data.success) {
                        vue.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Export', title, null, '700', '400');
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                this.$refs.btngroup.pagebox('Import', title, null, '900', '600');
            },
            //打开子页面
            openSubpage: function (page, entity, title, width, height, icon) {
                var title = '“' + entity.Th_Name + '(' + entity.Th_AccName + ')”的' + title;
                var param = { ico: icon };
                this.$refs.btngroup.pagebox(page + '?id=' + entity.Th_ID, title, null, width, height, param);
            }
        }
    });

}, ['Components/photo.js',
    "/Utilities/Components/education.js"]);