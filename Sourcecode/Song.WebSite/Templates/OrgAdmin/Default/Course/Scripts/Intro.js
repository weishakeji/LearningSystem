$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //课程id           
            course: {},     //当前课程
            form: {

            },
            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            if (th.id == '' || th.id == null) return;
            $api.put('Course/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                    document.title = '课程简价：' + th.course.Cou_Name;
                } else {
                    throw '未查询到数据';
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading_init = false);

        },
        created: function () {

        },
        computed: {

        },
        watch: {

        },
        methods: {

            btnEnter: function (formName) {
                this.$confirm('此操作仅保存“课程简介”的内容，不包括课程管理的其它选项卡, 是否继续?', '是否保存修改', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    if (th.loading) return;
                    th.loading = true;
                    var obj = { 'Cou_ID': th.course.Cou_ID };
                    obj['Cou_Intro'] = th.course.Cou_Intro;
                    $api.post('Course/ModifyJson', { 'course': obj }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                type: 'success',
                                message: '修改成功!',
                                center: true
                            });
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => alert(err))
                        .finally(() => th.loading = false);
                });
            },
            //为上传数据作处理
            clone: function (entity) {
                var props = ['Cou_Intro'];
                //通过prop，保留仅表单项的entity属性，以防止提交冗余数据
                var obj = {};
                for (let k in entity) {
                    const index = props.findIndex(item => item == k);
                    if (index >= 0) obj[k] = entity[k];
                }
                obj['Cou_ID'] = this.id;
                return obj;
            },
        }
    });

});
