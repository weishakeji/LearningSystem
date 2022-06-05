$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id')

        },
        computed: {

        },
        watch: {

        },
        created: function () {
            this.handleCurrentChange();
        },
        methods: {
            //加载数据页
            handleCurrentChange: function (index) {

            }

        },
        components: {

        }
    });

});