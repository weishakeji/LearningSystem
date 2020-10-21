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
            No_interval: []
        },
        details: '',
        activeName: 'tab02',
        interval: '',
        rules: {
            No_Ttl: [
                { required: true, message: '标题不得为空', trigger: 'blur' }
            ],
            No_OpenCount: [
                { required: true, message: '不得为空', trigger: 'blur' },
                { pattern: /^[0-9]\d*$/, message: '请输入大于零的整数', trigger: 'blur' }
            ],
            SI_Timespan: [
                { required: true, message: '工时信息不得为空', trigger: 'blur' },
                { pattern: /^([1-9]\d*)|([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$/, message: '请输入大于零的数字', trigger: 'blur' }
            ]
        },
        loading: false

    },
    watch: {
    },
    created: function () { },
    methods: {
        //详情输入框更改时
        updateDetails: function (data) {
            this.formData.No_Context = data;
        },
        btnEnter: function (formName) { },
        handleAvatarSuccess: function (res, file) {
            if (file.status == "success") {
                this.account.Acc_Photo = res.url;
                //this.btnEnter('account');                
            }
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
            return isJPG && isLt2M;
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
            if (this.formData.No_interval == '' || this.formData.No_interval == null) this.formData.No_interval = [];
            //验证重复
            var exist = false;
            var arr = this.formData.No_interval;
            for (var i = 0; i < arr.length; i++) {
                if (arr[i]['start'] == start && arr[i]['end'] == end) {
                    exist = true;
                    break;
                }
            }
            if (!exist) {
                this.formData.No_interval.push({
                    'start': start,
                    'end': end
                });
            }
            this.formData.No_interval = this.formData.No_interval.sort(function (a, b) {
                return a.start - b.start
            })
            return true;
        },       
        timeformat: function (time, fmt) {
            return time.format(fmt);
        }
    }
});