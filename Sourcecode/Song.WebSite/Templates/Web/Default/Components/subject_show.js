
//专业列表，显示课程
//事件:
//complete: //加载完成事件，参数为课程数
Vue.component('subject_show', {
    //专业，取多少条记录，低于多少条不再显示,排序方式
    props: ["subject", 'count', 'mincount', 'order'],
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
                this.getcourse(this);
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
        getcourse: function (th) {
            var th = this;
            th.loading = true;
            var sbjid = th.subject == null ? 0 : th.subject.Sbj_ID;
            $api.cache('Course/ShowCount',
                { 'sbjid': sbjid, 'orgid': '', 'search': '', 'order': th.order, 'count': th.count })
                .then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        th.show = th.courses.length >= th.mincount;
                        if(th.show){
                            th.$nextTick(function(){
                                //加载完成事件，参数为课程数
                                th.$emit('complete',th.courses.length);
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });
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
        <subject :name="subject.Sbj_Name" v-if="subject!=null" @click="gocourses(subject)">           
            <img :src="subject.Sbj_logo" v-if="subject.Sbj_logo && subject.Sbj_logo!=''"/>
            <img :src="path+'images/nosubject.jpg'" v-else />
        </subject>
        <courses>
            <loading v-if="loading">loading ... </loading>
            <template v-else>
                <div v-for="cour in courses" @click="godetail(cour.Cou_ID)" :rec="cour.Cou_IsRec">
                    <img :src="cour.Cou_LogoSmall" v-if="cour.Cou_LogoSmall && cour.Cou_LogoSmall!=''"/>
                    <img :src="path+'images/cou_nophoto.jpg'" v-else />
                    <div class="name">{{ cour.Cou_Name }}</div>
                    <div class="price">
                        <span class="free" v-if="cour.Cou_IsFree">免费</span>
                        <span class="money" v-else>
                            <icon>&#xe818;</icon>
                            {{cour.Cou_Price}}元/{{cour.Cou_PriceSpan}}{{cour.Cou_PriceUnit}}             
                        </span>
                        <span class="view" title="访问量">{{cour.Cou_ViewNum}}</span>
                    </div>
                </div>
            </template>
        </courses>
    </weisha>`
});
