$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            form: {
                lsid: '',
                isused: false,
                isback: false,
                isdisable: false,
                card: '',
                account: '',
                size: 20,
                index: 1
            },
            cardset: { 'courses': [] },     //学习卡的设置项
            datas: [],      //卡号列表数据
            current: {},     //查看当前行
            currentVisible: false,       //是否显示账号详情
            cardsetVisible: false,       //是否显示设置项
            cardsetLoading: false,       //是否在加载设置信息
            loading: false,
            loadingid: false,
            selects: [], //数据表中选中的行   
            total: 1, //总记录数
            totalpages: 1, //总页数
            num: {
                total: 0,
                used: 0,     //已经使用的个数
                rollbak: 0,      //回滚的个数
                disable: 0       //禁用个数
            }
        },
        watch: {
            'cardsetVisible': function (nl, vl) {
                if (nl) this.getdatainfo();
            }
        },
        computed: {

        },
        created: function () {
            var th = this;
            this.form.lsid = $api.querystring('id');
            th.cardsetLoading = true;
            $api.get('Learningcard/SetForID', { 'id': th.form.lsid }).then(function (req) {
                th.cardsetLoading = false;
                if (req.data.success) {
                    th.cardset = req.data.result;
                    th.cardset.courses = [];
                    $api.get('Learningcard/SetCourses', { 'id': th.form.lsid }).then(function (req) {
                        if (req.data.success) {
                            th.cardset.courses = [];
                            th.cardset['courses'] = req.data.result;
                        }
                    }).catch(function (err) {
                        th.$alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.$alert(err);
                th.cardsetLoading = false;
                console.error(err);
            });
        },
        methods: {
            //添加单个
            addsingle: function (acc) {
                console.log(acc);
            },
            //批量添加
            addbatch:function(accounts){
                console.log(accounts);
            }
        }
    });

}, ['/Utilities/Components/student_batch.js']);