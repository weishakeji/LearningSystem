//试题编辑中的确定按钮
Vue.component('enter_button', {
    //question：当前试题
    //verify:验证试题是否满足编辑条件的方法
    props: ["question", "verify", "organ"],
    data: function () {
        return {
            loading: false,
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/enter_button.css']);
    },
    methods: {
        //常规验证，主要验证试题所属专业、课程等
        general_verify: function () {
            if (!this.question) return false;
            var th = this;
            let qus = this.question;
            //题干不得为空
            if (!qus.Qus_Title || qus.Qus_Title == '') {
                return this.alert('试题题干不得为空！', 0);
            }
            //是否设置专业
            if (!qus.Sbj_ID || qus.Sbj_ID <= 0) {
                return this.alert('试题必须设置所属专业', 1);
            }
            //是否设置课程
            if (!qus.Cou_ID || qus.Cou_ID <= 0) {
                return this.alert('试题必须设置所属课程', 1);
            }
            return true;
        },
        //提交
        btnEnter: function () {
            //通用验证
            if (!this.general_verify()) return;
            //自定义验证
            if (this.verify) {
                if (!this.verify(this.question, this.alert)) return;
            }
            var th = this;
            th.loading = true;
            var isadd = this.question.Qus_ID && this.question.Qus_ID > 0 ? false : true;
            var apipath = isadd ? api = 'add' : 'Modify';
            if (isadd) {
                th.question.Org_ID = th.organ.Org_ID;
            }
            $api.post('Question/' + apipath, { 'entity': th.question }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    th.$message({
                        type: 'success',
                        message: '操作成功!',
                        center: true
                    });
                    //如果是新增
                    //if (isadd) {
                    //var url = $api.dot(result, window.location.href);
                    //window.location.href = url;
                    window.setTimeout(function () {
                        th.operateSuccess();
                    }, 300);
                    //}
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                th.$alert(err, '错误');
            });
        },
        //提示信息
        alert: function (msg, tabindex) {
            var th = this;
            this.$alert(msg, '提示', {
                confirmButtonText: '确定',
                distinguishCancelAndClose: true,
                type: 'warning',
                callback: action => {
                    if (action == 'confirm') {
                        if (tabindex != null) {
                            var p = th.$parent;
                            if (p) p.$refs['main'].setindex(tabindex);
                        }
                    }
                }
            });
            return false;
        },
        //操作成功
        operateSuccess: function () {
            var from = $api.querystring('from');
            //如果是在课程管理中
            if (from == "course_modify") {
                console.log(from);
                 //如果处于课程编辑页，则刷新
                 var pagebox = window.top.$pagebox;
                 if (pagebox && pagebox.source.box)
                     pagebox.source.box(window.name, 'vapp.fresh_frame("vapp.handleCurrentChange")', true);
            } else {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', true);
            }
        }
    },
    template: `<div class="footer">
        <el-button type="primary" native-type="submit" :disabled="loading" :loading="loading" plain @click="btnEnter('entity')">
            <icon v-if="!loading">&#xa048</icon>修改
        </el-button>
        <el-button type='close' :disabled="loading" >
            取消
        </el-button>
    </div>`
});