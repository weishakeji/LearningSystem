//课程的价格
Vue.component('course_prices', {
    props: ["course"],
    data: function () {
        return {
            prices: [],
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.getPrices(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/course_prices.css']);
    },
    methods: {
        getPrices: function (cou) {
            if (cou.Cou_IsFree || cou.Cou_IsLimitFree) return;
            var th = this;
            th.loading = true;
            $api.get('Course/Prices', { 'uid': cou.Cou_UID }).then(function (req) {
                if (req.data.success) {
                    th.prices = req.data.result;
                    cou.prices = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //显示所有价格信息
        showdetail: function () {
            var price = '';
            for (let i = 0; i < this.prices.length; i++) {
                const item = this.prices[i];
                price += item.CP_Span + item.CP_Unit + item.CP_Price + "元\n";
            }
            return price;
        }
    },
    template: `<div class="course_prices">
        <template v-if="course.Cou_IsFree">
            <el-tag type="success"><icon>&#xe71e</icon>免费</el-tag>
        </template>
        <template v-else-if="course.Cou_IsLimitFree">
            <el-tag><icon>&#xe81a</icon>限时免费</el-tag>
            <span>{{course.Cou_FreeStart|date('yyyy.M.dd')}} - {{course.Cou_FreeEnd|date('yyyy.M.dd')}} </span>
        </template>           
        <template v-else>            
            <span class="el-icon-loading" v-if="loading"></span>               
            <template v-for="(item,i) in prices">
                <span v-if="i<2"  :class="{'price':true,'first':prices.length>1 && i==0}" :title="showdetail()">
                    <icon>&#xe624</icon>{{item.CP_Span}}{{item.CP_Unit}}{{item.CP_Price}}元
                </span>
            </template>              
        </template>          
    </div> `
});