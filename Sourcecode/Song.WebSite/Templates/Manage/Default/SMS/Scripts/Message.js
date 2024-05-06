
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            mark: $api.querystring('mark'),
            entity: { text: '' },      //当前接口对象
            format_text: '',         //处理后的实际效果
            organ: {},  //当前机构
            platinfo: {},        //平台信息                    
            rules: {
                text: [{ required: true, message: '不得为空', trigger: 'blur' }]
            },
            loading: false
        },
        watch: {
            'entity.text': {
                handler: function (nv, ov) {
                    if (nv == null || nv == '') return;
                    var pattern = /\{\w[^\}]+\}/gi;
                    var arr = nv.match(pattern);
                    if (arr.length < 1) return;

                    this.format_text = nv;
                    for (let i = 0; i < arr.length; i++)
                        this.format_text = this.replace(arr[i].toLowerCase(), this.format_text);
                }, immediate: true
            }
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Sms/getItem', { 'mark': th.mark }),
                $api.get('Sms/TemplateSMS', { 'mark': th.mark }),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(([item, template, plat, organ]) => {
                //获取结果
                th.entity = item.data.result;
                //th.entity.text = template.data.result;
                th.$set(th.entity, 'text', template.data.result);
                th.platinfo = plat.data.result;
                th.organ = organ.data.result;
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        computed: {

        },
        methods: {
            //替换，参数：被替换的标签，文本
            replace: function (tag, text) {
                if (tag == "{vcode}") {
                    var rnd = Math.floor(Math.random(0, 9999) * 10000);
                    while (rnd < 1000) rnd *= 10;
                    return text.replace(tag, rnd);
                }
                if (tag == "{plate}") return text.replace(tag, this.platinfo.title);
                if (tag == "{org}") return text.replace(tag, this.organ.Org_Name);
                if (tag == "{date}") return text.replace(tag, new Date().format('yyyy-M-dd'));
                if (tag == "{time}") return text.replace(tag, new Date().format('HH:mm:ss'));
                return text;
            },
            btnEnter: function (formName) {
                var th = this;
                th.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.loading = true;
                        $api.post('Sms/TemplateUpdate', { 'mark': th.mark, 'msg': th.entity.text }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.operateSuccess();
                            } else {
                                throw req.data.message;
                            }
                        }).catch(err => alert(err))
                            .finally(() => th.loading = false);
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //操作成功
            operateSuccess: function () {
                if (window.top.$pagebox && window.top.$pagebox.source)
                    window.top.$pagebox.source.tab(window.name, 'vapp.loadDatas', true);
            }
        },
    });

});
