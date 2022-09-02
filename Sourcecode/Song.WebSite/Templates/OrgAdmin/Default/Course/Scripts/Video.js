$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('couid'),
            olid: $api.querystring('olid'),
            uid: $api.querystring('uid'),
            tabs: [
                { name: '当前服务器的视频', tab: 'video', icon: '&#xe6bf', disabled: false },
                { name: '站外视频地址', tab: 'outer', icon: '&#xa029', disabled: false },
                { name: '视频平台链接', tab: 'other', icon: '&#xe609', disabled: false }
            ],
            tabName: 'video',      //顶部选项卡的示默认项


            filesize: 0,             //当前上传文件的大小
            accessory: {},           //视频附件的对象
            isouter: false,          //视频是否是外部网站的链接
            //外部链接的录入校验
            rules_outer: {
                As_FileName: [{ required: true, message: '不得为空', trigger: 'blur' },
                {
                    validator: function (rule, value, callback) {
                        if (value == '') return callback();
                        var url = encodeURI(value);
                        if (!$api.url.check(url)) {
                            callback(new Error('请输入有效的网络地址'));
                        } else {
                            return callback();
                        }
                    }, trigger: ["change"]
                }
                ]
            },
            loading: false,
            loading_up: false        //上传中的预载

        },
        mounted: function () {
            this.getaccessory();
        },
        created: function () {

        },
        computed: {
            //是否存在记录
            isexist: function () {
                return JSON.stringify(this.accessory) != '{}' && this.accessory != null;
            }
        },
        watch: {
            //选项卡切换
            'tabName': function (nv, ov) {
                if (nv != 'video') this.pause();
            },
            //视频附件对象变化
            'accessory': {
                handler: function (nv, ov) {
                    //console.log('变化了');
                }, deep: true
            }
        },
        methods: {
            //获取视频附件对象
            getaccessory: function () {
                var th = this;
                $api.get('Accessory/ForUID', { 'uid': th.uid, 'type': 'CourseVideo' }).then(function (req) {
                    if (req.data.success) {
                        th.accessory = req.data.result;
                        th.initState();
                        if (th.tabName == 'video' || th.tabName == 'outer')
                            th.createplayer(th.accessory.As_FileName);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    th.accessory = {};
                    th.initState();
                    th.createplayer('');
                    console.error(err);
                });
            },
            //计算初始状态
            initState: function () {
                if (!this.isexist) {
                    for (let i = 0; i < this.tabs.length; i++)
                        this.tabs[i].disabled = false;
                    return;
                }
                var data = this.accessory;
                //本地服务器视频
                if (!data.As_IsOuter && !data.As_IsOther) {
                    this.tabName = 'video';
                } else {
                    if (data.As_IsOuter && data.As_IsOther) this.tabName = 'other';
                    else this.tabName = 'outer';
                }
                for (let i = 0; i < this.tabs.length; i++)
                    this.tabs[i].disabled = this.tabs[i].tab != this.tabName;
            },
            //开始上传
            upload_start: function (file) {
                this.loading_up = true;
                this.filesize = file.size;     //文件大小
                console.log(file);
            },
            //上传完成的方法
            upload_success: function (file) {
                file['size'] = this.filesize;
                console.log(file);
                var th = this;
                $api.post('Accessory/SaveOutlineVideoFile',
                    { 'olid': th.olid, 'type': file.pathkey, 'fileinfo': file })
                    .then(function (req) {
                        th.loading_up = false;
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                message: '上传成功',
                                type: 'success'
                            });
                            th.getaccessory();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading_up = false;
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
            },
            //播放器是否准备好
            playready: function () {
                var player = window.video_player;
                if (player != null && player.engine) return player._isReady;
                return false;
            },
            //视频播放
            play: function () {
                if (this.playready())
                    window.video_player.play();
            },
            //视频暂停
            pause: function () {
                if (this.playready())
                    window.video_player.pause();
            },
            //创建播放器
            createplayer: function (urlVideo) {
                if (window.video_player != null) {
                    window.video_player.destroy();
                    $dom("#videoplayer").html("");
                }
                if (urlVideo != null && urlVideo != '') {
                    window.video_player = new QPlayer({
                        url: urlVideo,
                        container: document.getElementById("videoplayer"),
                        autoplay: true,
                    });
                    window.video_player.on("ready", function (total) {
                        //视频总时长，单位：秒
                        var duration = Math.floor(total);
                        var vapp = window.vapp;
                        vapp.pause();
                        if (vapp.accessory['As_Duration'] !== undefined && vapp.accessory.As_Duration != duration) {
                            vapp.accessory['As_Duration'] = duration;
                            $api.post('Accessory/Modify', { 'entity': vapp.accessory }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(function (err) {
                                Vue.prototype.$alert(err);
                                console.error(err);
                            });
                            console.log(duration);
                        }
                    });
                    return window.video_player;
                } else {

                }
            },
            //选择服务器端的文件
            selectfile: function (file) {
                this.$refs['videos'].hide();
                console.log(file);
                var th = this;
                file = $api.clone(file);
                file['name'] = file['filename'] = file.name;
                $api.post('Accessory/SelectOutlineVideoFile',
                    { 'olid': th.olid, 'type': 'CourseVideo', 'fileinfo': file })
                    .then(function (req) {
                        th.loading_up = false;
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                message: '操作成功',
                                type: 'success'
                            });
                            window.setTimeout(function () {
                                th.getaccessory();
                            }, 600);
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading_up = false;
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });

            },
            //删除视频附件
            deleteVideo: function () {

                this.$confirm('是否清除当前章节的视频资料?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    dangerouslyUseHTMLString: true,
                    type: 'warning'
                }).then(() => {
                    var th = this;
                    $api.delete('Accessory/DeleteForUID', { 'uid': th.uid, 'type': 'CourseVideo' }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.getaccessory();
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        Vue.prototype.$alert(err);
                        console.error(err);
                    });
                }).catch(() => { });
            },
            //保存为其它视频平台地址
            btnSaveOther: function (formName) {
                var th = this;
                var form = this.$refs[formName];
                form.clearValidate();
                form.validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var url = th.accessory.As_FileName;
                        $api.post('Accessory/SaveOtherVideo', { 'uid': th.uid, 'type': 'CourseVideo', 'url': url, 'outer': true, 'other': true })
                            .then(function (req) {
                                th.loading = false;
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.getaccessory();
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(function (err) {
                                th.loading = false;
                                Vue.prototype.$alert(err);
                                console.error(err);
                            });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //保存为外部视频链接
            btnSaveOuter: function (formName) {
                var th = this;
                var form = this.$refs[formName];
                form.clearValidate();
                form.validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        var url = th.accessory.As_FileName;
                        if (url.indexOf('%') > -1) url = decodeURIComponent(url);
                        $api.post('Accessory/SaveOtherVideo', { 'uid': th.uid, 'type': 'CourseVideo', 'url': url, 'outer': true, 'other': false })
                            .then(function (req) {
                                th.loading = false;
                                if (req.data.success) {
                                    var result = req.data.result;
                                    th.$message({
                                        message: '操作成功',
                                        type: 'success'
                                    });
                                    th.getaccessory();
                                } else {
                                    console.error(req.data.exception);
                                    throw req.config.way + ' ' + req.data.message;
                                }
                            }).catch(function (err) {
                                th.loading = false;
                                Vue.prototype.$alert(err);
                                console.error(err);
                            });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
        }
    });

}, ["Components/video_list.js",
    "/Utilities/Components/upload-chunked.js",
    '/Utilities/Qiniuyun/qiniu-web-player-1.2.3.js']);
