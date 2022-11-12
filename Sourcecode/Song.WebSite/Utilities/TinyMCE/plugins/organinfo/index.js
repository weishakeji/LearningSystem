$(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            editorid: $api.querystring('editorid'),  //编辑器的id
            organ: {},
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.get('Organization/Current').then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.organ = req.data.result;
                    console.log(th);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        updated: function () {
            var th=this;
            $("#vapp>div").unbind('dblclick');
            $("#vapp>div").dblclick(function (e) {
                var t = $(this);
                var txt = t.attr('title') + ':' + t.text();
                //var id=th.parent.attr('editorid');
                window.parent.organinfo_action(th.editorid,txt);
            });
        },
        created: function () {

        },
        computed: {
        },
        watch: {
        },
        methods: {
            dbclick_event: function (text) {

            }
        }
    });
});