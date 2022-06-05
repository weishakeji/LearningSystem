$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {}, //当前登录账号对象
            accPingyin: [],  //账号名称的拼音

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getAccount(null);
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取当前登录账号
            getAccount: function (func) {
                var th = this;
                th.loading_init = true;
                $api.get('Account/Current').then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.account = req.data.result;
                        if (func != null) func(th.account);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //名称转拼音
            pingyin: function () {
                this.accPingyin = makePy(this.account.Ac_Name);
                if (this.accPingyin.length > 0)
                    this.account.Ac_Pinyin = this.accPingyin[0];
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.remove_redundance(th.account);                       
                        th.loading = true;
                        var apipath = 'Account/ModifyJson';
                        $api.post(apipath, { 'acc': obj }).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                window.setTimeout(function () {
                                    th.getAccount(th.operateSuccess);
                                }, 100);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            operateSuccess: function (acc) {
                window.top.vapp.accountChange(acc);
            },
            //清理冗余的属性，仅保持当前form表单的属性，未在表单中的不提交到服务器
            remove_redundance: function (obj) {
                //表单中的字段
                var props = ['Ac_ID'];
                var fields = this.$refs['account'].fields;
                for (var i = 0; i < fields.length; i++)
                    props.push(fields[i].prop);
                //obj的属性字段,如果表单上没有，则删除               
                for (let att in obj) {
                    var exist = false;
                    for (var i = 0; i < props.length; i++) {
                        if (att == props[i]) {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)delete obj[att];
                }
                return obj;
            }
        }
    });

}, ["/Utilities/Scripts/hanzi2pinyin.js",
    "/Utilities/Components/education.js"]);
