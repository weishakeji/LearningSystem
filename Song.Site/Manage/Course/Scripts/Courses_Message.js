var mesvue = new Vue({
    data: {
        outlines: [],     //当前课程的章节列表（树形）
        messages: [],        //咨询留言
        msgobj: {},             //当前咨询留言的对象
        dialogFormVisible: false,        //弹窗
        couid: $api.querystring("couid"),
        olid: $api.querystring("olid"),
        size: 15,        //每页多少条    
        index: 1,     //第几页
        total: 0,        //总数  
        loading: false         //右侧章节列表加载中
    },
    computed: {
        //表格的最大高度
        tableHeight: function () {
            var area = document.getElementById("editRight").offsetHeight - 30;
            return area;
        }
    },
    created: function () {
        var couid = $api.querystring("couid");
        $api.get("Outline/tree", { couid: couid }).then(function (req) {
            mesvue.outlines = req.data.result;
            mesvue.outlineClick($api.querystring("olid"), null);
        }).catch(function (err) {

        })
    },
    methods: {
        //章节点击事件
        outlineClick: function (olid, event) {
            //判断是否是当前章节
            if (event != null) {
                var classlist = event.target.classList;
                for (var t in classlist) {
                    if (classlist[t] == 'current') return;
                }
            }
            olid = olid == null || olid == '' ? 0 : olid;
            var url = $api.setpara("olid", olid);
            mesvue.olid = olid;
            history.pushState({}, null, url);
            mesvue.index = 1;
            mesvue.msgGet(olid);
        },
        handleCurrentChange: function (index) {
            if (index > 0) mesvue.index = index;
            mesvue.msgGet($api.querystring("olid"));
        },
        //获取当前章节的留言信息
        msgGet: function (olid) {
            //计算每页多少行
            var area = document.getElementById("editRight").offsetHeight - 70;
            mesvue.size = Math.floor(area / 48);
            var vm = mesvue;
            $api.get('Message/Pager', { 'couid': vm.couid, 'olid': olid, 'search': '', 'size': vm.size, 'index': vm.index }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    mesvue.messages = result;
                    mesvue.total = req.data.total;
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        //表格行的点击事件
        rowClick: function (datarow, column, event) {
            mesvue.dialogFormVisible = true;
            mesvue.msgobj = datarow;
        },
        //修改留言
        msgEdit: function () {
            $api.get('Message/Update',{'msid':mesvue.msgobj.Msg_Id,'msg':mesvue.msgobj.Msg_Context}).then(function(req){
                if(req.data.success){
                    var result=req.data.result;
                    mesvue.$message({
                        type: 'success',
                        message: '操作成功!'
                    });
                    mesvue.dialogFormVisible = false;
                    mesvue.handleCurrentChange(-1);
                }else{
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        //删除留言
        msgDel: function () {
            var msg = '此操作将永久该项（“' + mesvue.msgobj.Msg_Context + '”） <br/>是否继续?'
            this.$confirm(msg, '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                dangerouslyUseHTMLString: true,
                type: 'warning'
            }).then((btn) => {
                if (btn != 'confirm') return;
                $api.get('Message/Delete', { 'msid': mesvue.msgobj.Msg_Id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        mesvue.$message({
                            type: 'success',
                            message: '删除成功!'
                        });
                        mesvue.dialogFormVisible = false;
                        mesvue.handleCurrentChange(-1);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            }).catch(() => { });
        }
    },
    //过滤器
    filters: {
        //日期
        date: function (val, fmt) {
            if ($api.getType(val) != 'Date') return val;
            return val.Format(fmt);
        }
    },
    components: {
        "msgcount": {
            // 声明 props，用于向组件传参
            props: ['olid'],
            data: function () {
                return {
                    count: 0,     //章节下留言的数据
                    loading: true,       //预载中
                    open: false      //是否打开方法列表
                }
            },
            created: function () {
                var th = this;
                var couid = $api.querystring("couid");
                $api.get('Message/Count', { 'couid': couid, 'olid': th.olid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.count = result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            },
            template: "<span> \
            {{count}}\
             </span>"
        }
    }
});
mesvue.$mount('#app');



