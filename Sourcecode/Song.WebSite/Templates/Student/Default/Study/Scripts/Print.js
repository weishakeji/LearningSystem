$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            stid: $api.dot(),   //学员Id
            courseids: $api.querystring('courses'), //选中的课程id

            acc: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项 
            stamp: {},         //公章信息（参数，path:公章图片路径;positon:位置）

            courses: [],        //选择的课程
            loading_init: true
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/Uploadpath:9999', { 'key': 'Org' }),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, upload, org) {
                vapp.loading_init = false;
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
            })).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
            //获取课程
            var arr = this.courseids.split(',');
            this.courseids.split(',').forEach(function (item, index) {
                console.log(item + "." + index);
                th.getCourse(item, index);
            });
            this.qrcode();
        },
        created: function () {

        },
        computed: {
            //学员是存在
            isexist: function () {
                return JSON.stringify(this.acc) != '{}' && this.acc != null;
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
                var th = this;
                $api.get('Course/ForID', { 'id': couid }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        Vue.set(th.courses, index, result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
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
            //生成二维码
            qrcode: function () {
                var len = $(".qrcode").length;
                if (len <= 0) window.setTimeout(this.qrcode, 100);;
                ///console.log('qrcode:'+$(".qrcode").length);
                //console.log('img:'+$(".qrcode img").length);
                if ($(".qrcode").length > $(".qrcode img").length) {
                    window.setTimeout(this.qrcode, 100);
                }
                //生成学员学习证明的二维码 
                $(".qrcode").each(function () {
                    if ($(this).find("img").length > 0) return;
                    var acid = $api.dot();
                    var cous = $api.querystring('courses');
                    var md5 = $api.md5(acid + cous);
                    var url = $api.url.set(window.location.origin + "/mobi/certify",
                        { "acid": acid, "cous": cous, "secret": md5 });
                    jQuery($(this)).qrcode({
                        render: "canvas", //也可以替换为table
                        width: 100,
                        height: 100,
                        foreground: "#000",
                        background: "#FFF",
                        text: url
                    });
                    //将canvas转换成img标签，否则无法打印
                    var canvas = $(this).find("canvas").hide()[0];  /// get canvas element
                    var img = $(this).append("<img/>").find("img")[0]; /// get image element
                    img.src = canvas.toDataURL();
                });
            },
            //打印
            print: function () {
                //$("body .page").jqprint();
                window.print();
            }
        }
    });

}, ['Components/progress_value.js',
    '/Utilities/Scripts/jquery.qrcode.min.js',
    '/Utilities/Scripts/jquery.jqprint-0.3.js']);
