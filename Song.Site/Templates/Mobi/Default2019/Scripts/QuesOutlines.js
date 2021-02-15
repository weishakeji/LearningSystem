window.vapp = new Vue({
    el: '#vapp',
    data: {
        couid: 0,        //课程id
        organ: {},       //当前机构
        config: {},      //当前机构配置项     
        account: null,     //当前登录学员 
        outlines: [],        //章节树
        course: {},         //当前课程对象  
        loading: false       //加载状态
    },
    watch: {},
    computed: {
        //总题量
        sumcount: function () {
            var sum = 0
            for (var i = 0; i < this.outlines.length; i++) {
                if (parseInt(this.outlines[i].Ol_PID) == 0)
                    sum += parseInt(this.outlines[i].Ol_QuesCount);
            }
            return sum;
        },
        //已经练习的题数
        learnques: function () {
            var arr = [];
            for (var i = 0; i < this.outlines.length; i++) {
                var olid = this.outlines[i].Ol_ID;
                var keyname = state.name.get("QuesExercises", this.couid, olid);
                var data = state.read(keyname);
                if (data.items.length < 1) continue;
                var items = data.items;
                //判断是否存在
                for (var n = 0; n < items.length; n++) {
                    if (items[n].data.correct == 'null') continue;
                    var exist = false;
                    for (var j = 0; j < arr.length; j++) {
                        if (items[n].qid == arr[j]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) arr.push(items[n].qid);
                }
            }
            return arr.length;
        },
        //课程所有试题的通过率
        passrate: function () {
            var arr = [];
            for (var i = 0; i < this.outlines.length; i++) {
                var olid = this.outlines[i].Ol_ID;
                var keyname = state.name.get("QuesExercises", this.couid, olid);
                var data = state.read(keyname);
                if (data.items.length < 1) continue;
                var items = data.items;
                //判断是否存在
                for (var n = 0; n < items.length; n++) {
                    if (items[n].data.correct == 'null') continue;
                    var exist = false;
                    for (var j = 0; j < arr.length; j++) {
                        if (items[n].qid == arr[j]) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist && items[n].data.correct == 'succ') arr.push(items[n].qid);
                }
            }
            var rate = Math.floor(arr.length / this.sumcount * 10000) / 100;
            return rate;
        },
        //最后一次学习的记录
        last: function () {
            var last = null;
            for (var i = 0; i < this.outlines.length; i++) {
                var olid = this.outlines[i].Ol_ID;
                var keyname = state.name.get("QuesExercises", this.couid, olid);
                var data = state.read(keyname);
                if (data.current != null) {
                    if (last == null) last = data.current;
                    if (last.last < data.current.last) last = data.current;
                }
            }
            return last;
        }
    },
    created: function () {
        //当前机构，当前登录学员
        $api.bat(
            $api.get('Organ/Current'),
            $api.get('Account/Current')
        ).then(axios.spread(function (organ, account) {
            //判断结果是否正常
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i].status != 200)
                    console.error(arguments[i]);
                var data = arguments[i].data;
                if (!data.success && data.exception != '') {
                    console.error(data.exception);
                    throw data.message;
                }
            }
            //获取结果
            vapp.organ = organ.data.result;
            vapp.account = account.data.result;
            console.log(vapp.account);
        })).catch(function (err) {
            console.error(err);
        });
        //课程信息
        this.couid = $api.querystring("couid");
        $api.cache('Course/ForID', { 'id': this.couid }).then(function (req) {
            if (req.data.success) {
                vapp.course = req.data.result;
                //vapp.course.Cou_Target = vapp.clearTag(vapp.course.Cou_Target);
                vapp.course.Cou_Intro = $api.trim(vapp.course.Cou_Intro);
                document.title = vapp.course.Cou_Name;
                //课程章节，价格，购买人数,通知，教师，是否购买,课程访问数
                var couid = vapp.course.Cou_ID;
                $api.bat(
                    $api.cache('Outline/Tree', { 'couid': couid }),
                    $api.get('Course/Studied', { 'couid': couid }),
                    $api.get('Course/StudyAllow', { 'couid': couid })
                ).then(axios.spread(function (outlines, isbuy, canStudy) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != '') {
                            console.error(data.exception);
                            throw data.message;
                        }
                    }
                    vapp.loading = false;
                    //获取结果                  
                    vapp.outlines = outlines.data.result;
                    vapp.isbuy = isbuy.data.result;
                    vapp.canStudy = canStudy.data.result;
                })).catch(function (err) {
                    console.error(err);
                });

            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    },
    methods: {
        //主菜单按钮点击事件
        mainmenuClick: function (url) {
            var href = url + "?couid=" + this.course.Cou_ID;
            document.location.href = href;
        },
        //练习过的章节试题数
        learnedNum: function (olid) {
            //读取记录
            var keyname = state.name.get("QuesExercises", this.couid, olid);
            var data = state.read(keyname);
            if (data == null) return 0;
            return state.clac(data.items).ansnum;
        },
        //章节试题的练习通过率
        correctper: function (olid) {
            //读取记录
            var keyname = state.name.get("QuesExercises", this.couid, olid);
            var data = state.read(keyname);
            if (data == null) return 0;
            var ret = state.clac(data.items);
            var sum = this.sumcount;
            //正确率（除以整体数量）
            var per = ret.sum > 0 ? Math.floor(ret.correct / ret.sum * 1000) / 10 : 0;
            per = per > 0 ? per : 0;
            return Math.floor(per / 10);
        },
        //章节点击事件
        outlineClick: function (node) {
            if (!node.Ol_IsFinish) return;
            var cou = this.course;
            if (this.isbuy || cou.Cou_IsFree || cou.Cou_IsLimitFree || (cou.Cou_IsTry && node.Ol_IsFree)) {
                var href = "QuesExercises.ashx?olid=" + node.Ol_ID + "&couid=" + cou.Cou_ID;
                //读取记录
                var keyname = state.name.get("QuesExercises", this.couid, node.Ol_ID);
                var data = state.read(keyname);
                //如果没有storage记录，直接进入链接
                if (data.items.length <= 0) {
                    window.location.href = href;
                    return false;
                }
                this.$confirm('点击“重新练习”，将清空本章节历史练习记录，重新计算正确率。', '是否继续上次练习？', {
                    confirmButtonText: '继续练习',
                    cancelButtonText: '重新练习',
                    type: 'warning'
                }).then(() => {
                    window.location.href = href;
                }).catch(() => {
                    this.$confirm('重新练习将清空本章节的学习记录，重新计算该章节正确率。', '是否确定？', {
                        confirmButtonText: '"确定',
                        cancelButtonText: '取消',
                        type: 'warning'
                    }).then(() => {
                        if (typeof state != "undefined") state.clear(keyname);
                        window.location.href = href;
                    }).catch();
                });
            } else {
                this.$confirm('当前章节需要购买后学习，点击“确定”进入课程购买', '购买课程', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    document.location.href = "CourseBuy.ashx?couid=" + cou.Cou_ID;
                }).catch();
            }
            return;
        }
    }
});

//底部按钮组件
Vue.component('quesfooter', {
    props: ['course'],
    data: function () {
        return {
            menus: [
                { label: '视频学习', url: 'CourseStudy.ashx', icon: '&#xe615', show: true },
                { label: '知识库', url: 'Knowledges.ashx', icon: '&#xe621', show: true },
                { label: '模拟测试', url: 'TestPapers.ashx', icon: '&#xe600', show: true },
                { label: '返回课程', url: 'CoursePage.ashx', icon: '&#xe60b', show: true }
            ]
        }
    },
    methods: {
        url: function (item) {
            return item.url + '?couid=' + this.course.Cou_ID;
        }
    },
    template: "<nav class='mui-bar mui-bar-tab footer'>\
	<a v-for='item in menus' :href='url(item)' v-if='item.show' type='link'>\
	<b v-html='item.icon'></b>\
	<span>{{item.label}}</span>\
	</a></nav>"
});
