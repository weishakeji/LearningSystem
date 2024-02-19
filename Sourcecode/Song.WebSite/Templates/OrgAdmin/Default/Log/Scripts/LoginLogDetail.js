$ready(['/Utilities/baiduMap/map_show.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                id: $api.dot() != '' ? $api.dot() : $api.querystring('id'),      //记录id
                entity: {},          //记录的实体对象
                account: {},        //账号对象
                loading: false,
                loading_bind: ''      //绑定或取消绑定中的状态，
            },
            mounted: function () {
                this.getEntity();
            },
            created: function () {

            },
            computed: {
                //学员账号是否存在
                existacc: t => !$api.isnull(t.account),
                existent: t => !$api.isnull(t.entity),
            },
            watch: {
            },
            methods: {
                //获取当前登录账号
                getEntity: function () {
                    var th = this;
                    th.loading = true;
                    $api.get('Account/LoginLogForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            th.entity = req.data.result;
                            th.getAccount(th.entity.Ac_ID);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                },
                //获取学员账号信息
                getAccount: function (acid) {
                    var th = this;
                    $api.get('Account/ForID', { 'id': acid }).then(function (req) {
                        if (req.data.success) {
                            th.account = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => { });
                },
                //显浏览时间
                timeclac: function (second) {
                    if (second < 60) return second + ' 秒';
                    let mm = Math.floor(second / 60);
                    if (mm < 60) return mm + ' 分钟';
                    let hh = Math.floor(mm / 60);
                    if (hh < 24) return hh + ' 小时' + (mm % 60 > 0 ? mm % 60 + '分钟' : '');
                },
                //地理位置
                address: function (row) {
                    if (!$api.isnull(row.Lso_Address) && row.Lso_Address!='') return row.Lso_Address;
                    if (row.Lso_Province == row.Lso_City) return row.Lso_Province + row.Lso_District
                    return row.Lso_Province + row.Lso_City + row.Lso_District;
                },
            }
        });

    }, []);
