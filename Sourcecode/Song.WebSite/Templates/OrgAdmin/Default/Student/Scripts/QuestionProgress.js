$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid", $api.dot()),        //课程id
            stid: $api.querystring("stid", 0),
            organ: {},       //当前机构
            config: {},      //当前机构配置项     
            account: null,     //当前登录学员 
            outlines: [],        //章节树
            course: {},         //当前课程对象  
            owned: false,       //是否购买或学员组关联课程
            purchase: null,          //课程购买记录     

            total: 0,     //章节总数
            state: [],           //学习记录的状态数据
            state_ques: [],      //所有试题的状态，来自state中的items
            last: null,      //最后练习的章节
            //一些统计数，基于课程的
            count: {
                sum: 0,         //总题量
                exercise: 0,        //已经练习的题数
                correct: 0         //正确数          
            },
            showalloutline: false,   //显示所有章节
            loading: true       //加载状态
        },

        watch: {
            //所有的章节状态
            state: function (nv, ov) {
                if (nv.length >= this.total) {
                    this.last = this.getlast();
                    this.calcCount();
                }
            }
        },
        computed: {
            //是否登录
            islogin: t => !$api.isnull(t.account),
            //试题练习的通过率
            'rate': t => {
                if (t.purchase == null) return 0;
                return t.purchase.Stc_QuesScore;
            },
            //可以计算最后一次练习，要满足多个条件
            canbelast: function () {
                if (!this.islogin) return false;
                if ($api.isnull(this.outlines)) return false;
                if (this.outlines.length < 1) return false;
                if (this.state.length < 1) return false;
                return true;
            }
        },
        created: function () {
            var th = this;
            th.loading = true;
            th.getAccount().then(function (d) {
                th.account = d;
                th.stid = d ? d.Ac_ID : 0;
                $api.get('Course/Owned', { 'couid': th.couid, 'acid': th.stid }).then(function (req) {
                    if (req.data.success) {
                        th.owned = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }).catch(function (err) {
                alert(err);
            });
            //当前机构，当前登录学员
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Course/ForID', { 'id': th.couid }),
                $api.cache('Outline/Tree', { 'couid': th.couid, 'isuse': true }),
                $api.get('Course/Purchaselog', { 'couid': th.couid, 'stid': th.stid }),
            ).then(([organ, course, outlines, purchase]) => {
                //机构和当前学员
                th.organ = organ.data.result;
                //课程
                th.course = course.data.result;
                th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                document.title = th.course.Cou_Name;
                //章节
                th.outlines = outlines.data.result;
                //购买记录
                th.purchase = purchase.data.result;
                th.calcSerial(null, '');
                th.$nextTick(() => th.loading = false);

            }).catch(function (err) {
                console.error(err);
            });
        },
        methods: {
            //获取当前学员
            getAccount: async function () {
                var th = this;
                return new Promise(function (resolve, reject) {
                    var api = Number(th.stid) > 0 ? $api.get('Account/ForID', { 'id': th.stid }) : $api.get('Account/Current');
                    api.then(function (req) {
                        if (req.data.success) {
                            resolve(req.data.result);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        reject(err);
                    });
                });
            },
            //将每个章节的状态都记录下来
            statepush: function (data) {
                this.$set(this.state, this.state.length, data);
                //将记录数据中的items单独列出来
                if (data.items && data.items.length > 0) {
                    for (let i = 0; i < data.items.length; i++) {
                        const d = data.items[i];
                        var exist = false;
                        for (let j = 0; j < this.state_ques.length; j++) {
                            const q = this.state_ques[j];
                            if (q.qid == d.qid) {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist) this.state_ques.push(d);
                    }
                }
            },
            //计算序号
            calcSerial: function (outline, lvl) {
                var childarr = outline == null ? this.outlines : (outline.children ? outline.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].serial = lvl + (i + 1) + '.';
                    this.total++;
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            //计算各项统计数，包括题量、练习数、通过率等
            calcCount: function () {
                //计算总题量               
                let sum = 0;
                if (this.outlines != null && this.outlines.length > 0) {
                    for (var i = 0; i < this.outlines.length; i++) {
                        if (parseInt(this.outlines[i].Ol_PID) == 0) {
                            sum += parseInt(this.outlines[i].Ol_QuesCount);
                        }
                    }
                }
                this.count.sum = sum;
                //计算已经做过的试题数              
                let exercise = 0, correct = 0;
                for (var i = 0; i < this.state_ques.length; i++) {
                    const qt = this.state_ques[i];
                    if (($api.getType(qt.ans) == "String" && qt.ans != "") ||
                        ($api.getType(qt.ans) == "Number" && qt.ans > 0) || qt.ans != "") {
                        exercise++;
                        if (qt.correct == 'succ') correct++;
                    }
                }
                this.count.exercise = exercise;
                this.count.correct = correct;
                //整体的通过率
                let rate = Math.round(this.count.correct / this.count.sum * 10000) / 100;
                this.rate = rate === Infinity || rate === -Infinity || isNaN(rate) ? 0 : rate;
                return;
            },
            //最后练习的章节
            getlast: function () {
                var last = null, couid = 0, olid = 0;
                if (!this.account || this.outlines == null || this.outlines.length < 1) return last;
                var acid = this.account.Ac_ID;
                for (let i = 0; i < this.state.length; i++) {
                    const s = this.state[i];
                    if (s.current == null || s.current.time == null) continue;
                    if (last == null) last = s;
                    if (last.time < s.time) last = s;
                }
                return last;
            },
            //继续练习
            gocontinue: function () {
                var last = this.last;
                if (last == null) return;
                var url = $api.url.set('Exercise', {
                    'couid': this.couid,
                    'olid': last.olid,
                    'back': true
                });
                window.location.href = url;
            }
        }
    });
}, ['Components/ExerciseState.js',
    'Components/OutlineList.js']);
