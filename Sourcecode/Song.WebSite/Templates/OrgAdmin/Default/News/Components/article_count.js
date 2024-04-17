//栏目下的文章数
$dom.load.css([$dom.pagepath() + 'Components/Styles/article_count.css']);
Vue.component('article_count', {
    props: ["column", "orgid"],
    data: function () {
        return {
            count: 0,
            loading: true
        }
    },
    watch: {
        'column': {
            handler: function (nv, ov) {
                if (nv) this.getcount();
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {
        getcount: function () {
            var th = this;
            th.loading = true;
            $api.get('News/Count', { 'orgid': th.orgid, 'uid': th.column.Col_UID, 'isuse': '' }).then(function (req) {
                if (req.data.success) {
                    th.count = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },
    template: `<span class="article_count" v-if="count>0">({{count}})</span>`
});