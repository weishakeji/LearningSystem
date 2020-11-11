window.addEventListener('load', function () {
    window.noticeApp = new Vue({
        el: "#notice_box",
        created: function () {

        },
    });
}, false);
// 注册
Vue.component('popup-notice', {
    props: ['forpage'],
    data: function () {
        return {
            items: [],          //通知公告的项目
            islogin: false,       //学员是否登录
            account: null        //当前登录学员账号
        }
    },
    watch: {
        items: function (nl, ol) {
            if (nl.length == ol.length) return;           
        }
    },
    computed: {
        //最外侧样式
        shell: function () {          
            return 'width:calc(100vw * ' + this.items.length + ');\
            height: 100vh;position: fixed;\
            top: 0px;left: 0px;z-index: 99999;overflow: hidden;\
            background-color: rgba(0, 0, 0, 0.8);';
        },
        //当前视图
        view: function () {
            return 'text-align: center;line-height: 100%;display: flex;\
            flex-wrap: wrap;width: 100vw;height: 100%;float:left;\
            position: relative;';
        },
        //通知内容区域
        notice: function () {
            return function (item) {
                var mobi = this.ismoblie();
                var width, height;
                if (mobi) {
                    width = (item.No_Width > 100 ? 100 : item.No_Width) + '%';
                    height = (item.No_Height > 100 ? 100 : item.No_Height) + '%';
                } else {
                    width = (item.No_Width < 100 ? 100 : item.No_Width) + 'px';
                    height = (item.No_Height < 100 ? 100 : item.No_Height) + 'px';
                }
                return 'width:' + width + ';height:' + height + ';\
                background-color: #fff;border-radius: 5px;overflow: hidden;margin: auto;';
            }
        },
        //标题样式，如果没有图片，显示文字内容，
        title: function () {
            return 'width: 100%;line-height: 40px;padding: 15px;font-size: 18px;';
        },
        context: function () {
            return 'padding: 0px 15px ;font-size: 16px;text-align: left;overflow: auto;height: calc(100% - 90px);';
        },
        //关闭按钮
        close: function () {
            return function (item) {
                var mobi = this.ismoblie();
                var postion = '', color = '', wh = 30;
                if (mobi) {
                    postion = 'bottom:calc((100vh - ' + item.No_Height + '%)/4 - 15px);left: calc((100% - 30px)/2);';
                    color = 'rgb(214, 67, 67)';
                } else {
                    var width = item.No_Width < 100 ? 100 : item.No_Width;
                    var height = item.No_Height < 100 ? 100 : item.No_Height;
                    postion = 'top: calc((100% - ' + height + 'px)/2 + 5px); right: calc((100vw - ' + width + 'px)/2 + 5px);';
                    color = 'rgb(214, 67, 67)';
                    wh = 20;
                }
                var shadow = 'box-shadow: -3px 0px 3px rgba(255, 255, 255, 0.6), \
                0px -3px 3px rgba(255, 255, 255, 0.6), \
                3px 0px 3px rgba(255, 255, 255, 0.6), \
                0px 3px 3px rgba(255, 255, 255, 0.6);';
                return postion + ' position: absolute;font-size: ' + wh + 'px;line-height: ' + (wh * 0.85) + 'px;text-align: center;\
                width: '+ wh + 'px;height: ' + wh + 'px;border-radius: 15px;z-Index:10;cursor: pointer;\
                background-color: rgba(255, 255, 255,0.8);border: '+ color + ' solid 2px;\
                color: '+ color + ';'+(mobi ? '' : shadow);
            }
        },
        //数秒的样式
        second: function () {
            return function (item) {
                var mobi = this.ismoblie();
                var postion = ''
                if (mobi) {
                    var width = item.No_Width > 100 ? 100 : item.No_Width;
                    var height = item.No_Height > 100 ? 100 : item.No_Height;
                    postion = 'top: calc((100vh - ' + height + '%)/2 + 5px); right: calc((100vw - ' + width + '%)/2 + 5px); ';
                } else {
                    var width = item.No_Width < 100 ? 100 : item.No_Width;
                    var height = item.No_Height < 100 ? 100 : item.No_Height;
                    postion = 'top: calc((100vh - ' + height + 'px)/2 + 5px); right: calc((100vw - ' + width + 'px)/2 + 40px); ';
                }
                postion += 'position: absolute; padding: 5px; color: red; font-size: 16px; ';
                return postion;
            }
        }
    },
    mounted: function () {
        var th = this;
        $api.bat(
            $api.get('Notice/OpenItems', { 'forpage': this.forpage }),
            $api.get('Account/Current')).then(axios.spread(function (n, a) {
                //当前登录学员
                th.islogin = a.data.success;
                if (a.data.success) th.account = a.data.result;
                //通知公告
                if (n.data.success) {
                    //校验时间段
                    th.items = th.verifyTimeSpan(n.data.result);
                    //验证分组
                    th.items = th.verifySort(th.items, th.account);
                    //验证打开次数
                    th.items = th.verifyCount(th.items);
                }
            })).catch(function (err) {
                console.error(err);
            });
        //关闭时间倒计时
        window.setTimeout(function () {
            window.setInterval(function () {
                if (th.items.length > 0) {
                    if (th.items[0].No_Timespan <= 0) return;
                    if (th.items[0].No_Timespan > 1) {
                        th.items[0].No_Timespan--;
                    } else {
                        th.btnClose(th.items[0].No_Id);
                    }
                }
            }, 1000);
        }, 500);
    },
    methods: {
        //是否是手机端
        ismoblie: function () {
            var prefix = '';
            if (this.forpage.indexOf('_'))
                prefix = this.forpage.substring(0, this.forpage.indexOf('_'));
            return prefix == 'mobi';
        },
        //打开网址
        goUrl: function (item) {
            var mobi = this.ismoblie();
            var page = mobi ? '/Mobile/notice.ashx' : '/notice.ashx'
            window.location.href = item.No_Linkurl != '' ? item.No_Linkurl : page + '?id=' + item.No_Id;
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
                var interval = items[i].No_Interval == '' ? [] : JSON.parse(items[i].No_Interval);
                if (interval.length < 1) continue;
                var exist = false;
                for (var j = 0; j < interval.length; j++) {
                    interval[j]['start'] = Date.parse(interval[j]['start']);
                    interval[j]['end'] = Date.parse(interval[j]['end']);
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
            var storagename = "notice-count-record";
            var countrecord = $api.storage(storagename);
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
            $api.storage(storagename, countrecord);
            return items;
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: '<div :style="shell" remark="区域" >\
        <div v-for="(item,index) in items" :style="view" remark="通知视图">\
            <div :style="notice(item)" v-on:click="goUrl(item)" remark="通知内容">\
                <img :src="item.No_BgImage" v-if="item.No_BgImage!=\'\'" v-on:click="goUrl(item)" \
                style="width:100%;height:100%;" />\
                <template v-else>\
                    <div :style="title">{{item.No_Ttl}}</div>\
                    <div :style="context" v-html="item.No_Context"></div>\
                </template>\
            </div>\
            <div remark="关闭按钮" :style="close(item)" v-on:click="btnClose(item.No_Id)">&times</div>\
            <div remark="数秒" :style="second(item)" v-if="item.No_Timespan>0">{{item.No_Timespan}}</div>\
        </div>\
    <div>'
})
