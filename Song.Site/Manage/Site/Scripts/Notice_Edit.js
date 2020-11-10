Vue.use(VueHtml5Editor, {
    showModuleName: true,
    image: {
        sizeLimit: 512 * 1024,
        compress: true,
        width: 500,
        height: 400,
        quality: 80
    }
});
window.vapp = new Vue({
    el: '#app',
    data: {
        id: $api.querystring('id'),
        //当前实体
        formData: {
            No_IsShow: true,
            No_Type: 1,
            No_Page: 'mobi_home',
            No_Range: 1,
            No_Interval: '',
            No_Timespan: 6,
            No_OpenCount: 1,
            No_StudentSort: '',
            No_BgImage: ''
        },
        details: '',
        activeName: 'tab01',
        interval: '',       //有效时间段的添加时的值
        No_Interval: [],     //有效时间段的临时数组
        accountSort: [],     //学员账号分组
        No_StudentSort: [],      //选中的学员账号分组
        imgLimit: ['jpg', 'png', 'gif'],     //上传图片的类型限制
        scale: true,         //等比例缩放
        imgWidth: 0,
        imgHeight: 0,
        rules: {
            No_Ttl: [
                { required: true, message: '标题不得为空', trigger: 'blur' }
            ],
            No_OpenCount: [
                { required: true, message: '不得为空', trigger: 'blur' },
                { pattern: /^[0-9]\d*$/, message: '请输入大于零的整数', trigger: 'blur' }
            ]
        },
        testPhone: '',       //用于测试短信的手机号
        loading: false

    },
    watch: {
        No_Interval: function (nl, ol) {
            var arr = [];
            for (var i = 0; i < this.No_Interval.length; i++) {
                var item = this.No_Interval[i];
                arr.push({
                    'start': item.start.format('HH:mm'),
                    'end': item.end.format('HH:mm'),
                });
            }
            this.formData.No_Interval = JSON.stringify(arr);
        },
        No_StudentSort: function (nl, ol) {
            this.formData.No_StudentSort = JSON.stringify(this.No_StudentSort);
        },
        No_Context: function (nl, ol) {
            this.formData.No_Context = nl;
        }
    },
    computed: {
        //是否是手机端
        ismoblie: function () {
            var str = this.formData.No_Page;
            var prefix = '';
            if (str.indexOf('_'))
                prefix = str.substring(0, str.indexOf('_'));
            return prefix == 'mobi';
        }
    },
    created: function () {
        var th = this;
        th.id = $api.querystring('id');
        $api.get('Account/SortPager', { 'index': '1', 'size': '99999' }).then(function (req) {
            if (req.data.success) {
                var results = req.data.result;
                results.forEach(function (item, index) {
                    vapp.accountSort.push({
                        label: item.Sts_Name,
                        key: item.Sts_ID,
                        index: index
                    });
                });
                if (th.id == '') return;
                $api.get('Notice/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.formData = result;
                        vapp.details = vapp.formData.No_Context;
                        vapp.imgWidth = vapp.formData.No_Width;
                        vapp.imgHeight = vapp.formData.No_Height;
                        if (vapp.formData.No_Page == '') vapp.formData.No_Page = 'mobi_home';
                        //时间段的初始化
                        if (vapp.formData.No_Interval != '') {
                            var interval = JSON.parse(vapp.formData.No_Interval);
                            for (var i = 0; i < interval.length; i++) {
                                interval[i]['start'] = Date.parse(interval[i]['start']);
                                interval[i]['end'] = Date.parse(interval[i]['end']);
                            }
                            vapp.No_Interval = interval;
                        }
                        //学员分组信息
                        if (vapp.formData.No_StudentSort != '')
                            th.No_StudentSort = JSON.parse(vapp.formData.No_StudentSort);
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            } else {
                throw req.data.message;
            }
            //其它数据，随机学员、机构信息，用于短信的转义

        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    },
    methods: {
        //详情输入框更改时
        updateDetails: function (data) {
            if (data != null) this.formData.No_Context = data;
            //当是短信时
            if (this.formData.No_Type == 3) {
                var txt = this.formData.No_Context;
                var date = (new Date()).format('yyyy-MM-dd');
                txt = txt.replace(new RegExp("{date}"), date)
                return txt;
            }
            return data;
        },
        btnEnter: function (formName) {
            var th = this;
            this.$refs[formName].validate(function (valid) {
                if (valid) {
                    var apipath = 'Notice/' + (th.id == '' ? 'add' : 'Modify');
                    $api.post(apipath, { 'entity': vapp.formData }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            vapp.$message({
                                type: 'success',
                                message: '操作成功!',
                                center: true
                            });
                            //vue.operateSuccess();
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        //window.top.ELEMENT.MessageBox(err, '错误');
                        vapp.$alert(err, '错误');
                    });
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        //图片上传
        beforeAvatarUpload: function (file) {
            return false;
        },
        imgChange: function (file, fileList) {
            //后缀名限制
            var suffix = file.name.indexOf('.') > -1 ? file.name.substring(file.name.lastIndexOf('.') + 1) : '';
            if (this.imgLimit.indexOf(suffix.toLowerCase()) < 0) {
                this.$message.error('上传图片只能是 ' + this.imgLimit.join('、') + ' 格式!');
                return false;
            }
            if (file.size / 1024 / 1024 > 2) {
                this.$message.error('上传图片大小不能超过 2MB!');
                return false;
            }
            var th = this;
            this.getBase64(file.raw).then(function (res) {
                th.formData.No_BgImage = res;
                th.loading = true;
                window.setTimeout(function () {
                    th.loading = false;
                    //自动生成图片宽度，实现等比例缩放，以免图像失真
                    var img = document.getElementById('BgImage');
                    var width = img.naturalWidth;
                    var height = img.naturalHeight;
                    if (th.ismoblie) {
                        //手机端
                        var def = 70;
                        var wp = Math.floor(width / height * 16 / 9 * def);
                        var hp = Math.floor(height / width * def * 9 / 16);
                        th.formData.No_Width = wp >= hp ? def : wp;
                        th.formData.No_Height = wp >= hp ? hp : def;
                    } else {
                        //电脑端，max为最大宽高
                        var max = 600, percent = 0;
                        percent = max / width > max / height ? max / height : max / width;
                        th.formData.No_Width = width < max ? width : Math.floor(percent * width);
                        th.formData.No_Height = width < max ? height : Math.floor(percent * height);
                    }
                    th.imgWidth = th.formData.No_Width;
                    th.imgHeight = th.formData.No_Height;
                }, 500);
            });
        },
        getBase64: function (file) {
            return new Promise(function (resolve, reject) {
                let reader = new FileReader();
                let imgResult = "";
                reader.readAsDataURL(file);
                reader.onload = function () {
                    imgResult = reader.result;
                };
                reader.onerror = function (error) {
                    reject(error);
                };
                reader.onloadend = function () {
                    resolve(imgResult);
                };
            });
        },
        imgWidthChange: function (val) {
            if (this.scale) {
                this.imgHeight = Math.floor(val / this.formData.No_Width * this.imgHeight);
                this.formData.No_Height = this.imgHeight;
            }
            this.formData.No_Width = val;
        },
        imgHeightChange: function (val) {
            if (this.scale) {
                this.imgWidth = Math.floor(val / this.formData.No_Height * this.imgWidth);
                this.formData.No_Width = this.imgWidth;
            }
            this.formData.No_Height = val;
        },
        //起始或终止时间更改时
        changeTime: function () {
            var start = this.formData.No_StartTime;
            var end = this.formData.No_EndTime;
            if (end < start) {
                var msg = "结束时间不能小于开始时间";
                this.$alert(msg, '提示', {
                    confirmButtonText: '确定',
                    callback: function (action) { }
                });
                return false;
            }
            return true;
        },
        //添加时间段
        addInterval: function () {
            var type = $api.getType(this.interval);
            if (type != 'Array') return false;
            //添加
            var start = Date.parse(this.interval[0]);
            var end = Date.parse(this.interval[1]);
            if (this.No_Interval == '' || this.No_Interval == null) this.No_Interval = [];
            //验证重复
            var exist = false;
            var arr = this.No_Interval;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]['start'].getTime() == start.getTime() &&
                    arr[i]['end'].getTime() == end.getTime()) {
                    exist = true;
                    break;
                }
            }
            if (!exist) {
                this.No_Interval.push({
                    'start': start,
                    'end': end
                });
            }
            this.No_Interval = this.No_Interval.sort(function (a, b) {
                return a.start - b.start
            })
            return true;
        },
        timeformat: function (time, fmt) {
            return time.format(fmt);
        },
        //关闭窗体
        btnClose: function () {
            new top.PageBox().Close(window.name);
        }
    }
});