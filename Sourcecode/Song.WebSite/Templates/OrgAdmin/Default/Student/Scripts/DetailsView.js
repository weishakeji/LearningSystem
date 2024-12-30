$ready(function () {
    //禁用鼠标右键
    document.addEventListener('contextmenu', function (e) {
        //设置章节学习进度为完成
        window.vapp.updatePercentConfirm();
        e.preventDefault();
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            stid: $api.querystring('stid'),
            couid: $api.dot(),
            account: {},     //当前登录账号       

            outlines: [],
            logdatas: [],

            loading_init: true,
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/ForID', { 'id': th.stid }),
                $api.cache('Outline/TreeList', { 'couid': th.couid })
            ).then(([account, outlines]) => {
                //获取结果
                th.account = account.data.result;
                th.outlines = outlines.data.result;
                console.log(th.outlines);
                if (th.islogin) th.getlogs(true);
            }).catch(function (err) {
                alert(err);
                console.error(err);
            }).finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
            //是否登录        
            islogin: t => !$api.isnull(t.account)
        },
        watch: {
        },
        methods: {
            //加载日志数据,iscache:是否启用缓存
            getlogs: function (iscache) {
                var th = this;
                th.loading = true;
                var acid = th.account.Ac_ID;
                let active = iscache ? 10 : 'update';
                $api.cache('Course/LogForOutlineVideo:' + active, { 'stid': acid, 'couid': th.couid }).then(function (req) {
                    if (req.data.success) {
                        th.logdatas = req.data.result;
                        //console.log(th.logdatas);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //修改课程的学习进度，所有章节
            updatePercentConfirm: function () {
                var outlines = this.$refs['outline'];
                let unfinished = false;
                for (var i = 0; i < outlines.length; i++) {
                    let data = outlines[i].data;
                    if (data == null) continue;
                    let percentage = outlines[i].percentage;
                    if (percentage < 100) {
                        unfinished = true;
                        break;
                    }
                }
                //如果都都完成了，则不提示
                if (!unfinished) return;
                //
                var th = this;
                th.$confirm('是否将当前课程的所有章节设置为完成?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    th.updatePercent();
                }).catch(() => { });
            },
            //修改课程的学习进度，所有章节
            updatePercent: function () {
                var outlines = this.$refs['outline'];
                for (var i = 0; i < outlines.length; i++) {
                    let data = outlines[i].data;
                    if (data == null) continue;
                    let percentage = outlines[i].percentage;
                    if (percentage < 100) {
                        outlines[i].updatePercent();
                    }
                    //console.log(percentage);
                }
                //console.log(outlines);
                //alert(1);
            },
        }
    });

}, ['Components/outline_progress.js']);
