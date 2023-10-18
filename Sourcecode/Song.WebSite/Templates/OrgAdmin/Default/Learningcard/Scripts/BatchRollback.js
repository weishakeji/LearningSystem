$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            cardset: {},
            cards: [],
            usednum: 0,    //已经使用过的学习
            backnum: 0,      //需要回滚的学习卡          
            loading: false,
            backing: false      //正在回滚
        },
        watch: {
            'cards': function (nl, ol) {
                //已经使用过的学习，已经回滚过的学习卡
                let usednum = 0, backnum = 0;
                for (let index = 0; index < nl.length; index++) {
                    const element = nl[index];
                    if (element.Lc_IsUsed) {
                        usednum++;
                        if (element.Lc_State == -1)
                            backnum++;
                    }
                }
                this.usednum = usednum;
                this.backnum = backnum;
            }
        },
        created: function () {
            this.loading = true;
            $api.bat(
                $api.get('Learningcard/SetForID', { 'id': this.id }),
                $api.get('Learningcard/Cards', { 'lsid': this.id, 'enable': '', 'used': ''  })
            ).then(axios.spread(function (cardset, cards) {
                vue.loading = false;
                //获取结果
                vue.cardset = cardset.data.result;
                vue.cards = cards.data.result;
            })).catch(function (err) {
                console.error(err);
            });
        },
        computed: {
            percentage: function () {
                var need = this.usednum - this.backnum;
                if (need == 0) return 100;
                var p = this.backnum / need;
                return Math.round(p * 10000) / 100;
            }
        },
        methods: {
            goback: function (clear) {
                if ((this.usednum - this.backnum) <= 0) {
                    this.$alert('没有供回滚的学习卡', '提示', {
                        confirmButtonText: '确定',
                        callback: action => { }
                    });
                    return;
                }
                var msg = clear ? ' 回滚，且清除学习记录' : '回滚，但保留学习记录';
                this.$confirm('您选择了“' + msg + '”<br/>是否确定进行批量回滚?', '确认', {
                    dangerouslyUseHTMLString: true,
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.gobackFunc(clear);
                }).catch(action => {

                });
            },
            gobackFunc: function (clear) {
                if ((this.usednum - this.backnum) <= 0) return;
                this.backing = true;
                for (let index = 0; index < this.cards.length; index++) {
                    const element = this.cards[index];
                    if (!element.Lc_IsUsed) continue;
                    if (element.Lc_State -= 1) continue;
                    var para = { 'code': element.Lc_Code, 'pw': element.Lc_Pw, 'clear': clear };
                    $api.post('Learningcard/CardRollback', para).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            vue.backnum++;
                            if ((vue.usednum - vue.backnum) <= 0)
                                vue.backing = false;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }
            }
        },
        mounted: function () {

        }
    });
});