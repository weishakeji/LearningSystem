$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},      //当前机构
            //图片文件
            upfile: null, //本地上传文件的对象  
            //公章信息 
            stamp: {
                "positon": "right-bottom",
                "path": ""
            },
            //位置
            positions: [
                { "text": "左上", "posi": "left-top" },
                { "text": "右上", "posi": "right-top" },
                { "text": "左下", "posi": "left-bottom" },
                { "text": "右下", "posi": "right-bottom" }],

            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    $api.get('Organization/Stamp', { 'orgid': th.organ.Org_ID }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.stamp = req.data.result;
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
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });

        },
        created: function () {
        },
        computed: {

        },
        watch: {
        },
        methods: {
            //图片文件上传
            filechange: function (file) {
                var th = this;
                th.upfile = file;
            },
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.stamp.path = '';
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid, obj) => {
                    if (valid) {
                        th.loading = true;
                        var apipath = 'Organization/StampUpdate';
                        //接口参数，如果有上传文件，则增加file
                        var para = { 'stamp': th.stamp, 'orgid': th.organ.Org_ID };
                        if (th.upfile != null) para['file'] = th.upfile;
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        return false;
                    }
                });
            },
        }
    });

});
