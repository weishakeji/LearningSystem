
//热门课程
Vue.component('courses_hot', {
    //sbjid:专业id
    //couid:当前课程，在获取的课程中去除该课程
    props: ["sbjid", "couid", "org", "count"],
    data: function () {
        return {
            show: false,         //是否显示，  
            datas: [],
            loading: false,
        }
    },
    watch: {
        'org': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                this.getcourses(nv.Org_ID, this.count);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.path() + 'Viewport/Components/Styles/courses_hot.css';
        $dom.load.css([css]);
    },
    methods: {
        //跳转
        godetail: function (id) {
            return $api.url.dot(id, '/web/Course/Detail');
        },
        //从指专业内获取课程列表
        getcourses: function (orgid, count) {
            var th = this;
            th.loading = true;
            var form = {
                'orgid': orgid, 'sbjid': '', 'start': '', 'end': '', 'size': count, 'index': 1
            };
            $api.get('Course/MostHot', form)
                .then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },

    template: `<weisha_courses>  
        <loading v-if="loading">loading ... </loading>        
        <slot v-else v-for="(cour,index) in datas" name="item" :data="cour" :index="index">
            <div :rec="cour.Cou_IsRec" :class="{'rightmost':(index+1)%4==0}">
                <a :href="godetail(cour.Cou_ID)" target="_blank">
                    <img :src="cour.Cou_LogoSmall" v-if="cour.Cou_LogoSmall && cour.Cou_LogoSmall!=''"/>
                    <img src="/Utilities/images/cou_nophoto.jpg" v-else />
                </a>
                <el-tag type="warning" class="type" v-if="cour.Cou_Type">试题库</el-tag>                  
                <a :href="godetail(cour.Cou_ID)" target="_blank" class="name">{{index+1}}. {{ cour.Cou_Name }}</a>
                <el-tag type="warning" class="view" title="选修人次"><icon student></icon>{{cour.Cou_TryNum}}</el-tag>              
            </div>       
        </slot>        
    </weisha_courses>`
});
