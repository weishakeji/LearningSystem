$ready(function () {
    //获取某一个登录配置项的记录
    Vue.component('login_item', {
        props: ['tag'],
        data: function () {
            return {
                item: {},        //当前登录配置项的对象
                loading: false
            }
        },
        watch: {
            'tag': {
                handler: function (nv, ov) {
                    this.getobject(nv);
                }, immediate: true
            }
        },
        methods: {
            getobject: function (tag) {
                var th = this;
                th.loading = true;
                $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                    if (req.data.success) {
                        th.item = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        },
        template: `<div>              
                    <loading v-if="loading"></loading>
                    <slot v-else name="item" :item="item"></slot>
                </div>`
    });
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
                window.location.reload();
            }
        }
    });
    
}, ['/Utilities/OtherLogin/config.js']);