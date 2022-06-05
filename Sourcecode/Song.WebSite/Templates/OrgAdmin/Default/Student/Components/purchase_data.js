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
            $api.get('Course/Purchaselog:5', { 'couid': this.couid, 'stid': this.stid }).then(function (req) {
                if (req.data.success) {
                    th.data = req.data.result;
                    //...
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `  <div class="purchase_data"> 
    <icon>&#xe671</icon>{{data.Stc_EndTime|date("yyyy-M-d ")}} 过期                 
                </div>`
});