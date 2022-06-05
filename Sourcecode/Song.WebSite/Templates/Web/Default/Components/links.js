
//友情链接
Vue.component('links', {
    //友情链接分类，取多少条记录，低于多少条不再显示,排序方式
    props: ["sort", "org", 'count', 'mincount', 'order'],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，  
            datas: []
        }
    },
    watch: {
        'org': {
            handler: function (nv, ov) {
                if (JSON.stringify(nv) != '{}' && nv != null)
                    this.getdata();
            }, immediate: true
        }
    },
    computed: {
    },
    mounted: function () {
        var css = $dom.path() + 'Components/Styles/links.css';
        var isexist = false;
        $dom("link[href]").each(function () {
            var href = $dom(this).attr("href");
            if (href.indexOf(css) > -1) {
                isexist = true;
                return false;
            }
        });
        if (!isexist) $dom.load.css([css]);
        this.mincount = !parseInt(this.mincount) ? 0 : parseInt(this.mincount);
    },
    methods: {
        //获取数据
        getdata: function () {
            var th = this;
            if (!(!!this.org.Org_ID)) return;
            var sortid = th.sort == null ? 0 : th.sort.Ls_Id;
            var orgid = th.org == null ? 0 : th.org.Org_ID;

            $api.cache('Link/Count',
                { 'orgid': orgid, 'sortid': sortid, 'use': true, 'show': true, 'search': '', 'count': 20 })
                .then(function (req) {
                    if (req.data.success) {
                        th.datas = req.data.result;
                        th.show = th.datas.length >= th.mincount;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
        }
    },

    template: `<weisha class="links" v-show="show">
        <div class="links-tit">
            <a href="links.ashx"  v-if="sort==null" target="_blank">友情链接</a>
            <a href="links.ashx"  v-else target="_blank">{{sort.Ls_Name}}</a>
        </div>       
        <div class="link-items">
          <div v-for="d in datas">
            <div class="link-item"><a :href="d.Lk_Url" target="_blank">{{d.Lk_Name}}</a></div>
          </div>
        </div>
    </weisha>`
});
