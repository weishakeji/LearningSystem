
$ready(function () {

    window.vue = new Vue({
        el: '#vapp',
        data: {
            accounts: [],     //要删除的账号列表
            total: 0,            //总计个数
            //用于计算每条数据的操作时间
            time: {
                start: null,
                end: null
            },
            timespan: 0,

            loading: false,       //正在操作中
            loadingid: 0             //当前正在操作的id

        },
        watch: {
            'accounts': {
                handler: function (nv, ov) {
                    //当要删除的学员不存在了，则预载状态也去除
                    if (nv.length < 1) this.loading = false;
                }, deep: true
            }
        },
        computed: {
            //剩余的时间，小时分
            'time_remaining': function () {
                var time = this.timespan/(this.total - this.accounts.length) * this.accounts.length;
                var secondTime = parseInt(time / 1000);// 秒
                var minuteTime = 0;// 分
                var hourTime = 0;// 小时
                if (secondTime > 60) {//如果秒数大于60，将秒数转换成整数
                    //获取分钟，除以60取整数，得到整数分钟
                    minuteTime = parseInt(secondTime / 60);
                    //获取秒数，秒数取佘，得到整数秒数
                    secondTime = parseInt(secondTime % 60);
                    //如果分钟大于60，将分钟转换成小时
                    if (minuteTime > 60) {
                        //获取小时，获取分钟除以60，得到整数小时
                        hourTime = parseInt(minuteTime / 60);
                        //获取小时后取佘的分，获取分钟除以60取佘的分
                        minuteTime = parseInt(minuteTime % 60);
                    }
                }
                var result = "" + parseInt(secondTime) + "秒";
                if (minuteTime > 0) result = "" + parseInt(minuteTime) + "分" + result;
                if (hourTime > 0) result = "" + parseInt(hourTime) + "小时" + result;
                return result;
            }
        },
        created: function () {

        },
        methods: {
            //选择要删除的账号
            selected: function (datas) {
                if (datas.length < 1) return;
                for (let i = 0; i < datas.length; i++) {
                    const el = datas[i];
                    let index = this.accounts.findIndex(x => x.Ac_ID == el.Ac_ID);
                    if (index < 0) this.accounts.push(el);
                }
                //console.log(this.accounts);
            },
            //全部删除的按钮事件
            btn_enter_event: function () {
                this.$confirm('此操作将永久删除账号信息, 且不可恢复；<br/> 包括其产生的学习记录等关联信息也会被删除；<br/> 是否继续?', '敬告', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    dangerouslyUseHTMLString: true,
                    type: 'warning'
                }).then(() => {
                    this.$confirm('此操作不可恢复，建议在该操作之前做好数据库备份；<br/> 是否继续?', '再次确认', {
                        confirmButtonText: '确定',
                        cancelButtonText: '取消',
                        dangerouslyUseHTMLString: true,
                        type: 'error'
                    }).then(() => {
                        //开始删除
                        this.loading = true;
                        this.total = this.accounts.length;
                        this.timespan = 0;
                        this.delete_func();
                    }).catch(() => { });
                }).catch(() => { });
            },
            //删除的具体方法
            delete_func: function () {
                if (this.accounts.length < 1) return;
                var th = this;
                th.loadingid = th.accounts[0].Ac_ID;
                th.time.start = new Date();
                $api.delete('Account/Delete', { 'id': th.loadingid }).then(function (req) {
                    th.loadingid = 0;
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.accounts.findIndex(x => x.Ac_ID == result);
                        th.$delete(th.accounts, index);
                        th.time.end = new Date();
                        th.timespan += th.time.end.getTime() - th.time.start.getTime();
                        th.delete_func();
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loadingid = 0;
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ['/Utilities/Components/student_batch.js']);
