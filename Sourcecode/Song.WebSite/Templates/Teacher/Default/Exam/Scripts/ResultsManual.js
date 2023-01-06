$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            entity: {}, //当前考试对象
            form: {
                examid: $api.querystring('id'),
                name: '', idcard: '',
                min: -1, max: -1,
                size: 20, index: 1
            },
            results: [],     //当前考试的所有成绩信息
            total: 1, //总记录数
            totalpages: 1, //总页数

            accountVisible: false,   //是否显示当前学员
            account: {},      //当前学员信息
            current: {},     //当前行对象


            loading: false,
            loadingid: 0,
        },
        computed: {

        },
        watch: {

        },
        created: function () {
            //获取考试信息
            var th = this;
            $api.get('Exam/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    th.entity = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
            this.handleCurrentChange(1, 0);
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index, move) {
                if (move == null) move = 0;
                if (index != null) this.form.index = index;
                var th = this;
                this.loading = true;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 105;
                th.form.size = Math.floor(area / 40);
                $api.get("Exam/Result4Exam", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.results = d.data.result;
                        if (move == 0 && th.results.length > 0)
                            th.setCurrent(th.results[0]);
                        if (move < 0 && th.results.length > 0)
                            th.setCurrent(th.results[th.results.length - 1]);
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //计算考试用时
            calcSpan: function (d1, d2) {
                if (d1 == null || d2 == null) return '';
                var total = (d2.getTime() - d1.getTime()) / 1000;
                var span = Math.floor(total / 60);
                return span <= 0 ? "<1" : span;
            },
            //设置为当前学员
            setCurrent: function (row, column, event) {
                row.index = this.getArrayIndex(row);
                this.current = row;
                this.$refs['datatables'].setCurrentRow(row);
                this.getaccount(this.current);
                console.log(row);
            },
            // 获取一个元素在数组中的下标
            getArrayIndex: function (obj) {
                var arr = this.results;
                var i = arr.length;
                while (i--) {
                    if (arr[i].Exr_ID === obj.Exr_ID) {
                        return i;
                    }
                }
                return -1;
            },
            //取上一位和下一位，move为移动值，1或-1
            moveCurrent: function (move) {
                var index = this.current.index + move;
                if (index < 0) {
                    if (this.form.index > 1)
                        this.handleCurrentChange(--this.form.index, move);
                    return;
                }
                if (index >= this.results.length) {
                    if (this.form.index < this.totalpages)
                        this.handleCurrentChange(++this.form.index, 0);
                    return;
                }
                this.setCurrent(this.results[index]);
            },
            //当查看学员信息时，获取当前学员信息
            getaccount: function (row) {
                var th = this;                
                $api.get('Account/ForID', { 'id': row.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.account = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },

        },
        components: {
            //得分的输出，为了小数点对齐
            'score': {
                props: ['number'],
                data: function () {
                    return {
                        prev: '',
                        dot: '.',
                        after: ''
                    }
                },
                created: function () {
                    var num = String(Math.round(this.number * 100) / 100);
                    if (num.indexOf('.') > -1) {
                        this.prev = num.substring(0, num.indexOf('.'));
                        this.after = num.substring(num.indexOf('.') + 1);
                    } else {
                        this.prev = num;
                        this.dot = '&nbsp;';
                    }
                },
                template: `<div class="score">
                    <span class="prev">{{prev}}</span>
                    <span class="dot" v-html="dot"></span>
                    <span class="after">{{after}}</span>
                    </div>`
            }
        }
    });

});