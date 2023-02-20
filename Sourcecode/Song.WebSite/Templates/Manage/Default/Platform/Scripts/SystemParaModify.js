
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            rules: {
                Sys_Key: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            }         
        },
        created: function () {
            //如果是新增界面
            if (this.id == '') {
                this.entity.MM_IsShow = true;
                this.entity.MM_IsUse = true;
            } else {
                //如果是修改界面
                var th = this;
                $api.get('Platform/ParamForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            }            
        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.id == '' ? th.add() : th.modify();
                    }
                });
            },
            add: function () {
                if (this.loading) return;
                this.loading = true;
                var th = this;
                $api.post('Platform/ParamAdd', { 'entity': th.entity }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '添加成功!',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    th.loading = false;
                }).catch(function (err) {
                    alert(err);
                });
            },
            modify: function () {
                if (this.loading) return;
                this.loading = true;
                var th = this;
                $api.post('Platform/ParamModify', { 'entity': vue.entity }).then(function (req) {
                    if (req.data.success) {
                        vue.$message({
                            type: 'success',
                            message: '修改成功!',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                    th.loading = false;
                }).catch(function (err) {
                    vue.$alert(err, '错误');
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.getData', true);
            }
        },
    });

});
