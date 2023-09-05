//支付接口的类型
$dom.load.css([$dom.pagepath() + 'Components/Styles/modifybox.css']);
Vue.component('modifybox', {
    //entity:接口的对象实体 
    props: ['entity', 'formref', 'loading'],
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
    },
    methods: {
        btnEnter: function (isclose) {
            var th = this;
            th.$parent.$refs[th.formref].validate((valid) => {
                if (valid) {
                    var obj = $api.clone(th.entity);
                    th.$emit('enter', obj, isclose, th.operateSuccess);
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
        <slot></slot>
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