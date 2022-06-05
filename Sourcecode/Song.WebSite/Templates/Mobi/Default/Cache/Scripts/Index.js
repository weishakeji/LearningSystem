$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //跳转到api缓存管理页面
            goApipage: function () {               
                var url = $api.url.set('api', {});
                //history.pushState({}, "", url);
                window.location.href = url;
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
