
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //分组id
            id: $api.querystring('id', '0'),
            organ: {},       //当前机构
            config: {},
            sort: {},//当前学员组

            courses: [],
            form: { 'sortid': '', 'name': '', 'index': 1, 'size': '' },
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
                $api.get('Organization/Current'),
                $api.get('Account/SortForID', { 'id': th.id })
            ).then(axios.spread(function (organ, sort) {
                th.loading_init = false;
                //获取结果             
                th.organ = organ.data.result;
                th.sort = sort.data.result;
                document.title = th.sort.Sts_Name;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                th.form.sortid = th.id;
                th.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });

        },
        mounted: function () {
            this.$refs['btngroup'].addbtn({
                text: '添加课程', tips: '添加课程到当前学员组',
                id: 'addcourse', type: 'primary',
                icon: 'e813'
            });
            this.$refs['btngroup'].addbtn({
                text: '移除', tips: '批量移除课程',
                id: 'batremove', type: 'warning',
                icon: 'e800'
            });
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 120;
                th.form.size = Math.floor(area / 65);
                $api.get("Student/SortCoursePager", th.form).then(function (d) {
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
            //打开添加课程的面板
            addcourse_show: function () {
                var ctr = this.$refs['addcourse'];
                if (ctr != null) ctr.show();
            },
            //完成添加课程
            addfinish: function (stsid, couid) {
                console.log(couid);
                this.handleCurrentChange();
                this.operateSuccess();
            },
            //窗体右侧的移除，包括列上方的批量移除
            remove: function (item) {
                var id = [];
                if (item != null) {
                    id.push(item.Cou_ID);
                } else {
                    for (let i = 0; i < this.courses.length; i++) {
                        id.push(this.courses[i].Cou_ID);
                    }
                }
                if (id.length <= 0) return;
                this.remove_func(id.join(','));
            },
            //批量移除课程，左上方按钮
            batremove: function (btn, ids, t) {
                if (ids == '') return;
                var count = ids.split(',').length;
                this.$confirm('移除 ' + count + ' 个课程, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.remove_func(ids);
                }).catch(() => { });
            },
            remove_func: function (ids) {
                var th = this;
                console.log(ids);
                var loading = this.$fulloading();
                $api.delete('Student/SortCourseRemove', { 'sortid': this.id, 'couid': ids }).then(function (req) {
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
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => loading.close());
            },
            //操作成功
            operateSuccess: function () {
                try {
                    window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', false);
                } catch { }
            },
        },
    });
    //添加课程
    Vue.component('addcourse', {
        //stsid:学员组id
        //courses:学员组关联的课程
        props: ['stsid', 'organ', 'courses'],
        data: function () {
            return {
                showpanel: false,        //是否显示面板
                //查询课程
                form: { 'orgid': 0, 'sbjids': 0, 'thid': '', 'search': '', 'order': '', 'size': 10, 'index': 1 },
                datas: [],
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
        computed: {

        },
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
                }).finally(()=>{});
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
                var maxhg = $dom('#courses_list').height();
                maxhg = maxhg <= 0 ? document.documentElement.clientHeight - 160 : maxhg;
                //console.log(maxhg);
                th.form.size = Math.floor(maxhg / 70);
                $api.get("Course/Pager", th.form).then(function (d) {
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        console.error(d.data.exception);
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                }).finally(()=>{});
            },
            //验证选择按钮是否能用
            checkdisabled: function (cou) {
                if (this.courses.length < 1) return false;
                var n = this.courses.find((item, index) => {
                    return item.Cou_ID == cou.Cou_ID;
                });
                return n != null;
            },
            //验证批量选择按钮是否能用
            checkbatdisabled: function () {
                if (this.datas.length < 1) return true;
                if (this.courses.length > 0) {
                    var count = 0;
                    for (let i = 0; i < this.datas.length; i++) {
                        const element = this.datas[i];
                        var n = this.courses.find((item, index) => {
                            return item.Cou_ID == element.Cou_ID;
                        });
                        if (n != null) count++;
                    }
                    if (count == this.datas.length) return true;
                }
                return false;
            },
            //添加关联
            addCourse: function (cou) {
                var loading = this.$fulloading();
                var th = this;
                $api.post('Student/SortCourseAdd', { 'sortid': this.stsid, 'couid': cou }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result > 0) {
                            th.$message({
                                type: 'success',
                                message: '操作成功! 添加 ' + result + ' 个课程',
                                center: true
                            });
                        } else {
                            th.$message({
                                type: 'warning',
                                message: '未正常添加课程',
                                center: true
                            });
                        }
                        th.$nextTick(function () {
                            th.$emit('addfinish', th.stsid, cou);
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
                }).finally(()=>{});
            },
            //批量添加
            addbatcourse: function () {
                var count = 0;
                for (let i = 0; i < this.datas.length; i++) {
                    const element = this.datas[i];
                    var n = this.courses.find((item, index) => {
                        return item.Cou_ID == element.Cou_ID;
                    });
                    if (n == null) count++;
                }
                this.$confirm('当前 ' + count + ' 个课程添加到学员组, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var idarr = [];
                    for (var i = 0; i < this.datas.length; i++)
                        idarr.push(this.datas[i].Cou_ID);
                    this.addCourse(idarr.join(','));
                }).catch(() => { });
            }
        },
        //
        template: `<el-drawer :visible.sync="showpanel" size="60%" direction="ltr" :show-close="true"
            custom-class="addcourse">
            <span slot="title">  
                <icon>&#xe813</icon>添加课程              
            </span>
            <el-form ref="formdata" :model="form" :rules="rules" @submit.native.prevent label-width="60px">               
                <el-form-item label="专业" prop="subject">
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
                <el-form-item label="课程">
                    <div class="course_row">
                        <el-input v-model="form.search" placeholder="课程名称查询" clearable @clear="getcourses(1)"></el-input>  
                        <div>                      
                            <el-button type="primary" @click="getcourses(1)"><icon>&#xa00b</icon>查询</el-button>   
                            <el-button type="success" @click="addbatcourse()" :disabled="checkbatdisabled()"><icon>&#xa04d</icon>批量选择</el-button>        
                        </div>                                 
                    </div>                  
                </el-form-item>
            </el-form>
           <div class="courses" id="courses_list"  v-if="datas.length>0">
                <div v-for="(item,i) in datas" class="course">
                    <div class="cour_img">
                        <a target="_blank" :href="'/web/course/detail.'+item.Cou_ID">
                            <img :src="item.Cou_LogoSmall" v-if="item.Cou_LogoSmall!=''" />
                            <img src="/Utilities/images/cou_nophoto.jpg" v-else />
                        </a>
                        <span class="rec" v-if="item.Cou_IsRec"></span>
                    </div>
                    <div class="cour_name">
                        <span>{{(form.index - 1) * form.size + i+ 1}}.</span>
                        <a target="_blank" :href="'/web/course/detail.'+item.Cou_ID"> {{item.Cou_Name}}</a>
                        <div class="subject" title="课程专业">
                            <a :href="'/web/Course?sbjid='+item.Sbj_ID" target="_blank">
                                <icon>&#xe750</icon>{{item.Sbj_Name}}
                            </a>
                        </div>
                    </div>
                    <div class="cour_btn">
                        <el-link icon="el-icon-right" type="primary" @click="addCourse(item.Cou_ID)"
                        :disabled="checkdisabled(item)" >选择</el-link>
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
