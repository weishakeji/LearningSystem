
//专业列表，显示课程
//事件:
//complete: //加载完成事件，参数为课程数
Vue.component('subject_show', {
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
            }, immediate: true
        },
        'org': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                this.getcourse();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.path() + 'Components/Styles/subject_show.css';
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
                        if (th.show) {
                            th.$nextTick(function () {
                                //加载完成事件，参数为课程数
                                th.$emit('complete', th.courses.length);
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //跳转
        godetail: function (id) {
            var url = $api.url.dot(id, '/web/Course/detail');
            window.location.href = url;
        },
        //跳转到课程中心
        gocourses: function (sbj) {
            var url = $api.url.set('/web/Course', { 'sbjid': sbj.Sbj_ID });
            window.location.href = url;
        }
    },
    template: `<weisha class="subject_show" v-if="show">
        <div class="subject" :name="subject.Sbj_Name" v-if="subject!=null" @click="gocourses(subject)">     
            <img :src="subject.Sbj_Logo" v-if="subject.Sbj_Logo && subject.Sbj_Logo!=''"/>
            <img :src="path+'images/nosubject.jpg'" v-else />
        </div>
        <courses>
            <loading v-if="loading">loading ... </loading>          
            <course  v-else v-for="(item, i) in courses" :item="item" :org="org"></course>
        </courses>
    </weisha>`
});
