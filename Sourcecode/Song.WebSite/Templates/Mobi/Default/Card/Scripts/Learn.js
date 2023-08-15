$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项      
            //学习卡信息，  
            carddata: {
                'count': 0,     //总数
                'usecount': 0,  //使用过的数量
                'cards': []         //学员的学习卡
            },
            input_code: '',      //要输入的学习卡卡号
            showhelp: false,     //显示帮助信息

            showcard: false,
            currentcard: {},         //当前要查看的学习卡

            search_code: '',         //用于查询的字符串
            loading_init: true
        },
        mounted: function () {
            this.init_code();
        },
        created: function () {
            //当文件选择输入框变更时
            $dom("#upload_qrcode").bind('change', function (e) {
                var files = e.target.files;
                if (files && files.length > 0) {
                    console.log(files[0]);
                    var url = window.getObjectURL(files[0]);                   
                    qrcode.decode(url);

                    qrcode.callback = function (imgMsg) {
                        var txt = imgMsg;
                        var code = $api.url.get(txt, 'code');
                        var pw = $api.url.get(txt, 'pw');
                        console.log(code + '-' + pw);
                        vapp.input_code = code + '-' + pw;
                        vapp.$notify({ type: 'success', message: '卡号填入输入框，请选择后续操作' });
                    }
                }
            });

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); },
            //个人学习卡集合
            'mycards': function () {
                var cards = this.carddata.cards;
                if ($api.trim(this.search_code) == '') return cards;
                var arr = [];
                for (var i = 0; i < cards.length; i++) {
                    if (cards[i].Lc_Code.indexOf(this.search_code) > -1) {
                        arr.push(cards[i]);
                    }
                }
                return arr;
            }
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                    var th = this;
                    $api.bat(
                        $api.get('Learningcard/AccountOfCount', { 'acid': th.account.Ac_ID }),
                        $api.get('Learningcard/ForAccount', { 'acid': th.account.Ac_ID })
                    ).then(axios.spread(function (count, cards) {
                        th.carddata.count = count.data.result.count;
                        th.carddata.usecount = count.data.result.usecount;
                        th.carddata.cards = cards.data.result;
                    })).catch(function (err) {
                        console.error(err);
                    });
                    this.loading_init = false;
                }, immediate: true
            },
            'search_code': function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            login: function () {
                window.location.href = this.commonaddr('signin');
            },
            myself: function () {
                window.location.href = "/mobi/account/myself";
            },
            //初始化学习卡号，当扫码时，会带参数跳转到这里
            init_code: function () {
                var code = $api.querystring('code');
                var pw = $api.querystring('pw');
                if (code != '' && pw != '') {
                    this.input_code = code + '-' + pw;
                    var url = window.location.href;
                    url = $api.url.set(url, {
                        'code': null,
                        'pw': null
                    });
                    console.log(url);
                    window.history.pushState({}, '', url);
                }
            },
            //是否临近过期，离过期七天以内
            nearexpire: function (c) {
                var time = new Date().setDate((new Date().getDate() - 7));
                var end = c.Lc_LimitEnd;
                return end > time && end < new Date();
            },
            //是否过期
            expire: function (c) {
                return c.Lc_LimitEnd < new Date();
            },
            //学习卡使用结束时间
            useendtime: function (c) {
                var time = new Date(c.Lc_UsedTime.getTime());
                time = time.setDate(time.getDate() + c.Lc_Span);
                return time;
            },
            //将学习卡号填入输入框
            usenow: function (c) {
                var code = c.Lc_Code + '-' + c.Lc_Pw;
                this.input_code = code;
                this.$notify({ type: 'success', message: '卡号填入输入框，请选择后续操作' });
            },
            //显示卡片请情
            carddetail: function (c) {
                this.showcard = true;
                this.currentcard = c;
            },
            //使用学习卡
            usecode: function () {
                if ($api.trim(this.input_code) == '') {
                    this.$notify({ type: 'danger', message: '卡号不得为空' });
                    return;
                }
                $api.get('Learningcard/UseCode', { 'code': this.input_code }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$dialog.alert({
                            message: '通过使用该学习卡，您成功选修 ' + result.length + ' 门课程。',
                        }).then(() => {
                            window.location.reload();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$dialog({ message: err });
                    console.error(err);
                });
            },
            //暂存学习卡
            acceptcode: function () {
                if ($api.trim(this.input_code) == '') {
                    this.$notify({ type: 'danger', message: '卡号不得为空' });
                    return;
                }
                $api.get('Learningcard/AcceptCode', { 'code': this.input_code }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$dialog.alert({
                            message: '操作成功，学习卡被暂存在名下，后续可以在合适时间使用它。',
                        }).then(() => {
                            window.location.reload();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$dialog({ message: err });
                    console.error(err);
                });
            },
            //打开二维码图片
            openqrcode: function () {
                var upload = $dom("#upload_qrcode");
                upload[0].click();
            },
            //显示卡号，如果有查询，红色显示
            showcode: function (val) {
                var sear = vapp.search_code;
                if ($api.trim(sear) == '') return val;
                if ($api.trim(val) == '') return '';
                var regExp = new RegExp(sear, 'ig');
                val = val.replace(regExp, `<red>${sear}</red>`);
                return val;
            }
        },
        filters: {
        }
    });
    //卡片详情
    Vue.component('carddetail', {

        props: ['card'],
        data: function () {
            return {
                cardset: {},      //学习卡设置项
                courses: [],         //关联的课程
                loading: 0       //预载
            }
        },
        watch: {
            'card': {
                handler(nv, ov) {
                    this.loading = 0;
                    this.getcardset();
                    this.getcourses();
                },
                immediate: true
            }
        },
        created: function () {

        },
        methods: {
            //获取卡片设置项
            getcardset: function () {
                if (this.card == null) return;
                var th = this;
                $api.get('Learningcard/SetForID', { 'id': this.card.Lcs_ID }).then(function (req) {
                    th.loading++;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.cardset = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading++;
                    console.error(err);
                });
            },
            //获取关联的课程
            getcourses: function () {
                if (this.card == null) return;
                var th = this;
                $api.get('Learningcard/SetCourses', { 'id': this.card.Lcs_ID }).then(function (req) {
                    th.loading++;
                    if (req.data.success) {
                        th.courses = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading++;
                    console.error(err);
                });
            }
        },
        template: `<div>
                <div class="title"><icon>&#xe60f</icon> {{card.Lc_Code}} - {{card.Lc_Pw}}</div>  
                <van-cell v-if="loading<2"><van-loading size="24px">加载中...</van-loading></van-cell>
                <template v-else>
                    <van-cell :title="cardset.Lcs_Theme" class="lcstheme"> </van-cell>
                    <van-cell title="面额" :value="cardset.Lcs_Price+'元'"> </van-cell>
                    <van-cell title="学习时间" :value="cardset.Lcs_Span+cardset.Lcs_Unit"> </van-cell>
                    <van-cell title="有效期" :value="cardset.Lcs_LimitStart|date('yyyy-MM-dd')"> </van-cell>
                    <van-cell title="" :value="cardset.Lcs_LimitEnd|date('yyyy-MM-dd')"> </van-cell>
                    <card>
                        <card-title>
                            <icon course></icon>
                            关联课程
                            <span>（{{courses.length}}）</span>
                        </card-title>
                        <card-context>
                            <div v-for="(c,i) in courses">
                                {{i+1}}、{{c.Cou_Name}}
                            </div>
                        </card-context>
                    </card>  
                </template>              
            </div>`
    });
}, ['../Components/page_header.js',    
    '/Utilities/Scripts/reqrcode.js']);


window.getObjectURL = function (file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}