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
            $api.get('Course/Datainfo', { 'couid': th.course.Cou_ID }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                    th.course.data = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        //显示数值，过大的以千为单位显示,例如 10k
        shownum: function (num) {
            if (num < 1000) return num;
            return Math.floor(num / 1000) + 'k';
        },
        //学员的事件
        onstudent: function (num) {
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
            var url = '/orgadmin/course/students';
            var attrs = {
                width: '80%',
                height: '80%',
                url: url + '?id=' + this.course.Cou_ID,
                ico: node && node.MM_IcoS != '' ? node.MM_IcoS : 'e67d',
                showmask:true,min:false,
                pid: window.name,
                title: tit + '《' + this.course.Cou_Name + '》' + '的学员学习记录'
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
        <template v-else>
          
                <div :title="'章节数：'+data.outline" :class="{'zero':data.outline<=0}">
                    <icon>&#xe841</icon> 章节：{{shownum(data.outline)}}
                </div>
                <div :title="'试题数：'+data.question" :class="{'zero':data.question<=0}">
                    <icon>&#xe75e</icon> 试题：{{shownum(data.question)}}
                </div>
                <div :title="'试卷数'+data.testpaper" :class="{'zero':data.testpaper<=0}">
                    <icon>&#xe731</icon> 试卷： {{shownum(data.testpaper)}}
                </div>                   
           
                <div :title="'视频数：'+data.video" :class="{'zero':data.video<=0}">
                    <icon>&#xe6bf</icon>视频： {{shownum(data.video)}}
                </div>  
                <div :title="'学员数：'+data.student"  :class="{'zero':data.student<=0,'link':show_student}"
                    @click="onstudent(data.student)">
                    <icon>&#xa043</icon>学员： {{shownum(data.student)}}
                </div>
                <div :title="'访问量：'+data.view" :class="{'zero':data.view<=0}">
                    <icon>&#xa03a</icon>访问量： {{shownum(data.view)}}
                </div>                     
                      
        </template>          
    </div> `
});