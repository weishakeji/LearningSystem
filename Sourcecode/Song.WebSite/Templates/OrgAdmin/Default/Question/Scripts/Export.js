
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: Number($api.querystring('couid', 0)),        //课程id
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            subjects: [],       //专业
            sbjids: [],

            courses: [],     //课程列表      

            outlines: [],     //章节
            outline_panel: false,        //显示章节选择的面板
            //outline_se
            defaultOutlinesProps: {
                children: 'children',
                label: 'Ol_Name'
            },
            outlineFilterText: '',

            form: { 'types': [], 'diffs': [], 'part': 1, 'sbjid': '', 'couid': '', 'olid': '' },

            loading: false,
            loading_export: false,       //生成的预载

            files: [],
            filepanel: false      //显示文件列表的面板
        },
        created: function () {
            this.couid = isNaN(this.couid) ? 0 : this.couid;

            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (org, types) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                th.organ = org.data.result;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;

                th.getSubjects(th.organ);
                if(th.couid>0){
                    th.changeCourse(th.couid);
                }
            })).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });

            // this.getFiles();
        },
        watch: {

        },
        computed: {
            //禁止选择专业与课程，（例如在课程管理中的试题编辑）
            'disable_select': function () {
                var couid = Number($api.querystring('couid', 0));
                couid = isNaN(couid) ? 0 : couid;
                return couid != 0;
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
            enter: function () {
                console.log(this.form);
            },
            //获取课程专业的数据
            getSubjects: function (organ) {
                if (organ == null || !organ || !organ.Org_ID) return;
                var th = this;
                var form = { orgid: organ.Org_ID, search: '', isuse: null };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        //将当前课程的专业，在控件中显示
                        var sbjid = th.form.sbjid && th.form.sbjid > 0 ? th.form.sbjid : 0;
                        sbjid = th.course && th.course.Sbj_ID > 0 ? th.course.Sbj_ID : 0;
                        if (sbjid > 0) {
                            var arr = [];
                            arr.push(sbjid);
                            var sbj = th.traversalQuery(sbjid, th.subjects);
                            if (sbj == null) {
                                throw '课程的专业“' + th.course.Sbj_Name + '”不存在，或该专业被禁用';
                            }
                            arr = th.getParentPath(sbj, th.subjects, arr);
                            th.sbjids = arr;
                        }
                        th.getCourses();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //专业更改时
            changeSbj: function (val) {
                this.form['sbjid'] = val.length > 0 ? val[val.length - 1] : 0;
                this.outlines = [];
                this.getCourses();
                //关闭级联菜单的浮动层
                this.$refs["subjects"].dropDownVisible = false;
            },
            //获取当前专业的上级路径
            getParentPath: function (entity, datas, arr) {
                if (entity == null) return null;
                var obj = this.traversalQuery(entity.Sbj_PID, datas);
                if (obj == null) return arr;
                arr.splice(0, 0, obj.Sbj_ID);
                arr = this.getParentPath(obj, datas, arr);
                return arr;
            },
            //从树中遍历对象
            traversalQuery: function (sbjid, datas) {
                var obj = null;
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    if (d.Sbj_ID == sbjid) {
                        obj = d;
                        break;
                    }
                    if (d.children && d.children.length > 0) {
                        obj = this.traversalQuery(sbjid, d.children);
                        if (obj != null) break;
                    }
                }
                return obj;
            },
            //获取课程
            getCourses: function () {
                var th = this;
                var orgid = th.organ.Org_ID;
                var sbjid = 0;
                if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
                th.courses = [];
                $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        th.getOultines();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //当试题的课程更改时
            changeCourse: function (val) {
                var th = this;
                this.form['couid'] = val;
                this.getOultines();
                //如果没有选择专业
                var sbj = this.form['sbjid'];
                var course = this.courses.find((item) => {
                    return item.Cou_ID == val;
                });
                if (course && sbj != course.Sbj_ID) {
                    this.form['sbjid'] = course.Sbj_ID;
                    this.sbjids = [];
                    var arr = [];
                    arr.push(course.Sbj_ID);
                    var sbj = th.traversalQuery(course.Sbj_ID, th.subjects);
                    arr = th.getParentPath(sbj, th.subjects, arr);
                    this.sbjids = arr;
                    this.getCourses();
                }
            },
            //所取章节数据，为树形数据
            getOultines: function () {
                var th = this;
                this.loading = true;
                var couid = th.form.couid && th.form.couid != '' ? th.form.couid : -1;
                $api.get('Outline/Tree', { 'couid': couid, 'isuse': true }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.outlines = req.data.result;
                        th.calcSerial(null, '');
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.outlines = [];
                });
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
            //章节节点点击事件
            outlineClick: function (data, node, el) {
                this.form.olid = data.Ol_ID;
                this.outline_panel = false;
            }
        },
    });

});
