$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项

            form: { 'orgid': 0, 'search': '', 'size': 10, 'index': 1 },
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数

            loading: true,
        },
        mounted: function () {
            $api.bat(
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, org) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                vapp.platinfo = platinfo.data.result;
                vapp.org = org.data.result;
                vapp.config = $api.organ(vapp.org).config;
                vapp.form.orgid = vapp.org.Org_ID;
                vapp.handleCurrentChange(1);
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {
        },
        watch: {
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                th.loading = true;
                $api.get("Notice/ShowPager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        th.datas = d.data.result;
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //清除html代码，并截取字符长度
            clearhtml: function (html, len) {
                var regex = /(<([^>]+)>)/ig;
                html = html.replace(regex, "");
                if (html < len) return html;
                return html.substring(0, len) + "...";
            }
        }
    });
    //新闻列表
    Vue.component('news', {
        props: ["org", "count"],
        data: function () {
            return {
                datas: [],
                loading: false
            }
        },
        watch: {
            'org': {
                handler: function (nv, ov) {
                    this.getdatas();
                }, immediate: true
            }
        },
        computed: {
        },
        methods: {
            //获取新闻
            getdatas: function () {
                if (!this.org.Org_ID) return;
                var th = this;
                var orgid = this.org.Org_ID;
                th.loading = true;
                $api.get('News/ArticlesShow', { 'orgid': orgid, 'uid': '', 'count': th.count, 'order': 'hot' }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            }
        },
        template: `<weisha_newslist>
        <loading v-if="loading">  </loading>
        <div v-else>
            <template v-if="datas.length<1">  </template>
            <dl v-else>    
                <dd v-for="(item,i) in datas">
                    <slot :item="item" :index="i"></slot>
                </dd>     
            </dl>           
        </div>
    </weisha_newslist>`
    });

});
