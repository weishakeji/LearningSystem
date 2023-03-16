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
                    <slot v-else name="item" :obj="obj"></slot>
                </div>`
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading_tag: ''      //当前刷新的
        },
        watch: {
        },
        created: function () {

        },
        methods: {
            //打开设置项的窗体
            opensetup: function (item) {
                var file = item.tag;
                //文件路径
                var url = window.location.href;
                url = url.substring(0, url.lastIndexOf("/") + 1) + file;
                var boxid = "OtherLogin_" + item.name + "_" + file;
                //创建
                var box = window.top.$pagebox.create({
                    width: item.width, height: item.height,
                    resize: true, id: boxid, pid: window.name,
                    ico: item.icon, url: url
                });
                box.title = item.name + " - 设置项";
                box.open();
            },
            //更改使用状态
            changeuse: function (item) {
                var th = this;
                $api.post('OtherLogin/ModifyUse', { 'tag': item.Tl_Tag, 'isue': item.Tl_IsUse }).then(function (req) {
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
                var th = this;
                th.loading_tag = tag;
                $api.get('OtherLogin/GetObject', { 'tag': tag }).then(function (req) {
                    if (req.data.success) {
                        var obj = req.data.result;
                        var items = th.$refs['config'].items;
                        for (let i = 0; i < items.length; i++) {
                            if (items[i].tag == tag) {
                                console.log(items[i].tag);
                                items[i].obj = obj;
                                //th.$set(items[i].obj, obj);
                                th.$set(items, i, $api.clone(items[i]));
                            }
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_tag = '');
                // window.location.reload();
            }
        }
    });

}, ['/Utilities/OtherLogin/config.js']);