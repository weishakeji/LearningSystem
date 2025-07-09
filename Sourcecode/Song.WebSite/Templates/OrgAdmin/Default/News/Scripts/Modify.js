
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            //当前数据实体
            entity: {
                Art_ID: 0,
                Art_IsUse: true,
                Art_IsShow: true,
                Art_Details: '',
                Art_Logo: '',
                Art_Uid: new Date().getTime(),
                Col_UID: []
            },
            accessories: [],     //新闻附件
            columns: [],       //新闻栏目
            defaultProps: {
                children: 'children',
                label: 'Col_Name',
                value: 'Col_UID',
                expandTrigger: 'hover',
                checkStrictly: true
            },

            activeName: 'tab1',     //选项卡
            rules: {
                Art_Title: [{ required: true, message: '标题不得为空', trigger: 'blur' }],
                Col_UID: [{ type: 'array', required: true, message: '请选择新闻栏目', trigger: 'change' }]
            },
            //图片文件
            upfile: null, //本地上传文件的对象


            loading: false,
            loading_init: true,
            loading_upload: false        //附件上传的预载
        },
        computed: {
            //是否新增账号
            isadd: t => t.id == null || t.id == '' || this.id == 0,
        },
        watch: {
        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    $api.get('News/ColumnsTree', { 'orgid': th.organ.Org_ID })
                        .then(function (req) {
                            if (req.data.success) {
                                th.columns = req.data.result;
                                th.getAtricle();
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err))
                        .finally(() => { });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }

            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        mounted: function () {

        },
        methods: {
            //获取文章信息
            getAtricle: function () {
                var th = this;
                if (th.id == '') {
                    $api.get('Snowflake/Generate').then(function (req) {
                        if (req.data.success) {
                            th.entity.Art_ID = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                    return;
                }
                th.loading = true;
                $api.get('News/Article', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.entity = result;
                        //将当前新闻文章的分类，在控件中显示
                        var arr = [];
                        arr.push(th.entity.Col_UID);
                        var sbj = th.traversalQuery(th.entity.Col_UID, th.columns);
                        if (sbj == null) {
                            throw '文章的栏目“' + th.entity.Col_Name + '”不存在，或该栏目被禁用';
                        }
                        arr = th.getParentPath(sbj, th.columns, arr);
                        th.entity.Col_UID = arr;
                        //加载附件
                        th.getAccessory();
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => th.loading = false);
            },
            btnEnter: function (formName, isclose) {
                var th = this;
                this.$refs[formName].validate((valid, fields) => {
                    if (valid) {
                        if (th.loading) return;
                        th.loading = true;
                        //为上传数据作处理
                        var obj = $api.clone(th.entity);
                        if ($api.getType(obj.Col_UID) == "Array" && obj.Col_UID.length > 0)
                            obj.Col_UID = obj.Col_UID[obj.Col_UID.length - 1];
                        //接口路径
                        var apipath = 'News/' + (th.id == '' ? 'add' : 'Modify');
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'entity': obj };
                        else
                            para = { 'file': th.upfile, 'entity': obj };
                        $api.post(apipath, para).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                //刷新客户端与服务器端缓存
                                $api.cache('News/Article:clear', { 'id': th.id })
                                    .catch(err => console.error(err))
                                    .finally(() => {
                                        th.operateSuccess(isclose);
                                    });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => th.loading = false);
                    } else {
                        //未通过验证的字段
                        let field = Object.keys(fields)[0];
                        let label = $dom('label[for="' + field + '"]');
                        while (label.attr('tab') == null)
                            label = label.parent();
                        th.activeName = label.attr('tab');
                        return false;
                    }
                });
            },
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.entity.Art_Logo = '';
            },
            //获取当前栏目的上级路径
            getParentPath: function (entity, datas, arr) {
                if (entity == null) return null;
                var obj = this.traversalQuery(entity.Col_PID, datas);
                if (obj == null) return arr;
                arr.splice(0, 0, obj.Col_UID);
                arr = this.getParentPath(obj, datas, arr);
                return arr;
            },
            //从树中遍历对象
            traversalQuery: function (uid, datas) {
                var obj = null;
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    if (d.Col_UID == uid) {
                        obj = d;
                        break;
                    }
                    if (d.children && d.children.length > 0) {
                        obj = this.traversalQuery(uid, d.children);
                        if (obj != null) break;
                    }
                }
                return obj;
            },
            //获取附件
            getAccessory: function () {
                var th = this;
                var uid = this.entity.Art_Uid;
                $api.cache('News/Accessory:clear', { 'uid': uid }).then(function (req) {
                    if (req.data.success) {
                        th.accessories = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //附件文件上传
            uploadAccessory: function (file) {
                var th = this;
                th.loading_upload = true;
                var uid = this.entity.Art_Uid;
                $api.post('Accessory/Upload', { 'uid': uid, 'type': 'News', 'file': file }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getAccessory();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_upload = false);
            },
            //删除附件
            delAccessory: function (obj) {
                var th = this;
                var id = obj.As_Id;
                $api.delete('Accessory/DeleteForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getAccessory();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        },
    });

});
