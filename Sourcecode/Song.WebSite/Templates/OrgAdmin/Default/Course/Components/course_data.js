//课程的数据信息，例如章节数、试题数等
Vue.component('course_data', {
    props: ["course", "show_student"],
    data: function () {
        return {
            data: {},
            loading: true
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.getcount();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/course_data.css']);
    },
    methods: {
        getcount: function () {
            var th = this;
            th.loading = true;
            $api.put('Course/Datainfo', { 'couid': th.course.Cou_ID }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                    th.course.data = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
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
            var url = $dom.routpath() + 'students';
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
        <span class="el-icon-loading" v-if="loading"></span>
        <template v-else-if="data==null">(数据异常)</template>
        <template v-else>
            <div>
                <div :title="'章节数：'+data.outline" :class="{'zero':data.outline<=0}">
                    <icon>&#xe841</icon> {{shownum(data.outline)}}
                </div>
                <div :title="'试题数：'+data.question" :class="{'zero':data.question<=0}">
                    <icon>&#xe75e</icon> {{shownum(data.question)}}
                </div>
                <div :title="'试卷数'+data.testpaper" :class="{'zero':data.testpaper<=0}">
                    <icon>&#xe731</icon> {{shownum(data.testpaper+data.testfinal)}}
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