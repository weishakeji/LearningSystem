
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
                var loading = this.showloading();
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
            //显示全屏Loading
            showloading: function () {
                return this.$loading({
                    lock: true,
                    text: 'Loading',
                    spinner: 'el-icon-loading',
                    background: 'rgba(255, 255, 255, 0.3)'
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
                    this.form.orgid = nv;
                    this.getpaper();
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
                        th.$emit('add', th.stsid, item.Ac_ID);
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
        template: `<el-drawer title="我是标题" :visible.sync="showpanel" size="60%" direction="ltr" :show-close="true"
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
});
