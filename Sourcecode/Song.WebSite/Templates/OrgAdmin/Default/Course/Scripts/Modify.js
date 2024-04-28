$ready(function () {

    //加载窗体组件
    $ctrljs(function () {
        window.$dom.load.css(['/Utilities/panel/skins/education/pagebox.css']);
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),
            course: {},     //当前课程
            tabs: [
                { name: '基本信息', tab: 'general', icon: '&#xa038' },
                { name: '简介', tab: 'intro', icon: '&#xe63d' },
                { name: '资费', tab: 'money', icon: '&#xe81c' },
                { name: '章节', tab: 'outline', icon: '&#xe841' },
                { name: '试题', tab: 'question', icon: '&#xe75e' },
                { name: '试卷/考试', tab: 'testpaper', icon: '&#xe810' },
                //{ name: '结课考试', tab: 'finaltest', icon: '&#xe816' },
                { name: '知识点', tab: 'knowledge', icon: '&#xe76d' },
                { name: '公告', tab: 'guide', icon: '&#xe840' },
                { name: '留言', tab: 'message', icon: '&#xe817' }
            ],
            tabName: 'general',      //顶部选项卡的示默认项
            loading: true
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.put('Course/ForID', { 'id': this.id }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.course = result;
                    if (th.exist) {
                        //将当前窗体最大化
                        if (window.top.$pagebox) {
                            var box = window.top.$pagebox.get(window.name);
                            //if (box != null) box.full = true;
                        }
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () {

        },
        computed: {
            //是否存在课程
            exist: t => { return !$api.isnull(t.course); },
            //当前窗体name
            winname: () => { return window.name; }
        },
        watch: {
            //选项卡切换时，切换显示内容（iframe)
            'tabName': {
                handler: function (nv, ov) {
                    this.$nextTick(function () {
                        $dom("#content_panel>iframe").hide();
                        $dom("#" + nv).show();
                    });
                }, immediate: true
            }
        },
        methods: {
            //是否显示iframe
            isshow: function (tab) {
                if (tab == this.tabName) return true;
                //如果之前已经加载过的iframe，不重新加载
                var iframe = $dom('#content_panel>#' + tab + '');
                if (iframe.length) return true;
            },
            //双击选项卡，刷新对应的Iframe
            doubletab: function (item) {
                var iframe = $dom('iframe#' + item);
                if (iframe.length < 1) return;
                var src = iframe.attr('src');
                src = $api.url.set(src, 'time', new Date().getTime());
                iframe.attr('src', src);
            },
            //保存课程名称
            savename: function (name, course) {
                this.course = course;
                this.close_fresh('vapp.freshrow("' + course.Cou_ID + '")');

            },
            //当前工作环境，是处于机构管理，还是教师或学员管理
            workplace: function () {
                let meta = $dom('meta[device]', window.top.document.documentElement);
                let device = meta.attr('device');
                return device;
            },
            //关闭自身窗体，并刷新父窗体列表
            close_fresh: function (func) {
                //如果有选项卡组件，就处理选项卡页面中的事件
                if (window.top.$tabs && this.workplace() == 'orgadmin') {
                    window.top.$pagebox.source.tab(window.name, func, false);
                } else {
                    //如果处在学员或教师管理界面
                    var winname = window.name;
                    if (winname.indexOf('_') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('_'));
                    if (winname.indexOf('[') > -1)
                        winname = winname.substring(0, winname.lastIndexOf('['));
                    window.top.vapp.fresh(winname, func);
                }
            },
            //刷新选项卡的frame页
            fresh_frame: function (func) {
                console.log('刷新fresh_frame:' + this.tabName);
                if (func == null) {
                    this.doubletab(this.tabName);
                } else {
                    var iframe = $dom('iframe#' + this.tabName);
                    if (iframe.length < 1) return;
                    var win = iframe[0].contentWindow;
                    if (win && func != null) {
                        if (func.charAt(func.length - 1) == ')') { eval('win.' + func); }
                        else {
                            var f = eval('win.' + func);
                            if (f != null) f();
                        }
                    }
                    //console.error(func);
                }
            }
        }
    });

    //更改课程名称
    Vue.component('course_name', {
        props: ["course"],
        data: function () {
            return {
                data: {
                    name: '', ischange: false,       //是否更改了标题
                },
                state: false,    //为true时是编辑状态
                rules: {
                    name: [{ required: true, message: '名称不得为空', trigger: 'blur' },
                    { min: 1, max: 100, message: '最长输入100个字符', trigger: 'change' },
                    { validator: validate.name.proh, trigger: 'change' },   //禁止使用特殊字符
                    { validator: validate.name.danger, trigger: 'change' },
                    {
                        validator: function (rule, value, callback) {
                            let v = $api.trim(value);
                            if (v == '' || v.length < 1) return callback(new Error('名称不能全部是空格'));
                            return callback();
                        }, trigger: 'blur'
                    }]
                },

                loading: false
            }
        },
        watch: {
            'course': {
                handler: function (nv, ov) {
                    this.data.name = $api.clone(this.course.Cou_Name);
                }, immediate: true
            },
            'data.name': function (nv, ov) {
                this.data.ischange = nv != this.course.Cou_Name;
            }
        },
        computed: {},
        mounted: function () { },
        methods: {
            btnChange: function () {
                if (this.data.name == '') {
                    this.data.name = $api.clone(this.course.Cou_Name);
                    this.data.ischange = false;
                }
                if (!this.data.ischange) {
                    this.state = false;
                    return;
                }
                var th = this;
                this.$refs['cou_name'].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Course/ModifyName', { 'name': this.data.name, 'couid': this.course.Cou_ID }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.course.Cou_Name = th.data.name;
                                th.$message({
                                    type: 'success',
                                    showClose: true,
                                    message: '修改课程名称成功！',
                                    center: true
                                });
                                th.state = false;
                                th.$emit('save', th.course.Cou_Name, th.course);
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(err => console.error(err))
                            .finally(() => th.loading = false);
                    } else {
                        console.log('course name input verification error !!');
                        return false;
                    }
                });

            },
            //取消
            btnCancel: function () {
                this.data.name = $api.clone(this.course.Cou_Name);
                this.state = false;
            }
        },
        template: `<div class="header">
        <template  v-if="!state">
            <div class="cou_name"> 《 {{data.name}} 》</div> 
            <el-link type="primary" v-if="!state" class="btnName" :underline="false" @click="state=!state">
                <span class="el-icon-edit">编辑</span>           
            </el-link>
        </template>
        <el-form ref="cou_name" :rules="rules" v-else :model="data" label-width="80px" :inline="true">
            <el-form-item label="" label-width="0"  prop="name">
                <el-input v-model="data.name" clearable></el-input>
            </el-form-item>
            <el-form-item label="" label-width="0">
                <el-link type="primary" define="enter" :disabled="loading" :underline="false" @click="btnChange">   
                    确认
                </el-link>
                <el-link type="info" define="cancel" :disabled="loading" :underline="false" @click="btnCancel">   
                    取消
                </el-link>
            </el-form-item>
        </el-form>    
    </div>`
    });
});
