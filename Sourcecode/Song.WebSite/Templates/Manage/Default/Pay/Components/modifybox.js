//支付接口的类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/modifybox.css']);
Vue.component('modifybox', {
    //entity:接口的对象实体 
    props: ['entity', 'config', 'rules', 'loading'],
    data: function () {
        return {
            id: $api.querystring('id'),
        }
    },
    watch: {
        'entity': {
            handler: function (nv, ov) {
                if (nv == null) return;
            }, immediate: true
        }
    },
    computed: {
        //是否新增账号
        isadd: t => t.id == null || t.id == '' || this.id == 0
    },
    mounted: function () {
        var th = this;
        new Promise((resolve, reject) => {
            if (th.id == '') return resolve(th.entity);
            th.loading = true;
            $api.get('Pay/ForID', { 'id': th.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    resolve(req.data.result);
                } else {
                    reject(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }).then(function (ent) {
            th.entity = ent;
            if (!$api.isnull(th.entity.Pai_Config))
                th.config = $api.xmlconfig.tojson(th.entity.Pai_Config);
            else th.config = {};
            th.$emit('init', th.entity, th.config);
        });
    },
    methods: {
        //获取实体
        getEntity: function () {
            var th = this;
            return new Promise((resolve, reject) => {
                if (th.id == '') return resolve(th.entity);
                th.loading = true;
                $api.get('Pay/ForID', { 'id': th.id }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        resolve(req.data.result);
                    } else {
                        reject(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            });
        },
        //确认操作
        btnEnter: function (isclose) {
            var th = this;
            th.$refs['entity'].validate((valid) => {
                if (valid) {
                    var obj = $api.clone(th.entity);
                    obj.Pai_Config = $api.xmlconfig.toxml(th.config);
                    var apipath = 'Pay/' + (th.id == '' ? api = 'add' : 'Modify');
                    th.loading = true;
                    $api.post(apipath, { 'entity': obj }).then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                type: 'success', center: true,
                                message: '操作成功!',
                            });
                            th.operateSuccess(isclose);
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
        operateSuccess: function (isclose) {
            window.top.$pagebox.source.tab(window.name, 'vapp.handleCurrentChange', isclose);
        }
    },
    template: `<div>
        <el-form ref="entity" :model="entity" :rules="rules" @submit.native.prevent label-width="100px">
            <slot></slot>
        </el-form>
         <div class="footer">
            <el-button type="primary" native-type="submit" :loading="loading" plain @click="btnEnter(true)">
                <icon v-if="!loading">&#xa048</icon>保存
            </el-button>
            <el-button v-if="!isadd" type="primary" define="apply" native-type="submit" :loading="loading" plain
                @click="btnEnter(false)">应用
            </el-button>
            <el-button type='close'>
                取消
            </el-button>
        </div>
    </div>`
});