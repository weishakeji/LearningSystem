//顶部面包屑导航
Vue.component('breadcrumb', {
    //当前课程，所有专业
    props: ["course", "subjects"],
    data: function () {
        return {
            parentPath: []       //父级路径
        }
    },
    watch: {
        'subjects': {
            handler: function (nv, ov) {
                this.parentPath = this.getparents();
            }, immediate: true
        }
    },
    computed: {
    },
    mounted: function () {

    },
    methods: {
        //获取上级路径
        getparents: function () {
            var arr = [];
            if (this.course == null || JSON.stringify(this.course) == '{}') return arr;
            var parent = this.getsubject(this.course.Sbj_ID, this.subjects);
            while (parent != null) {
                arr.push(parent);
                parent = this.getsubject(parent.Sbj_PID, this.subjects);
            }
            return arr.reverse();
        },
        //获取当前专业
        getsubject: function (sbjid, subjects) {
            var subject = null;
            for (var i = 0; i < subjects.length; i++) {
                if (sbjid == subjects[i].Sbj_ID) {
                    subject = subjects[i];
                    break;
                }
                if (subject == null && subjects[i].children)
                    subject = this.getsubject(sbjid, subjects[i].children);
            }
            return subject;
        },
        //跳转,sbj:专业的id
        gourl: function (sbj) {
            var url = '/web/course/index';
            url = $api.url.set(url, 'sbjid', sbj);
            window.location.href = url;
        }
    },
    template: `<el-breadcrumb separator-class="el-icon-arrow-right">
                <el-breadcrumb-item><a href="/">首页</a></el-breadcrumb-item>
                <el-breadcrumb-item><b @click="gourl()">课程中心</b></el-breadcrumb-item>
                <el-breadcrumb-item v-for="(item,index) in parentPath">
                <span @click="gourl(item.Sbj_ID)">{{item.Sbj_Name}}</span>
                </el-breadcrumb-item>
            </el-breadcrumb>`
});