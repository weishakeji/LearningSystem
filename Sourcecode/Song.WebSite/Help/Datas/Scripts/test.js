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
        verison: '', //数据库版本
        connState: false,    //数据库是否链接       
        compDatas: [],       //数据完整性信息，这里是缺失的表和字段
        loadingConn: false,
        loadingComp: false,

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
            th.loadingConn = true;
            th.connState = false;
            th.verison = '';
            $api.bat(
                $api.post('Platform/DbConnection'),
                $api.post("Platform/DbVersion")
            ).then(axios.spread(function (conn, ver) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }             
                //获取结果
                th.connState = conn.data.result;
                th.verison = ver.data.result;
                th.checkComplete();
            })).catch(function (err) {
                console.error(err);             
                th.connState = false;
                th.verison = '';
            }).finally(() => {
                th.loadingConn = false;
            });
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
            }).catch(function (err) {               
                console.error(err);
            }).finally(() => {
                th.loadingComp = false;
            });
        }
    }
});

