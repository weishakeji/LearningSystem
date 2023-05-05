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
                'orgid': '', 'uid': '', 'search': '', 'verify': '', 'del': '', 'order': '', size: 20,
                index: 1
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
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                //获取结果             
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.form.orgid = vapp.organ.Org_ID;
                vapp.getColumnsTree();
            })).catch(function (err) {
                console.error(err);
            });
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
                $api.get('News/ColumnsTree', { 'orgid': vapp.organ.Org_ID }).then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.columns = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                    //th.loading = false;
                }).catch(function (err) {
                    th.loading_init = false;
                    console.error(err);
                });
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
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            fresh: function () {
                this.getColumnsTree();
                this.handleCurrentChange();
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('News/ArticleDelete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.fresh();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
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
                $api.post('News/ArticleModifyState',
                    { 'id': row.Art_ID, 'use': row.Art_IsUse, 'top': row.Art_IsTop })
                    .then(function (req) {
                        th.loadingid = -1;
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!'
                            });
                        } else {
                            throw req.data.message;
                        }
                        th.loadingid = 0;
                    }).catch(function (err) {
                        th.$alert(err, '错误');
                        th.loadingid = 0;
                    });
            },
        }
    });

}, ['Components/article_count.js']);
