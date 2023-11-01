//课程图片
$dom.load.css([$dom.pagepath() + 'Components/Styles/courimg.css']);
Vue.component('courimg', {
    //course:课程
    props: ["course"],
    data: function () {
        return {}
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {},
    template: `<div class="cour_img">
            <a target="_blank" :href="'/web/course/detail.'+course.Cou_ID">
                <img :src="course.Cou_LogoSmall" v-if="course.Cou_LogoSmall!=''" />
                <img src="/Utilities/images/cou_nophoto.jpg" v-else />
            </a>
            <span class="rec" v-if="course.Cou_IsRec"></span>
            <div class="subject" title="课程专业"><a :href="'/web/Course?sbjid='+course.Sbj_ID"
                    target="_blank">
                    {{course.Sbj_Name}}
                </a> </div>
        </div>`
});