$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {},
        watch: {},
        created: function () { },
        methods: {
            //打开设置项的窗体
            opensetup: function (item) {
                if (item.disabled) {
                    alert('尚未开发，敬请期待');
                    return;
                }
                var file = item.tag;
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "OtherLogin_" + item.name + "_" + file;
                //创建
                if (window.top.$pagebox) {
                    var box = window.top.$pagebox.create({
                        width: item.width, height: item.height,
                        resize: true, id: boxid, pid: window.name,
                        ico: item.icon, url: url
                    });
                    box.title = item.name + " - 设置项";
                    box.open();
                }
            },
            //更改使用状态
            changeuse: function (item) {
                var th = this;
                let obj = item.obj;
                $api.post('OtherLogin/ModifyUse', { 'tag': item.tag, 'id': obj.Tl_ID, 'isue': obj.Tl_IsUse })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                message: '修改状态成功',
                                type: 'success'
                            });
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        console.error(err);
                    });
            },
            //刷新
            reload: function (tag) {
                this.$refs['config'].fresh(tag);
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js']);