$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            page: decodeURIComponent($api.querystring('page')),
            //帮助文件,依次是 文件名，完整路径，文件是否存在
            file: {
                name: '', url: '', exist: false
            },
            //是否为本机
            islocal: $api.islocal(),
            admin: null,    //管理员信息

            edit:false,      //是否为编辑模式
            loading: false,
          
        },
        mounted: function () {

        },
        created: function () {
            var th = this;
            //判断是否登录
            $api.login.current('admin',
                d => this.admin = d,
                () => this.admin = null);
        },
        computed: {

        },
        watch: {
            'page': {
                handler: function (nv, ov) {
                    let name = nv;
                    if (name.indexOf('.') > -1) name = name.substring(0, name.lastIndexOf('.'));
                    if (name.charAt(0) === '/') name = name.substring(1);
                    name = name.replace(/\//g, "_");
                    this.file['name'] = name.toLowerCase();
                    this.file['url'] = ("/help/Documents/Files/" + name + ".html").toLowerCase();
                    this.fileExist(this.file['url']);

                }, immediate: true
            }
        },
        methods: {
            //文件是否存在
            fileExist: function (url) {
                var th = this;
                th.loading = true;
                $api.get('Helper/Fileexist', { 'file': url }).then(req => {
                    if (req.data.success) {
                        th.file['exist'] = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        }
    });

}, []);
