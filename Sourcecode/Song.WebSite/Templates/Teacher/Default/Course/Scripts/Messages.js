$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项   

            form:
                { 'orgid': '', 'couid': '', 'olid': '', 'search': '', 'size': 20, 'index': 1 },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            modifyVisible: false,        //打开编辑面板
            modifyObj: {},       //当前要编辑的对象
            rules: {
                Msg_Context: [{ required: true, message: '不得为空', trigger: 'blur' },
                { min: 2, max: 255, message: '长度在 2 到 255 个字符', trigger: 'blur' }]
            },

            loadingid: -1,
            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;

                th.handleCurrentChange(1);
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 57);
                th.loading = true;
                $api.get("Message/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Message/DeleteBatch', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //更改状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Msg_Id;
                $api.post('Message/Modify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loadingid = 0);
            },
            //打开编辑面板
            modify_show: function (row) {
                this.modifyVisible = true;
                this.modifyObj = $api.clone(row);

                var form = this.$refs['modifyObj'];
                if (form != null) form.clearValidate();
            },
            //保存修改
            btnSave: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Message/Modify', { 'entity': th.modifyObj }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.modifyVisible = false;
                                    th.handleCurrentChange();
                                }, 100);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err, '错误');
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            }
        }
    });

});
