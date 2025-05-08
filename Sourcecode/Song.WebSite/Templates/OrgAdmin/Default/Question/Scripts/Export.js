
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid', '0'),        //课程id
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项


            subjects: [],       //专业        
            courses: [],     //专业下的课程列表    
            course: {},          //当前课程  
            outlines: [],     //章节
            outline_panel: false,        //显示章节选择的面板
            //章节过滤的字符
            outlineFilterText: '',

            form: {
                'types': [], 'diffs': [], 'part': 1, 'orgid': 0, 'sbjid': '',
                'couid': $api.querystring('couid', ''), 'olid': ''
            },
            //试题总记录
            questotal: 0,       //总记录数
            loading_total: false,   //获取试题数的加载中

            loading: true,
            loading_export: false,       //生成的预载

            files: [],
            filepanel: false      //显示文件列表的面板
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(([org, types]) => {
                th.organ = org.data.result;
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.types = types.data.result;
                if (th.couid != '' && th.couid != '0') {
                    th.getCourse(th.couid);
                }
                th.getCourses();
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
            //获取已经导出的文件
            this.getFiles();
        },
        watch: {
            //章节查询的字符
            outlineFilterText: function (val) {
                this.$refs.tree.filter(val);
            },
            //监听表单数据变化
            'form': {
                handler: function (val) {
                    this.gettotal();
                }, deep: true, immediate: false
            }
        },
        computed: {
            //禁止选择专业与课程，（例如在课程管理中的试题编辑）
            'disable_select': function () {
                var couid = $api.querystring('couid', '0');
                return couid != '0' && couid != '';
            },
            //选中的章节名称
            'selected_outline': function () {
                if (this.outlines.length <= 0) return '';
                if (this.form.olid == '' || this.form.olid < 0) return '';
                var outline = getname(this.form.olid, this.outlines);
                if (outline == null) return '';
                return outline.serial + ' ' + outline.Ol_Name;
                function getname(olid, outlines) {
                    var ol = null;
                    for (let i = 0; i < outlines.length; i++) {
                        if (outlines[i].Ol_ID == olid) {
                            ol = outlines[i];
                            break;
                        }
                        if (outlines[i].children && outlines[i].children.length > 0) {
                            ol = getname(olid, outlines[i].children);
                            if (ol != null) break;
                        }
                    }
                    return ol;
                }
            }
        },
        methods: {
            //计算试题总数
            gettotal: function () {
                var th = this;
                if (th.loading_total || th.form.orgid == 0) return;
                th.loading_total = true;
                //console.error(th.form);
                $api.get('Question/Total', th.form).then(req => {
                    if (req.data.success) {
                        th.questotal = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_total = false);
            },
            //获取当前课程
            getCourse: function (couid) {
                var th = this;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    if (req.data.success) {
                        th.course = req.data.result;
                        th.form.sbjid = th.course.Sbj_ID;
                        th.form.couid = th.course.Cou_ID;
                        //获取课程下的章节
                        th.getOultines();
                        //设置当前专业
                        th.$refs['subject'].setsbj(th.course.Sbj_ID);

                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //专业更改时
            changeSbj: function (val) {
                this.form['sbjid'] = val;
                this.outlines = [];
                this.getCourses(val);
            },
            //获取课程
            getCourses: function (sbjid) {
                var th = this;
                var orgid = th.organ.Org_ID;
                th.courses = [];
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'use': '', 'live': '', 'free': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        th.getOultines();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //当试题的课程更改时
            changeCourse: function (couid) {
                var th = this;
                this.form['couid'] = couid;
                this.getOultines();
                //当前课程的对象
                var course = this.courses.find(function (item) {
                    return item.Cou_ID == couid;
                });
                //如果没有选择专业
                var sbj = this.form['sbjid'];
                if (course) sbj = course.Sbj_ID;
                this.$refs['subject'].setsbj(sbj);
                this.getCourses(sbj);

            },
            //所取章节数据，为树形数据
            getOultines: function () {
                var th = this;
                this.loading = true;
                var couid = th.form.couid && th.form.couid != '' ? th.form.couid : -1;
                $api.cache('Outline/Tree', { 'couid': couid, 'isuse': true }).then(function (req) {
                    if (req.data.success) {
                        th.outlines = req.data.result;
                        th.calcSerial(null, '');
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => th.outlines = []).finally(() => th.loading = false);
            },
            //过滤章节树形
            filterNode: function (value, data, node) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Ol_Name.toLowerCase().indexOf(txt) !== -1;
            },
            //计算章节序号
            calcSerial: function (outline, lvl) {
                var childarr = outline == null ? this.outlines : (outline.children ? outline.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].serial = lvl + (i + 1) + '.';
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            //导出文件的按钮事件
            btnExportEvent: function () {
                if (this.questotal <= 1) {
                    alert("当前选择的试题数量为 0，无法导出");
                } else if (this.questotal <= 1000) {
                    this.exportFile();
                } else {
                    this.$confirm('试题数量 '+this.questotal+' 道, 导出时间会比较长，请耐心等待。点击确定继续', '提示', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        this.exportFile();
                    }).catch(() => { });
                }
            },
            //生成导出文件
            exportFile: function () {
                var th = this;
                var form = $api.clone(th.form);
                //将题型从数组转为字符串
                form.types = th.form.types.join(',');
                //将难度等级从数组转为字符串
                form.diffs = th.form.diffs.join(',');
                //console.log(form);
                th.loading_export = true;
                $api.get('Question/ExcelExport', form).then(function (req) {
                    if (req.data.success) {
                        let result = req.data.result;
                        console.error(result);
                        th.getFiles();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => {
                    console.error(err);
                    alert(err);
                }).finally(() => th.loading_export = false);
            },
            //获取文件列表
            getFiles: function () {
                var th = this;
                th.loading = true;
                $api.get('Question/ExcelFiles', { 'path': 'QuestionToExcel', 'couid': th.couid }).then(function (req) {
                    if (req.data.success) {
                        th.files = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err)).finally(() => th.loading = false);
            },
            //删除文件
            deleteFile: function (file) {
                var th = this;
                th.loading = true;
                $api.delete('Question/ExcelDelete', { 'filename': file, 'path': 'QuestionToExcel' }).then(function (req) {
                    if (req.data.success) {
                        th.getFiles();
                        th.$notify({
                            message: '文件删除成功！',
                            type: 'success', position: 'bottom-right', duration: 2000
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err)).finally(() => th.loading = false);
            },
        },
    });

}, ['Components/ques_type.js',
    '/Utilities/Components/sbj_cascader.js']);
