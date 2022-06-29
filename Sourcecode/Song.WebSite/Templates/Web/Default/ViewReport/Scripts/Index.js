
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            platinfo: {},
            organ: {},
            config: {},

            quantity: {},        //数量

            test: { number: [56], content: '{nt}个' }

        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (platinfo, organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                th.platinfo = platinfo.data.result;
                document.title += ' - ' + th.platinfo.title;
                th.organ = organ.data.result;
                th.config = $api.organ(th.organ).config;

                th.getquantity();

            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {


        },
        methods: {
            getquantity: function () {
                var th = this;
                $api.get('Platform/Datas', { 'orgid': '' }).then(function (req) {
                    if (req.data.success) {
                        th.quantity = req.data.result;
                        console.group("相关数据统计")
                        console.log(th.quantity);

                        th.test.number = [th.quantity.course];
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    Vue.prototype.$alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ["/Utilities/Viewport/datav.min.vue.js",
    "Components/number.js"]);
