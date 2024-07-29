//章节的直播设置
//事件：save保存完成触发
Vue.component('outline_live', {
    props: [],
    data: function () {
        return {
            outline: {},
            showpanel: false,     //是否显示
            datas: [],       //附件列表
            rules: {},
            loading: false
        }
    },
    watch: {
        'outline': {
            handler: function (nv, ov) {
                //this.getPrices(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Course/Components/Styles/outline_live.css']);
    },
    methods: {
        //设置初始值
        setvalue: function (ol) {
            this.outline = ol;
            this.showpanel = true;
        },
        title_panel: function () {
            var name = this.outline ? this.outline.Ol_Name : '';
            return '直播设置 - ' + name;
        },
        btnEnter: function (formName) {
            var th = this;
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    th.loading = true;
                    $api.post('Outline/Modify', { 'entity': th.outline }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            var result = req.data.result;
                            th.$message({
                                type: 'success',
                                message: '操作成功!',
                                center: true
                            });
                            th.showpanel = false;
                            th.$emit('save', this.outline);
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                    });
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        }
    },
    template: `<el-dialog :visible.sync="showpanel" append-to-body custom-class="live_dialog"
    :show-close="true" :close-on-click-modal="false">
        <template slot="title">
            <icon>&#xe6bf</icon> {{title_panel()}}
        </template>
        <el-form ref="entity" :model="outline" :rules="rules" @submit.native.prevent label-width="120px">
            <el-form-item label="">
                <el-checkbox v-model="outline.Ol_IsLive">当前课程章节作为直播课</el-checkbox>
            </el-form-item>
            <el-form-item label="开始时间">
                <el-date-picker v-model="outline.Ol_LiveTime" type="datetime" placeholder="选择日期时间"></el-date-picker>
            </el-form-item>
            <el-form-item label="直播时长">
                <el-input v-model="outline.Ol_LiveSpan">
                    <template slot="prepend">计划直播</template>
                    <template slot="append">分钟</template>
                </el-input>
            </el-form-item>
            <el-divider></el-divider>
            <el-form-item label="推流地址">
               
            </el-form-item>
            <el-form-item label="HLS播放地址">
               
            </el-form-item>
            <el-form-item label="RTMP播放地址">
               
            </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
            <el-button type="primary" native-type="submit" :loading="loading" plain @click="btnEnter('entity')">
                <icon v-if="!loading">&#xa048</icon>保存
            </el-button>
            <el-button @click="()=> { showpanel = false;}">取 消</el-button>
        </div>
    </el-dialog> `
});