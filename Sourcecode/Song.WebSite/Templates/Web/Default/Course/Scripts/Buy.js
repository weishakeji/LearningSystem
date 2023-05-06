$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring("couid") == "" ? $api.dot() : $api.querystring("couid"),        //课程id
            account: {},     //当前登录账号       
            organ: {},
            config: {},      //当前机构配置项     
            subjects: [],        //当前机构下的专业

            course: {},         //当前课程对象
            couinfo: {},         //课程的一些数据信息，例如多少道题
            owned: false,

            sum: 0,              //购买课程的人数
            teacher: null,     //课程教师        
            prices: [],          //课程价格          

            selected_price: {},          //选中的价格         

            loading: false,       //加载状态
            loading_init: true,
            loading_buy: false      //购买中的状态
        },
        mounted: function () {
            var th = this;
            this.loading_init = true;
            //当前的机构、登录学员、课程
            $api.bat(
                $api.post('Organization/Current'),
                $api.get('Course/ForID', { 'id': th.couid }),
                $api.cache('Course/Datainfo', { 'couid': th.couid }),
            ).then(axios.spread(function (organ, course, info) {
                //机构配置信息
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;
                th.couinfo = info.data.result;
                //获取专业
                $api.cache('Subject/TreeFront', { 'orgid': th.organ.Org_ID }).then(function (req) {
                    th.loading_init = false;
                    if (req.data.success) {
                        th.subjects = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_init = false;
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
                //当前课程
                th.course = course.data.result;
                if (th.course != null) {
                    th.course.Cou_Target = th.clearTag(th.course.Cou_Target);
                    th.course.Cou_Intro = $api.trim(th.course.Cou_Intro);
                    document.title = th.course.Cou_Name;
                }
                if (!th.course) return;
                //课程章节，价格，购买人数,通知，教师，是否购买,购买的记录，是否可以学习（如果课程免费不购买也可以）               
                $api.bat(
                    $api.get('Course/Prices', { 'uid': th.course.Cou_UID }),
                    $api.get('Course/StudentSum', { 'couid': th.couid }),
                    $api.get('Teacher/ForID', { 'id': th.course.Th_ID })
                ).then(axios.spread(function (prices, sum, teacher) {
                    th.loading_init = false;
                    //获取结果                
                    th.prices = prices.data.result;
                    if (th.prices.length > 0)
                        th.select(th.prices[0]);
                    th.sum = sum.data.result;
                    th.teacher = teacher.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
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
            },
            //课程为空,或课程被禁用
            nullcourse: function () {
                return JSON.stringify(this.course) == '{}' || this.course == null || !this.course.Cou_IsUse;
            }
        },
        watch: {
            'nullcourse': function (nv, ov) {
                console.log(nv);
                this.$nextTick(function () {
                    this.qrcode();
                });
            }
        },
        methods: {
            //当学员登录后
            forlogin: function (acc) {
                var th = this;
                th.account = acc;
                $api.get('Course/Owned', { 'couid': th.couid, 'acid': th.account.Ac_ID }).then(function (req) {
                    if (req.data.success) {
                        th.owned = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //清理Html标签
            clearTag: function (html) {
                if (!html) return "";
                var txt = html.replace(/<\/?.+?>/g, "");
                txt = $api.trim(txt);
                return txt;
            },
            //生成二维码
            qrcode: function () {
                var box = $("#course-qrcode");
                if (box.length < 1) {
                    window.setTimeout(this.qrcode, 200);
                }
                box.each(function () {
                    if ($(this).find("img").length > 0) return;
                    //var url = $api.url.dot($api.dot(), window.location.origin + "/mobi/course/buy");
                    var url = window.location.href;
                    jQuery($(this)).qrcode({
                        render: "canvas", //也可以替换为table
                        width: 75,
                        height: 75,
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
            //计算日均多少钱
            averageday: function (item) {
                var span = item.CP_Span;    //时间数
                var unit = item.CP_Unit;    //时间单位
                var price = item.CP_Price;      //价格
                //平均每天多少钱
                var average = this.actualpay(item) / getday(span, unit);
                //var average = price / getday(span, unit);
                if (average > 0.01)
                    average = Math.floor(average * 100) / 100 + "元";
                else
                    average = Math.floor(average * 10000) / 100 + "分";
                return average;
                //计算课程计算项的天数
                function getday(span, unit) {
                    var day = span;
                    if (unit == "日") day = span;
                    if (unit == "周") day = span * 7;
                    if (unit == "月") day = span * 30;
                    if (unit == "年") day = span * 365;
                    return day;
                }
            },
            //实际支付多少钱（价格扣除卡券）
            actualpay: function (item) {
                //var account = this.account;
                //var mycoupon = account.Ac_Coupon;
                var price = item.CP_Price;      //价格
                var coupon = item.CP_Coupon;   //卡券
                return (price - coupon) <= 0 ? 0 : price - coupon;
            },
            //选中价格
            select: function (data) {
                if (data == null || (this.selected_price.CP_ID && this.selected_price.CP_ID == data.CP_ID)) {
                    return;
                } else {
                    this.selected_price = data;
                }
            },
            //计算资金是否购买课程的
            clacmoney: function () {
                var selected = this.selected_price;
                var account = this.account;
                if (!selected.CP_ID) return {};
                //account.Ac_Money = 150;
                var obj = {
                    span: selected.CP_Span,        //当前选中的购买项的数量
                    unit: selected.CP_Unit,        //当前选中的购买项的日期单位
                    mprice: selected.CP_Price,      //选中项的资金价格
                    cprice: selected.CP_Coupon,      //选中项的卡券价格
                    money: account.Ac_Money,       //余额
                    coupon: account.Ac_Coupon,        //卡券
                    need: {
                        money: 0,   //如果购买，需要的资金数
                        coupon: 0   //需要的卡券数
                    },
                    recharge: 0,     //需要充值的数钱
                    //是否能够买得起
                    pass: false,
                    passclac: function () {
                        var t = this;
                        return t.money >= t.mprice || t.money >= (t.mprice - (t.coupon > t.cprice ? t.cprice : t.coupon));
                    }
                };
                obj.need.coupon = obj.coupon > obj.cprice ? obj.cprice : obj.coupon;   //消耗的卡券数
                obj.need.money = obj.mprice - obj.need.coupon;
                obj.recharge = obj.need.money - obj.money;
                obj.pass = obj.passclac();
                return obj;
            },
            //课程购买的按钮事件，主要是用来显示提醒
            btnBuyClick: function () {
                var th = this;
                //this.account.Ac_Coupon = 10
                var calc = this.clacmoney();
                console.log(calc);
                var cour = this.course;
                var price = this.selected_price;
                var info = '为期' + price.CP_Span + price.CP_Unit + '，原价<red>' + price.CP_Price + '</red>元；';
                var coupon = '可以用券抵扣<b>' + price.CP_Coupon + '</b>元，您实际拥有' + this.account.Ac_Coupon + '个券，';
                if (calc.need.money > 0)
                    coupon += '抵扣后仍将支付<red>' + calc.need.money + '</red>元。';
                else
                    coupon += '抵扣之后可以<red>0元</red>购。';
                if (price.CP_Coupon > 0) {
                    info += coupon;
                }
                var msg = '您要购买的课程为<b>《' + cour.Cou_Name + '》</b>，' + info + '<br/><br/>网课一经售出概不退换，是否确认购买该课程？'
                this.$confirm(msg, '购买提醒', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    dangerouslyUseHTMLString: true,
                    type: 'warning'
                }).then(() => {
                    th.buycourse();
                }).catch(() => {
                    th.select(null);
                });
            },
            //购买课程的具体操作方法，用于提交操作到服务器端
            buycourse: function () {
                var th = this;
                th.loading_buy = true;
                var form = {
                    'couid': this.course.Cou_ID,
                    'cpid': this.selected_price.CP_ID
                };
                $api.get('Course/Buy', form).then(function (req) {
                    th.loading_buy = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        console.log(result);
                        th.completebuy(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //完成购买的操作
            completebuy: function (sc) {
                var cour = this.course;
                var start = sc.Stc_StartTime.format('yyyy年M月d日');
                var end = sc.Stc_EndTime.format('yyyy年M月d日');
                var msg = '您已经购买课程<b>《' + cour.Cou_Name + '》</b>，该课程有效期为 ' + start + ' 至' + end + ' ，请在有效期内完成学习并参与考试。'
                this.$alert(msg, '购买成功', {
                    confirmButtonText: '确定',
                    dangerouslyUseHTMLString: true
                }).then(() => {
                    var link = $api.querystring('link');
                    if (link != '') window.location.href = decodeURIComponent(link);
                    else
                        window.location.href = $api.url.dot(cour.Cou_ID, '/web/course/detail');
                });
            },
            //学员登录
            gologin: function () {
                var link = window.location.href;
                link = link.substring(link.indexOf(window.location.pathname));
                var url = $api.url.set('/web/sign/in', {
                    'link': encodeURIComponent(link),
                    'referrer': encodeURIComponent(document.referrer)
                });
                // console.log(url);               
                window.location.href = url;
            }
        }
    });

}, ["Components/largebutton.js",        //购买课程的按钮
    "Components/breadcrumb.js",         //顶部面包屑    
    "../Components/courses.js",
    '/Utilities/Scripts/jquery.qrcode.min.js']);
