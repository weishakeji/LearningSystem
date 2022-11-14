
window.vapp = new Vue({
    el: '#vapp',
    data: {
        uploadkey: $api.querystring('uploadkey'),   //上传资源的路径配置项，来自web.config中的upload节点
        dataid: $api.querystring('dataid'),         //数据项的id，例如试题id,新闻id
        editorid: $api.querystring('editorid'),     //编辑器的id
        param: $api.querystring('param'),           //来自编辑器的参数

        images: [],          //服务器端的资源图片
        img_sel: {},         //当前选中的图片

        upimage: {},     //要上传的对象
        upfile:{},          //要上传的文件
        //organ: {},
        loading: false
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
        }
    }
});
