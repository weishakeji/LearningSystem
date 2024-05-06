$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            org: {},
            orgid: 0,

            stat: {},       //机构的各种统计数据
            video: {},     //视频数据
            resources: {},   //课程图文资料
            question: {},       //试题资料
            news: {},          //新闻资料存储空间数据

            loading_init: true,
            loading_stat: true
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([org]) => {
                //获取结果             
                th.org = org.data.result;
                th.orgid = th.org.Org_ID;

                th.getStatistics();
                th.getVideo();
                th.getcourseRes();
                th.getquestion();
                th.getnews();

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
            //获取统计数据
            getStatistics: function (org) {
                var th = this;
                th.loading_stat = true;
                $api.cache('Organization/Statistics', { 'orgid': th.orgid }).then(function (req) {
                    if (req.data.success) {
                        th.stat = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading_stat = false);
            },
            //获取视频存储信息
            getVideo: function () {
                var th = this;
                $api.cache('Course/StorageVideo', { 'orgid': th.orgid, 'isreal': true }).then(function (req) {
                    if (req.data.success) {
                        th.video = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //获取课程图文资料
            getcourseRes: function () {
                var th = this;
                $api.cache('Course/StorageResources', { 'orgid': th.orgid, 'isreal': false }).then(function (req) {
                    if (req.data.success) {
                        th.resources = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //获取试题资料存储空间的数据
            getquestion: function () {
                var th = this;
                $api.cache('Question/StorageResources', { 'orgid': th.orgid, 'sbjid': '', 'couid': '', 'olid': '' }).then(function (req) {
                    if (req.data.success) {
                        th.question = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => { });
            },
            //获取新闻资料存储空间数据
            getnews: function () {
                var th = this;
                //批量访问过中会验证结果是否异常，但不会触发catch
                $api.bat(
                    $api.cache('News/StorageResources', { 'orgid': th.orgid, 'isreal': true }),
                    $api.get('News/Count', { 'orgid': th.orgid, 'uid': '', 'isuse': '' })
                ).then(([res, count]) => {
                    th.news = res.data.result;
                    th.news['total'] = count.data.result;
                }).catch(err => alert(err))
                    .finally(() => {
                        console.log('finally');
                    });
            }
        },
        filters: {
            //数字转三位带逗号
            'commas': function (number) {
                return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        }
    });

}, []);
