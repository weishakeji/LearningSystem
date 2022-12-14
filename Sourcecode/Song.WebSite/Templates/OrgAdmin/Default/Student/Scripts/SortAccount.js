
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //分组id
            id: Number($api.querystring('id', '0')),
            organ: {},       //当前机构
            config: {},

            accounts: [],
            form: { 'orgid': '', 'sortid': '', 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '', 'index': 1, 'size': '' },
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading_init: false,
            loadingid: 0,
            loading: false
        },
        created: function () {
            var th = this;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                th.loading_init = false;
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
                th.organ = organ.data.result;
                if (th.id == "") th.entity.Org_ID = th.organ.Org_ID;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.orgid = th.organ.Org_ID;
                th.form.sortid = th.id;
                th.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });

        },
        mounted: function () {
            //var t = this.$refs['btngroup'];
            this.$refs['btngroup'].addbtn({
                text: '批量添加', tips: '批量添加学员到当前组',
                id: 'batadd', type: 'primary',
                icon: 'e7cd'
            });
            this.$refs['btngroup'].addbtn({
                text: '批量移除', tips: '批量移除学员',
                id: 'batremove', type: 'warning',
                icon: 'e800'
            });
            //console.log(t);
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 120;
                th.form.size = Math.floor(area / 41);
                $api.get("Account/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //窗体上方按钮组的批量移除
            batremove: function (btn, items) {
                if (items == '') {
                    this.$message({
                        message: '请选中要操作的数据行',
                        type: 'error'
                    });
                    return;
                }
                var arr = items.split(',');
                var th = this;
                this.$confirm('是否确认移除这 ' + arr.length + ' 个学员? ', '谨慎操作', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(function () {
                    th.remove_func(items);
                }).catch(function () { });
            },
            //窗体右侧的移除，包括列上方的批量移除
            remove: function (item) {
                var id = [];
                if (item != null) {
                    id.push(item.Ac_ID);
                } else {
                    for (let i = 0; i < this.accounts.length; i++) {
                        id.push(this.accounts[i].Ac_ID);
                    }
                }
                if (id.length <= 0) return;
                this.remove_func(id.join(','));
            },
            remove_func: function (ids) {
                var th = this;
                var loading = this.$fulloading();
                $api.delete('Account/SortRemoveStudent', { 'stsid': this.id, 'id': ids }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.handleCurrentChange();
                        th.$message({
                            type: 'success',
                            message: '移除成功!',
                            center: true
                        });
                        th.$nextTick(function () {
                            th.operateSuccess();
                            loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', false);
            },
            //新增学员到当前组
            studentadd_show: function () {
                var ctr = this.$refs['studentadd'];
                if (ctr != null) ctr.show();
            },
            //批量新增学员，打开组件面板
            batadd_show: function () {
                var ctr = this.$refs['studentbatadd'];
                if (ctr != null) ctr.show();
            },
            //新增学员完成的事件方法
            studentadd_event: function (stsid, acid) {
                var th = this;
                th.handleCurrentChange();
                th.$nextTick(function () {
                    th.operateSuccess();
                    //loading.close();
                });
                th.$message({
                    type: 'success',
                    message: '添加成功!',
                    center: true
                });
            }
        },
    });
    // 新增学员到学员组（单个新增）
    Vue.component('student_add', {
        props: ['stsid', 'orgid'],
        data: function () {
            return {
                showpanel: false,        //是否显示面板

                accounts: [],
                form: { 'orgid': '', 'sortid': '', 'use': null, 'acc': '', 'name': '', 'phone': '', 'idcard': '', 'index': 1, 'size': '' },
                total: 1, //总记录数
                totalpages: 1, //总页数
                selects: [], //数据表中选中的行

                loading: false
            }
        },
        watch: {
            'orgid': {
                handler: function (nv, ov) {
                    if (nv) {
                        this.form.orgid = nv;
                        this.getpaper();
                    }
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
            //加载数据页
            getpaper: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 30);
                $api.get("Account/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.accounts = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);

                });
            },
            //增加学员
            add: function (item) {
                var th = this;
                th.loading = true;
                $api.post('Account/SortAddStudent', { 'stsid': th.stsid, 'id': item.Ac_ID }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$emit('addfinish', th.stsid, item.Ac_ID);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    th.loading = false;
                    console.error(err);
                });
            }
        },
        //
        template: `<el-drawer :visible.sync="showpanel" size="60%" direction="ltr" :show-close="true"
            custom-class="student_add">
            <template slot="title">
               
                <el-form :inline="true" :model="form" class="demo-form-inline" v-on:submit.native.prevent>
                    <el-form-item label="">
                        <el-input v-model="form.name" placeholder="姓名" clearable style="width:110px"></el-input>
                    </el-form-item>
                    <el-form-item label="">
                        <el-input v-model="form.idcard" placeholder="身份证" clearable style="width:110px"></el-input>
                    </el-form-item>                  
                    <el-form-item>
                        <el-button type="primary" v-on:click="getpaper(1)" :loading="loading"
                            native-type="submit" plain>
                        <icon>&#xa00b</icon>
                        </el-button>
                    </el-form-item>
                </el-form>
            </template>

            <dl class="list">
                <dd v-for="(item,i) in accounts">
                    <span class="index"> 
                        {{(form.index-1) * form.size+i+1}}   
                    </span>
                    <span class="name">                       
                        <icon v-if="item.Ac_Sex==2" class="woman" title="女性">&#xe647</icon>
                        <icon v-else class="man">&#xe645</icon>
                        <span v-html='item.Ac_Name' :class="{'woman':item.Ac_Sex=='2','name':true}"></span>
                    </span>
                    <span class="idcard"> 
                        {{item.Ac_IDCardNumber}}   
                    </span>
                    <span class="btn"> 
                        <el-link type="primary" @click="add(item)" title="添加到学员组">添加</el-link>
                    </span>
                </dd>
            </dl>

            <el-pagination v-on:current-change="getpaper" :current-page="form.index" :page-sizes="[1]"
                :page-size="form.size" :pager-count="4" layout="total, prev, pager, next" :total="total">
            </el-pagination> 
            
        </el-drawer>`
    });
    //批量添加学员到学员组
    Vue.component('student_batadd', {
        props: ['stsid', 'orgid'],
        data: function () {
            return {
                showpanel: false,        //是否显示面板

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
            //增加学员
            add: function () {
                var num = this.query_valid();
                if (num <= 0) {
                    this.$alert('请提供有效的学员信息', '没有数据', {
                        confirmButtonText: '确定'
                    });
                    return;
                }
                this.$confirm('是否将当前 ' + num + '个学员添加到当前组, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var arr = [];
                    for (let i = 0; i < this.datas.length; i++) {
                        if (this.datas[i].state == 1) {
                            arr.push(this.datas[i].account.Ac_ID);
                        }
                    }
                    if (arr.length > 0) this.add_func(arr.join(','));
                }).catch(() => { });
                return;

            },
            //添加学员到学员组的具体方法
            add_func: function (ids) {
                console.log(ids);
                var th = this;
                var loading = th.$fulloading();
                $api.post('Account/SortAddStudent', { 'stsid': th.stsid, 'id': ids }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$emit('addfinish', th.stsid, ids);
                        th.$nextTick(function () {
                            loading.close();
                            th.datas = [];
                            th.inputText = '';
                            th.inputIsChange = false;        //是否有输入变化
                            th.operstatus = 1;        //操作状态，默认1录入数据，2为解析数据
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    loading.close();
                    console.error(err);
                });
            }
        },
        //
        template: `<el-drawer :visible.sync="showpanel" size="60%" direction="ltr" :show-close="true"
            custom-class="student_batadd">
            <span slot="title">
               <icon>&#xe7cd</icon>批量添加
            </span>
            <div class="first_intro">
                请在下面输入框录入学员信息，换行分隔<br />并明确录入的是：
                <el-radio-group v-model="search_type">
                    <el-radio label="acc">账号</el-radio>
                    <el-radio label="card">身份证</el-radio>
                    <el-radio label="mobi">手机号</el-radio>
                </el-radio-group>
            </div>
            <div>
                <el-button type="primary" plain v-if="operstatus==1" @click="operstatus=2">
                    <icon>&#xe83c</icon>解析录入的信息
                </el-button>
                <template v-else>
                    <el-button type="success" plain  @click="operstatus=1">
                        <icon>&#xe63d</icon>继续编辑内容
                    </el-button>
                    <el-button type="primary" plain @click="add">
                        <icon>&#xe6ea</icon>全部添加<span>（{{query_valid()}}条）</span>
                    </el-button>         
                </template>       
            </div>
            <el-input type="textarea" class="inputText" :rows="2" placeholder="请输入内容" v-if="operstatus==1"  v-model="inputText">
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
                            <account :item="scope.row" :text="scope.row.text" :type="search_type"></account>
                        </template>
                    </el-table-column>
                </el-table>
        </el-drawer>`
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
