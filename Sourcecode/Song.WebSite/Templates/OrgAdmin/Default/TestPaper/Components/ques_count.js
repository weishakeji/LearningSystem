//试题数量
$dom.load.css([$dom.path() + 'TestPaper/Components/Styles/ques_count.css']);
Vue.component('ques_count', {
    //qtype:题型
    props: ['couid', 'qtype', 'olid'],
    data: function () {
        return {
            count: 0,
            loading: false
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                if (nv == null || nv == 0) return;
                this.getcount();
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
            var query = { 'orgid': '-1', 'sbjid': '-1', 'couid': th.couid, 'olid': th.olid, 'type': th.qtype, 'use': true };
            $api.get('Question/Total', query).then(function (req) {
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
    template: `<span class="ques_count">
    <loading bubble v-if="loading"></loading>
    <template v-else>(共 {{count}} 道)</template>
    </span> `
});
