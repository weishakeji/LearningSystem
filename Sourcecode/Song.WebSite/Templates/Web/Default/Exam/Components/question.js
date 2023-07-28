//试题的展示
$dom.load.css([$dom.pagepath() + 'Components/Styles/question.css']);
Vue.component('question', {
    //exam:当前考试
    //account:当前登录的学员账号
    //index:索引号
    //groupindex:试题题型的分组，用于排序号
    //total: 试题总数
    props: ['exam', 'account', 'ques', 'index', 'groupindex', 'types', 'total'],
    data: function () {
        return {
            ext_limit: "png,jpg,gif,zip,rar,pdf,ppt,pptx,doc,docx,xls,xlsx",
            accessory: {},           //试题附件
            loading_upload: false,        //上传的状态
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                if (nv && nv.Qus_Type == 4)
                    this.accessoryLoad();
                this.$nextTick(function () {
                    //没有内容的html元素，不显示
                    var qbox = $dom('card[qid="' + this.ques.Qus_ID + '"]');
                    window.ques.clearempty(qbox.find('card-title'));
                    window.ques.clearempty(qbox.find('.ans_area'));
                    //公式渲染
                    this.$mathjax([qbox[0]]);
                });
            }, immediate: true
        }
    },
    computed: {},
    updated: function () { },
    mounted: function () { },
    methods: {
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            let gindex = this.groupindex - 1;
            let initIndex = 0;
            while (gindex >= 0) {
                initIndex += vapp.paperQues[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //选项的序号转字母
        showIndex: function (index) {
            return String.fromCharCode(65 + index);
        },
        /*  */
        //单选题的选择
        type1_select: function (ans, items) {
            for (let index = 0; index < items.length; index++) {
                const element = items[index];
                if (element.Ans_ID == ans.Ans_ID) continue;
                element.selected = false;
            }
            ans.selected = !ans.selected;
            if (ans.selected) this.$parent.swipe({ 'direction': 2 });
        },
        //多选题的选择
        type2_select: function (ans) {
            ans.selected = !ans.selected;
        },
        //判断题的选择,logic为true或false
        type3_select: function (logic) {
            let answer = String(logic);
            if (this.ques.Qus_Answer == answer) this.ques.Qus_Answer = '';
            else {
                this.ques.Qus_Answer = answer;
                this.$parent.swipe({ 'direction': 2 });
            }
        },
        //填空题
        type5_input: function (ques) {
            var ansstr = '';
            for (let index = 0; index < ques.Qus_Items.length; index++) {
                const element = ques.Qus_Items[index];
                ansstr += element.Ans_Context + ",";
            }
            this.ques.Qus_Answer = ansstr;
        },
        /*  */
        //附件文件上传
        accessoryUpload: function (file) {
            var th = this;
            th.loading_upload = true;
            $api.post('Exam/FileUp',
                { 'stid': th.account.Ac_ID, 'examid': th.exam.Exam_ID, 'qid': th.ques.Qus_ID, 'file': file })
                .then(function (req) {
                    th.loading_upload = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.accessory = req.data.result;
                        th.ques['Qus_Explain'] = th.accessory.name;
                        if (result.state) {

                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_upload = false;
                    alert(err);
                    console.error(err);
                });
        },
        //加载附件
        accessoryLoad: function () {
            var th = this;
            th.loading_upload = true;
            $api.get('Exam/FileLoad', { 'stid': th.account.Ac_ID, 'examid': th.exam.Exam_ID, 'qid': th.ques.Qus_ID })
                .then(function (req) {
                    if (req.data.success) {
                        th.accessory = req.data.result;

                        console.log(th.accessory);
                        th.ques['Qus_Explain'] = th.accessory.state ? th.accessory.name : '';
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(function () {
                    th.loading_upload = false;
                });
        },
        //删除附件
        accessoryDelete: function () {
            this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                var th = this;
                th.loading_upload = true;
                $api.delete('Exam/FileDelete', { 'stid': th.account.Ac_ID, 'examid': th.exam.Exam_ID, 'qid': th.ques.Qus_ID })
                    .then(function (req) {
                        if (req.data.success) {
                            th.accessory = {};
                            th.ques['Qus_Explain'] = '';
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(function () {
                        th.loading_upload = false;
                    });

            }).catch(() => {
            });
        },
        //附件查看
        accessoryview: function (url) {
            var ext = url.indexOf('.') > -1 ? url.substring(url.lastIndexOf('.') + 1).toLowerCase() : '';
            var canpreview = "jpg,gif,png,pdf";
            var exist = canpreview.split(',').findIndex(x => x == ext);
            if (exist > -1) {
                if (ext == 'pdf') url = $api.pdfViewer(url);
                var obj =
                {
                    'url': url, 'ico': 'a022',
                    'pid': window.name,
                    'title': '预览',
                    'width': 900,
                    'height': '80%'
                }
                obj['showmask'] = true; //始终显示遮罩
                obj['min'] = false;
                var box = window.$pagebox.create(obj).open();
            } else {
                alert('该文件类型不可预览');
            }
            return false;
        }
    },
    template: `<dd :qid="ques.Qus_ID">
        <info>
            {{calcIndex(index+1)}}/{{total}}
            [ {{this.types[ques.Qus_Type - 1]}}题 ] 
            <span>（{{ques.Qus_Number}} 分）</span>
        </info>
        <card :qid="ques.Qus_ID">   
            <card-title v-html="ques.Qus_Title"></card-title>
            <card-context>
                <div class="ans_area type1" v-if="ques.Qus_Type==1"  remark="单选题">
                    <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" 
                    :selected="ans.selected" @click="type1_select(ans,ques.Qus_Items)">
                        <i>{{showIndex(i)}} .</i>
                        <span v-html="ans.Ans_Context"></span>
                    </div>
                </div>
                <div  class="ans_area type2" v-if="ques.Qus_Type==2"  remark="多选题">
                    <div v-for="(ans,i) in ques.Qus_Items" :ansid="ans.Ans_ID" :selected="ans.selected" @click="type2_select(ans)">
                        <i>{{showIndex(i)}} .</i>
                        <span v-html="ans.Ans_Context"></span>
                    </div>
                </div>
                <div class="ans_area type3" v-if="ques.Qus_Type==3"  remark="判断题">
                    <div :selected="ques.Qus_Answer=='true'"  @click="type3_select(true)">
                        <i> 正确</i>
                    </div>
                    <div :selected="ques.Qus_Answer=='false'"  @click="type3_select(false)">
                        <i> 错误</i>
                    </div>
                </div>
                <div v-if="ques.Qus_Type==4" class="ans_area type4"  remark="简答题">
                    <textarea rows="10" placeholder="这里输入文字" v-model.trim="ques.Qus_Answer"></textarea>
                    <loading v-if="loading_upload">正在上传...</loading>
                    <div v-else-if="accessory.state" class="accessory">
                        附件: <a @click="accessoryview(accessory.url)">{{accessory.name}}</a>
                        <icon delete @click="accessoryDelete()">删除</icon>
                        <a :href="accessory.url" :download="accessory.name"><icon>&#xa029</icon>下载</a>
                    </div>
                    <upload-file v-else @change="accessoryUpload" :data="ques" :size="5120" height="30"
                        :ext="ext_limit">
                        <el-tooltip :content="'允许的文件类型：'+ext_limit" placement="right"
                            effect="light">
                            <el-button type="primary" plain>
                                <icon>&#xe6ea</icon>点击上传附件
                            </el-button>
                        </el-tooltip>
                    </upload-file>
                </div>
                <div  class="ans_area type5" v-if="ques.Qus_Type==5" remark="填空题">
                    <div v-for="(ans,i) in ques.Qus_Items">
                        <input type="text" v-model="ans.Ans_Context" @input="type5_input(ques)"/>               
                    </div>
                </div>    
            </card-context>
        </card>
    </dd>`
});