$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            form: {
                orgid: -1,
                search: '',
                size: 20,
                index: 1
            },
            loading: false,
            loadingid: 0,        //当前操作中的对象id
            datas: [],          //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [] //数据表中选中的行
        },
        mounted: function () {
            this.$nextTick(function () {

            });
            this.$refs.btngroup.addbtn({
                text: '充值码查询',
                tips: '按卡号查询所有',
                id: 'cardquery',
                type: 'info', enable: true,
                icon: 'e60f'
            });
        },
        created: function () {
            this.handleCurrentChange(1);
        },
        computed: {
           
        },
        methods: {
            //删除
            deleteData: function (datas) {
                $api.delete('RechargeCode/SetDelete', { 'id': datas }).then(function (req) {
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
                th.form.size = Math.floor(area / 41);
                $api.get("RechargeCode/SetPager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modifyrow(row);
            },
            //更改使用状态
            changeUse: function (row) {
                var th = this;
                this.loadingid = row.Rs_ID;               
                var entity = $api.clone(row);
                $api.post('RechargeCode/SetModify', { 'entity': entity }).then(function (req) {
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
            outputExcel: function (row) {
                var file = 'OutputExcel';
                var title = ' - “' + row.Rs_Theme + "”导出Excel";
                var boxid = "RechargeCode_" + row.Rs_ID + "_" + file;
                this.$refs.btngroup.pagebox(file + '?id=' + row.Rs_ID, title, boxid, 600, 400,
                    { pid: window.name, resize: true });
            },
            //导出二维码
            outputQrCode: function (row) {
                var file = 'OutputQrcode';
                var boxid = "RechargeCode_" + row.Rs_ID + "_" + file;
                var title = ' - “' + row.Rs_Theme + "”导出二维码";
                this.$refs.btngroup.pagebox(file + '?id=' + row.Rs_ID, title, boxid, 600, 400,
                    { pid: window.name, resize: true, full: true });
            },
            //学习卡查询
            cardquery: function (obj) {
                var file = 'Codequery';
                var boxid = "RechargeCode_" + obj.id + "_" + file;
                var title = ' - ' + obj.tips;
                this.$refs.btngroup.pagebox(file, title, boxid, 800, 600,
                    { pid: window.name, resize: true });
            }
        }
    });

});