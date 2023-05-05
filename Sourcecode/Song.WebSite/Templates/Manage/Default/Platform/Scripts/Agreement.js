
$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            activeName: 'first',
            accounts_details: '',
            teacher_details: '',
            //
            state: {
                accounts: 'Agreement_accounts',  //学员注册协议
                teacher: 'Agreement_teacher',    //教师注册协议
            },
            //提示信息
            tooltip:{
                platform:'平台名称',
                org:'机构简称',
                domain:'域名'
            }
        },
        created: function () {
            $api.bat(
                $api.post('Platform/Parameter', { 'key': this.state.accounts }),
                $api.post("Platform/Parameter", { 'key': this.state.teacher })
            ).then(axios.spread(function (accounts, teacher) {
                //获取结果
                vue.accounts_details = accounts.data.result;
                vue.teacher_details = teacher.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            updateDetails: function (data) {
                var text = this[data + '_details'];
                $api.post('Platform/ParamUpdate', { 'key': 'Agreement_' + data, 'val': text }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });
});
