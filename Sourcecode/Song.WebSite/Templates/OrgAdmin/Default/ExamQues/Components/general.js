//试题编辑中的基本信息

Vue.component('general', {
    props: ["question", "org"],
    data: function () {
        return {
            //标签输入框
            taginput: '',
            tagShowInput: false,
            tagmax: 6,       //最多可输入的标签数量

            //试题关联的分类
            parts: [],

            loading: false
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                //如果是新增试题，总之难度小于零，默认为3
                if (!nv.Qus_Diff || nv.Qus_Diff <= 0) nv['Qus_Diff'] = 3;
                if (nv.Qus_Diff >= 5) nv.Qus_Diff = 5;
                //默认为启用状态
                if (!nv.Qus_IsUse) nv['Qus_IsUse'] = true;
                if (!nv.Qus_Order) nv['Qus_Order'] = 0;

            }, immediate: true,
        },
        'question.Qus_ID': {
            handler: function (nv, ov) {
                //获取试题的关键字
                if (nv != null && nv != 0) {
                    this.gettags();
                    this.getparts();
                }
            }, immediate: true,
        },

    },
    computed: {
        //试题关键字列表
        taglist: t => t.question.Tags,
        //试题的分类
        //parts: t => t.question.Parts,
    },
    mounted: function () {

    },
    methods: {
        //试题关键字
        gettags: function () {
            var th = this;
            if (th.loading || th.taglist != null) return;
            th.loading = true;
            $api.get("ExamQues/TagForQues", { "quesid": th.question.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        th.taglist = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //显示新增标签输入框
        showTagInput() {
            this.tagShowInput = true;
            this.$nextTick(_ => {
                this.$refs.taginput.$refs.input.focus();
            });
        },
        //编辑标签
        tagedit: function () {
            console.error(this.taginput);
            if (this.taginput == null || this.taginput.length < 1) return;
            let str = this.taginput.replace(/，/g, ",");
            str = str.replace(/\s/g, ",");
            str = str.replace(/,+/g, ",");
            if (str == null || str.length < 1) {
                this.tagShowInput = false;
                return;
            }
            let arr = str.split(",");
            if (this.taglist == null) this.taglist = [];
            for (let i = 0; i < arr.length; i++) {
                arr[i] = $api.trim(arr[i]);
                if (arr[i] == null || arr[i].length < 1) continue;
                if (!this.taglist.some(tag => tag.Qtag_Name == arr[i]))
                    this.taglist.push({ 'Qtag_Name': arr[i], 'Org_ID': this.org.Org_ID, 'Qtag_IsDeleted': false });
            }
            if (this.taglist.length > this.tagmax) {
                this.$message({
                    message: '最多可以添加' + this.tagmax + '个标签',
                    type: 'warning'
                });
                this.taglist.splice(this.tagmax)
            }
            this.tagShowInput = false;
            this.taginput = '';

        },
        //查询关键字
        tagquery: function (search, cb) {
            var th = this;
            $api.get("ExamQues/TagPager", { "orgid": th.question.Org_ID, "couid": 0, "isdeleted": false, "name": search, "size": 10, "index": 1 })
                .then(req => {
                    if (req.data.success) {
                        let result = req.data.result;
                        cb(result);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => { });
        },
        //选择下拉的关键字时
        selecttag: function (item) {
            if (!this.taglist.some(tag => tag.Qtag_Name == item.Qtag_Name))
                this.taglist.push(item);
            this.taginput = item.Qtag_Name;
            this.tagedit();
        },
        //获取当前试题的分类
        getparts: function () {
            var th = this;
            th.partloading = true;
            $api.get("ExamQues/PartForQues", { "qusid": th.question.Qus_ID })
                .then(req => {
                    if (req.data.success) {
                        th.parts = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                .finally(() => th.partloading = false);
        },

        updatepart: function (parts, partid) {
            this.parts = [];
            this.parts = parts;
            this.question.Parts = parts;
            //console.error(parts);
            //console.error(partid);
        }
    },
    template: `<div class="general">
        <el-form ref="question" :model="question" @submit.native.prevent label-width="80px">    
            <el-form-item label="难度" prop="Qus_Diff" class="form_diff">
                <el-rate title="点击难度值" v-model="question.Qus_Diff" :max="5"></el-rate>
                <ques_diff :ques="question"></ques_diff>
            </el-form-item>
            <el-form-item label="排序号" prop="Qus_Order" v-if="false">
                <el-input-number v-model="question.Qus_Order"></el-input-number>
                <help>数值越小越靠前，可以为负值</help>
            </el-form-item>
            <el-form-item label="" prop="Qus_IsUse">
                <el-switch  v-model="question.Qus_IsUse"  active-text="启用"  inactive-text="禁用"></el-switch>
            </el-form-item>         
            <el-form-item label="关键字" prop="taginput" class="taglist">               
                <el-tag type="warning" size="medium" v-for="(tag,idx) in taglist" closable @close="taglist.splice(idx, 1)"> {{tag.Qtag_Name}}</el-tag>
                <el-button v-if="!tagShowInput" class="button-new-tag" size="small" @click="showTagInput">+ 添加关键字</el-button>
                <el-autocomplete class="input-new-tag" clearable v-else v-model="taginput" ref="taginput"
                  :fetch-suggestions="tagquery"  placeholder="请输入内容"  @select="selecttag"
                  @keyup.enter.native="tagedit" @blur="tagedit">
                    <template slot-scope="{ item }">
                        <div class="name" v-html="showsearch(item.Qtag_Name,taginput)"></div>                       
                    </template>
                </el-autocomplete>               
            </el-form-item>   
            <el-form-item label="试题分类" prop="parts" class="parts-area">
                <selectpart :orgid="question.Org_ID" :parts="parts" @update="updatepart"></selectpart>              
            </el-form-item>    
        </el-form>
    </div> `
});