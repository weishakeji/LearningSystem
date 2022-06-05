//栏目下的新闻列表
Vue.component('articles', {
    props: ['column', 'count'],
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
        $api.get('News/Articles', { 'uid': this.column.Col_UID, 'count': this.count, 'order': '' }).then(function (req) {
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
        <a :href="'/web/news/article.'+art.Art_Id"  v-for="(art,i) in datas">
            <icon>&#xe649</icon>{{art.Art_Title}}        
            <span v-if="false">{{art.Art_PushTime|date("yyyy-MM-dd")}}</span>
        </a>
      
        <div  v-if="datas.length==0 && !loading" class="noarticle">没有内容</div>
        </card-context>`
});

