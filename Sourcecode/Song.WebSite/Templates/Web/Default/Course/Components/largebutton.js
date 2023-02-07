//课程购买或学习的按钮
Vue.component('largebutton', {
    //当前课程，当前学员
    props: ["course", "account", "studied", "finaltest", "purchase","forever","loading"],
    data: function () {
        return {}
    },
    watch: {},
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        },
        //是否存在结课考试
        istest: function () {
            return JSON.stringify(this.finaltest) != '{}' && this.finaltest != null;
        },
        //课程为空,或课程被禁用
        nullcourse: function () {
            return JSON.stringify(this.course) == '{}' || this.course == null || !this.course.Cou_IsUse;
        },
        //是否购买记录
        purchased: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
        },
        //可以学习
        canstudy: function () {
            return this.studied && (this.purchased && this.purchase.Stc_IsEnable);
        }
    },
    mounted: function () { },
    methods: {
        url: function (type) {
            var urls = [
                { 'type': 'login', 'link': '/web/sign/in' },
                { 'type': 'study', 'link': '/web/course/study.' + this.course.Cou_ID },
                { 'type': 'buy', 'link': '/web/course/buy.' + this.course.Cou_ID },
                { 'type': 'test', 'link': '/web/test/paper.' + this.finaltest.Tp_Id },
            ];
            var url = urls.find(function (item) {
                return item.type == type;
            });
            return url.link;
        }
    },
    template: `<div class="couBtnBox">
        <loading v-if="loading && islogin"></loading>
        <template v-else>
            <a v-if="!islogin" :href="url('login')">登录学习</a> 
            <template v-else-if="canstudy || forever">
                <a :href="url('study')">开始学习</a>
                <a :href="url('test')" v-if="istest" class="finaltest"><icon>&#xe810</icon>结课考试</a>
            </template>      
            <a v-else-if="course.Cou_IsFree" :href="url('study')">开始学习</a>
            <template v-else-if="course.Cou_IsLimitFree" remark="限时免费">
                <a :href="url('study')">开始学习</a>
                <a :href="url('buy')" class="buy">选修该课程</a>
            </template>
            <template v-else-if="course.Cou_IsTry" remark="可以试学">
                <a :href="url('study')">试学</a>
                <a :href="url('buy')"  class="buy">选修该课程</a>
            </template>
            <a v-else :href="url('buy')" class="buy">选修该课程</a> 
        </template>
    </div>`
});