$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //章节id,学员账号
            olid: $api.querystring("olid"),
            acc: $api.querystring("acc"),
            //数据实体
            messages: [], //咨询留言
            //
            msg: '', //要提交的信息
            loading: false, //加载中
            loading_init: false       //初始加载
        },
        watch: {

        },
        created: function () {
            //定时刷新（加载）咨询留言
            window.setInterval('vapp.msgGet()', 1000 * 6);
        },
        mounted: function () {           
            this.msgGet(true);
        },
        methods: {
            //发送消息
            msgSend: function () {
                var th = this;
                if ($api.trim(th.msg) == '') return;
                var span = Date.now() - Number($api.cookie("msgtime"));
                if (span / 1000 < 10) {
                    return th.$notify({
                        message: '不要频繁发消息！',
                        position: 'bottom-right'
                    });
                }
                $api.cookie("msgtime", Date.now());
                th.loading = true;
                $api.post("message/add", {
                    acc: th.acc, msg: th.msg, playtime: 0, couid: 0, olid: th.olid
                }).then(function (req) {
                    if (req.data.success) {
                        th.msg = '';
                        th.msgGet();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert("信息添加发生异常！详情：\r" + err);
                }).finally(() => th.loading = false);
            },
            //获取消息，init：是否为初始加载，初始加载显示loading图标
            msgGet: function (init) {
                if (!this.olid || this.olid == '') return;
                var th = this;
                if (init) th.loading_init = true;
                $api.post("message/All", { olid: th.olid, order: 'asc' }).then(function (req) {
                    if (req.data.success) {
                        th.messages = req.data.result;
                        window.setTimeout(function () {
                            var dl = document.getElementById("chatlistdl");
                            document.getElementById("chatlist").scrollTop = dl.offsetHeight;
                        }, 1000);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert("信息加载异常！详情：\r" + err);
                }).finally(() => th.loading_init = false);
            }
        }
    });
});