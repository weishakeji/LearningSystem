$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            orgid: 0,
            form: {
                'orgid': '', 'stsid': '', 'couid': '',
                'acc': '', 'name': '', 'idcard': '', 'mobi': '', 'start': '', 'end': '',
                'size': '10', 'index': '1'
            },
            datas: [],       //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading: false
        },
        watch: {

        },
        computed: {

        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                //获取结果             
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.handleCurrentChange(1);
            })).catch(err => console.error(err))
                .finally(() => { });
        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                //this.handleCurrentChange(1);
            },
             //显示电话
             showTel: function (row) {
                if (row.Ac_MobiTel1 == '' && row.Ac_MobiTel2 == '') {
                    return '';
                }
                if (row.Ac_MobiTel1 == '') row.Ac_MobiTel1 = row.Ac_MobiTel2;
                if (row.Ac_MobiTel1 == row.Ac_MobiTel2) {
                    return row.Ac_MobiTel1;
                }
                return row.Ac_MobiTel1 + (row.Ac_MobiTel2 != '' ? '/' + row.Ac_MobiTel2 : '');
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 115;
                th.form.size = Math.floor(area / 41);
                th.loading = true;
                $api.get("Account/purchasepager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }                  
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
        }
    });

}, []);