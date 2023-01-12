//试题的展示,用于回顾
$dom.load.css(['/Utilities/Components/Question/Styles/review.css']);
Vue.component('question', {
    //exam:当前考试
    //stid:当前考试的学员id
    //state:显示状态，success显示正确的，error显示答错的题，unasnwered未做的题，默认为所有
    //groupindex:试题题型的分组，用于排序号
    props: ['exam', 'stid', 'qans', 'index', 'state', 'groupindex', 'questions', 'org'],
    data: function () {
        return {
            ques: {},       //试题        
            ismobi: $api.ismobi(),       //是否是手机端    

            accessory: {},           //试题附件
            loading: false,
            loading_file: false          //加载附件文件
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                //if (!this.existques) return;
                if (this.exam && nv.Qus_Type == 4) {
                    this.accessoryLoad();
                }
                this.$nextTick(function () {
                    var dom = $dom("card[qid='" + this.qans.id + "']");
                    //console.error(dom);
                    //清理空元素                
                    window.ques.clearempty(dom.find('card-title'));
                    window.ques.clearempty(dom.find('.ans_area'));
                    //公式渲染
                    this.$mathjax([dom[0]]);
                });
            },
            immediate: true
        },
        'qans': {
            handler(nv, ov) {               
                //console.log(nv);
            },
            immediate: true
        }
    },
    computed: {
        //是否试题加载完成
        existques: function () {
            return JSON.stringify(this.ques) != '{}' && this.ques != null;
        }
    },
    mounted: function () {
        var th = this;
        th.loading = true;
        $api.cache('Question/ForID:60', { 'id': this.qans.id }).then(function (req) {
            th.loading = false;
            if (req.data.success) {
                th.ques = req.data.result;
                if (th.ques.Qus_Type == 1 || th.ques.Qus_Type == 2 || th.ques.Qus_Type == 5)
                    th.ques = window.ques.parseAnswer(th.ques);                   
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            th.loading = false;
            console.error(err);
        });
    },
    methods: {
        //是否显示试题，根据选项卡状态
        showQues: function () {
            if (this.state == "success") {
                return this.qans.success;
            }
            if (this.state == "error") {
                return !this.qans.success;
            }
            if (this.state == "unasnwered") {
                return this.qans.ans == '';
            }
            return true;
        },
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            var gindex = this.groupindex - 1;
            var initIndex = 0;
            while (gindex >= 0) {
                initIndex += this.questions[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //选项的序号转字母
        toletter: function (index) {
            return String.fromCharCode(65 + index);
        },
        //正确答案
        sucessAnswer: function () {
            if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                var ansstr = '';
                for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                    if (this.ques.Qus_Items[j].Ans_IsCorrect) {
                        ansstr += this.toletter(j) + "、";
                    }
                }
                if (ansstr.indexOf("、") > -1)
                    ansstr = ansstr.substring(0, ansstr.length - 1);
                return ansstr;
            }
            if (this.ques.Qus_Type == 3) {
                return this.ques.Qus_IsCorrect ? "正确" : "错误";
            }
            if (this.ques.Qus_Type == 4) {
                return this.ques.Qus_Answer;
            }
            if (this.ques.Qus_Type == 5) {
                var arr = this.ques.Qus_Items;
                if (arr.length == 0) return '';
                if (arr.length == 1) return arr[0].Ans_Context;
                var txt = '';
                for (let i = 0; i < arr.length; i++) {
                    txt += (i + 1) + "、" + arr[i].Ans_Context;
                    if (i < arr.length - 1) txt += "；";
                }
                return txt;
            }
        },
        //实际答题
        actualAnswer: function () {
            if (this.qans.ans == '') return '未做';
            if (this.ques.Qus_Type == 1 || this.ques.Qus_Type == 2) {
                //学员答题信息
                var answer = this.qans.ans.split(',');
                var ansstr = '';
                for (var j = 0; j < this.ques.Qus_Items.length; j++) {
                    var ishav = false;
                    for (var i = 0; i < answer.length; i++) {
                        if (answer[i] == '') continue;
                        if (answer[i] == this.ques.Qus_Items[j].Ans_ID) {
                            ansstr += this.toletter(j) + "、";
                        }
                    }
                }
                if (ansstr.indexOf("、") > -1)
                    ansstr = ansstr.substring(0, ansstr.length - 1);
                return ansstr;
            }
            if (this.ques.Qus_Type == 3) {
                return this.qans.ans == '0' ? "正确" : "错误";
            }
            if (this.ques.Qus_Type == 4) {
                return this.qans.ans;
            }
            //填空题
            if (this.ques.Qus_Type == 5) {
                if (this.qans.ans == null || this.qans.ans == '') return '';
                var arr = this.qans.ans.split(',');
                for (let i = 0; i < arr.length; i++) {
                    if (arr[i] == '') {
                        arr.splice(i, 1);
                        i--;
                    }
                }
                if (arr.length == 0) return '';
                if (arr.length == 1) return arr[0];
                var txt = '';
                for (let i = 0; i < arr.length; i++) {
                    txt += (i + 1) + "、" + arr[i];
                    if (i < arr.length - 1) txt += "；";
                }
                return txt;
            }
        },
        //加载附件
        accessoryLoad: function () {
            var th = this;
            th.loading_file = true;
            $api.get('Exam/FileLoad', { 'stid': th.stid, 'examid': th.exam.Exam_ID, 'qid': th.ques.Qus_ID })
                .then(function (req) {
                    if (req.data.success) {
                        th.accessory = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(function () {
                    th.loading_file = false;
                });
        },
        //附件查看
        accessoryview: function (url) {
            var ext = url.indexOf('.') > -1 ? url.substring(url.lastIndexOf('.') + 1).toLowerCase() : '';
            var canpreview = "jpg,gif,png,pdf";
            var exist = canpreview.split(',').findIndex(x => x == ext);
            if (exist > -1) {
                var box = window.top.$pagebox;
                if (box == null) return;
                if (ext == 'pdf') url = $api.pdfViewer(url);

                var params = {
                    'url': url, 'ico': 'e6ef', 'min': false, 'showmask': true,
                    'pid': window.name,
                    'title': '预览',
                    'width': 900,
                    'height': '80%'
                }
                if (this.ismobi) {
                    params['width'] = '99%';
                    params['height'] = '100%';
                }
                var box = window.top.$pagebox.create(params).open();
            } else {
                alert('该文件类型不可预览');
            }
            return false;
        }
    },
    template: `<card :qid="qans.id" v-if="showQues()">
        <card-title :index="calcIndex(index+1)" v-if="loading">
            <loading type="spinner" size="24px" > 加载中...</loading>
        </card-title>        
        <card-title :index="calcIndex(index+1)" :num="qans.num" v-else-if="ques.Qus_Title" v-html="ques.Qus_Title">           
        </card-title>
        <card-title :index="calcIndex(index+1)" :num="qans.num"  v-else><span class="null">(试题不存在)</span></card-title>
        <card-context>
        <div class="ans_area type1" v-if="ques.Qus_Type==1">
            <div v-for="(ans,i) in ques.Qus_Items" :correct="ans.Ans_IsCorrect" :selected="ans.selected">
                <i>{{toletter(i)}} .</i>
                <span v-html="ans.Ans_Context"></span>
             </div>
        </div>
        <div  class="ans_area type2" v-if="ques.Qus_Type==2">
            <div v-for="(ans,i) in ques.Qus_Items" :correct="ans.Ans_IsCorrect" :selected="ans.selected">
                <i>{{toletter(i)}} .</i>
                <span v-html="ans.Ans_Context"></span>
            </div>
        </div>
        <div  class="ans_area type3" v-if="ques.Qus_Type==3">
            <div :correct="ques.Qus_IsCorrect" :selected="qans.ans=='0'">
                <i>正确</i> 
            </div>
            <div :correct="!ques.Qus_IsCorrect" :selected="qans.ans=='1'">
                <i>错误</i>
            </div>
        </div>
        <div v-if="ques.Qus_Type==4">
        
        </div>
        <div v-if="ques.Qus_Type==5">
        
        </div>
        <div class="resultBox" :qtype="ques.Qus_Type">
            <div :mobi="ismobi">
                <div>正确答案：<span v-html="sucessAnswer()"></span></div>
                <div>实际答题：<span v-html="actualAnswer()"></span></div>               
                <div v-if="accessory.state" class="accessory">
                    附件: <a @click="accessoryview(accessory.url)">{{accessory.name}}</a>                 
                    <a :href="accessory.url" :download="accessory.name"><icon>&#xa029</icon>下载</a>
                </div>               
                <div class="result_score" :success="qans.success">得分：{{qans.score}} 分</div>
            </div>
            <div v-if="ques.Qus_Explain!=''">试题解析：<span v-if="ques.Qus_Explain!=''" v-html="ques.Qus_Explain"></span>
                <span v-else>无</span>
            </div>
        </div>
         <slot :ques="ques" :qans="qans"></slot>
        </card-context>
       
        <div class="orgname noview" v-if="org">{{org.Org_Name}}</div>
      </card>`
});