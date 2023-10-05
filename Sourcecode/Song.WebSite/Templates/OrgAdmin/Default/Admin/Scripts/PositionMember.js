$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            organ: {},
            config: {},      //当前机构配置项     
            position: {},        //当前岗位         
            //人员查询
            form: {
                name: '',
                size: 20,
                index: 1
            },
            accounts: [], //账号列表
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //左内里数据表中选中的行

            posiacconts: [],     //当前岗位的人员列表
            posiselects: [],     //选中岗位人员

            loading: {
                init: false,
                left: false,
                right: false,
                update: false
            }
        },
        mounted: function () {
            var th = this;
            th.loading.init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.get('Position/ForID', { 'id': th.id })
            ).then(axios.spread(function (organ, posi) {
                th.loading.init = false;
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
                th.employelist(1);
                th.getPosiAccount();
            })).catch(function (err) {
                th.loading.init = false;
                th.$alert(err, '错误');
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //加载人员列表
            employelist: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 150;
                th.form.size = Math.floor(area / 41);
                th.loading.left = true;
                $api.get("Admin/List", th.form).then(function (d) {
                    th.loading.left = false;
                    if (d.data.success) {
                        for (var i = 0; i < d.data.result.length; i++) {
                            d.data.result[i].isAdminPosi = false;
                        }
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.loading.left = false;
                    console.error(err);
                });
            },
            //加载当前岗位的人员
            getPosiAccount: function () {
                var th = this;
                th.loading.right = true;
                $api.get('Position/Emplyees', { 'id': th.id }).then(function (req) {
                    th.loading.right = false;
                    if (req.data.success) {
                        th.posiacconts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading.right = false;
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //向右选择，即选中的人员到右侧岗位人员列表
            toRight: function () {
                var list = this.selects;
                if (list.length < 1) return;
                var tm = [];
                for (var i = 0; i < this.posiacconts.length; i++)
                    tm.push(this.posiacconts[i]);
                for (var i = 0; i < list.length; i++) {
                    var exist = false
                    for (var j = 0; j < tm.length; j++) {
                        if (tm[j].Acc_Id == list[i].Acc_Id) {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist) tm.push(list[i]);
                }
                this.posiacconts = [];
                this.posiacconts = tm;
            },
            //向左，即取消岗位人员
            toLeft: function () {
                var sel = this.posiselects;
                var acc = $api.clone(this.posiacconts);
                for (var i = 0; i < sel.length; i++) {
                    for (var j = 0; j < acc.length; j++) {
                        if (acc[j].Acc_Id == sel[i].Acc_Id) {
                            acc.splice(j, 1);
                            j = j - 1;
                        }
                    }
                }
                this.posiacconts = [];
                this.posiacconts = acc;
                this.posiselects = [];
                console.log(sel);
            },
            btnEnter: function (isclose) {
                var arr = [];
                var acc = this.posiacconts;
                for (var j = 0; j < acc.length; j++) {
                    arr.push(acc[j].Acc_Id);
                }
                var th = this;
                th.loading.update = true;
                $api.post('Position/UpdateEmp4Posi', { 'datas': arr, 'posid': th.id })
                    .then(function (req) {
                        th.loading.update = false;
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$notify({
                                type: 'success', position: 'bottom-left', duration: 2000,
                                message: '操作成功!'
                            });
                            th.operateSuccess(isclose);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading.update = false;
                        th.$alert(err, '错误');
                        console.error(err);
                    });
            },
            //操作成功
            operateSuccess: function (isclose) {
                if (window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', isclose);
            }
        }
    });

});
