window.addEventListener('load', function () {
    window.noticeApp = new Vue({
        el: "#notice_box",
        data: {
            items: [],          //通知公告的项目
            islogin: false,       //学员是否登录
            account: null        //当前登录学员账号
        },
        watch: {
        },
        created: function () {
            $api.bat(
                $api.get('Notice/OpenItems', { 'forpage': 'mobi_home' }),
                $api.get('Account/Current')).then(axios.spread(function (n, a) {
                    //当前登录学员
                    noticeApp.islogin = a.data.success;
                    if (a.data.success) noticeApp.account = a.data.result;
                    //通知公告
                    if (n.data.success) {
                        //校验时间段
                        noticeApp.items = noticeApp.verifyTimeSpan(n.data.result);
                        //验证分组
                        noticeApp.items = noticeApp.verifySort(noticeApp.items, noticeApp.account);
                        //验证打开次数
                        noticeApp.items = noticeApp.verifyCount(noticeApp.items);
                    }
                })).catch(function (err) {
                    console.error(err);
                });
            //关闭时间倒计时
            window.setTimeout(function () {
                window.setInterval(function () {
                    if (noticeApp.items.length > 0) {
                        if (noticeApp.items[0].No_Timespan > 1) {
                            noticeApp.items[0].No_Timespan--;
                        } else {
                            noticeApp.btnClose(noticeApp.items[0].No_Id);
                        }
                    }
                }, 1000);
            }, 500);
        },
        methods: {
            //打开网址
            goUrl: function (item) {
                window.location.href = item.No_Linkurl != '' ? item.No_Linkurl : '/Mobile/notice.ashx?id=' + item.No_Id;
            },
            //关闭公告
            btnClose: function (id) {
                for (var i = 0; i < this.items.length; i++) {
                    if (this.items[i].No_Id == id) {
                        this.items.splice(i, 1);
                        break;
                    }
                }
            },
            //校验时间段，不在时间段的公告不弹出
            verifyTimeSpan: function (items) {
                //判断时间段
                var time = new Date();
                for (var i = 0; i < items.length; i++) {
                    var interval = JSON.parse(items[i].No_Interval);
                    if (interval.length < 1) continue;
                    var exist = false;
                    for (var j = 0; j < interval.length; j++) {
                        interval[j]['start'] = new Date(time.format('yyyy-MM-dd ') + interval[j]['start']);
                        interval[j]['end'] = new Date(time.format('yyyy-MM-dd ') + interval[j]['end']);
                        var start = interval[j]['start'];
                        var end = interval[j]['end'];
                        if (interval[j]['start'] <= time && time <= interval[j]['end']) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) {
                        items.splice(i, 1);
                        i--;
                    }
                }
                return items;
            },
            //验证学员分组（前提是登录）
            verifySort: function (items, account) {
                for (var i = 0; i < items.length; i++) {
                    //所有学员都弹出
                    if (items[i].No_Range == 1) continue;
                    //未登录学员弹出
                    if (items[i].No_Range == 2) {
                        if (account == null) {
                            continue;
                        } else {
                            items.splice(i, 1);
                            i--;
                        }
                    }
                    //已登录学员弹出
                    if (items[i].No_Range == 3 || items[i].No_Range == 4) {
                        if (account == null) {
                            items.splice(i, 1);
                            i--;
                            continue;
                        }
                        //没有处于指定分组的通知，不弹出
                        if (items[i].No_Range == 4) {
                            var isexist = false;
                            if (items[i].No_StudentSort != '') {
                                var sorts = JSON.parse(items[i].No_StudentSort);
                                for (var j = 0; j < sorts.length; j++) {
                                    if (sorts[j] == account.Sts_ID) {
                                        isexist = true;
                                        break;
                                    }
                                }
                            }
                            if (!isexist) {
                                items.splice(i, 1);
                                i--;
                            }
                        }
                    }
                }
                return items;
            },
            //验证打开次数，每天打开几次
            verifyCount: function (items) {
                var countrecord = $api.storage("countrecord");
                if (countrecord == undefined) countrecord = {};
                var date = (new Date()).format('yyyy-MM-dd');
                //今天的弹出记录
                var today = countrecord[date];
                if (today == undefined) {
                    today = {};
                    for (var i = 0; i < items.length; i++)
                        today['id_' + items[i].No_Id] = 1;
                    countrecord[date] = today;
                } else {
                    for (var i = 0; i < items.length; i++) {
                        if (items[i].No_OpenCount <= 0) continue;   //小于等于0，为无限次
                        var history = today['id_' + items[i].No_Id];
                        if (history >= items[i].No_OpenCount) {
                            items.splice(i, 1);
                            i--;
                        } else {
                            today['id_' + items[i].No_Id] += 1;
                        }
                    }
                }
                $api.storage("countrecord", countrecord);               
                return items;
            }
        }
    });
}, false);