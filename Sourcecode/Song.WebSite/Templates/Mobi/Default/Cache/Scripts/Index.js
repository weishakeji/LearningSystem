$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {
        },
        methods: {
            //跳转到api缓存管理页面
            goApipage: function () {
                let url = $api.url.set('api', {});
                //history.pushState({}, "", url);
                window.navigateTo(url);
            },
            //清除试题练习的记录
            clearQues: function () {
                this.$dialog.alert({
                    title: '禁止',
                    message: '功能暂未开放，请前往试题练习中清理',
                }).then(() => {
                    // on close
                });
            },
            //清除状态缓存
            clearState: function () {
                var storage = $api.storage();
                if (storage.length > 0) {
                    var msg = '用于存储状态信息的localStorage中，有' + storage.length + '条数据，是否全部清理？'
                    this.$dialog.confirm({
                        title: '是否清理状态信息',
                        message: msg,
                    }).then(() => {
                        for (var i = 0; i < storage.length; i++) {
                            var key = localStorage.key(i);
                            storage.removeItem(key);
                        }
                        this.$notify({ type: 'success', message: '清理成功！' });

                    }).catch(function () {

                    });
                } else {
                    this.$dialog.alert({
                        title: '提示',
                        message: '没表相关的数据信息，无须清理。',
                    }).then(() => {
                        // on close
                    });
                }
            }
        }
    });

}, ['../Components/page_header.js']);
