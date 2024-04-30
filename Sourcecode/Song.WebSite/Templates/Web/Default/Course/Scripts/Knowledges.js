$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),
            sorts: [],         //课程知识库的分类      
            form: { 'couid': '', 'kns': '', 'isuse': '', 'search': '', 'size': 10, 'index': 1 },
            knls: [],           //知识点的集合
            total: 1, //总记录数
            totalpages: 1, //总页数

            knlVisible: false,       //显示知识
            sort_current: null,      //当前选中的知识分类
            knl_current: null,           //当前要展示的知识点

            loading: false,
            loading_init: true
        },
        mounted: function () {
            this.getTreeData();
            this.handleCurrentChange(1);
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //获取分类的数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading_init = true;
                $api.get('Knowledge/SortTree', { 'couid': th.couid, 'search': '', 'isuse': true }).then(function (req) {
                    if (req.data.success) {
                        th.sorts = req.data.result;
                    } else {
                        th.sorts = [];
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            },
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.form.couid = this.couid;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 41);
                if ($api.getType(th.form.uid) == "Array" && th.form.uid.length > 0) {
                    th.form.uid = th.form.uid[th.form.uid.length - 1];
                }
                $api.get("Knowledge/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.knls = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //分类节点击事件
            nodeclick: function (data) {
                this.sort_current = data;
                this.form.kns = data.Kns_ID;
                this.handleCurrentChange(1);
            },
            nodeclose: function () {
                this.sort_current = null;
                this.form.kns = 0;
                this.handleCurrentChange(1);
            },
        }
    });

}, []);
