$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {},         //当前实体
            organ: {},
            admin: {},       //当前管理员
            teacher: {},     //当前教师

            subjects: [],     //所有专业数据           
            //图片文件
            upfile: null, //本地上传文件的对象   

            rules: {
                Cou_Tax: [{ required: true, message: '不得为空', trigger: 'blur' }]
            },
            loading_obj: {},
            loading: false,
            loading_init: true   //初始加载
        },
        mounted: function () {
            var th = this;
            this.loading_obj = this.$fulloading();
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Admin/General'),
                $api.get('Teacher/Current')
            ).then(([org, admin, teach]) => {
                //获取结果
                th.organ = org.data.result;
                th.admin = admin.data.result;
                th.teacher = teach.data.result;
                //th.getCourse();
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //有没有图片
            'islogo': function () {
                var et = this.entity;
                var etlogo = JSON.stringify(et) != '{}' && et != null && et['Cou_Logo'];
                return etlogo || this.upfile != null
            },
            //管理员是否登录
            'adminlogin': function () {
                return !$api.isnull(this.admin);
            },
            //教师是否登录
            'teachlogin': function () {
                return !$api.isnull(this.teacher);
            },
            //是否处于教师管理页面
            'teachpage': function () {
                var href = window.top.location.href.toLowerCase();
                return href.indexOf('/web/teach') > -1;
            }

        },
        watch: {

        },
        methods: {
            //获取课程实体
            getCourse: function () {
                var th = this;
                if (th.id == '' || th.id == null) return;
                $api.put('Course/ForID', { 'id': th.id }).then(function (req) {
                    th.loading_obj.close();
                    if (req.data.success) {
                        th.entity = req.data.result;
                        if (th.$refs['subject']) th.$refs['subject'].setsbj(th.entity.Sbj_ID);
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_init = false);
            },
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        this.$confirm('此操作仅保存“基础信息”的内容，不包括课程管理的其它选项卡, 是否继续?', '是否保存修改', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(() => {
                            var obj = th.clone(th.entity);
                            //接口参数，如果有上传文件，则增加file
                            var para = { 'course': obj };
                            if (th.upfile != null) para['file'] = th.upfile;
                            if (th.loading) return;
                            th.loading = true;
                            $api.post('Course/Modify', para).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.$message({
                                        type: 'success',
                                        message: '修改成功!',
                                        center: true
                                    });
                                    th.fresh_parent(result.Cou_ID);
                                } else {
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                alert(err);
                            }).finally(() => th.loading = false);
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }

                });
            },
            //为上传数据作处理
            clone: function (entity) {
                //获取表单项中的prop的值
                var formitems = this.$refs['entity'].fields;
                var props = [];
                for (let i = 0; i < formitems.length; i++) {
                    let prop = formitems[i].$options.propsData.prop;
                    props.push(prop);
                }
                //通过prop，保留仅表单项的entity属性，以防止提交冗余数据
                var obj = {};
                for (let k in entity) {
                    const index = props.findIndex(item => item == k);
                    if (index >= 0) obj[k] = entity[k];
                }
                obj['Cou_ID'] = this.id;
                return obj;
            },
            //调用父级方法
            fresh_parent: function (id) {
                var win = window.parent;
                if (win && win.vapp && win.vapp.close_fresh) {
                    win.vapp.close_fresh('vapp.freshrow("' + id + '")');
                }
            }
        }
    });

});
