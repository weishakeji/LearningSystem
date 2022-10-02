
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
                text: '添加课程', tips: '添加课程到当前学员组',
                id: 'addcourse', type: 'primary',
                icon: 'e813'
            });
            this.$refs['btngroup'].addbtn({
                text: '批量移除', tips: '批量移除课程',
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
            //打开添加课程的面板
            addcourse_show: function () {
                var ctr = this.$refs['addcourse'];
                if (ctr != null) ctr.show();
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
        },
    });
    //添加课程
    Vue.component('addcourse', {
        props: ['stsid', 'organ'],
        data: function () {
            return {
                showpanel: false,        //是否显示面板
                //查询课程
                form: { 'orgid': 0, 'sbjids': 0, 'thid': '', 'search': '', 'order': '', 'size': 10, 'index': 1 },
                courses: [],
                total: 1, //总记录数
                totalpages: 1, //总页数
                selects: [], //数据表中选中的行
                rules: {

                },

                //专业树形下拉选择器的配置项
                defaultSubjectProps: {
                    children: 'children',
                    label: 'Sbj_Name',
                    value: 'Sbj_ID',
                    expandTrigger: 'hover',
                    checkStrictly: true
                },
                subjects: [],       //专业
                sbjids: [],


                loading: false
            }
        },
        watch: {
            'organ': {
                handler: function (nv, ov) {
                    if (nv) {
                        this.form.orgid = nv.Org_ID;
                        this.getSubjects(nv);
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
            //获取课程专业的数据
            getSubjects: function (organ) {
                if (organ == null || !organ || !organ.Org_ID) return;
                var th = this;
                var form = { orgid: organ.Org_ID, search: '', isuse: null };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.getcourses();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //专业更改时
            changeSbj: function (val) {
                var th = this;
                if (th.sbjids.length > 0) this.form.sbjids = th.sbjids[th.sbjids.length - 1];
                else
                    this.form.sbjids = 0;

                this.getcourses(1);
                //关闭级联菜单的浮动层
                this.$refs["subjects"].dropDownVisible = false;
            },
            //加载数据页
            getcourses: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 160;
                var maxhg=$dom('#courses_list').height();
                maxhg=maxhg<=0 ?  document.documentElement.clientHeight - 160 : maxhg;
                //console.log(maxhg);
                th.form.size = Math.floor(maxhg / 70);
                $api.get("Course/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.courses = d.data.result;
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

        },
        //
        template: `<el-drawer :visible.sync="showpanel" size="60%" direction="ltr" :show-close="true"
            custom-class="addcourse">
            <span slot="title">  
                <icon>&#xe813</icon>添加课程              
            </span>
            <el-form ref="formdata" :model="form" :rules="rules" @submit.native.prevent label-width="60px">               
                <el-form-item label="专业" prop="courses">
                    <el-cascader ref="subjects" style="width: 100%;" clearable v-model="sbjids" placeholder="-- 请选择课程专业 --"
                        :options="subjects" separator="／" :props="defaultSubjectProps" filterable @change="changeSbj">
                        <template slot-scope="{ node, data }">
                            <span>{{ data.Sbj_Name }}</span>
                            <span class="sbj_course" v-if="data.Sbj_CouNumber>0">
                                <icon>&#xe813</icon>{{ data.Sbj_CouNumber }}
                            </span>
                        </template>
                    </el-cascader>                  
                </el-form-item>
                <el-form-item label="课程" prop="courses">
                    <div class="course_row">
                        <el-input v-model="form.search" placeholder="课程名称查询" clearable @clear="getcourses(1)"></el-input>                        
                        <el-button type="primary" @click="getcourses(1)"><icon>&#xa00b</icon>查询</el-button>                                           
                    </div>                  
                </el-form-item>
            </el-form>
           <div class="courses" id="courses_list"  v-if="courses.length>0">
                <div v-for="(item,i) in courses" class="course">
                    <div class="cour_img">
                        <a target="_blank" :href="'/web/course/detail.'+item.Cou_ID">
                            <img :src="item.Cou_LogoSmall" v-if="item.Cou_LogoSmall!=''" />
                            <img src="/Utilities/images/cou_nophoto.jpg" v-else />
                        </a>
                        <span class="rec" v-if="item.Cou_IsRec"></span>
                    </div>
                    <div class="cour_name">
                        <span>{{i+1}}.</span>
                        <a target="_blank" :href="'/web/course/detail.'+item.Cou_ID"> {{item.Cou_Name}}</a>
                        <div class="subject" title="课程专业">
                            <a :href="'/web/Course?sbjid='+item.Sbj_ID" target="_blank">
                                <icon>&#xe750</icon>{{item.Sbj_Name}}
                            </a>
                        </div>
                    </div>
                    <div class="cour_btn">
                        <el-link icon="el-icon-right" type="primary" @click="removeCourse(i)">选择</el-link>
                    </div>
                </div>               
           </div>
           <div v-else class="nocourse"><icon>&#xe839</icon>没有满足条件的课程</div>
            <el-pagination v-on:current-change="getcourses" :current-page="form.index" :page-sizes="[1]"
                :page-size="form.size" :pager-count="4" layout="total, prev, pager, next" :total="total">
            </el-pagination> 
            
        </el-drawer>`
    });
});
