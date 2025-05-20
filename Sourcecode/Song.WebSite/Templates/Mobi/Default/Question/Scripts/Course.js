$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid"),        //课程id
            olid: $api.querystring("olid", 0),
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
            },
            //章节id
            'olid': {
                handler: function (nv, ov) {
                    if (nv != 0) {
                        var uri = $api.url.set('exercise', {
                            'couid': this.couid,
                            'olid': nv,
                        });
                        //window.navigateTo(uri);
                    }
                }, immediate: true
            },
        },
        computed: {
            //是否登录
            'islogin': t => JSON.stringify(t.account) != '{}' && t.account != null,
            //试题练习的通过率
            'rate': t => {
                if (t.purchase == null) return 0;
                return t.purchase.Stc_QuesScore;
            }
        },
        created: function () {
            var th = this;
            th.loading = true;
            //当前机构，当前登录学员,当前课程
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Account/Current'),
                $api.cache('Course/ForID', { 'id': th.couid })
            ).then(([organ, account, course]) => {
                //机构和当前学员
                th.organ = organ.data.result;
                th.account = account.data.result;
                //课程
                th.course = course.data.result;
                //vapp.course.Cou_Target = vapp.clearTag(vapp.course.Cou_Target);
                th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                document.title = th.course.Cou_Name;
                if (!th.islogin) return;
                var couid = th.course.Cou_ID;
                //课程章节，价格，购买人数,通知，教师，是否购买,课程访问数
                $api.bat(
                    $api.cache('Outline/Tree', { 'couid': couid, 'isuse': true }),
                    $api.get('Course/Owned', { 'couid': couid, 'acid': th.account.Ac_ID }),
                    $api.get('Course/Purchaselog', { 'couid': couid, 'stid': th.account.Ac_ID }),
                ).then(([outlines, owned, purchase]) => {
                    //章节                   
                    th.outlines = th.calcSerial(outlines.data.result, '');
                    th.owned = owned.data.result;
                    //购买记录
                    th.purchase = purchase.data.result;
                    //
                    th.$nextTick(() => th.loading = false);
                }).catch(err => console.error(err));

            }).catch(err => console.error(err));
        },
        methods: {
            //将每个章节的状态都记录下来
            statepush: function (data) {
                this.$set(this.state, this.state.length, data);
                if (data.items && data.items.length > 0) {
                    const quesSet = new Set(this.state_ques.map(q => q.qid));
                    data.items.forEach(d => {
                        if (!quesSet.has(d.qid)) {
                            this.state_ques.push(d);
                            quesSet.add(d.qid); // 保持同步，避免后续重复添加
                        }
                    });
                }
            },
            //计算序号
            calcSerial: function (list, lvl) {
                if  (!list) return list;
                for (let i = 0; i < list.length; i++) {
                    let node = list[i];
                    node.serial = lvl + (i + 1) + '.';
                    this.total++;
                    this.count.sum += node.Ol_QuesCount;
                    node.Ol_QuesCount = this.calcQuescount(node);
                    if (node.children && node.children.length > 0)
                        node.children = this.calcSerial(node.children, node.serial);
                }
                return list;
            },
            //计算某个章节的试题总数
            calcQuescount: function (outline) {
                let total = outline.Ol_QuesCount;
                var childarr = outline.children ? outline.children : null;
                if (childarr == null) return total;
                for (const node of childarr) {
                    // 累加当前节点的试题数量
                    if (node.Ol_QuesCount) total += parseInt(node.Ol_QuesCount);
                    // 递归处理子节点
                    if (node.children && node.children.length > 0)
                        total += this.calcQuescount(node.children);
                }
                return total;
            },
            //计算各项统计数，包括题量、练习数、通过率等
            calcCount: function () {
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
            //继续练习的按纽事件
            gocontinue: function () {
                var last = this.last;
                if (last == null) return;
                var url = $api.url.set('exercise', {
                    'couid': this.couid,
                    'olid': last.olid
                });
                window.navigateTo(url);
            }
        }
    });
    // 中间的按钮组
    Vue.component('menus', {
        props: ["couid", "olid"],
        data: function () {
            return {
                menus: [
                    { name: '错题回顾', url: 'Error', icon: '&#xe732', size: 30, show: true, evt: null },
                    { name: '我的收藏', url: 'Collects', icon: '&#xe747', size: 29, show: true, evt: null },
                    { name: '我的笔记', url: 'Notes', icon: '&#xa02e', size: 29, show: true, evt: null },
                    { name: '高频错题', url: 'Often', icon: '&#xe75e', size: 30, show: true, evt: null }
                ]
            }
        },
        watch: {},
        computed: {},
        mounted: function () { },
        methods: {
            btnEvt: function (item) {
                var url = item.url + '?couid=' + this.couid;
                window.navigateTo(url);
            }
        },
        template: `<div class="mainmenu">           
                    <div v-for="(m,i) in menus" @click="!!m.evt ? item.evt(m) : btnEvt(m)">
                        <icon v-html="m.icon"  :style="'font-size: '+m.size+'px'"></icon>
                        <name>{{m.name}}</name>
                    </div> 
                </div> `
    });
}, ['Components/ExerciseState.js',
    'Components/OutlineList.js']);
