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

    },
    template: `<div class="cour-card" :sbjid="sbjid" v-if="datas.length>0">
            <div class="cour-title" @click="gomore">
                <van-loading type="spinner" v-if="loading" size="14"></van-loading>
                <icon v-else>&#xa011</icon>
                <b v-html="title"></b>
                <more remark="更多"></more>                
            </div>
            <div class="cour-content">
                <cour-box v-for="(c,i) in datas" :course="c" :mremove="mremove" @open="open"></cour-box>           
            </div>               
        </div>`
});
Vue.component('cour-box', {
    //mremove:是否移除资金相关信息
    props: ['course', 'mremove'],
    data: function () {
        return {
            defpic: '/Utilities/images/cou_nophoto.jpg'
        }
    },
    watch: {
        'course': {
            handler: function (val, old) {
                if ($api.isnull(val)) return;
                var th = this;
                this.$nextTick(function () {
                    let box = $dom('.cour-box[couid="' + val.Cou_ID + '"]');
                    let img = box.find('img');
                    // 
                    let src = val.Cou_LogoSmall;
                    if (src == undefined || src == null || src == '') return;
                    img[0].setAttribute('src', src);
                });
            },
            immediate: true
        },
    },
    created: function () {
    },
    methods: {
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
                price = "<m>" + decodeURIComponent(cour.Cou_PriceSpan) + decodeURIComponent(cour.Cou_PriceUnit) + cour.Cou_Price + "元</m>";
            }
            return price;
        },
        //点击事件
        clickevent: function () {
            this.$emit("open", this.course);
        },
        imgonload: function (event) {
           
        }
    },
    template: `<div class="cour-box" :couid="course.Cou_ID" v-on:click.stop="clickevent">
                <rec v-if="course.Cou_IsRec"></rec>
                <img :src="defpic" @load='imgonload'/>
                <name>
                    <live v-if="course.Cou_ExistLive"></live>                     
                    <t v-if="course.Cou_IsTry"></t>{{course.Cou_Name}}
                </name>
                <price v-html="price(course)" v-if="!mremove">{{price(course)}}</price>
        </div> `
});