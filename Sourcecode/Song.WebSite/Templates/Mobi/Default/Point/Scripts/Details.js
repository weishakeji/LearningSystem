$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项       
            loading: false,
            loading_init: true,
            show_detail: false,      //显示详情
            show_item: {},           //当前显示的项

            datas: [],           //数据列表
            finished: false,
            query: { 'acid': '', 'start': '', 'end': '', 'type': '', 'search': '', 'size': 10, 'index': 0 },
            total: 0
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            details: function () {
                var datas = this.datas;
                var fmt = "yyyy年 M月";
                var month = [];
                for (var i = 0; i < datas.length; i++) {
                    var str_mh = datas[i]["Pa_CrtTime"].format(fmt);
                    let isexist = false;
                    for (var j = 0; j < month.length; j++) {
                        if (month[j]['month'] == str_mh) {
                            isexist = true;
                            break;
                        }
                    }
                    if (!isexist)
                        month.push({
                            'month': str_mh,
                            'items': []
                        });
                }
                for (var i = 0; i < datas.length; i++) {
                    var str_mh = datas[i]["Pa_CrtTime"].format(fmt);
                    for (var j = 0; j < month.length; j++) {
                        if (month[j]['month'] == str_mh) {
                            month[j]['items'].push(datas[i]);
                        }
                    }
                }
                console.log(month);
                return month;
            }
        },
        watch: {
        },
        methods: {
            onload: function () {
                var th = this;
                if (!th.islogin) {
                    window.setTimeout(function () {
                        vapp.onload();
                    }, 100);
                    return;
                }
                th.query.index++;
                var query = $api.clone(this.query);
                console.log(query);
                $api.get('Point/PagerForAccount', query).then(function (req) {
                    if (req.data.success) {
                        th.total = req.data.total;
                        var result = req.data.result;
                        for (var i = 0; i < result.length; i++) {
                            th.datas.push(result[i]);
                        }
                        var totalpages = req.data.totalpages;
                        // 数据全部加载完成
                        if (th.datas.length >= th.total || result.length == 0) {
                            th.finished = true;
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }

                }).catch(function (err) {
                    th.error = err;
                    th.finished = true;
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除记录
            btnDelete: function (item) {
                var th = this;
                this.$dialog.confirm({
                    title: '删除记录',
                    message: '您是否确定删除当前记录？',
                }).then(() => {
                    $api.delete('Point/DeleteSingle', { 'id': item.Pa_ID }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            if (result == true) {
                                th.$toast.success('删除成功');
                                th.datas = [];
                                th.query.index = 0;
                                th.finished = false;
                                th.total = false;
                                th.onload();
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                }).catch(() => {
                    // on cancel
                });
            },
            //备注
            remark: function (item) {
                var remark = $api.trim(item.Pa_Remark) != "" ? item.Pa_Remark : item.Pa_Info;
                return remark;
            }
        }
    });

}, ['../Components/page_header.js']);
