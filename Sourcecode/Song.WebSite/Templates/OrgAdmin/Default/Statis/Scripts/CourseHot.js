$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        
            
            subjects: [],     //所有专业数据
            defaultProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            sbjSelects: [],      //选择中的专业项           
            form: {
                'orgid': '', 'sbjid': '', 'count': 20
            },
            datas: [],      //数据集
            loading: false,
            loading_init: true
        },
        mounted: function () {
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
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
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                vapp.form.orgid = vapp.organ.Org_ID;
                vapp.getTreeData();
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取课程专业的数据
            getTreeData: function () {
                var th = this;              
                var form = {
                    orgid: vapp.organ.Org_ID,
                    search: '', isuse: true
                };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.getCourseHot();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取热门课程
            getCourseHot: function () {
                var th = this;
                th.loading = true;
                if (this.sbjSelects && this.sbjSelects.length > 0)
                    th.form.sbjid = this.sbjSelects[this.sbjSelects.length - 1];
                $api.get('Course/MostHot', th.form).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.datas=result;
                        console.log(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ['../Course/Components/course_data.js',
'../Course/Components/course_income.js',
'../Course/Components/course_prices.js']);
