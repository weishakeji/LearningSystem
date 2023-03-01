
// 通知公告的弹窗
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
        //通知内容区域
        notice: function () {
            return function (item) {
                var mobi = this.ismoblie();
                var width, height;
                if (mobi) {
                    width = (item.No_Width > 100 || item.No_Width <= 0 ? 100 : item.No_Width) + '%';
                    height = (item.No_Height > 100 || item.No_Height <= 0 ? 100 : item.No_Height) + '%';
                } else {
                    width = (item.No_Width < 100 ? 100 : item.No_Width) + 'px';
                    height = (item.No_Height < 100 ? 100 : item.No_Height) + 'px';
                }
                let css = 'width:' + width + ';height:' + height + ';';
                if (item.No_BgImage == '' || item.No_BgImage == undefined) {
                    return 'width:' + width + '  !important;height:' + height + '  !important;background-color: #fff;';
                } else {
                    return 'width:' + width + ';height:' + height + ';';
                }
            }
        }
    },
    created: function () {
        $dom.load.css(['/Utilities/Components/Styles/popup-notice.css']);
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
        //图片加载完成
        imgload: function (event) {
            if (!this.ismoblie) return;
            const width = event.target.width;
            const height = event.target.height;
            //const max = width > height ? width : height;
            var parent = $dom(event.target).parent();
            parent.height(height);
            //parent.css('background-color', '#fff');
            console.log(width);
        },
        //打开网址
        goUrl: function (item) {
            var mobi = this.ismoblie();
            var page = mobi ? '/mobi/notice/Detail' : '/web/notice/Detail'
            window.location.href = item.No_Linkurl != '' ? item.No_Linkurl : page + '.' + item.No_Id;
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
            if (items.length < 1) return items;
            for (var i = 0; i < items.length; i++) {
                //所有学员都弹出
                if (items[i].No_Range == 1) continue;
                //未登录学员弹出
                if (items[i].No_Range == 2) {
                    if (account == null) continue; else items.splice(i--, 1);
                    if (items.length < 1) break;
                }
                //已登录学员弹出
                if (items[i].No_Range == 3 || items[i].No_Range == 4) {
                    if (account == null) {
                        items.splice(i--, 1);
                        if (items.length < 1) break; else continue;
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
                            items.splice(i--, 1);
                            if (items.length < 1) break;
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
    //
    template: `<div id="notice_box" :style="'width:calc(100vw * ' + items.length + ')'" class="open_notice_shell" remark="区域" :mobi="ismoblie()" >
        <div v-for="(item,index) in items" class="open_notice_view" remark="通知视图">
            <div :style="notice(item)" remark="通知内容">
                <img :src="item.No_BgImage" v-if="item.No_BgImage!=''" @load="imgload" v-on:click="goUrl(item)"
                    style="max-width:100%;max-height:100%;"  />
                <template v-else>
                    <div class="open_notice_title"  v-on:click="goUrl(item)">{{item.No_Ttl}}</div>
                    <div class="open_notice_context"  v-on:click="goUrl(item)" v-html="item.No_Context"></div>
                </template>
                <div remark="关闭" class="open_notice_close" @click="btnClose(item.No_Id)">&#xe72c</div>
                <div remark="数秒" class="open_notice_second" v-if="item.No_Timespan>0">{{item.No_Timespan}}</div>                
            </div>            
        </div>
    </div>`
})
