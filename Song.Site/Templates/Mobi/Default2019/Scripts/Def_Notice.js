window.addEventListener('load', function () {
    window.noticeApp = new Vue({
        el: "#notice_box",
        data: {
            items: []
        },
        watch: {
        },
        created: function () {
            $api.get('Notice/OpenItems', { 'forpage': 'mobi_home' }).then(function (req) {
                if (req.data.success) {
                    var items = req.data.result;
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
                            if (interval[j]['start'] < time && time <= interval[j]['end']) {
                                exist = true;
                                break;
                            }
                        }
                        if (!exist) {
                            items.splice(i, 1);
                            i--;
                        }
                    }

                    noticeApp.items = items;
                    console.debug(3);
                    console.error(3)
                    //...
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
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
            btnClose: function (id) {
                //console.log('关闭公告:' + id);
                for (var i = 0; i < this.items.length; i++) {
                    if (this.items[i].No_Id == id) {
                        this.items.splice(i, 1);
                        break;
                    }
                }
            }
        }
    });
}, false);