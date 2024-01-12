//试题编辑中的确定按钮
Vue.component('enter_button', {
    //question：当前试题
    //verify:验证试题是否满足编辑条件的方法
    props: ["question", "verify", "organ"],
    data: function () {
        return {
            loading: false,
            id: $api.dot(),          //来自地址栏的试题id
            disabled: false      //按钮是否禁用
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/enter_button.css']);
        console.log(this.id);
    },
    computed: {
        //试题是否为空
        'quesnull': function () {
            var ques = JSON.stringify(this.question) != '{}' && this.question != null;
            return !ques || this.question.Qus_ID == 0;
        },
        //是否是新增试题
        'isadd': t => t.id == '',
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
            //是否设置课程
            if (!qus.Cou_ID || qus.Cou_ID <= 0) {
                return this.alert('试题必须设置所属课程', 1);
            }
            //是否设置专业
            if (!qus.Sbj_ID || qus.Sbj_ID <= 0) {
                return this.alert('试题必须设置所属专业', 1);
            }

            return true;
        },
        //提交
        btnEnter: function (isclose) {
            //通用验证
            if (!this.general_verify()) return;
            //自定义验证
            if (this.verify) {
                if (!this.verify(this.question, this.alert)) return;
            }
            var th = this;

            if (th.loading) return;
            th.loading = true;
            if (th.isadd) th.question.Org_ID = th.organ.Org_ID;
            let apipath = th.isadd ? api = 'Question/add' : 'Question/Modify';
            $api.post(apipath, { 'entity': th.question }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.$message({
                        type: 'success', center: true,
                        message: '操作成功!'
                    });
                    window.setTimeout(function () {
                        th.operateSuccess(isclose);
                    }, 300);
                } else {
                    throw req.data.message;
                }
            }).catch(err => alert(err, '错误'))
                .finally(() => th.loading = false);
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
        operateSuccess: function (isclose) {
            let pagebox = window.top.$pagebox;
            if (!(pagebox && pagebox.source.box)) return;

            //来源，判断是否处理课程管理中
            let from = $api.querystring('from');
            if (from == "course_modify") {
                //如果处于课程编辑页，则刷新
                if (this.isadd)
                    pagebox.source.top(window.name, 'vapp.fresh_frame("vapp.handleCurrentChange")', isclose);
                else
                    pagebox.source.tab(window.name, 'vapp.fresh_frame(\'vapp.freshrow("' + this.id + '")\')', isclose);

            } else {
                if (this.isadd)
                    pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
                else
                    pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        }
    },
    template: `<div class="footer" v-if="!quesnull">
        <el-button type="primary" define="enter" native-type="submit" :disabled="loading || disabled" 
        :loading="loading" plain @click="btnEnter(true)">保存</el-button>
        <el-button type="primary" define="apply" native-type="submit" :loading="loading" plain v-if="id != ''"
            @click="btnEnter(false)">应用</el-button>
        <el-button type='close' :disabled="loading" >
            取消
        </el-button>
    </div>`
});