//栏目下的新闻
Vue.component('articles', {
    props: ['column', 'index'],
    data: function () {
        return {
            datas: [],       //新闻列表
            loading: false
        }
    },
    watch: {},
    computed: {},
    mounted: function () {
        var th = this;
        th.loading = true;
        $api.get('News/Articles', { 'uid': this.column.Col_UID, 'count': 10, 'order': '' }).then(function (req) {
            th.loading = false;
            if (req.data.success) {
                th.datas = req.data.result;
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    },
    methods: {},
    template: `<card-context> 
        <div v-if="loading"><van-loading size="24px">加载中...</van-loading></div>
        <row v-for="(art,i) in datas" v-if="datas.length>0" @click="window.location.href='article.'+art.Art_Id">
        <a href="#">{{art.Art_Title}}        
            <van-tag color="#eee" text-color="#666">{{art.Art_PushTime|date("yyyy-MM-dd")}}</van-tag>
        </a>
        </row>
        <div  v-if="datas.length==0 && !loading" class="noarticle">没有内容</div>
        </card-context>`
});

