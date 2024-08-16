$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            stid: $api.querystring("acid"),   //学员Id
            courseids: $api.querystring('cous'), //选中的课程id

            acc: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项 
            stamp: {},         //公章信息（参数，path:公章图片路径;positon:位置）

            courses: [],        //选择的课程
            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/ForID', { 'id': this.stid }),
                $api.cache('Platform/Uploadpath:9999', { 'key': 'Org' }),
                $api.get('Organization/Current')
            ).then(([account, upload, org]) => {
                //获取结果
                th.acc = account.data.result;
                th.upload = upload.data.result;
                th.org = org.data.result;
                //机构配置信息
                th.config = $api.organ(th.org).config;
                //公章信息
                th.stamp = {
                    'positon': th.config.StampPosition ? th.config.StampPosition : 'right-bottom',
                    'path': th.config.Stamp && th.config.Stamp != '' ? th.upload.virtual + th.config.Stamp : ''
                };
            }).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
            //获取课程
            this.courseids.split(',').forEach(function (item, index) {
                console.log(item + "." + index);
                th.getCourse(item, index);
            });

        },
        created: function () {

        },
        computed: {
            //学员是存在
            isexist: function () {
                return JSON.stringify(this.acc) != '{}' && this.acc != null;
            },
            account: function () {
                if (!this.isexist) return {};
                var acc = $api.clone(this.acc);
                if (acc.Ac_Name.length > 1) acc.Ac_Name = this.asterisk(acc.Ac_Name, 1, 2);
                if (acc.Ac_IDCardNumber.length > 1) acc.Ac_IDCardNumber = this.asterisk(acc.Ac_IDCardNumber, 6, 14);
                return acc;
            },
            //加载是否全部完成
            loadcomplete: function () {
                //初始数据加载
                if (this.loading_init) return false;
                //课程加载
                var course = true;
                var th = this;
                this.courseids.split(',').forEach(function (item, index) {
                    if (!th.courses[index]) {
                        course = false
                        return;
                    }
                });
                if (!course) return false;
                //课程进度加载
                var progress = this.$refs["progress"];
                var loading = false;
                if (progress && progress.length > 0) {
                    for (let i = 0; i < progress.length; i++) {
                        if (progress[i].loading) {
                            loading = true;
                            break;
                        }
                    }
                }
                if (loading) return false;
                return true;
            }
        },
        watch: {
        },
        methods: {
            //获取课程，index为this.courses数组的索引号，按传的couid
            getCourse: function (couid, index) {
                if (couid == '') return;
                var th = this;
                th.loading = true;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$set(th.courses, index, result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取学历
            getedu: function (val) {
                var education = {
                    "81": "小学",
                    "71": "初中",
                    "61": "高中",
                    "41": "中等职业教育",
                    "31": "大学（专科）",
                    "21": "大学（本科）",
                    "14": "硕士",
                    "11": "博士",
                    "90": "其它"
                };
                for (var t in education) {
                    if (t == val) return education[t];
                }
                return '';
            },
            //生日
            birthday: function (fmt, date) {
                if (date == null || date == '') return '';
                date = new Date(date);
                if ((new Date().getFullYear() - date.getFullYear()) > 100) return '';
                return date.format(fmt);
            },
            //显示完成度
            showcomplete: function (num) {
                num = num > 100 ? 100 : num;
                num = Math.round(num * 10000) / 10000;
                return num;
            },
            //替换星号
            asterisk: function (str, start, end) {
                var len = str.length;
                end = end >= len ? len : end;
                start = start > len || start < 0 ? 0 : start;

                var bStr = str.substr(0, start);
                var eStr = str.substr(end, len);

                var asterisk = "";
                //var size=
                while (end - start > asterisk.length) asterisk += "*";
                return bStr + asterisk + eStr;
            }
        }
    });
    //课程视频的学习值
    Vue.component('progress_value', {
        props: ['course', 'stid', 'config'],
        data: function () {
            return {
                data: {},        //进度信息         
                percent: 0,     //完成的真实百分比
                tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
                purchase: {},        //购买记录，其中包含学习进度等
                score: 0,        //最终成绩
                loading: false
            }
        },
        watch: {
            'course': {
                handler: function (nv, ov) {
                    this.onload();
                }, immediate: true, deep: true
            },
            'config': {
                handler: function (nv, ov) {
                    if (nv && nv.VideoTolerance) {
                        this.tolerance = Number(nv.VideoTolerance);
                        this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                        this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                    }
                }, immediate: true, deep: true
            },
        },
        computed: {
            //完成度，加了容差之后的
            'progress': function () {
                return this.percent + this.tolerance >= 100 ? 100 : this.percent;
            },
        },
        mounted: function () { },
        methods: {
            onload: function () {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.cache('Course/LogForVideo:5', { 'couid': th.course.Cou_ID, 'stid': th.stid }),
                    $api.get('Course/Purchaselog', { 'stid': th.stid, 'couid': th.course.Cou_ID }),
                ).then(([cou, purchase]) => {
                    var result = cou.data.result;
                    if (result != null && result.length > 0) {
                        th.data = result[0];
                        th.data.lastTime = new Date(th.data.lastTime);
                        th.percent = th.data.complete;
                        console.log(th.data);
                    } else {
                        th.data = null;
                        th.percent = 0;
                    }
                    //购买记录
                    th.purchase = purchase.data.result;                 
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
        },
        // 同样也可以在 vm 实例中像 "this.message" 这样使用
        template: `<span>
                <loading v-if="loading"></loading>
                <slot v-else :value='progress' :score="score" :course='data' :purchase="purchase"></slot>
        </span>`
    });
});
