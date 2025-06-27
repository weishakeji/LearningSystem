$ready(['Components/topbar.js',
    '/Utilities/Components/avatar.js',
    '/Utilities/Scripts/marked.min.js'],
    function () {
        window.vapp = new Vue({
            el: '#vapp',
            data: {
                couid: $api.querystring("couid"),        //课程id
                loading: false,
            },
            mounted: function () {

            },
            created: function () {

            },
            computed: {

            },
            watch: {

            },
            methods: {

            },
            filters: {

            },
            components: {

            }
        });
    });