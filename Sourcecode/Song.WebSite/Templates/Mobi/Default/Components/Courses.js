//课程列表
$dom.load.css([$dom.path() + 'Components/Styles/courses.css']);
Vue.component('courses', {
    //sbjid:专业id,小于等于0，取推荐课程
    props: ['sbjid', 'title', 'orgid', 'config', 'count'],
    data: function () {
        return {
            datas: [],  //课程数据集        
            defcount: 8,     //默认取多少条
            mremove: false,  //移除金额、充值相关
            loading: false       //预载
        }
    },
    watch: {
        'sbjid': {
            handler: function (val, old) {
                this.getCourse(val, 8, 1);
            },
            immediate: true
        },
        'orgid': {
            handler: function (val, old) {
                var count = this.count > 0 ? parseInt(this.count) : this.defcount;
                this.getCourse(this.sbjid, count, 1);
            },
            immediate: true
        },
        'config': {
            handler: function (val, old) {
                //是否移除充值金额相关
                if (!!val.IsMobileRemoveMoney)
                    this.mremove = val.IsMobileRemoveMoney;
            },
            immediate: true
        },
    },
    created: function () {

    },
    methods: {
        getCourse: function (sbj, size, index) {
            if (this.orgid == null) return;
            if (sbj == null) sbj = this.sbjid;
            var th = this;
            th.loading = true;
            $api.cache('Course/ShowPager:30',
                { 'orgid': this.orgid, 'sbjids': sbj, 'search': '', 'order': 'rec', 'size': size, 'index': index })
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.datas = result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
        },
        //打开课程详情
        open: function (cour) {
            let url = $api.url.dot(cour.Cou_ID, '/mobi/course/Detail');
            window.navigateTo(url);
        },
        //查看更多
        gomore: function () {
            var url = "/mobi/course/index";
            if (this.sbjid == 0 || this.sbjid == '') {
                return window.navigateTo(url);
            }
            url = $api.url.set(url, {
                'sbjid': this.sbjid,
                'sbjname': encodeURIComponent(this.title)
            });
            window.navigateTo(url);
        },
        //显示价格
        price: function (cour) {
            var price = "";
            if (cour.Cou_IsFree) {
                price = "<f>免费</f>";
            } else if (cour.Cou_IsLimitFree) {
                var end = Date.parse(cour.Cou_FreeEnd);
                var date = end.format("yyyy-M-d");
                price = "<l>免费至" + date + "</l>";
            } else if (cour.Cou_Price > 0) {
                price = "<m>" + unescape(cour.Cou_PriceSpan) + unescape(cour.Cou_PriceUnit) + cour.Cou_Price + "元</m>";
            }

            return price;
        }
    },
    template: `<div class="cour-card" :sbjid="sbjid" v-if="datas.length>0">
            <div class="cour-title" @click="gomore">
                <van-loading type="spinner" v-if="loading" size="14"></van-loading>
                <icon v-else>&#xa011</icon>
                <b v-html="title"></b>
                <more remark="更多"></more>                
            </div>
            <div class="cour-content">
                <div class="cour-box" :couid="c.Cou_ID" v-for="(c,i) in datas" v-on:click.stop="open(c)">
                    <rec v-if="c.Cou_IsRec"></rec>
                    <img :src="c.Cou_LogoSmall" v-if="c.Cou_LogoSmall!=''"/>
                    <img src="/Utilities/images/cou_nophoto.jpg" v-else />
                    <name>
                        <live v-if="c.Cou_ExistLive"></live>                     
                        <t v-if="c.Cou_IsTry"></t>{{c.Cou_Name}}
                    </name>
                    <price v-html="price(c)" v-if="!mremove">{{price(c)}}</price>
                </div>
            </div>               
        </div>`
});