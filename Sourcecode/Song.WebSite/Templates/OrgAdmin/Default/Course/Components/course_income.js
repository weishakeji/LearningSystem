
//课程的收益
Vue.component('course_income', {
    props: ["course"],
    data: function () {
        return {
            count: 0,
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.getnumber(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/course_income.css']);
    },
    methods: {
        getnumber: function (cou) {
            var th = this;
            th.loading = true;
            $api.get('Course/Income', { 'couid': cou.Cou_ID })
                .then(function (req) {
                    if (req.data.success) {
                        th.count = req.data.result;
                        th.course.income = th.format(req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //数字转为带有逗号的字符串
        format: function (num) {
            num = String(num);
            if (/^.*\..*$/.test(num)) {
                varpointIndex = num.lastIndexOf(".");
                varintPart = num.substring(0, pointIndex);
                varpointPart = num.substring(pointIndex + 1, num.length);
                intPart = intPart + "";
                var re = /(-?\d+)(\d{3})/
                while (re.test(intPart)) {
                    intPart = intPart.replace(re, "$1,$2")
                }
                num = intPart + "." + pointPart;
            } else {
                num = num + "";
                var re = /(-?\d+)(\d{3})/
                while (re.test(num)) {
                    num = num.replace(re, "$1,$2")
                }
            }
            return num;
        }
    },
    template: `<div class="course_income">
        <span class="el-icon-loading" v-if="loading"></span>
        <span v-else-if="count>0">            
            <icon>&#xe746</icon> {{format(count)}}                      
        </span>  
        <span v-else class="income_null">无</span>
        </div> `
});