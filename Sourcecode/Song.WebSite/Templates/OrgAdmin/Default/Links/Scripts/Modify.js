
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            //当前数据实体
            entity: {
                Lk_IsUse: true,
                Lk_IsShow: true,
                Lk_Logo: '',
                Lk_LogoSmall: ''
            },
            sorts: [],       //友情链接的分类
            rules: {
                Lk_Name: [
                    { required: true, message: '名称不得为空', trigger: 'blur' },
                    { min: 2, max: 255, message: '长度在 2 到 255 个字符', trigger: 'blur' }
                ]
            },
            //图片文件
            upfile: null, //本地上传文件的对象              

            loading: false,
            loading_init: true
        },
        computed: {
            //是否新增对象
            isadd: t => { return t.id == null || t.id == ''; },
        },
        created: function () {
            var th=this;
            $api.get('Organization/Current').then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    $api.get('Link/SortCount',
                        { 'orgid': th.organ.Org_ID, 'use': true, 'show': '', 'search': '', 'count': 0 })
                        .then(function (req) {
                            if (req.data.success) {
                                th.sorts = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
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
        mounted: function () {
            var th = this;
            if (th.id == '') return;
            th.loading = true;
            $api.get('Link/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.entity = result;
                    if (th.entity.Ls_Id == 0)
                        th.entity.Ls_Id = '';
                } else {
                    throw '未查询到数据';
                }
            }).catch(function (err) {
                th.$alert(err, '错误');
            });
        },
        methods: {
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        //接口路径
                        var apipath = 'Link/' + (th.id == '' ? 'add' : 'Modify');
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'entity': th.entity };
                        else
                            para = { 'file': th.upfile, 'entity': th.entity };
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$notify({
                                    type: 'success',
                                    message: '操作成功!',
                                    position: 'bottom-left'
                                });
                                window.setTimeout(function () {
                                    th.operateSuccess(isclose);
                                }, 600);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //图片文件上传
            filechange: function (file) {
                var th = this;
                th.upfile = file;
            },
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.entity.Lk_Logo = '';
                this.entity.Lk_LogoSmall = '';
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
            }
        },
    });

});
