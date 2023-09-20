$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项    

            form: {
                orgid: '', sortid: '', use: '',
                show: '', name: '', link: '',
                size: 20, index: 1
            },
            sorts: [],       //友情链接分类
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: false,
            loadingid: 0,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                $api.get('Link/SortCount',
                    { 'orgid': th.organ.Org_ID, 'use': true, 'show': '', 'search': '', 'count': 0 })
                    .then(function (req) {
                        if (req.data.success) {
                            th.sorts = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                        th.handleCurrentChange(1);
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.loading = true;
                $api.cache("Link/Pager:update", th.form).then(function (d) {
                    console.log(3);
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                $api.delete('Link/Delete', { 'id': datas }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //双击事件
            rowdblclick: function (row, column, event) {
                this.$refs.btngroup.modify(row[this.$refs.btngroup.idkey]);
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                th.loadingid = row.Lk_Id;
                $api.post('Link/Modify', { 'entity': row }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loadingid = -1);
            }
        }
    });

}, ['Components/links_count.js']);
