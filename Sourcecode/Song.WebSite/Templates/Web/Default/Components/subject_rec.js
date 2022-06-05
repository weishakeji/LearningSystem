
//推荐课程的列表
Vue.component('subject_rec', {
    //专业，取多少条记录，低于多少条不再显示,排序方式
    props: ["subject", 'count', 'mincount', 'order'],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，例如当前专业下没有课程       
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
        getcourse: function (th) {
            var sbjid = th.subject == null ? 0 : th.subject.Sbj_ID;
            $api.cache('Course/ShowCount',
                { 'sbjid': sbjid, 'orgid': '', 'search': '', 'order': th.order, 'count': th.count })
                .then(function (req) {
                    if (req.data.success) {
                        th.courses = req.data.result;
                        th.show = th.courses.length >= th.mincount;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
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
          <div v-for="(cour,i) in courses" @click="godetail(cour.Cou_ID)" :rec="cour.Cou_IsRec" :class="{'rightmost':(i+1)%4==0}">
            <img :src="cour.Cou_LogoSmall" v-if="cour.Cou_LogoSmall && cour.Cou_LogoSmall!=''"/>
            <img :src="path+'images/cou_nophoto.jpg'" v-else />
            <div class="name">{{ cour.Cou_Name }}</div>
            <div class="price">
                <span class="free" v-if="cour.Cou_IsFree">免费</span>
                <span class="money" v-else>
                    <icon>&#xe625;</icon>
                    {{cour.Cou_Price}}元/{{cour.Cou_PriceSpan}}{{cour.Cou_PriceUnit}}             
                </span>
                <span class="view" title="访问量">{{cour.Cou_ViewNum}}</span>
            </div>
          </div>
        </courses>
    </weisha>`
});
