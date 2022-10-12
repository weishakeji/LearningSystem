// 课程详情
Vue.component('course_data', {
    props: ['couid', 'viewnum'],
    data: function () {
        return {
            //课程数据信息
            data: {
                'outline': 0,
                'question': 0,
                'video': 0
            },
            loading: false
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true
        }

    },
    computed: {},
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            $api.cache('Course/Datainfo:20', { 'couid': this.couid }).then(function (req) {
                if (req.data.success) {
                    th.data = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
        }
    },
    //
    template: `<div class="cur_data">
        <el-tag type="info">
            <icon>&#xe841</icon>
            章节 {{data.outline}} 
        </el-tag>
        <el-tag type="info">
            <icon>&#xe6b0</icon>
            试题 {{data.question}} 
        </el-tag>
        <el-tag type="info">
            <icon>&#xe83a</icon>
            视频 {{data.video}}
        </el-tag>
        <el-tag type="info">
            <icon>&#xa03a</icon>
            关注 {{viewnum}}
        </el-tag>
    </div>`
});