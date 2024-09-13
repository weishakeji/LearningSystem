//课程的数据信息，例如章节数、试题数等
Vue.component('course_data', {
    props: ["course", "show_student", "index"],
    data: function () {
        return {
            data: {},
            init: false,     //是否初始化
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                if (!$api.isnull(nv)) {
                    if (this.index == 0) this.startInit();
                }
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/course_data.css']);
    },
    methods: {
        //初始加载
        startInit: function () {
            this.init = true;
            //加载完成，则加载后一个组件，实现逐个加载的效果
            this.getcount().finally(() => {
                var vapp = window.vapp;
                var ctr = vapp.$refs['course_data_' + (this.index + 1)];
                if (ctr != null) ctr.startInit();
            });
        },
        //获取课程的数据信息
        getcount: function () {
            var th = this;
            return new Promise(function (res, rej) {
                th.setcount(th.course);
                th.loading = true;
                $api.put('Course/StudentSum', { 'couid': th.course.Cou_ID }).then(function (req) {
                    if (req.data.success) {
                        th.data['student'] = req.data.result;
                        th.course.data = req.data.result;
                        return res();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => {
                    th.loading = false;
                    return res();
                });
            });
        },
        //设置已知的数据
        setcount: function (cou) {
            var th = this;
            th.data['outline'] = cou.Cou_OutlineCount;
            th.data['question'] = cou.Cou_QuesCount;
            th.data['testpaper'] = cou.Cou_TestCount;
            th.data['video'] = cou.Cou_VideoCount;
            th.data['view'] = cou.Cou_ViewNum;
        },
        //显示数值，过大的以千为单位显示,例如 10k
        shownum: function (num) {
            if (num == null || num == undefined) num = 0;
            if ($api.getType(num) != 'Number') num = Number(num);
            if (isNaN(num)) num = 0;
            if (num < 1000) return num;
            return Math.floor(num / 1000) + 'k';
        },
        //学员的事件
        onstudent: function (num, course) {
            if (!this.show_student) return;   //不显示学员详情
            if (num < 1) {
                this.$alert('当前课程没有学员', '提示', {
                    confirmButtonText: '确定',
                    callback: () => { }
                });
                return;
            }
            //打开新窗口
            var node = this.getnode();
            var tit = node ? node.title : $dom('title').text();
            var url = $dom.routepath() + 'students';
            course = course == null ? this.course : course;
            var attrs = {
                width: '80%',
                height: '80%',
                url: url + '?id=' + course.Cou_ID,
                ico: node && node.MM_IcoCode != '' ? node.MM_IcoCode : 'e77c',
                pid: window.name,
                title: tit + '《' + course.Cou_Name + '》' + '的学员学习记录'
            };
            var pbox = top.$pagebox.create(attrs);
            pbox.open();
        },
        //获取树菜单节点数据
        getnode: function () {
            var tree = top.tree;
            if (tree) return tree.getData(window.name);
            return null;
        },
    },
    template: `<div class="course_data">
        <template v-if="!init"></template>
        <span class="el-icon-loading" v-else-if="loading"></span>       
        <template v-else>
            <div>
                <div :title="'章节数：'+data.outline" :class="{'zero':data.outline<=0}">
                    <icon>&#xe841</icon> {{shownum(data.outline)}}
                </div>
                <div :title="'试题数：'+data.question" :class="{'zero':data.question<=0}">
                    <icon>&#xe75e</icon> {{shownum(data.question)}}
                </div>
                <div :title="'试卷数'+data.testpaper" :class="{'zero':data.testpaper<=0}">
                    <icon>&#xe731</icon> {{shownum(data.testpaper)}}
                </div>                   
            </div>
            <div>
                <div :title="'视频数：'+data.video" :class="{'zero':data.video<=0}">
                    <icon>&#xe6bf</icon> {{shownum(data.video)}}
                </div>  
                <div :title="'学员数：'+data.student"  :class="{'zero':data.student<=0,'link':show_student}"
                    @click="onstudent(data.student)">
                    <icon>&#xa043</icon> {{shownum(data.student)}}
                </div>
                <div :title="'访问量：'+data.view" :class="{'zero':data.view<=0}">
                    <icon>&#xa03a</icon> {{shownum(data.view)}}
                </div>                     
            </div>               
        </template>          
    </div> `
});