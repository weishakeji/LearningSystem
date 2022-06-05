$ready(function () {
    window.vm = new Vue({
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
                order:'def',       //排序方式
                size: 10000,
                index: 1
            },
            loading: false
        },
        watch: {
            form: {
                handler(val, oldval) {
                    if (!oldval) return;
                    vm.orgChange(val.orgid);
                    vm.searchCourse();
                },
                immediate: true,
                deep: true
            },
            sbjids: {
                handler(val, oldval) {
                    if (val.length > 0) {
                        vm.form.sbjids = val[val.length - 1];
                    }
                }
            }
        },
        computed: {
          
        },
        created: function () {
            $api.get('Organization/AllUse').then(function (req) {
                if (req.data.success) {
                    vm.orgs = req.data.result;
                    window.setTimeout(function () {
                        if (vm.orgs.length > 0) vm.form.orgid = vm.orgs[0].Org_ID;
                        vm.searchCourse();
                    }, 100);

                    //获取已经选中的课程，来自父窗体 
                    var parent = window.top.$pagebox.parent(window.name);
                    var doc = parent.document();
                    vm.selects = doc.vapp.courses;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });

        },
        mounted: function () {
        },
        methods: {
            //当机构选择框变化时
            orgChange: function (orgid) {
                $api.cache('Subject/list', { 'orgid': orgid,'search':'','isuse':'true' }).then(function (req) {
                    if (req.data.success) {
                        var result = vm.buildSbjtree(req.data.result, 0);
                        vm.sbjs = result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
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
                    list[i].children = vm.buildSbjtree(data, list[i].value);
                }
                return list.length > 0 ? list : null;
            },
            //检索课程
            searchCourse: function () {
                $api.get('Course/ShowPager', vm.form).then(function (req) {
                    if (req.data.success) {
                        vm.courses = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            //添加选中的课程
            handleSelectCourse: function (data) {
                var arr = $api.clone(vm.selects);
                vm.selects = [];
                if (data) {
                    var isExist = false;
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].Cou_ID == data.Cou_ID) {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist) arr.push(data);
                    vm.selects = arr;
                } else {
                    for (var i = 0; i < vm.courses.length; i++) {
                        vm.handleSelectCourse(vm.courses[i]);
                    }
                }
                //获取已经选中的课程，来自父窗体 
                var parent = window.top.$pagebox.parent(window.name);
                var doc = parent.document();
                doc.vapp.coursesChange(vm.selects);
            },
            //清除选中的课程
            handleRemoveourse: function (data) {
                var arr = $api.clone(vm.selects);
                vm.selects = [];
                if (data) {                   
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].Cou_ID == data.Cou_ID) {
                            arr.splice(i, 1);
                            break;
                        }
                    }
                    vm.selects = arr;
                }
                //console.log(vm.selects);
                //获取已经选中的课程，来自父窗体 
                var parent = window.top.$pagebox.parent(window.name);
                var doc = parent.document();
                doc.vapp.coursesChange(vm.selects);
            },
            handleEnter: function (data) {
            },
            //保存信息
            btnEnter: function () {
                if (vm.loading) return;
            },

        }
    });
    vm.$mount('#app');

    $api.effect(function () {
        vm.loading = true;
    }, function (response) {
        vm.loading = false;
    });
});