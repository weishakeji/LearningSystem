//试题编辑中的确定按钮
$dom.load.css([$dom.path() + 'Question/Components/Styles/enter_button.css']);
Vue.component('enter_button', {
    //question：当前试题
    //verify:验证试题是否满足编辑条件的方法
    props: ["question", "verify", "organ"],
    data: function () {
        return {
            loading: false,
            id: $api.dot(),          //来自地址栏的试题id
            disabled: false,      //按钮是否禁用
            //
            ischanged: false,         //试题内容是否变更
            quesNext: null,      //下一道试题
            quesPrev: null       //上一道试题
        }
    },
    mounted: function () {
        this.getRelatedQues("Question/next").then(res => this.quesNext = res);
        this.getRelatedQues("Question/prev").then(res => this.quesPrev = res);
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                if ($api.isnull(ov) || !ov.Qus_ID || ov.Qus_ID == 0) return;
                if ($api.isnull(nv)) return;
                this.ischanged = true;
            }, deep: true
        },
    },
    computed: {
        //试题是否为空
        'quesnull': function () {
            return $api.isnull(this.question) || this.question.Qus_ID == 0;
        },
        //是否是新增试题
        'isadd': t => t.id == '' || t.id == '0' || t.id == 0,
    },
    methods: {
        //常规验证，主要验证试题所属专业、课程等
        general_verify: function () {
            if (!this.question) return false;
            let qus = this.question;
            //题干不得为空
            if (!qus.Qus_Title || qus.Qus_Title == '')
                return this.prompt('试题题干不得为空！', 0);
            //是否设置课程
            if (!qus.Cou_ID || qus.Cou_ID <= 0)
                return this.prompt('试题必须设置所属课程', 1);
            //是否设置专业
            if (!qus.Sbj_ID || qus.Sbj_ID <= 0) {
                console.error(qus);
                return this.prompt('试题必须设置所属专业', 1);
            }
            return true;
        },
        //提交
        btnEnter: function (isclose) {
            //通用验证
            if (!this.general_verify()) return;
            //自定义验证
            if (this.verify) {
                if (!this.verify(this.question, this.prompt)) return;
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
                    th.ischanged = false;
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
        prompt: function (msg, tabindex) {
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
        //isclose:是否关闭当前窗体，一般为true时，是保存按钮的操作；为false是，是应用按钮的操作
        operateSuccess: function (isclose) {
            if (isclose) {
                //编辑状态
                if (!this.isadd) {
                    this.$confirm('当前试题保存成功, 是否继续转下一题?<br/>点击“取消”，关闭窗口。', '提示', {
                        confirmButtonText: '确定', cancelButtonText: '取消',
                        type: 'info', dangerouslyUseHTMLString: true
                    }).then(() => {
                        this.operateSuccessHandler(false);
                        window.setTimeout(() => this.goRelatedQues('下一题'), 300);
                    }).catch(() => this.operateSuccessHandler(isclose));
                } else {
                    //新增状态
                    this.$confirm('当前试题新建成功, 是否再次创建?<br/>点击“取消”，关闭窗口。', '提示', {
                        confirmButtonText: '确定', cancelButtonText: '取消',
                        type: 'info', dangerouslyUseHTMLString: true
                    }).then(() => {
                        this.operateSuccessHandler(false);
                        window.setTimeout(() => this.goRelatedQues(), 300);
                    }).catch(() => this.operateSuccessHandler(isclose));
                }
            } else this.operateSuccessHandler(isclose);
        },
        //操作成功后的处理
        operateSuccessHandler: function (isclose) {
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
        },
        //获取关联的试题，即上一题与下一题
        getRelatedQues: function (apipath) {
            var th = this;
            var couid = $api.querystring('couid');
            return new Promise(function (resolve, reject) {
                $api.get(apipath, { 'id': th.id, 'olid': '', 'couid': couid, 'sbjid': '' }).then(req => {
                    if (req.data.success) {
                        return resolve(req.data.result);
                    } else {
                        //console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            });
        },
        //跳转到关联的试题之前的提示与判断
        btnRelatedClick: function (direction) {
            if (this.ischanged) {
                this.$confirm('当前试题未保存, 是否继续跳转' + direction + '?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.goRelatedQues(direction);
                }).catch(() => { });
            } else this.goRelatedQues(direction);
        },
        //跳转到关联的试题，即上一题与下一题
        goRelatedQues: function (direction) {
            let href = window.location.href;
            //设置文件名
            let filename = 'modify_type';
            if (direction == '上一题' && this.quesPrev) filename += this.quesPrev.Qus_Type;
            if (direction == '下一题' && this.quesNext) filename += this.quesNext.Qus_Type;
            href = href.replace(/modify_type\d{1}/, filename);
            //console.error(href);
            //return;
            //设置试题id
            let quesid = 0;
            if (direction == '上一题') quesid = this.quesPrev.Qus_ID;
            if (direction == '下一题') quesid = this.quesNext.Qus_ID;
            if (quesid != 0) {
                href = $api.url.dot(quesid, href);
                href = $api.url.set(href, { 'id': quesid });
            }
            //设置试题的课程id
            let couid = $api.querystring('couid', 0);
            if (couid != 0 && !$api.isnull(couid))
                href = $api.url.set(href, { 'couid': couid });
            //跳转
            if (window.top.$pagebox) {
                let box = window.top.$pagebox.get(window.name);
                box.url = href;
            } else window.location.href = href;
            //console.error(href);            
        },
    },
    template: `<div class="footer" v-if="!quesnull">
        <div class="movebtn">
            <template v-if="id != ''">
                <el-link :underline="false" @click="btnRelatedClick('上一题')" :disabled="$api.isnull(quesPrev)"><icon>&#xe803</icon>上一题</el-link>
                <el-link :underline="false" @click="btnRelatedClick('下一题')" :disabled="$api.isnull(quesNext)">下一题<icon>&#xe802</icon></el-link>  
            </template>   
        </div>
        <div>
            <el-button type="primary" define="enter" native-type="submit" :disabled="loading || disabled" 
            :loading="loading" plain @click="btnEnter(true)">保存</el-button>
            <el-button type="primary" define="apply" native-type="submit" :loading="loading" plain v-if="id != ''"
                @click="btnEnter(false)">应用</el-button>
            <el-button type='close' :disabled="loading" >
                取消
            </el-button>
        </div>
    </div>`
});