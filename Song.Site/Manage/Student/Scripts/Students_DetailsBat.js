//打印按钮事件
$("input#btnPrint").click(function () {
    $("body .page").jqprint();
    /*$("body .page").jqprint({
        debug: false, //如果是true则可以显示iframe查看效果（iframe默认高和宽都很小，可以再源码中调大），默认是false
        //importCSS: true, //true表示引进原来的页面的css，默认是true。（如果是true，先会找$("link[media=print]")，若没有会去找$("link")中的css文件）
        printContainer: true, //表示如果原来选择的对象必须被纳入打印（注意：设置为false可能会打破你的CSS规则）。
        operaSupport: true//表示如果插件也必须支持歌opera浏览器，在这种情况下，它提供了建立一个临时的打印选项卡。默认是true
    });*/
});

var vm = new Vue({
    el: '#app-area',
    data: {
        sts: $api.querystring('sts'),//学员分组
        students: [],    //所有学员
        loading: true,
        finishcount: 0,      //加载完成数
    },
    watch: {
        //当所有学员的进度获取完成，loading状态
        'finishcount': function (val, old) {
            if (val >= this.students.length) this.loading = false;
        }
    },
    computed: {

    },
    methods: {
        //获取所有学员
        getall: function () {
            var th = this;
            $api.get('Student/All', { 'sts': th.sts }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.students = result;
                    if (th.students.length == 0) th.loading = loading;
                    result.forEach(element => {
                        //console.log(element.Ac_ID);
                        //获取学员的课程进度
                        $api.get('Student/CourseCompletion', { 'stid': element.Ac_ID }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                element.courses = result;
                                //console.log(element);
                                vm.finishcount++;
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //alert(err);
                        });
                    });
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
            });
        },
        //获取公章信息
        getstamp: function () {
            $api.get('Platform/Stamp').then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    $("#stamp").attr("src", result.stamp);
                    //...
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        //生成二维码
        qrcode: function (stid) {
            jQuery($(this)).qrcode({
                render: "canvas", //也可以替换为table
                width: 75,
                height: 75,
                foreground: "#000",
                background: "#FFF",
                text: $().getHostPath() + "Mobile/certify.ashx?acid=" + stid
                //text:acid
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
        birthday: function (fmt,date) {
            date = new Date(date);
            if ((new Date().getFullYear() - date.getFullYear()) > 100) return '';
            return this.format(fmt, date);
        },
        format: function (fmt, date) {
            fmt = fmt.replace(/\Y/g, "y");

            var h24 = date.toLocaleString();
            try {
                h24 = date.toLocaleString('chinese', {
                    hour12: false
                });
            } catch (e) { }
            h24 = h24.substring(h24.indexOf(' ') + 1, h24.indexOf(':'));
            var h12 = date.toLocaleString();
            try {
                h12 = date.toLocaleString('chinese', {
                    hour12: true
                });
            } catch (e) { }
            h12 = h12.substring(h12.indexOf(' ') + 1, h12.indexOf(':'));
            var week = ['天', '一', '二', '三', '四', '五', '六'];
            //
            var ret;
            var opt = {
                "yyyy": date.getFullYear().toString(),
                "yy": date.getFullYear().toString().substring(2),
                "M+": (date.getMonth() + 1).toString(),
                "d+": date.getDate().toString(),
                "w+": week[date.getDay()],
                "H+": h24,
                "h+": h12,
                "m+": date.getMinutes().toString(),
                "s+": date.getSeconds().toString()
            };
            for (var k in opt) {
                ret = new RegExp("(" + k + ")").exec(fmt);
                if (ret) {
                    fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
                };
            };
            return fmt;
        }
    },
    mounted() {
        this.getall();
        this.getstamp();
    },
    created() {

    },

});