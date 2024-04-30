
//课程，单个课程
Vue.component('course', {
    //item:课程项
    props: ["item", "org"],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，  
            mremove: false,  //移除金额、充值相关
            datas: []
        }
    },
    watch: {
        'org': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) == '{}' || nv == null) return;
                var config = $api.organ(nv).config;
                //是否移除充值金额相关
                if (!!config.IsMobileRemoveMoney)
                    this.mremove = config.IsMobileRemoveMoney;
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.path() + 'Components/Styles/course.css';
        $dom.load.css([css]);
    },
    methods: {
        //跳转
        godetail: function (id) {
            var url = $api.url.dot(id, '/web/Course/Detail');
            window.location.href = url;
        },
        seturl: function () {
            return $api.url.dot(this.item.Cou_ID, '/web/Course/Detail');
        }
    },

    template: `<weisha_course  v-if="item" :rec="item.Cou_IsRec">
        <a class="name" :href="seturl()" target="_blank">
            <img :src="item.Cou_LogoSmall" v-if="item.Cou_LogoSmall && item.Cou_LogoSmall!=''"/>
            <img src="/Utilities/images/cou_nophoto.jpg" v-else />
        </a>
        <el-tag type="warning" class="type" v-if="item.Cou_Type">试题库</el-tag>   
        <a class="name" :href="seturl()" target="_blank">{{ item.Cou_Name }}</a>
        <div class="price" v-if="!mremove">
            <span class="free" v-if="item.Cou_IsFree">免费</span>
            <span class="money" v-else-if="item.Cou_Price>0">
                <icon>&#xe818;</icon>
                {{item.Cou_Price}}元/{{item.Cou_PriceSpan}}{{item.Cou_PriceUnit}}             
            </span>
            <span class="view" title="访问量">{{item.Cou_ViewNum}}</span>
        </div>
    </weisha_course>`
});
