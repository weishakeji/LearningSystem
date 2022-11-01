//学员的批量处理
$dom.load.css(['/Utilities/Components/Styles/student_batch.css']);
//事件:
//addsingle:单个学员的添加事件
Vue.component('student_batch', {
    //bathadd_text：批量添加的按钮文本
    //add_icon:单个按钮的图标
    props: ['bathadd_text', 'add_icon', 'orgid'],
    data: function () {
        return {
            datas: [],
            search_type: 'card',    //检索类型，账号acc,身份证card，手机mobi

            inputText: '',
            inputIsChange: false,        //是否有输入变化
            operstatus: 1,        //操作状态，默认1录入数据，2为解析数据
            loading: false
        }
    },
    watch: {
        'orgid': {
            handler: function (nv, ov) {

            }, immediate: true
        },
        'inputText': function (nv, ov) {
            this.inputIsChange = true;
        },
        //操作状态，默认1录入数据，2为解析数据
        'operstatus': {
            handler: function (nv, ov) {
                if (nv == 2)
                    this.parseInput();
            }, immediate: true
        }

    },
    computed: {},
    mounted: function () { },
    methods: {
        //显示面板
        show: function () {
            this.showpanel = true;
        },
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
        //显示全屏Loading
        showloading: function () {
            return this.$loading({
                lock: true,
                text: 'Loading',
                spinner: 'el-icon-loading',
                background: 'rgba(255, 255, 255, 0.3)'
            });
        },
        //单个学员的操作
        single_add: function (acc) {
            //console.log(acc);
            this.$emit('addsingle', acc);
        },
        //批量添加
        batch_add: function () {          
            var list=[];
            for (let i = 0; i < this.datas.length; i++) {
                list.push(this.datas[i].account);                
            }          
            this.$emit('addbatch', list);
        }
    },
    //
    template: `<div class="student_batch">
            <div class="first_intro">
                <div>请明确录入的是：</div>
                <el-radio-group v-model="search_type" :disabled="operstatus==2">
                    <el-radio-button label="acc"><icon>&#xe687</icon>账号</el-radio-button>
                    <el-radio-button label="card"><icon>&#xe68f</icon>身份证</el-radio-button>
                    <el-radio-button label="mobi"><icon>&#xe677</icon>手机号</el-radio-button>
                </el-radio-group>
            </div>
            <div class="inputText">    
                <el-input type="textarea" resize="none" placeholder="录入学员信息，换行分隔" v-if="operstatus==1"  v-model="inputText">
                </el-input>          
                <el-table ref="datatables" class="table_datas" :stripe="true" :data="datas" tooltip-effect="dark" v-if="operstatus==2" 
                    style="width: 100%">
                    <el-table-column type="index" label="#" align="center">
                        <template slot-scope="scope">
                            <span>{{scope.$index + 1}}</span>
                        </template>
                    </el-table-column>
                    <el-table-column label="录入的信息">
                        <template slot="header" slot-scope="scope">
                            <span v-if="search_type=='acc'">学员账号</span>
                            <span v-if="search_type=='card'">身份证号</span>
                            <span v-if="search_type=='mobi'">手机号</span>
                            <span title="总数">：{{datas.length}} 条</span>
                        </template>
                        <template slot-scope="scope">
                            {{scope.row.text}}
                        </template>
                    </el-table-column>
                    <el-table-column label="账号查询">
                        <template slot="header" slot-scope="scope">
                            查询完成{{query_complete()}}条，有效{{query_valid()}}条
                        </template>
                        <template slot-scope="scope">
                            <student_batch_getaccount :item="scope.row" :text="scope.row.text" @add="single_add"
                            :type="search_type" :add_icon="add_icon">
                            </student_batch_getaccount>
                        </template>
                    </el-table-column>
                </el-table>
            </div>
            <div class="buttons_area">
                <el-button type="primary" v-if="operstatus==1" @click="operstatus=2">
                    <icon>&#xe83c</icon>解析录入的信息
                </el-button>
                <template v-else>
                    <el-button type="primary" plain  @click="operstatus=1">
                        <icon>&#xe727</icon>继续编辑内容
                    </el-button>
                    <el-button type="primary" plain @click="batch_add()">
                        <icon>&#xa048</icon>
                        <span v-if="bathadd_text" v-html="bathadd_text"></span>
                        <span v-else>全部添加</span>
                        <span>（{{query_valid()}}条）</span>
                    </el-button>         
                </template>       
            </div>
        </div>`
});

//账号信息的获取
Vue.component('student_batch_getaccount', {
    //item:录入项,
    //text:录入的内容，可能是账号或身份证号
    //type:搜索类型
    props: ["item", "text", "type", "add_icon"],
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
        },
        //单个学员的点击事件
        btnClick: function (data) {
            this.$emit('add', data);
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
        <icon v-html="'&#x'+add_icon" class="add_btn" @click="btnClick(data)"></icon>
    </span> `
});
