$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            thid: $api.querystring('thid'),  //创建课程的教师
            organ: {},
            config: {},      //当前机构配置项        
            teacher: {},        //当前登录的教师
            form: { 'name': '', 'orgid': 0, 'sbjid': 0, 'thid': 0 },
            entity: {},
            //图片文件
            upfile: null, //本地上传文件的对象    
            subjects: [],        //专业数据 
            filterText: '',      //专业过虑字符
            sbj_panel: false,       //是否显示专业选择的面板

            success: false,       //操作成功
            countdown: 5,            //成功后的倒计时
            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Teacher/ForID', { 'id': th.thid })
            ).then(axios.spread(function (organ, teach) {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.getTreeData();
                th.form.orgid = th.organ.Org_ID;
                th.teacher = teach.data.result;
                if (th.teacher)
                    th.form.thid = th.teacher.Th_ID;
            })).catch(function (err) {
                //Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {
            console.log(window.name);
            //document.write(window.name);
        },
        computed: {},
        watch: {
            //专业树形的过滤
            filterText: function (val) {
                this.$refs.tree.filter(val);
            },
            //完成后的倒计时，小于零退出当前窗口
            countdown: function (val) {
                if (val < 0) {
                    this.callback_modify(this.entity.Cou_ID);
                }
            }
        },
        methods: {
            //所取专业的数据，为树形数据
            getTreeData: function () {
                var th = this;
                th.loading = true;
                $api.get('Subject/Tree', {
                    orgid: this.organ.Org_ID, search: '', isuse: true
                }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.subjects = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert('没有查询到专业信息，请先创建专业信息', '错误', {
                        confirmButtonText: '确定',
                        callback: action => {
                            if (window.top.$pagebox) {
                                var box = window.top.$pagebox.get(window.name);
                                if (box) box.shut();
                            }
                        }
                    });
                    console.error(err);
                });
            },
            //选中专业
            select_sbj(data, node, tree) {
                this.sbj_panel = false;
                this.form.sbjid = data.Sbj_ID;
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Sbj_Name.toLowerCase().indexOf(txt) !== -1;
            },
            //专业的路径，从子级上溯
            subjectPath: function () {
                var sbjid = this.form.sbjid;
                if (sbjid == 0) return '';
                //获取专业的路径，从顶级到子级
                var arr = [];
                var sbj = null;
                do {
                    sbj = getsbj(sbjid, this.subjects);
                    if (sbj == null) break;
                    sbjid = sbj.Sbj_PID;
                    arr.push(sbj);
                } while (sbj && sbj.Sbj_PID > 0);
                //输出专业的路径
                var path = '';
                for (let i = arr.length - 1; i >= 0; i--) {
                    path += arr[i].Sbj_Name;
                    if (i != 0) path += '<b>／</b>'
                }
                return path != '' ? path : (course ? course.Sbj_Name : '');
                function getsbj(id, arr) {
                    var obj = null;
                    for (let i = 0; i < arr.length; i++) {
                        const el = arr[i];
                        if (arr[i].Sbj_ID == id) {
                            obj = arr[i];
                            break;
                        }
                        if (arr[i].children && arr[i].children.length > 0) {
                            obj = getsbj(id, arr[i].children);
                            if (obj != null) return obj;
                        }
                    }
                    return obj;
                }
            },
            btnEnter: function () {
                var th = this;
                if ($api.trim(this.form.name) == '') {
                    this.$alert('课程名称不得为空！', '提示', {
                        showConfirmButton: false,
                        closeOnClickModal: true,
                        type: 'warning',
                        callback: () => { }
                    });
                    return;
                }
                if (this.form.sbjid == 0) {
                    this.$alert('请选择课程专业！', '提示', {
                        showConfirmButton: false,
                        closeOnClickModal: true,
                        type: 'warning',
                        callback: () => { }
                    });
                    return;
                }
                //判断是否存在重名
                var query = { 'name': th.form.name, 'orgid': th.form.orgid, 'sbjid': th.form.sbjid };
                $api.get('Course/NameExist', query).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result == true) {
                            th.$confirm('在当前选择的课程专业下，课程名称存在重复，是否继续创建课程?', '提示', {
                                confirmButtonText: '确定',
                                cancelButtonText: '取消',
                                type: 'warning'
                            }).then(() => {
                                th.create_course();
                            }).catch(() => { });
                        } else {
                            th.create_course();
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //创建课程
            create_course: function () {
                var th = this;
                th.loading = true;
                var para = $api.clone(th.form);
                if (th.upfile != null) para.file = th.upfile;
                $api.post('Course/Add', para).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                        //console.error('课程id:'+th.entity.Cou_ID);
                        th.operateSuccess();
                        th.success = true;
                        window.setInterval(function () {
                            if (th.countdown >= 0)
                                th.countdown--;
                        }, 1000);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //回调课程编辑（创建课程成功后，打开更详细的课程编辑界面）
            callback_modify: function (id) {
                //console.error('callback_modify 课程id:'+id);
                //打开编辑界面
                if (window.top.$pagebox && window.top.$tabs && this.workplace() == 'orgadmin') {
                    window.top.$pagebox.source.tab(window.name, 'vapp.btnmodify("' + id + '")', true);
                } else {
                    //如果处在学员或教师管理界面
                    var winname = window.name;
                    if (winname.indexOf('_') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('_'));
                    if (winname.indexOf('[') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('['));

                    window.top.vapp.fresh(winname, 'vapp.btnmodify("' + id + '")');
                    window.setTimeout(function () {
                        window.top.$pagebox.shut(window.name);
                    }, 1000);
                }
            },
            //操作成功
            operateSuccess: function () {
                //课程列表重新加载
                if (window.top.$pagebox && window.top.$tabs && this.workplace() == 'orgadmin') {
                    window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', false);
                } else {
                    //如果处在学员或教师管理界面
                    var winname = window.name;
                    if (winname.indexOf('_') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('_'));
                    if (winname.indexOf('[') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('['));
                    window.top.vapp.fresh(winname, 'vapp.handleCurrentChange');
                }
            },
            //当前工作环境，是处于机构管理，还是教师或学员管理
            workplace:function(){
                let meta = $dom('meta[device]', window.top.document.documentElement);
                let device = meta.attr('device'); 
                return device;
            },
        }
    });

});
