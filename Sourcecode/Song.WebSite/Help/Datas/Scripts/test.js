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
        dbmsName: '未知',   //数据库类型（即数据库产品名称）
        dbName: '',         //数据库名称
        dbverison: '',        //数据库版本
        editon: null,         //当前学习系统的产品版本信息

        connState: false,    //数据库是否链接       
        compDatas: [],       //数据完整性信息，这里是缺失的表和字段

        loadstate: {
            init: false,        //初始化
            conn: false,         //数据库连接
            info: false,         //数据库信息，名称与版本
            check: false,      //检测数据库完整性
            editon: false          //产品版本
        },
        //错误信息
        errorstate: {
            init: '',
            conn: '',
            info: '',
            check: '',
            editon: ''
        },
    },
    computed: {
        //是否正在加载
        loading: function () {
            if (!this.loadstate) return false;
            for (let key in this.loadstate) {
                if (this.loadstate.hasOwnProperty(key)
                    && this.loadstate[key])
                    return true;
            }
            return false;
        },
        //是否存在错误
        existerr: function () {
            if (!this.errorstate) return false;
            for (let key in this.errorstate) {
                if (this.errorstate.hasOwnProperty(key)
                    && this.errorstate[key] != '')
                    return true;
            }
            return false;
        }
    },
    watch: {

    },
    created: function () {
        //检测数据库链接
        this.getDbms();
        //获取产品商业版本的信息
        this.getEditon();
    },
    methods: {
        //检测数据库的类型（即数据库产品名称）
        getDbms: function () {
            var th = this;
            th.loadstate.init = true;
            th.errorstate.init = '';
            $api.get('DataBase/DBMS').then(req => {
                if (req.data.success) {
                    th.dbmsName = req.data.result;
                    th.checkConn();
                } else {
                    throw req.data.message;
                }
            }).catch(err => {
                console.error(err);
                th.errorstate.init = err;
            }).finally(() => th.loadstate.init = false);
        },
        //检测链接
        checkConn: function () {
            var th = this;
            //检测链接是否正常          
            th.loadstate.conn = true;
            th.connState = false;
            th.errorstate.conn = '';
            $api.post('DataBase/CheckConn').then(function (req) {
                if (req.data.success) {
                    th.connState = req.data.result;
                    if (th.connState) {
                        th.getdbinfo();     //获取数据库信息 
                        th.checkComplete(); //检查字段是否完整
                    }
                } else {                   
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => {
                console.error(err);
                th.errorstate.conn = err;
            }).finally(() => th.loadstate.conn = false);
        },
        //获取数据库信息，如版本，数据库名称
        getdbinfo: function () {
            var th = this;
            th.loadstate.info = true;
            th.errorstate.info = '';
            $api.bat(
                $api.post('DataBase/DbVersion'),
                $api.post('DataBase/DbName'),
            ).then(([ver, name]) => {
                th.dbverison = ver.data.result;
                th.dbName = name.data.result;
            }).catch(err => {
                console.error(err);
                th.errorstate.info = err;
            }).finally(() => th.loadstate.info = false);
        },
        //检测完整性
        checkComplete: function () {
            var th = this;
            th.loadstate.check = true;
            th.errorstate.check = '';
            th.compDatas = [];
            $api.post('DataBase/CheckFully').then(function (req) {
                if (req.data.success) {
                    th.compDatas = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => {
                console.error(err);
                th.errorstate.check = err;
            }).finally(() => th.loadstate.check = false);
        },
        //获取当前学习系统的产品版本信息
        getEditon: function () {
            var th = this;
            th.loadstate.editon = true;
            th.errorstate.editon = '';
            $api.post('Platform/edition').then(function (req) {
                if (req.data.success) {
                    th.editon = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => {
                console.error(err);
                th.errorstate.editon = err;
            }).finally(() => th.loadstate.editon = false);
        }
    }
});

