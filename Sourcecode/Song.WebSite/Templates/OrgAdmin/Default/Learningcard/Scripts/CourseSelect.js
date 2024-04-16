$ready(function () {
    window.vm = new Vue({
        el: '#vapp',
        data: {
            orgs: {},       //机构列表
            sbjs: {},        //专业
            courses: [],     //专业下的课程
            selects: [],     //当前选中的课程
            sbjids: [],      //选中的专业id,为数组
            form: {
                orgid: '',        //当前机构id
                sbjids: '',        //当前专业id，字符串
                search: '',          //课程检索的输入
                order: 'def',       //排序方式
                size: 10000,
                index: 1
            },
            loading: false
        },
        watch: {
            form: {
                handler(val, oldval) {
                    if (!oldval) return;
                    this.orgChange(val.orgid);
                    this.searchCourse();
                },
                immediate: true,
                deep: true
            },
            sbjids: {
                handler(val, oldval) {
                    if (val.length > 0) {
                        this.form.sbjids = val[val.length - 1];
                    }
                }
            }
        },
        computed: {

        },
        created: function () {
            var th = this;
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.form.orgid = result.Org_ID;
                    th.searchCourse();
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
            //获取已经选中的课程，来自父窗体 
            var parent = window.top.$pagebox.parent(window.name);
            var doc = parent.document();
            th.selects = doc.vapp.courses;
        },
        mounted: function () {
        },
        methods: {
            //当机构选择框变化时
            orgChange: function (orgid) {
                var th = this;
                $api.cache('Subject/list', { 'orgid': orgid, 'search': '', 'isuse': 'true' }).then(function (req) {
                    if (req.data.success) {
                        var result = th.buildSbjtree(req.data.result, 0);
                        th.sbjs = result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => { });
            },
            //生成专业的树形
            buildSbjtree: function (data, pid) {
                var list = new Array();
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Sbj_PID == pid)
                        list.push({
                            value: data[i].Sbj_ID,
                            label: data[i].Sbj_Name,
                            key: data[i].Sbj_ID
                        });
                }
                for (var i = 0; i < list.length; i++) {
                    list[i].children = this.buildSbjtree(data, list[i].value);
                }
                return list.length > 0 ? list : null;
            },
            //检索课程
            searchCourse: function () {
                var th = this;
                th.loading = true;
                $api.get('Course/ShowPager', th.form).then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(() => th.loading = false);
            },
            //添加选中的课程
            handleSelectCourse: function (data) {
                var arr = $api.clone(this.selects);
                this.selects = [];
                if (data) {
                    let isExist = false;
                    for (let i = 0; i < arr.length; i++) {
                        if (arr[i].Cou_ID == data.Cou_ID) {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist) arr.push(data);
                    this.selects = arr;
                } else {
                    for (let i = 0; i < this.courses.length; i++) {
                        this.handleSelectCourse(this.courses[i]);
                    }
                }
                //获取已经选中的课程，来自父窗体 
                let parent = window.top.$pagebox.parent(window.name);
                let doc = parent.document();
                doc.vapp.coursesChange(this.selects);
            },
            //清除选中的课程
            handleRemoveourse: function (data) {
                var arr = $api.clone(this.selects);
                this.selects = [];
                if (data) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].Cou_ID == data.Cou_ID) {
                            arr.splice(i, 1);
                            break;
                        }
                    }
                    this.selects = arr;
                }
                //获取已经选中的课程，来自父窗体 
                var parent = window.top.$pagebox.parent(window.name);
                var doc = parent.document();
                doc.vapp.coursesChange(this.selects);
            },
        }
    });

});