
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //分组id
            id: Number($api.querystring('id', '0')),
            organ: {},       //当前机构
            config: {},

            accounts: [],
            form: { 'orgid': '', 'sortid': '', 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '', 'index': 1, 'size': '' },
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading_init: false,
            loadingid: 0,
            loading: false
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                th.organ = organ.data.result;
                if (th.id == "") th.entity.Org_ID = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.form.sortid = th.id;
                th.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });

        },
        mounted: function () {
            //var t = this.$refs['btngroup'];
            this.$refs['btngroup'].addbtn({
                text: '添加课程', tips: '添加课程到当前学员组',
                id: 'addcourse', type: 'primary',
                icon: 'e813'
            });
            this.$refs['btngroup'].addbtn({
                text: '批量移除', tips: '批量移除课程',
                id: 'batremove', type: 'warning',
                icon: 'e800'
            });
            //console.log(t);
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 120;
                th.form.size = Math.floor(area / 41);
                $api.get("Account/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.accounts = d.data.result;
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

            //显示全屏Loading
            showloading: function () {
                return this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(255, 255, 255, 0.3)'
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', false);
            },
        },
    });
});
