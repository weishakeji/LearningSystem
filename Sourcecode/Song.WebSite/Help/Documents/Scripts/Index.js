﻿$ready(function () {

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

            iframshow: false,   //是否显示iframe

            edit: false,      //是否为编辑模式
            content: '',     //帮助文件的内容
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
            },
            //是否为编辑状态
            'edit': {
                handler: function (nv, ov) {
                    if (nv) {
                        this.iframshow = false;
                        this.getcontent(this.file.url);
                        this.$nextTick(function () {
                            this.tinymcecss();
                        });
                    }
                    else {
                        this.$nextTick(function () {
                            this.iframecss();
                        });
                    }
                    this.fileExist(this.file['url']);
                }, immediate: true
            }
        },
        methods: {
            //设置iframe的样式
            iframecss: function () {
                const iframe = document.getElementById(this.file.name);
                if (iframe) {
                    var th = this;
                    iframe.onload = function () {
                        th.iframeAttachCss(iframe);
                        window.setTimeout(() => th.iframshow = true, 100);
                    };
                } else window.setTimeout(() => this.iframecss(), 10);
            },
            //编辑器的css加载
            tinymcecss: function () {
                const iframes = document.getElementsByTagName('iframe');
                var iframe = null;
                for (let i = 0; i < iframes.length; i++) {
                    const item = iframes[i];
                    if (item.id && item.id != '' && item.id.indexOf('tinymce_editor') > -1)
                        iframe = item;
                }
                if (iframe == null) {
                    window.setTimeout(() => this.tinymcecss(), 300);
                } else this.iframeAttachCss(iframe);
            },
            //给iframe附加css样式
            iframeAttachCss: function (iframe) {
                let styles = ['/Help/Documents/Styles/iframe.css', '/Utilities/Fonts/icon.css']
                for (let i = 0; i < styles.length; i++) {
                    let style = document.createElement('link');
                    style.rel = 'stylesheet';
                    style.href = styles[i];
                    iframe.contentWindow.document.head.appendChild(style);
                }
            },
            //文件是否存在
            fileExist: function (url) {
                var th = this;
                th.loading = true;
                $api.get('HelpDocument/Fileexist', { 'file': url }).then(req => {
                    if (req.data.success) {
                        th.file['exist'] = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取帮助文件的内容
            getcontent: function (url) {
                var th = this;
                th.loading = true;
                $api.get('HelpDocument/Filecontent', { 'file': url }).then(req => {
                    if (req.data.success) {
                        th.content = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //保存编辑状态
            btnSave: function () {
                var th = this;
                th.loading = true;
                $api.get('HelpDocument/FileSave', { 'file': th.file.url, 'content': th.content }).then(req => {
                    if (req.data.success) {
                        th.$message({ message: '保存成功！', type: 'success' });
                        this.edit = false;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //退出编辑状态
            btnCancel: function () {
                this.$confirm('是否要退出编辑, 请确认退出前已经保存?', '提示', {
                    confirmButtonText: '确定', cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.edit = false;
                }).catch(() => { });

            }
        }
    });

}, []);
