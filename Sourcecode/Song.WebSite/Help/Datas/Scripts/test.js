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
            this.loadingConn = true;
            vapp.connState = false;
            vapp.verison = '';
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
                vapp.loadingConn = false;
                //获取结果
                vapp.connState = conn.data.result;
                vapp.verison = ver.data.result;
                vapp.checkComplete();
            })).catch(function (err) {
                console.error(err);
                vapp.loadingConn = false;
                vapp.connState = false;
                vapp.verison = '';
            });
        },
        //检测完整性
        checkComplete: function () {
            this.loadingComp = true;
            this.compDatas=[];
            $api.post('Platform/DbCheck').then(function (req) {
                if (req.data.success) {
                    vapp.compDatas= req.data.result;
                    //...
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
                vapp.loadingComp = false;
            }).catch(function (err) {
                vapp.loadingComp = false;
                console.error(err);
            });
        }
    }
});

