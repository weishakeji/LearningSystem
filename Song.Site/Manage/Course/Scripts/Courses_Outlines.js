var vm = {
    data() {
        return {
            form: {},
            loading: false,
            tableDatas: [],
            multipleSelection: [],
            couid: $api.querystring('couid')
        }
    },
    watch: {
    },
    methods: {
        getDatas: function () {
            console.log(this.couid);
            var th = this;
            $api.post('Outline/Tree', { 'couid': this.couid }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.tableDatas = result;
                    //...
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
            });
        },
        handleSelectionChange: function (val) {
            this.multipleSelection = val;
        }
    },
    created: function () {
        this.getDatas();
    }
};
//vm.$mount('#app-area');

var Ctor = Vue.extend(vm)
new Ctor().$mount('#app-area');


