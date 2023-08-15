
//友情链接
$dom.load.css([$dom.path() + 'Components/Styles/links.css']);
//友情链接的分类
Vue.component('linksorts', {
    //友情链接分类，
    //sort:分类id,如果为空，取所有指定个数(count)
    //count:取多少个分类  
    //showdetail: 显示详情
    props: ["org", "sort", 'count', 'showdetail'],
    data: function () {
        return {
            sorts: [],           //链接分类   
            loading: false
        }
    },
    watch: {
        'org': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getsort();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //获取链接分类
        getsort: function () {
            var th = this;
            if (th.sort != null) {
                th.loading = true;
                $api.post('Link/SortForID', { 'id': th.sort }).then(function (req) {
                    if (req.data.success) {
                        th.$set(th.sorts, 0, req.data.result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            } else {
                th.loading = true;
                $api.post('Link/SortCount', { 'orgid': th.org.Org_ID, 'use': true, 'show': null, 'search': '', 'count': th.count })
                    .then(function (req) {
                        if (req.data.success) {
                            th.sorts = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        }
    },

    template: `<weisha class="linksorts" v-if="sorts.length>0">
        <slot name="title"></slot>           
        <links :sort="ls" v-for="(ls,i) in sorts" :showdetail="showdetail">
            <template slot="sortname">
                <slot name="sortname" :sort="ls"></slot>
            </template>  
        </links>       
    </weisha>`
});
//友情链接列表
Vue.component('links', {
    //sort:友情链接分类，  
    //count:取多少个链接
    props: ["sort", 'count', 'showdetail'],
    data: function () {
        return {
            datas: [],
            loading: false
        }
    },
    watch: {
        'sort': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getdata();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //获取数据
        getdata: function () {
            if (!(!!this.sort.Ls_Id)) return;
            var th = this;
            th.loading = true;
            $api.cache('Link/Count',
                { 'orgid': -1, 'sortid': th.sort.Ls_Id, 'use': true, 'show': '', 'search': '', 'count': th.count })
                .then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //是否显示详细信息，包括联系方式
        isshowdetail: function (item) {
            let show = $api.isnull(this.showdetail) ? false : this.showdetail;
            if (!show || !item.Lk_IsShow) return false;
            return item.Lk_Explain != '' || item.Lk_Mobile != '' || item.Lk_QQ != '' || item.Lk_SiteMaster != '';
        }
    },

    template: `<weisha class="links" v-if="datas.length>0">
        <slot name="sortname"></slot>   
        <div class="linksarea">
            <div class="link-item" v-for="d in datas" :show="isshowdetail(d)">
                <template v-if="sort.Ls_IsImg">
                    <a :href="d.Lk_Url" target="_blank"  v-if="d.Lk_Logo!=''" :title="d.Lk_Tootip">
                        <img :src="d.Lk_Logo"/>
                    </a>
                    <a :href="d.Lk_Url" target="_blank"  v-if="sort.Ls_IsText" :title="d.Lk_Tootip">
                        {{d.Lk_Name}}
                    </a>
                </template>
                <a :href="d.Lk_Url" target="_blank"  v-else :title="d.Lk_Tootip">
                    {{d.Lk_Name}}
                </a>
                <div v-if="isshowdetail(d)" class="detail">
                    <div v-html="d.Lk_Explain"></div>
                    <div>{{d.Lk_SiteMaster}} 
                    <a :href="'tel:'+d.Lk_Mobile+'#mp.weixin.qq.com'" v-if="d.Lk_Mobile!=''"><icon mobile>{{d.Lk_Mobile}}</icon></a>
                    <icon QQ v-if="d.Lk_QQ!=''">{{d.Lk_QQ}}</icon></div>
                </div>
            </div>    
        </div>       
    </weisha>`
});



