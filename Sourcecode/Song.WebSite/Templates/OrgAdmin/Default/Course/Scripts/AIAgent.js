$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //课程Id
            course: {},     //当前课程
            rules: {

            },
            loading: false,
        },
        mounted: function () {
            var th = this;
            if (th.id == '' || th.id == null) return;
            th.loading = true;
            $api.put('Course/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                } else {
                    throw '未查询到数据';
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {

        },
        watch: {

        },
        methods: {
            //更改课程的设置项
            updateCourse: function () {
                if (JSON.stringify(this.course) == '{}') return;
                var th = this;
                th.loading_course = true;
                //去除不相关属性
                var obj = th.remove_redundance(th.course);
                $api.post('Course/ModifyJson', { 'course': obj }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            position: 'bottom',
                            message: '修改课程信息成功',
                            center: true
                        });
                        th.fresh_course();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading_course = false);
            },
            //清理冗余的属性，仅保持当前form表单的属性，未在表单中的不提交到服务器
            remove_redundance: function (obj) {
                //表单中的字段
                const props = ['Cou_ID'];
                // 从 Vue 实例的 $refs 对象中获取名为 'course' 的引用，
                const fields = this.$refs['course'].fields;
                for (let field of fields) props.push(field.prop);
                const propSet = new Set(props);
                for (let att in obj)
                    if (!propSet.has(att)) delete obj[att];

                console.log(obj);
                return obj;
            },
            //刷新课程列表中的状态
            fresh_course: function () {
                var couid = this.course.Cou_ID;
                var win = window.parent;
                if (win && win.vapp && win.vapp.close_fresh) {
                    win.vapp.close_fresh('vapp.freshrow("' + couid + '")');
                    //win.vapp.close_fresh('vapp.handleCurrentChange()');
                }
            }
        },
        filters: {

        },
        components: {

        }
    });
});