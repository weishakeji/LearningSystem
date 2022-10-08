//试题数量
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
    computed: {


    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'TestPaper/Components/Styles/ques_count.css']);

    },
    methods: {
        getcount: function () {
            var th = this;
            th.loading = true;
            var query = { 'orgid': '', 'sbjid': '', 'couid': th.couid, 'olid': th.olid, 'type': th.qtype, 'use': true };
            console.log(query);
            $api.get('Question/Count', query)
                .then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.count = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
        }

    },
    template: `<span class="ques_count">
    <loading v-if="loading"></loading>
    <template v-else>(共 {{count}} 道)</template>
    </span> `
});
