$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //章节id
            uid: $api.querystring('uid'),       //章节的uid 

            datas: [],       //附件列表
            ext_limit: "zip,rar,pdf,ppt,pptx,doc,docx,xls,xlsx",
            loading_upload: false,
            loading: false
        },
        watch: {

        },
        mounted: function () {
            this.getDatas();
        },
        created: function () {

        },
        computed: {

        },
        methods: {
            //获取附件
            getDatas: function () {
                let uid = this.uid;
                if (uid == null || uid == '') return;
                var th = this;
                th.loading = true;
                $api.put("Accessory/List", { 'uid': uid, 'type': 'Course' }).then(function (acc) {
                    if (acc.data.success) {
                        th.datas = acc.data.result;
                    } else {
                        th.msg = "附件信息加载异常！详情：\r" + acc.data.message;
                    }
                }).catch(err => th.msg = err)
                    .finally(() => th.loading = false);
            },
            //附件的点击事件
            accessClick: function (file, tit, event) {
                var exist = file.substring(file.lastIndexOf(".") + 1).toLowerCase();
                if (exist == "pdf") {
                    event.preventDefault();
                    var obj = {
                        width: "60%", height: "60%",
                        full: true, max: true, min: false,
                        resize: true, move: true,
                        ico: "e848", showmask: true
                    };
                    obj.url = $api.pdfViewer(file);
                    window.top.$pagebox.create(obj).open();
                }
                return false;
            },
            //删除文件
            deleteitem: function (data) {
                var th = this;
                var id = data.As_Id;
                $api.delete('Accessory/DeleteForID', { 'id': id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        th.getDatas(data.As_Uid);
                        th.fresh_parent();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => { });
            },
            //附件文件上传
            uploadAccessory: function (file) {
                var th = this;
                th.loading_upload = true;
                var uid = this.uid;
                $api.post('Accessory/Upload', { 'uid': uid, 'type': 'Course', 'file': file }).then(function (req) {
                    if (req.data.success) {
                        // var result = req.data.result;
                        //th.getDatas(uid);
                        th.datas = req.data.result;
                        th.fresh_parent();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(err => alert(err))
                    .finally(() => th.loading_upload = false);
            },
            //刷新上级列表
            fresh_parent: function () {
                //如果处于课程编辑页，则刷新
                var pagebox = window.top.$pagebox;
                if (pagebox && pagebox.source.top)
                    pagebox.source.top(window.name, 'vapp.fresh_frame("vapp.getTreeData")', false);
            }
        }
    });

});
