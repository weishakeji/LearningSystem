$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            columns: [],     //新闻栏目
            curr_column: null,     //当前选中的栏目
            form: {
                'orgid': '', 'uid': '', 'search': '', 'verify': '', 'del': '', 'order': '', 'isintro': false,
                size: 20, index: 1
            },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: true,
            loading_init: true,
            loadingid: 0
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //console.error(th.organ);
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.getColumnsTree();
            }).catch(err => console.error(err))
                .finally(() => { });

            this.handleCurrentChange(1);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'curr_column': {
                deep: true,
                handler: function (newV, oldV) {
                    this.handleCurrentChange(1);
                }
            }
        },
        methods: {
            //加载栏目树
            getColumnsTree: function () {
                var th = this;
                $api.get('News/ColumnsTree', { 'orgid': th.organ.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.columns = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_init = false);
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.form.uid = this.curr_column ? this.curr_column.Col_UID : '';
                th.loading = true;
                $api.get("News/ArticlePager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                        th.rowdrop();
                    } else {
                        throw d.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //刷新行数据，
            freshrow: function (id) {
                this.getColumnsTree();
                if (id == null || id == '' || id == 0) return this.handleCurrentChange();
                if (this.datas.length < 1) return;
                //要刷新的行数据
                let entity = this.datas.find(item => item.Art_ID == id);
                if (entity == null) return;
                //获取最新数据，刷新
                var th = this;
                th.loadingid = id;
                $api.get('News/Article', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        let index = th.datas.findIndex(item => item.Art_ID == id);
                        if (index >= 0) th.$set(th.datas, index, result);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('News/ArticleDelete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });                        
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //设置置顶
            btnsettop: function (row) {
                row.Art_IsTop = !row.Art_IsTop;
                this.changeState(row);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Art_ID;
                $api.post('News/ArticleModifyState', { 'id': row.Art_ID, 'use': row.Art_IsUse, 'top': row.Art_IsTop })
                    .then(function (req) {
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!'
                            });
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loadingid = 0);
            },
             //行的拖动
             rowdrop: function () {
                // 首先获取需要拖拽的dom节点            
                const el1 = document.querySelectorAll('table > tbody')[0];
                Sortable.create(el1, {
                    disabled: false, // 是否开启拖拽
                    ghostClass: 'sortable-ghost', //拖拽样式
                    handle: '.draghandle',     //拖拽的操作元素
                    animation: 150, // 拖拽延时，效果更好看
                    group: { // 是否开启跨表拖拽
                        pull: false,
                        put: false
                    },
                    onEnd: (e) => {
                        var table = this.$refs.datatables;
                        let indexkey = table.$attrs['index-key'];
                        let arr = this.datas; // 获取表数据
                        arr.splice(e.newIndex, 0, arr.splice(e.oldIndex, 1)[0]); // 数据处理
                        this.$nextTick(function () {
                            this.datas = arr;
                            let tmarr = [];
                            for (let i = 0; i < this.datas.length; i++) 
                                tmarr.push(this.datas[i][indexkey]);                            
                            tmarr.sort((a, b) => b - a);
                            for (let i = 0; i < this.datas.length; i++)
                                this.datas[i][indexkey] = tmarr[i];                    
                            this.changeTax();
                        });
                    }
                });
            },
            //更新排序
            changeTax: function () {
                var arr = $api.clone(this.datas);
                var th = this;
                $api.post('News/ModifyArticleOrder', { 'items': arr }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改顺序成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            }
        }
    });

}, ['Components/article_count.js']);
