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
            No_IsOpen: true,
            No_Page: '',
            No_Range: 1,
            No_interval: '',
            No_Timespan: 6,
            No_OpenCount: 1
        },
        details: '',
        activeName: 'tab02',
        interval: '',       //有效时间的临时值
        No_interval: [],     //有效时间的临时值
        accountSort: [],     //学员账号分组
        rules: {
            No_Ttl: [
                { required: true, message: '标题不得为空', trigger: 'blur' }
            ],
            No_OpenCount: [
                { required: true, message: '不得为空', trigger: 'blur' },
                { pattern: /^[0-9]\d*$/, message: '请输入大于零的整数', trigger: 'blur' }
            ]
        },
        loading: false

    },
    watch: {
        No_interval: function (nl, ol) {
            this.formData.No_interval = JSON.stringify(this.No_interval);
        }
    },
    created: function () {
        $api.get('Account/SortPager', { 'index': '1', 'size': '99999' }).then(function (req) {
            if (req.data.success) {
                var results = req.data.result;
                // vapp.accountSort.push();
                results.forEach((item, index) => {
                    vapp.accountSort.push({
                        label: item.Sts_Name,
                        key: item.Sts_ID,
                        index: index
                    });
                });
                //...
            } else {
                throw req.data.message;
            }
        }).catch(function (err) {
            alert(err);
            console.error(err);
        });
    },
    methods: {
        //详情输入框更改时
        updateDetails: function (data) {
            this.formData.No_Context = data;
        },
        btnEnter: function (formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    var apipath = 'Notice/' + (this.id == '' ? 'add' : 'Modify');
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

        beforeAvatarUpload: function (file) {
            //console.log(file);
            const isJPG = file.type === 'image/jpeg' || file.type === 'image/png';
            const isLt2M = file.size / 1024 / 1024 < 2;

            if (!isJPG) {
                this.$message.error('上传头像图片只能是 JPG 格式!');
            }
            if (!isLt2M) {
                this.$message.error('上传头像图片大小不能超过 2MB!');
            }
            return false;
            return isJPG && isLt2M;
        },
        imgChange: function (file, fileList) {
            var th = this;
            this.getBase64(file.raw).then(res => {
                th.formData.No_BgImage = res;
                th.loading = true;
                window.setTimeout(function () {
                    window.vapp.loading = false;
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

        //起始或终止时间更改时
        changeTime: function () {
            var start = this.formData.No_StartTime;
            var end = this.formData.No_EndTime;
            if (end < start) {
                var msg = "结束时间不能小于开始时间";
                this.$alert(msg, '提示', {
                    confirmButtonText: '确定',
                    callback: action => { }
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
            var start = this.interval[0];
            var end = this.interval[1];
            if (this.No_interval == '' || this.No_interval == null) this.No_interval = [];
            //验证重复
            var exist = false;
            var arr = this.No_interval;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]['start'] == start && arr[i]['end'] == end) {
                    exist = true;
                    break;
                }
            }
            if (!exist) {
                this.No_interval.push({
                    'start': start,
                    'end': end
                });
            }
            this.No_interval = this.No_interval.sort(function (a, b) {
                return a.start - b.start
            })
            return true;
        },
        timeformat: function (time, fmt) {
            return time.format(fmt);
        },

    }
});