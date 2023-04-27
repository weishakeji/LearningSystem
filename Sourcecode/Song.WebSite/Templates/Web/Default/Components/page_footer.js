
//顶部导航
$dom.load.css([$dom.path() + 'Components/Styles/page_footer.css']);
Vue.component('page_footer', {
    props: ["organ"],
    data: function () {
        return {
            show: false,
            menus: []        //导航菜单
        }
    },
    watch: {
        'organ': {
            handler: function (nv, ov) {
                this.$nextTick(function () {

                });
                this.getnavi();
            }, immediate: true
        },
        'menus': {
            handler: function (nv, ov) {
                this.$nextTick(function () {

                });
            }, immediate: true
        }
    },
    computed: {
    },
    mounted: function () {        
       
    },
    methods: {
        //获取导航菜单
        getnavi: function () {
            if (!(this.organ && this.organ.Org_ID)) return;
            var th = this;
            $api.get('Navig/web', { 'orgid': this.organ.Org_ID, 'type': 'foot' }).then(function (req) {
                if (req.data.success) {
                    th.menus = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            });
        },
        //导航菜单的点击事件
        menuClick: function (sender, eventArgs) {
            var data = eventArgs.data;
            if (!data || data.url == '') return;
            window.location.href = data.url;
            console.log(data);
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `<weisha_page_footer v-if="organ && JSON.stringify(organ) != '{}'">
        <div id="foot_bar">
           <a v-for="item in menus" :href="item.Nav_Url" :target="item.Nav_Target">
            <icon v-if="item.Nav_Icon!=''" v-html="'&#x'+item.Nav_Icon"></icon>
            <b v-html="item.Nav_Name" v-if="item.Nav_IsBold"  :style="'color:'+item.Nav_Color"></b>
            <span v-html="item.Nav_Name" v-else  :style="'color:'+item.Nav_Color"></span>
           </a>         
        </div>
        <div id="foot_context">
        {{organ.Org_Name}}  &nbsp;   <telphone v-if="organ.Org_Phone!=''">{{organ.Org_Phone}}</telphone><br/>
        <address v-if="organ.Org_Address!=''">{{organ.Org_Address}}</address>
        <email v-if="organ.Org_Email!=''">{{organ.Org_Email}}</email><br/>
        <span>Copyright &copy; {{organ.Org_AbbrEnName}} All rights reserved</span><br/>
        <div class="beian"> 
            <a v-if="organ.Org_ICP!=''" title="ICP备案号" href="http://beian.miit.gov.cn/" target="_blank">
                <icon>&#xa054</icon>{{organ.Org_ICP}}
            </a> 
            <a v-if="organ.Org_GonganBeian!=''" title="公案备案号" :href="'http://www.beian.gov.cn/portal/registerSystemInfo?recordcode='+organ.Org_GonganBeian" target="_blank">
                <icon>&#xa02b</icon>{{organ.Org_GonganBeian}}
            </a>        
        </div>         
        </div>       
    </weisha_page_footer>`
});
