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
        $api.get('News/ArticlesShow', { 'orgid': -1, 'uid': th.column.Col_UID, 'count': this.count, 'order': 'top' }).then(function (req) {
            if (req.data.success) {
                th.datas = req.data.result;
            } else {
                console.error(req.data.exception);
                throw req.data.message;
            }
        }).catch(err => console.error(err))
            .finally(() => th.loading = false);
    },
    methods: {},
    template: `<card-context> 
        <div v-if="loading"><loading>加载中...</loading></div>    
        <div class="item" v-for="(art,i) in datas">  
            <a :href="'/web/news/article.'+art.Art_ID" target="_blank">
                <icon>&#xe649</icon>{{art.Art_Title}}
            </a>
            <span>[ {{art.Art_PushTime|date("MM-dd")}} ]</span>
        </div>
        <div  v-if="datas.length==0 && !loading" class="noarticle">没有内容</div>
    </card-context>`
});

