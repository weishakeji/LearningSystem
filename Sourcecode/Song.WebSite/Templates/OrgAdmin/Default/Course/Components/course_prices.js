//课程的价格
Vue.component('course_prices', {
    props: ["course", "uid", "index"],
    data: function () {
        return {
            prices: [],

            loading: false
        }
    },
    watch: {
        'uid': {
            handler: function (nv, ov) {
                if (!$api.isnull(nv)) {
                    if (this.index == 0) this.startInit();
                }
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/course_prices.css']);
    },
    methods: {
        //初始加载
        startInit: function () {
            //加载完成，则加载后一个组件，实现逐个加载的效果
            this.getPrices().finally(() => {
                var vapp = window.vapp;
                var ctr = vapp.$refs['prices' + (this.index + 1)];
                if (ctr != null) ctr.startInit();
            });
        },
        //加载价格信息
        getPrices: function () {
            var th = this;
            return new Promise(function (res, rej) {
                var cou = th.course;
                if (cou.Cou_IsFree || cou.Cou_IsLimitFree || cou.Cou_Price==0) return res();
                if (!$api.isnull(cou.Cou_Prices) && cou.Cou_Prices.length != 0) {
                    //console.error(cou.Cou_Prices);
                    th.prices = cou.prices = $api.parseJson(cou.Cou_Prices);
                    return res();
                }
                th.loading = true;
                $api.put('Course/PriceItems', { 'uid': cou.Cou_UID }).then(function (req) {
                    if (req.data.success) {
                        th.prices = req.data.result;
                        cou.prices = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => {
                        th.loading = false;
                        return res();
                    });
            });

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
            <div><el-tag><icon>&#xe81a</icon>限时免费</el-tag></div>
            <div>{{course.Cou_FreeStart|date('yyyy.M.dd')}} - {{course.Cou_FreeEnd|date('M.dd')}} </div>
        </template>           
        <template v-else>            
            <span class="el-icon-loading" v-if="loading"></span>               
            <template v-for="(item,i) in prices">
                <div v-if="i<2"  :class="{'price':true,'first':prices.length>1 && i==0}" :title="showdetail()">
                    <icon>&#xe624</icon>{{item.CP_Span}}{{item.CP_Unit}}{{item.CP_Price}}元
                </div>
            </template>              
        </template>          
    </div> `
});