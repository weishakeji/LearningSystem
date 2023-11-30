
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
            btnEnter: function (isclose) {
                var th = this;
                this.$refs['entity'].validate((valid) => {
                    if (valid) {
                        let path=th.id == '' ? 'Platform/ParamAdd' : 'Platform/ParamModify';                     
                        th.loading = true;
                        $api.post(path, { 'entity': th.entity }).then(function (req) {
                            if (req.data.success) {
                                th.$notify({
                                    type: 'success', position: 'bottom-left',
                                    message: '操作成功!'
                                });
                                th.operateSuccess(isclose);
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                            th.loading = false;
                        }).catch(function (err) {
                            alert(err);
                        }).finally(()=> th.loading = false);
                    }
                });
            },         
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.getData', isclose);
            }
        },
    });

});
