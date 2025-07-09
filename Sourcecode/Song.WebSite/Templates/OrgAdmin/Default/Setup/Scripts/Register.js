$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项
            rules: {},
            loading: false
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Organization/Current')
            ).then(([organ]) => {
                //获取结果             
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        created: function () { },
        computed: {},
        watch: {},
        methods: {
            btnEnter: function (formName) {
                //仅保留当前页要修改的字段
                let fields = this.$refs[formName].fields;
                let data = {};      //          
                for (let i = 0; i < fields.length; i++)
                    data[fields[i].prop] = this.config[fields[i].prop];
                //保存
                var th = this;
                if (th.loading) return;
                th.loading = true;
                $api.post('Organization/ConfigUpdate', {
                    "orgid": th.organ.Org_ID, 'config': data
                }).then(function (req) {
                    if (req.data.success && req.data.result) {
                        th.$message({
                            type: 'success', center: true,
                            message: '操作成功!'
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(err => alert(err, '错误'))
                    .finally(() => th.loading = false);
            },
        }
    });

});
