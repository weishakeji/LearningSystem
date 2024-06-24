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
        database: '',   //数据库名称
        verison: '', //数据库版本
        connState: false,    //数据库是否链接       
        compDatas: [],       //数据完整性信息，这里是缺失的表和字段
        error: '',          //提示信息

        loadingConn: false,     //
        loadingComp: false,     //比较字段是否完整的预载状态

    },
    computed: {

    },
    watch: {

    },
    created: function () {
        this.checkConn();
    },
    methods: {
        //检测链接
        checkConn: function () {
            var th = this;
            //检测数据库名称
            $api.get('Platform/DataBase').then(req => {
                if (req.data.success) {
                    th.database = req.data.result;
                } else {
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
            //检测链接是否正常
            th.error = '';
            th.loadingConn = true;
            th.connState = false;
            $api.post('Platform/DbConnection').then(function (req) {
                if (req.data.success) {
                    th.connState = req.data.result;
                    if (th.connState) {
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
            $api.post('Platform/DbVersion').then(function (req) {
                if (req.data.success) {
                    th.verison = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err));
        },
        //检测完整性
        checkComplete: function () {
            var th = this;
            th.loadingComp = true;
            th.compDatas = [];
            $api.post('Platform/DbCheck').then(function (req) {
                if (req.data.success) {
                    th.compDatas = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loadingComp = false);
        }
    }
});

