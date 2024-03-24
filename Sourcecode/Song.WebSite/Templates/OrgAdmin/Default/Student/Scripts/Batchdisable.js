$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            datas: [],              //要操作的数据
            inputText: '',           //输入的账号信息
            inputText_temp: '',        //输入的临时记录
            inputIsChange: false,        //是否有输入变化

            activeName: 'first',     //选项卡的标识

            search_type: 'card',      //检索类型，账号acc,身份证card，手机mobi
            loading: false,
            loading_init: true
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
            })).catch(err => console.error(err))
                .finally(() => th.loading_init = false);
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'activeName': function (nv, ov) {
                if (nv == 'second') {
                    this.inputText_temp = this.inputText;
                    this.parseInput();
                }
                if (nv == 'first') {
                    this.inputIsChange = this.inputText_temp != this.inputText
                }

            },
            'inputText': function (nv, ov) {
                this.inputIsChange = true;
            },
            'inputIsChange': function (nv, ov) {
                console.log(nv);
            },
            'search_type': function (nv, ov) {
                this.inputIsChange = true;
            },
            'datas': {
                deep: true, //深度监听设置为 true
                handler: function (nv, ov) {
                    this.$nextTick(function () {

                    });
                }
            }
        },
        methods: {
            //解析录入的学员账号信息
            parseInput: function () {
                if (!this.inputIsChange) return;
                var str = $api.trim(this.inputText);
                this.datas = [];
                if (str == '') return;

                //解析录入的信息
                var arr = str.split("\n");
                //校验证手机号，简单校验
                if (this.search_type == "mobi") {
                    var regPos = / ^\d+$/; // 非负整数 
                    for (var i = 0; i < arr.length; i++) {
                        const d = arr[i].replace(/\s*/g, "");
                        if (!regPos.test(d)) arr.splice(i, 1);
                    }
                }
                //校验身份证，简单校验而已
                if (this.search_type == "card") {
                    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
                    for (var i = 0; i < arr.length; i++) {
                        const d = arr[i].replace(/\s*/g, "");
                        if (!reg.test(d)) arr.splice(i, 1);
                    }
                }
                for (var i = 0; i < arr.length; i++) {
                    arr[i] = arr[i].replace(/\s*/g, "");
                    if (arr[i] == '') continue;
                    //state：状态,初始为-1，账号不存在为0，存在为1，处理完成为2
                    this.datas.push({ 'text': arr[i], 'account': {}, 'state': -1 });
                }
                console.log(arr);
            },
            //查询的完成数
            'query_complete': function () {
                var c = 0;
                for (var i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].state != -1) {
                        c++;
                    }
                }
                return c;
            },
            //有效的记录数
            'query_valid': function () {
                var c = 0;
                for (var i = 0; i < this.datas.length; i++) {
                    if (this.datas[i].state == 1) {
                        c++;
                    }
                }
                return c;
            },
            //批量操作按钮事件
            btnBatch: function () {
                //判断是否解析录入数据
                if (this.activeName != "second") {
                    this.$alert('请切换到“录入数据解析”界面', '提示');
                    return;
                }
                //判断是否有需要操作的账号
                var th = this;
                var surplus = 0;
                for (var i = 0; i < th.datas.length; i++) {
                    if (th.datas[i].state != 1) continue;
                    if (th.datas[i].account == null) continue;
                    if (!th.datas[i].account.Ac_IsUse) continue;
                    surplus++;
                }
                if (surplus == 0) {
                    this.$alert('没有要操作的账号', '提示');
                    return;
                }
                this.batchDisable();
            },
            //批量禁用
            batchDisable: function () {
                var th = this;
                th.loading = true;
                for (var i = 0; i < th.datas.length; i++) {
                    if (th.datas[i].state != 1) continue;
                    if (th.datas[i].account == null) continue;
                    if (!th.datas[i].account.Ac_IsUse) continue;
                    $api.get('Account/ModifyState', {
                        'acid': th.datas[i].account.Ac_ID,
                        'use': false,
                        'pass': th.datas[i].account.Ac_IsPass
                    }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            var surplus = 0;
                            for (var j = 0; j < th.datas.length; j++) {
                                if (th.datas[j].account == null) continue;
                                if (th.datas[j].account.Ac_ID == result) {
                                    var item = th.datas[j];
                                    item.account.Ac_IsUse = false;
                                    item.state = 2;
                                    th.$set(th.datas, j, item);
                                }
                                if (th.datas[j].state == 1) surplus++;
                            }
                            if (surplus != 0) {
                                th.loading = false;
                                //th.$alert('完成批量操作', '提示');
                            }
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err))
                        .finally(() => th.loading = false);
                }
            }
        }
    });


    //账号信息的获取
    Vue.component('account', {
        //item:录入项,
        //text:录入的内容，可能是账号或身份证号
        //type:搜索类型
        props: ["item", "text", "type"],
        data: function () {
            return {
                data: null,
                state: -1,
                loading: true
            }
        },
        watch: {
            'text': {
                handler: function (nv, ov) {
                    this.state = this.item.state;
                    this.getaccount();
                }, immediate: true, deep: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            getaccount: function () {
                var th = this;
                th.loading = true;
                //
                var apiurl = "Account/ForAcc", para = {};
                if (this.type == 'acc') {
                    apiurl = "Account/ForAcc";
                    para = { "acc": this.text };
                }
                if (this.type == 'card') {
                    apiurl = "Account/ForIDCard";
                    para = { "card": this.text };
                }
                if (this.type == 'mobi') {
                    apiurl = "Account/ForMobi";
                    para = { "phone": this.text };
                }
                $api.cache(apiurl, para).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.data = req.data.result;
                        th.item.account = req.data.result;
                        th.item.state = th.state = 1;
                    } else {
                        th.data = null;
                        th.item.account = null;
                        th.item.state = th.state = 0;
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            }
        },
        template: `<span title="学员信息">
            <span v-if="state==-1" class="el-icon-loading"></span>
            <span v-if="state==0"><el-tag type="info">不存在</el-tag></span>
            <span v-if="state==1"  :class="{'woman': data.Ac_Sex==2,'disable':!data.Ac_IsUse}">
                <icon v-if="data.Ac_Sex==2" title="女性">&#xe844</icon>
                <icon v-if="data.Ac_Sex==1" title="男性">&#xe645</icon>
                <icon v-if="data.Ac_Sex==0" title="性别未知">&#xa043</icon>
                {{data.Ac_Name}}
                <span v-if="!data.Ac_IsUse">（已经禁用）</span>
            </span>
        </span> `
    });
});
