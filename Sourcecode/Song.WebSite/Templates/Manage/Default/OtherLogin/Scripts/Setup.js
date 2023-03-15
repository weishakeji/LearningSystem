$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
           
        },
        watch: {
        },
        created: function () {

        },
        methods: {
            //打开设置项的窗体
            opensetup: function (item) {
                console.log(item.name);
                var file = item.tag;
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "OtherLogin_" + item.name + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: 600,
                    height: 400,
                    resize: true,
                    id: boxid,
                    pid: window.name,
                    ico: item.icon,
                    url: url + '?id=' + $api.querystring('id')
                });
                box.title = item.name + " - 设置项";
                box.open();
            },
            //刷新
            reload: function () {
                alert(2)
                window.location.reload();
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js']);