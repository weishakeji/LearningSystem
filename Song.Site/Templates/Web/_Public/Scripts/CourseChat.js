//章节id,学员账号
var olid = $api.querystring("olid");
var acc = $api.querystring("acc");
//
var vdata = new Vue({
    data: {
        //数据实体
        messages: [], //咨询留言
        //状态
        state: {}, //课程状态      
        olid: $api.querystring("olid"),
        loading: false //加载中
    },
    watch: {

    },
    methods: {
        //发送消息
        msgSend: function() {
            var msg = document.getElementById("messageinput").value;
            if ($api.trim(msg) == '') return;
            var span = Date.now() - Number($api.cookie("msgtime"));
            if (span / 1000 < 10) {
                vdata.$notify({
                    message: '不要频繁发消息！',
                    position: 'bottom-right'
                });
                return;
            }
            $api.cookie("msgtime", Date.now());
            $api.post("message/add", {
                acc: acc,
                msg: msg,
                playtime: 0,
                couid: 0,
                olid: olid
            }).then(function(req) {
                var d = req.data;
                if (d.success) {
                    document.getElementById("messageinput").value = '';
                    vdata.msgGet();
                } else {
                    alert("信息添加发生异常！详情：\r" + d.message);
                }
            });
        },
        msgGet: function() {
            if (!olid || olid < 1) return;
            $api.post("message/All", {
                olid: olid,
                order: 'asc'
            }).then(function(req) {
                var d = req.data;
                if (d.success) {
                    vdata.messages = d.result;
                    window.setTimeout(function() {
                        var dl = document.getElementById("chatlistdl");
                        document.getElementById("chatlist").scrollTop = dl.offsetHeight;
                    }, 1000);
                } else {
                    alert("留言信息加载异常！详情：\r" + d.message);
                }
            }).catch(function(err) {
                //alert("msgGet方法存在错误："+err);
            });
        }
    },
    created: function() {
        //定时刷新（加载）咨询留言
        window.setInterval('vdata.msgGet()', 1000 * 20);
    },
    mounted: function() {
        this.msgGet();
    }
});
vdata.$mount('#vue-app');
//全局过滤器，日期格式化
Vue.filter('date', function(value, fmt) {
    if ($api.getType(value) != 'Date') return value;
    var o = {
        "M+": value.getMonth() + 1,
        "d+": value.getDate(),
        "h+": value.getHours(),
        "m+": value.getMinutes(),
        "s+": value.getSeconds()
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (value.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
});