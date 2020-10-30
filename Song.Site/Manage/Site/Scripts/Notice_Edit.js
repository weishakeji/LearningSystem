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
            No_IsOpen: false,
            No_Page: 'mobi_home',
            No_Range: 1,
            No_Interval: '',
            No_Timespan: 6,
            No_OpenCount: 1,
            No_StudentSort: ''            
        },
        details: '',
        activeName: 'tab01',
        interval: '',       //有效时间的临时值
        No_Interval: [],     //有效时间的临时值
        accountSort: [],     //学员账号分组
        No_StudentSort: [],      //选中的学员账号分组
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
    created: function () {
        var th = this;
        th.id = $api.querystring('id');
        $api.get('Account/SortPager', { 'index': '1', 'size': '99999' }).then(function (req) {
            if (req.data.success) {
                var results = req.data.result;
                results.forEach((item, index) => {
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
                        //时间段的初始化
                        if (vapp.formData.No_Interval != '') {
                            var interval = JSON.parse(vapp.formData.No_Interval);
                            for (var i = 0; i < interval.length; i++) {
                                interval[i]['start'] = new Date('2020-01-01 ' + interval[i]['start']);
                                interval[i]['end'] = new Date('2020-01-01 ' + interval[i]['end']);
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
            if (this.No_Interval == '' || this.No_Interval == null) this.No_Interval = [];
            //验证重复
            var exist = false;
            var arr = this.No_Interval;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]['start'] == start && arr[i]['end'] == end) {
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