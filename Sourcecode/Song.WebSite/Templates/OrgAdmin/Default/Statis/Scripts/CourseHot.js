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
                'orgid': '', 'sbjid': '', 'start': '', 'end': '', 'size': 10, 'index': 1
            },
            datas: [],      //数据集
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.getTreeData();
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
        },
        methods: {
            //选择时间区间
            selectDate: function (start, end) {
                this.form.start = start;
                this.form.end = end;
                this.handleCurrentChange(1);
            },
            //获取课程专业的数据
            getTreeData: function () {
                var th = this;
                var form = { orgid: th.organ.Org_ID, search: '', isuse: true };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.handleCurrentChange(1);
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => console.error(err)).finally(() => { });
            },
            //获取热门课程
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 57);

                th.loading = true;
                $api.get('Course/MostHot', th.form).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.datas = result;
                        th.totalpages = Number(req.data.totalpages);
                        th.total = req.data.total;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        }
    });

}, ['/Utilities/Components/sbj_cascader.js',
    '../Course/Components/course_data.js',
    '../Course/Components/course_income.js',
    '../Course/Components/course_prices.js']);
