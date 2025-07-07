$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
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
            activeName: 'tab01',
            interval: '',       //有效时间段的添加时的值
            No_Interval: [],     //有效时间段的临时数组
            accountSort: [],     //学员账号分组
            No_StudentSort: [],      //选中的学员账号分组

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

            //图片文件
            upfile: null, //本地上传文件的对象        
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
            },
            //图片宽度变化
            imgWidth: function (nv, ov) {

            },
            imgHeight: function (nv, ov) {

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
            th.loading = true;
            $api.get('Account/SortAll', { 'orgid': '-1', 'use': '' }).then(function (req) {
                if (req.data.success) {
                    var results = req.data.result;
                    results.forEach(function (item, index) {
                        th.accountSort.push({
                            label: item.Sts_Name,
                            key: item.Sts_ID,
                            index: index
                        });
                    });
                    if (th.id == '') {
                        $api.get('Snowflake/Generate').then(function (req) {
                            if (req.data.success) {
                                th.formData.No_Id = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        }).finally(() => th.loading = false);
                        return;
                    }
                    $api.get('Notice/ForID', { 'id': th.id }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.formData = result;
                            th.imgWidth = th.formData.No_Width;
                            th.imgHeight = th.formData.No_Height;
                            if (th.formData.No_Page == '') th.formData.No_Page = 'mobi_home';
                            //时间段的初始化
                            if (th.formData.No_Interval != '') {
                                var interval = JSON.parse(th.formData.No_Interval);
                                for (var i = 0; i < interval.length; i++) {
                                    interval[i]['start'] = Date.parse(interval[i]['start']);
                                    interval[i]['end'] = Date.parse(interval[i]['end']);
                                }
                                th.No_Interval = interval;
                            }
                            //学员分组信息
                            if (th.formData.No_StudentSort != '')
                                th.No_StudentSort = JSON.parse(th.formData.No_StudentSort);
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    }).finally(() => th.loading = false);
                } else {
                    throw req.data.message;
                }
                //其它数据，随机学员、机构信息，用于短信的转义

            }).catch(function (err) {
                th.loading = false;
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
            //应用
            btnApply: function (formName) {
                this.save(formName, false, '数据已经保存，可随时查看效果');
            },
            btnEnter: function (formName) {
                this.save(formName, true, '操作成功!正在关闭当前窗体。');
            },
            //保存数据的具体方法，参数依次说明：表单对象，是否关闭窗体，完成后的消息
            save: function (formName, isclose, message) {
                var th = this;
                this.$refs[formName].validate(function (valid) {
                    if (valid) {
                        th.loading=true;
                        var apipath = 'Notice/' + (th.id == '' ? 'add' : 'Modify');
                        $api.post(apipath, { 'entity': th.formData }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: message,
                                    center: true
                                });
                                th.operateSuccess(isclose);
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                        }).finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //图片文件上传
            filechange: function (file) {
                this.upfile = file;
                this.formData.No_BgImage = file.base64;
            },
            //清除图片
            fileremove: function () {
                this.upfile = null;
                this.formData.No_BgImage = '';
            },
            //图片加载完成
            fileload: function (e) {
                if (this.ismoblie) {

                } else {
                    this.formData.No_Width = this.imgWidth = e.width;
                    this.formData.No_Height = this.imgHeight = e.height;
                }
                console.log(e);
            },
            imgWidthChange: function (val) {
                if (this.scale) {
                    var width = isNaN(Number(this.imgWidth)) ? 0 : Number(this.imgWidth);
                    var height = isNaN(Number(this.imgHeight)) ? 0 : Number(this.imgHeight);
                    var t = isFinite(height);
                    this.imgHeight = height == 0 ? val : Math.floor(val / this.formData.No_Width * this.imgHeight);
                    this.formData.No_Height = this.imgHeight;
                }
                this.formData.No_Width = val;
            },
            imgHeightChange: function (val) {
                if (this.scale) {
                    var width = isNaN(Number(this.imgWidth)) ? 0 : Number(this.imgWidth);
                    var height = isNaN(Number(this.imgHeight)) ? 0 : Number(this.imgHeight);
                    this.imgWidth = width == 0 ? val : Math.floor(val / this.formData.No_Height * this.imgWidth);
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
            //操作成功
            operateSuccess: function (isclose) {
                window.top.$pagebox.source.tab(window.name, 'vapp.freshrow("' + this.id + '")', isclose);
            }
        }
    });
});
