
//推荐课程的列表
Vue.component('subject_rec', {
    //专业，取多少条记录，低于多少条不再显示,排序方式
    props: ['subject', 'org', 'count', 'mincount', 'order'],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，例如当前专业下没有课程    

            loading: false,      //
            courses: []        //专业下的课程          
        }
    },
    watch: {
        'subject': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) == '{}' || nv == null) return;
                this.getcourse();
            }, immediate: true
        },
        'org': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) == '{}' || nv == null) return;
                this.getcourse();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.path() + 'Components/Styles/subject_rec.css';
        var isexist = false;
        $dom("link[href]").each(function () {
            var href = $dom(this).attr("href");
            if (href.indexOf(css) > -1) {
                isexist = true;
                return false;
            }
        });
        if (!isexist) $dom.load.css([css]);
        this.mincount = !parseInt(this.mincount) ? 0 : parseInt(this.mincount);
    },
    methods: {
        //获取课程
        getcourse: function () {
            var th = this;
            th.loading = true;
            var sbjid = th.subject == null ? 0 : th.subject.Sbj_ID;
            $api.cache('Course/ShowCount',
                { 'sbjid': sbjid, 'orgid': th.org.Org_ID, 'search': '', 'order': th.order, 'count': th.count })
                .then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        th.show = th.courses.length >= th.mincount;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //跳转
        godetail: function (id) {
            var url = $api.url.dot(id, '/web/Course/Detail');
            window.location.href = url;
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `<weisha class="subject_rec" v-if="show">
        <courses>
            <loading v-if="loading">loading ... </loading>
            <course  v-else v-for="(item, i) in courses" :item="item" :org="org"></course>
        </courses>
    </weisha>`
});
