// 课程购买信息
Vue.component('purchase_data', {
    props: ['couid', 'stid'],
    data: function () {
        return {
            //课程数据信息
            data: {},
            loading: false
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true
        }

    },
    computed: {},
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.cache('Course/Purchaselog:5', { 'couid': this.couid, 'stid': this.stid }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        }
    },
    template: `<slot v-else :data="data"></slot>`
});