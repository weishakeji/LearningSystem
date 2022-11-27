$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},
            fileList: [
                {
                    name: 'food.jpeg',
                    url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
                }, {
                    name: 'food2.jpeg',
                    url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100'
                }],
            loading_init: true
        },

        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
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
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            handleRemove(file, fileList) {
                console.log(file, fileList);
            },
            handlePreview(file) {
                console.log(file);
            },
            handleExceed(files, fileList) {
                this.$message.warning(`当前限制选择 3 个文件，本次选择了 ${files.length} 个文件，共选择了 ${files.length + fileList.length} 个文件`);
            },
            beforeRemove(file, fileList) {
                return this.$confirm(`确定移除 ${file.name}？`);
            },
            uploadSectionFile(param) {

                var file = param.file;    //需要分片的文件
                var name = file.name;       //文件名
                var size = file.size;        //文件大小
                var chunkedArr = [];      //分片后的数组
                const chunkSize = 10 * 1024 * 1024; // 1MB一片
                const chunkCount = Math.ceil(size / chunkSize); // 总片数
                //var uid=
                for (var i = 0; i < chunkCount; i++) {
                    //计算每一片的起始与结束位置
                    var start = i * chunkSize, end = Math.min(size, start + chunkSize);
                    var chunk = file.slice(start, end);

                    var uid = $api.md5(name + file.size);
                    console.log(uid);
                    //var form = new FormData();
                    //form.append("data", file.slice(start, end));
                    /*
                                        var form = new FormData();
                                        form.append("data", file.slice(start, end));
                                        form.append("path", "CourseVideo");
                                        form.append("filename", name);
                                        form.append("total", chunkCount);  //总片数
                                        form.append("index", i + 1);        //当前是第几片
                                        form.append("uid", file.uid);        //当前是第几片
                                        chunkedArr.push(form);
                    */
                    var obj = {
                        "file": file.slice(start, end),
                        "path": "Video",
                        "filename": name,
                        "total": chunkCount,
                        "index": i + 1,
                        "uid": uid
                    };
                    chunkedArr.push(obj);
                }
                var fail = 0;
                for (let i = 0; i < chunkedArr.length; i++) {
                    var form = chunkedArr[i];
                    $api.post('Upload/Chunked', form).then(function (req) {
                        if (req == null) {
                            fail++;
                            console.log("失败：" + fail);
                        }
                        if (req.data.success) {
                            var result = req.data.result;
                            //console.log(result);
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
                /*
                for (let i = 0; i < chunkedArr.length; i++) {
                    $.ajax({
                        url: "/Upload/Chunked",
                        type: "POST",
                        data: chunkedArr[i],
                        async: true,        //异步
                        processData: false,  //很重要，告诉jquery不要对form进行处理
                        contentType: false,  //很重要，指定为false才能形成正确的Content-Type
                        success: function (r) {
                            console.log(r);
                        }

                    });
                }*/
                return;
                var that = this;
                form.append('file', param.file)
                form.append('dir', 'temp1')
                that.$axios.post('http://192.168.1.65/upload', form, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    },
                    onUploadProgress: progressEvent => {
                        that.uploadPercent = (progressEvent.loaded / progressEvent.total * 100) | 0
                    }
                }).then((res) => {
                    console.log('上传结束')
                    console.log(res)
                }).catch((err) => {
                    console.log('上传错误')
                    console.log(err)
                })
            },

        }
    });

}, ["/Utilities/Components/upload-chunked.js"]);
