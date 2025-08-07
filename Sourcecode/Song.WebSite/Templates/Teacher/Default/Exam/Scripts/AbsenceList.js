$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                "examid": $api.querystring('id'),
                "name": "", "idcard": "", "phone": "", "stsid": "",
                "size": 8, "index": 1
            },
            entity: {},     //当前考试对象
            sorts: {},       //当前场次的学员组（根据参考学员判断）

            datas: [],       //数据
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {

        },
        created: function () {
            //获取考试信息
            var th = this;
            $api.get('Exam/ForID', { 'id': this.form.examid }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch((err) => console.error(err));
            th.getsorts();
            th.handleCurrentChange();
        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //获取学员分组，未参加考试（即缺考）学员的学员组
            getsorts: function () {
                var th = this;
                $api.get('Exam/AbsenceSort4Exam', { 'examid': this.form.examid }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //加载数据集
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loadstate.def = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 42);
                $api.get("Exam/AbsenceExamAccounts", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.def = false);
            },
            //显示手机号
            showmobi: function (row) {
                var phone = row.Ac_MobiTel1;
                return phone != '' ? phone : row.Ac_MobiTel2;
            },
            //时间是否为空
            timeisnull: function (time) {
                if (!(Object.prototype.toString.call(time) === '[object Date]')) return true;
                return false;
            },
            //复制到粘贴板
            copytext: function (val, textbox) {
                this.copy(val, textbox).then(function (th) {
                    th.$message({
                        message: '复制 “' + val + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
        },
        filters: {

        },
        components: {

        }
    });
});