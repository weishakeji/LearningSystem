//栏目下的新闻
$dom.load.css([$dom.pagepath() + 'Components/Styles/articles.css']);
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
        $api.get('News/ArticlesShow', { 'orgid': -1, 'uid': this.column.Col_UID, 'count': 10, 'order': 'top' }).then(function (req) {
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
    methods: {
        //行的点击事件
        clk: function (art) {
            navigateTo('article.' + art.Art_ID);
        }
    },
    template: `<card-context :notnull="datas.length>0"> 
        <div v-if="loading"><van-loading size="24px">加载中...</van-loading></div>
        <artrow  v-for="(art,i) in datas" :art="art" ></artrow>
        <div v-if="datas.length==0 && !loading" class="noarticle">
            <icon>&#xe839</icon>没有内容
        </div>
    </card-context>`
});
//新闻的标题行
Vue.component('artrow', {
    props: ['art'],
    data: function () {
        return {         
        }
    },
    watch: {},
    computed: {},
    mounted: function () {       
    },
    methods: {
        //行的点击事件
        clk: function (art) {
            navigateTo('article.' + art.Art_ID);
        }
    },
    template: `<row @click="clk(art)" :logo="art.Art_IsImg && art.Art_Logo!=''">
            <template v-if="!art.Art_IsImg || art.Art_Logo==''">
                <a href="#">
                    {{art.Art_Title}}        
                    <van-tag color="#eee" text-color="#666">{{art.Art_PushTime|date("yyyy-MM-dd")}}</van-tag>
                </a>
            </template>
            <template v-else>
                <img :src="art.Art_Logo"/>
                <div>
                    <span>{{art.Art_Title}}</span>
                    <i>{{art.Art_Number}} 浏览</i>
                    <van-tag color="#eee" text-color="#666">{{art.Art_PushTime|date("yyyy-MM-dd")}}</van-tag>
                </div>
            </template>
        </template>
    </row>`
});
