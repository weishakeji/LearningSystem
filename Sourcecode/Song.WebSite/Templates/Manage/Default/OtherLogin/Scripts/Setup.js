$ready(function () {
    //获取某一个登录配置项的记录
    Vue.component('login_item', {
        props: ['item'],
        data: function () {
            return {
                obj: {},        //当前登录配置项的对象
                loading: false
            }
        },
        watch: {
            'item': {
                handler: function (nv, ov) {
                    if (!nv.tag) return;
                    this.getobject(nv.tag);
                }, immediate: true
            }
        },
        methods: {
            getobject: function (tag) {
                var th = this;
                th.loading = true;
                $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                    if (req.data.success) {
                        th.obj = req.data.result;
                        th.item['obj'] = th.obj;
                        th.$set(th.item.obj, th.obj);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        },
        template: `<div :class="{'disabled':item.Tl_IsUse}">              
                    <loading v-if="loading"></loading>
                    <slot v-else name="item" :item="obj"></slot>
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
            //更改使用状态
            changeuse: function (item) {
                var th = this;
                $api.post('OtherLogin/ModifyUse', { 'tag': item.Tl_Tag, 'isue': item.Tl_IsUse}).then(function (req) {
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
            reload: function () {
                window.location.reload();
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js']);