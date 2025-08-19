/*!
 * 主 题：检测数据库链接与字段
 * 说 明：
 * 1、检测数据库链接是否正常
 * 2、检查数据库表和字段，与系统是否匹配;
 *
 * 作 者：微厦科技_宋雷鸣_10522779@qq.com
 * github开源地址:https://github.com/weishakeji/LearningSystem
 */
window.vapp = new Vue({
    el: '#vapp',
    data: {
        dbType: '未知',   //数据库类型
        dbName: '',     //数据库名称
        verison: '', //数据库版本

        connState: false,    //数据库是否链接       
        compDatas: [],       //数据完整性信息，这里是缺失的表和字段
        error: '',          //提示信息

        editon:{},      //版本信息

        loadingConn: false,     //
        loadingComp: false,     //比较字段是否完整的预载状态

    },
    computed: {

    },
    watch: {

    },
    created: function () {
        //检测数据库链接
        this.checkConn();
        
        //获取产品商业版本的信息
        this.getEditon();
    },
    methods: {
        //检测链接
        checkConn: function () {
            var th = this;
            //检测数据库名称
            $api.get('DataBase/DBMS').then(req => {
                if (req.data.success) {
                    th.dbType = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
            //检测链接是否正常
            th.error = '';
            th.loadingConn = true;
            th.connState = false;
            $api.post('DataBase/CheckConn').then(function (req) {
                if (req.data.success) {
                    th.connState = req.data.result;
                    if (th.connState) {
                        th.getDbname();
                        th.getversion();
                        th.checkComplete();
                    }
                } else {
                    th.error = req.data.message;
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loadingConn = false);
        },
        //获取数据库版本信息
        getversion: function () {
            var th = this;
            $api.post('DataBase/DbVersion').then(function (req) {
                if (req.data.success) {
                    th.verison = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err));
        },
        //获取数据库名称
        getDbname: function () {
            var th = this;
            $api.post('DataBase/DbName').then(function (req) {
                if (req.data.success) {
                    th.dbName = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => th.dbName = '');
        },
        //检测完整性
        checkComplete: function () {
            var th = this;
            th.loadingComp = true;
            th.compDatas = [];
            $api.post('DataBase/CheckFully').then(function (req) {
                if (req.data.success) {
                    th.compDatas = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loadingComp = false);
        },
        //获取版本信息
        getEditon: function () {
            var th = this;
            th.loading = true;
            $api.post('Platform/edition').then(function (req) {
                if (req.data.success) {
                    th.editon = req.data.result;
                    console.log(th.editon);
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err=> console.error(err));
        }
    }
});

