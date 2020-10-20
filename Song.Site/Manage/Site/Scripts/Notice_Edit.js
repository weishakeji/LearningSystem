Vue.use(VueHtml5Editor, {
    showModuleName: true,
    image: {
        sizeLimit: 512 * 1024,
        compress: true,
        width: 500,
        height: 500,
        quality: 80
    }
});
window.vapp = new Vue({
    el: '#app',
    data: {
        id: $api.querystring('id'),
        //当前实体
        formData: {
            No_IsOpen: false,
            No_Page: '',
            No_Range: 1
        },
        details: '',
        activeName: 'tab02',
        rules: {
            No_Ttl: [
                { required: true, message: '标题不得为空', trigger: 'blur' }
            ],
            SI_Price: [
                { required: true, message: '价格不得为空', trigger: 'blur' },
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
    }
});