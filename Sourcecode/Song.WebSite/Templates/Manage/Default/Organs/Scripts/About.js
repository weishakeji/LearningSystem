
$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            width: 500,
            height: 400,
            quality: 80
        }
    });
    window.vue = new Vue({
        el: '#app',
        data: {
            details: '',
            account: {}, //当前登录账号对象
            organ: {},       //当前机构
            loading: false
        },
        created: function () {
            $api.post('Admin/Super').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    vue.account = result;
                    $api.get('Organization/ForID', { 'id': result.Org_ID }).then(function (req) {
                        if (req.data.success) {
                            vue.organ = req.data.result;
                            vue.details = vue.organ.Org_Intro;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    throw '未登录，或登录状态已失效';
                }
            }).catch(function (err) {
                vue.account = null;
                alert(err);
            });
        },
        methods: {
            //当编辑的内容变更时
            changeIntro(data) {
                this.organ.Org_Intro = data;
            },
            updateDetails: function () {
                this.organ.Org_Intro = this.$refs.editor.getContent();         
                var th=this;                
                th.loading = true;
                $api.post('Organization/Modify', { 'entity': this.organ }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ["/Utilities/editor/vue-html5-editor.js"]);
