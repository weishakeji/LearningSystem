
$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            checkList: ['1', '2', '5'],   //选中的值
            //试题数量
            total: {
                type0: 0,       //总数
                type1: 0,       //单选题数量
                type2: 0,       //多选题数量
                type5: 0,       //填空题数量
            },
            //查询后的结果
            results: {
                type1: {
                    num: 0,      //已经处理的数
                    repair: 0,       //已经修复的数量
                    err: []      //错误的试题
                },
                type2: { num: 0, repair: 0, err: [] },
                type5: { num: 0, repair: 0, err: [] }
            },
            loading: false,
            load_check: false,
            load_repair: false       //修复中
        },
        watch: {
            'checked': function (nv, ov) {
                if (nv) this.load_check = false;
            },
            'repaired': function (nv, ov) {
                if (nv) {
                    this.load_repair = false;
                    for (k in this.results) {
                        this.results[k].num = 0;
                        this.results[k].repair = 0;
                        this.results[k].err = [];
                    }
                }
            }
        },
        computed: {
            //检查试题完成
            'checked': function () {
                //要检查的试题总数
                var total = 0
                for (k in this.checkList) total += this.total['type' + this.checkList[k]];
                //完成检查的试题数
                var complete = 0;
                for (k in this.checkList) complete += this.results['type' + this.checkList[k]].num;
                return complete >= total;
            },
            //是否存在错误试题
            'error': function () {
                var num = 0;
                for (k in this.results) {
                    num += this.results[k].err.length;
                }
                return num > 0;
            },
            //检查的过程信息
            checkinfo: function () {
                //要检查的试题总数
                var total = 0
                for (k in this.checkList) total += this.total['type' + this.checkList[k]];
                //完成检查的试题数
                var complete = 0;
                for (k in this.checkList) complete += this.results['type' + this.checkList[k]].num;
                return complete + ' / ' + total;
            },
            //修复完成
            repaired: function () {
                //错题总数
                var total = 0, complete = 0;
                for (k in this.results) {
                    total += this.results[k].err.length;
                    complete += this.results[k].repair;
                }
                return complete >= total;
            },
            //修复过程中的信息
            repairinfo: function () {
                //错题总数
                var total = 0, complete = 0;
                for (k in this.results) {
                    total += this.results[k].err.length;
                    complete += this.results[k].repair;
                }
                return complete + ' / ' + total;
            }
        },
        created: function () {
            this.getcount();
        },
        methods: {
            //获取数量
            getcount: function () {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': '', 'type': '', 'use': '' }),
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': '', 'type': 1, 'use': '' }),
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': '', 'type': 2, 'use': '' }),
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': '', 'type': 5, 'use': '' }),
                ).then(([t0, t1, t2, t5]) => {
                    //获取结果
                    th.total.type0 = t0.data.result;
                    th.total.type1 = t1.data.result;
                    th.total.type2 = t2.data.result;
                    th.total.type5 = t5.data.result;
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //检测
            check: function () {
                //要检查的试题总数
                var total = 0
                for (k in this.checkList) total += this.total['type' + this.checkList[k]];
                if (total <= 0) {
                    this.$alert('没有试题供检测', { showClose: false });
                    return;
                }
                this.load_check = true;
                for (k in this.results) {
                    this.results[k].num = 0;
                    this.results[k].repair = 0;
                    this.results[k].err = [];
                }
                for (k in this.checkList) {
                    var type = this.checkList[k];
                    this.quespager(1, type);
                }
            },
            //加载数据页
            quespager: function (index, type) {
                var form = {
                    'orgid': '', 'sbjid': '', 'couid': '', 'olid': '', 'type': type,
                    'use': '', 'error': '', 'wrong': '', 'search': '',
                    'size': 20, 'index': index
                };
                var th = this;
                if (!th.load_check) return;
                $api.get("Question/Pager", form).then(function (d) {
                    if (d.data.success) {
                        var result = d.data.result;
                        //th.calc_count = result.length;
                        for (let i = 0; i < result.length; i++) {
                            result[i] = window.ques.parseAnswer(result[i]);
                            th.checkItems(result[i], type);

                        }
                        //th.questions = result;
                        var totalpages = Number(d.data.totalpages);
                        //th.total = d.data.total;                       
                        //window.setTimeout(function () {
                        if (index < totalpages)
                            th.quespager(index + 1, type);
                        // }, 1000);
                        console.log(th.checked);
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //检查答案项的id是否重复
            checkItems: function (ques, type) {
                this.results['type' + type].num++;

                var items = ques.Qus_Items;
                var exist = false;
                for (let i = 0; i < items.length; i++) {
                    const a = items[i];
                    for (let j = 0; j < items.length; j++) {
                        if (i == j) continue;
                        const b = items[j];
                        if (a.Ans_ID == b.Ans_ID) {
                            exist = true;
                            break;
                        }
                    }
                    if (exist) break;
                }
                if (exist) {
                    ques.Qt_ID = 1;
                    console.error('重复');
                    this.results['type' + type].err.push(ques);
                    //console.log(this.err_num);
                    //this.err_ques.push(ques);
                }
                this.calc_count--;

                return ques;
            },
            //修复试题
            repair: function () {
                //错题总数
                var total = 0;
                for (k in this.results) total += this.results[k].err.length;
                if (total <= 0) {
                    this.$alert('没有试题供修复', { showClose: false });
                    return;
                }

                this.load_repair = true;
                for (k in this.results) {
                    var type = parseInt(k.substring(4));
                    var arr = this.results[k].err;
                    this.repair_handle(type, 0);
                }
            },
            //处理试题
            repair_handle: function (type, index) {
                var arr = this.results['type' + type].err;
                if (arr.length < 1) return;
                var ques = arr[index];
                var items = ques.Qus_Items;
                var random = Math.ceil(Math.random() * 1000);
                for (let i = 0; i < items.length; i++) {
                    items[i].Ans_ID = i + 1 + random;
                }
                var q = $api.clone(ques);
                q.Qt_ID = 0;
                console.log(q);
                var th = this;
                $api.post('Question/Modify', { 'entity': q }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.results['type' + type].repair++;
                        //var index = th.questions.findIndex(x => x.Qus_ID == q.Qus_ID);
                        if (index < arr.length - 1)
                            th.repair_handle(type, index + 1);
                        //th.$delete(th.questions, index);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    th.load_repair = false;
                    console.error(err);
                });
            },
            //是否显示错误题数
            errshow: function (type) {
                var err = this.results['type' + type].err.length;
                return err > 0;
            }
        }
    });
}, ['/Utilities/Components/question/function.js']);
