$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            dbType: '',  //数据库类型
            dbName: '',     //数据库名称
            version: '',    //数据库版本

            connState: false,    //数据库是否链接 
            error: '',          //提示信息   

            compDatas: [],       //数据完整性信息，这里是缺失的表和字段

            loadstate: {
                init: false,        //初始化
                version: false,         //获取版本信息
                conn: false,         //测试连接               
                name: false,          //获取数据库名称
                check:false,        //检测数据库
            }
        },
        mounted: function () {
            this.getDbtype();
            this.getDbinfo();
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
            //获取数据库类型
            getDbtype: function () {
                var th = this;
                th.loading.init = true;
                $api.get('DataBase/DbType').then(req => {
                    if (req.data.success) {
                        th.dbType = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading.init = false);
            },
            //获取数据库信息
            getDbinfo: function () {
                var th = this;
                //检测链接是否正常
                th.error = '';
                th.loadstate.conn = true;
                th.connState = false;
                $api.post('DataBase/CheckConn').then(function (req) {
                    if (req.data.success) {
                        th.connState = req.data.result;
                        if (th.connState) {
                            th.loadstate.version = true;
                            $api.post('DataBase/DbVersion').then(function (req) {
                                if (req.data.success) th.verison = req.data.result;
                            }).finally(() => th.loadstate.version = false);
                            th.loadstate.name = true;
                            $api.post('DataBase/DbName').then(function (req) {
                                if (req.data.success) th.dbName = req.data.result;
                            }).finally(() => th.loadstate.name = false);
                        }
                    } else {
                        th.error = req.data.message;
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.conn = false);
            },
            //检测完整性
            checkFully: function () {
                var th = this;
                th.loadstate.check = true;
                th.compDatas = [];
                $api.post('DataBase/CheckFully').then(function (req) {
                    if (req.data.success) {
                        th.compDatas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadstate.check = false);
            },
            //检测冗余
            checkRedundancy:function(){

            },
            //检测正确性，即字段与设计类型是否一致
            checkCorrect:function(){

            }
        },
        filters: {

        },
        components: {

        }
    });
});