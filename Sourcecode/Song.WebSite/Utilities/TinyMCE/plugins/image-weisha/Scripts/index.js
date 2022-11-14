
window.vapp = new Vue({
    el: '#vapp',
    data: {
        uploadkey: $api.querystring('uploadkey'),   //上传资源的路径配置项，来自web.config中的upload节点
        dataid: $api.querystring('dataid'),         //数据项的id，例如试题id,新闻id
        editorid: $api.querystring('editorid'),     //编辑器的id
        param: $api.querystring('param'),           //来自编辑器的参数

        images: [],          //服务器端的资源图片
        img_sel: {},         //当前选中的图片

        upfile: null,          //要上传的文件
        upimage: {},             //用来插件到编辑器的上传图片，其格式与img_sel一致

        outside: {},         //外网图片的对象，其格式与img_sel一致
        //organ: {},
        loading: true
    },
    mounted: function () {
        var th = this;
        this.getimages();
    },
    updated: function () {

    },
    created: function () {

    },
    computed: {
    },
    watch: {
        'upfile': function (nv, ov) {
            console.log(nv);
        }
    },
    methods: {
        //获取图片资源
        getimages: function () {
            var th = this;
            th.loading = true;
            $api.get('Upload/Images', { 'pathkey': th.uploadkey, 'dataid': th.dataid }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.images = req.data.result;
                    console.log(th.images);
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
        //将图片插入到编辑器
        insert: function () {
            window.parent.image_weisha_action(this.editorid, this.img_sel);
        },
        upload: function () {
            var th = this;
            th.loading = true;
            $api.post('Upload/ImageSave', {
                'file': th.upfile, 'pathkey': th.uploadkey, 'dataid': th.dataid,
                'small': false, 'swidth': '', 'sheight': ''
            }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    for (var k in result)
                        th.upimage[k] = result[k];
                    window.parent.image_weisha_action(th.editorid, th.upimage);
                    //...
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                alert(err);
                console.error(err);
            });
        },
        outside_insert: function () {
            window.parent.image_weisha_action(this.editorid, this.outside);
        },
        //加载外网图片后插入到编辑器
        load_insert: function () {
            var th = this;
            th.loading = true;
            $api.post('Upload/ImageLoad', {
                'url': th.outside.full, 'pathkey': th.uploadkey, 'dataid': th.dataid,
                'small': false, 'swidth': '', 'sheight': ''
            }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    var result = req.data.result;
                    var result = req.data.result;
                    for (var k in result)
                        th.outside[k] = result[k];
                    window.parent.image_weisha_action(th.editorid, th.outside);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        }
    }
});
