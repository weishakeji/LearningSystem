$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},
            //是否不再显示
            not_show: false,
            loading: false,
        },
        mounted: function () {
            var show = $api.storage('not_show');
            show = show == null ? false : JSON.parse(show);
            this.not_show = show;
        },
        created: function () {

            window.setTimeout(() => this.copyright(), 100);
        },
        computed: {

        },
        watch: {
            'not_show': {
                handler: function (newVal, oldVal) {
                    $api.storage('not_show', newVal);
                }//, immediate: true,
            }
        },
        methods: {
            copyright: function () {
                $api.get('Copyright/Info').then(function (req) {
                    if (req.data.success) {
                        let copyright = req.data.result;
                        //$api.copyright = copyright;
                        let nodes = document.querySelectorAll("*[copyright]");
                        for (let i = 0; i < nodes.length; i++) {
                            let node = nodes[i];
                            let name = node.tagName.toLowerCase();
                            let val = $api.trim(node.getAttribute("copyright"));
                            for (let attr in copyright) {
                                if (attr == val) {
                                    let txt = copyright[attr];
                                    switch (name) {
                                        case "a":
                                            node.setAttribute("href", txt);
                                            break;
                                        case "img":
                                            node.setAttribute("src", txt);
                                            break;
                                        default:
                                            node.innerText = txt;
                                    }
                                }
                            }
                        }
                    }
                }).catch(err => { });
            }
        }
    });
});
