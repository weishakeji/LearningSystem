$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            dbType: '',   //数据库类型
            dbName: '',     //数据库名称
            verison: '',    //数据库版本

            loadstate: {
                init: false,        //初始化
                def: false,         //默认
                get: false,         //加载数据
                update: false,      //更新数据
                del: false          //删除数据
            }
        },
        mounted: function () {
            this.getdbtype();
            this.getdbversion();
        },
        created: function () {

        },
        computed: {
            loading: function () {
                if (!this.loadstate) return false;
                for (let key in this.loadstate) {
                    if (this.loadstate.hasOwnProperty(key)
                        && this.loadstate[key])
                        return true;
                }
                return false;
            }
        },
        watch: {

        },
        methods: {
            //检测数据库名称
            getdbtype: function () {
                var th = this;
                th.loadstate.get = true;
                $api.get('DataBase/DbType').then(req => {
                    if (req.data.success) {
                        th.dbType = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.get = false);
            },
            //获取版本信息
            getdbversion: function () {
                var th = this;
                th.loadstate.def = true;
                $api.post('DataBase/DbVersion').then(function (req) {
                    if (req.data.success) {
                        th.verison = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.def = false);
            }
        },
        filters: {

        },
        components: {

        }
    });
});