//试题编辑中的知识点选择
Vue.component('knowledge', {
    props: ["question", "organ"],
    data: function () {
        return {
            //知识分类下拉选择器的配置项
            defaultSortsProps: {
                children: 'children',
                label: 'Kns_Name',
                value: 'Kns_UID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            couid: 0,
            sorts: [],       //知识分类
            sortuid: [],
            form: { 'couid': '', 'uid': '', 'isuse': true, 'search': '', 'size': 20, 'index': 1 },

            knl_uid: '',            //当前选中的知识的id
            knowledge: null,       //当前选中的知识
            loading_knl: false,

            datas: [],          //知识点列表
            total: 0,
            loading: false,

            dialogVisible: false,        //显示知识点详情的面板
            showKnowledge: {}            //当前要显示的知识点
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                if (!!nv.Cou_ID) this.couid = nv.Cou_ID;
                if (!!nv.Kn_Uid) this.knl_uid = nv.Kn_Uid;
            }, immediate: true, deep: true
        },
        'couid': {
            handler: function (nv, ov) {
                this.form.couid = nv;
                this.getsorts();
            }, immediate: true
        },
        'knl_uid': {
            handler: function (nv, ov) {
                if (nv != '') this.getKnl(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/knowledge.css']);
    },
    methods: {
        //获取知识分类
        getsorts: function () {
            var th = this;
            if (th.couid == 0) {
                th.sorts = [];
                return;
            }
            $api.get('Knowledge/SortTree', { 'couid': th.couid, 'search': '', 'isuse': true }).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    th.sorts = result;
                    th.handleCurrentChange(1);
                } else {
                    th.sorts = [];
                    th.handleCurrentChange(1);
                }
            }).catch(function (err) {
                th.sorts = [];
                th.handleCurrentChange(1);
                console.error(err);
            });
        },
        //获取单个知识点
        getKnl: function (nv) {
            var th = this;
            th.loading_knl = true;
            $api.get('Knowledge/ForUID', { 'uid': th.knl_uid }).then(function (req) {
                th.loading_knl = false;
                if (req.data.success) {
                    th.knowledge = req.data.result;
                } else {
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading_knl = false;
                th.knowledge = null;
                console.error(err);
            });
        },
        //当知识点分类变更时,获取列表
        getList: function (data) {
            var sortid = data.length < 1 ? '' : data[data.length - 1];
            this.form.uid = sortid;
            //console.log(sortid);
        },
        handleCurrentChange: function (index) {
            if (index != null) this.form.index = index;
            var th = this;
            //每页多少条，通过界面高度自动计算
            var area = $dom('.knl_list').height();
            area=area>0 ? area : $dom('body').height() - 130;
            th.form.size = Math.floor(Number(area) / 35);
            th.form.size = th.form.size < 1 ? 1 : th.form.size;
            th.loading = true;
            console.log(th.form);
            $api.get("Knowledge/Pager", th.form).then(function (d) {
                th.loading = false;
                if (d.data.success) {
                    var result = d.data.result;
                    th.datas = result;
                    th.totalpages = Number(d.data.totalpages);
                    th.total = d.data.total;
                } else {
                    throw d.data.message;
                }
            }).catch(function (err) {
                th.$alert(err, '错误');
                th.loading = false;
                console.error(err);
            });
        },
        //设置知识点与试题的关联
        setcontact: function (knl) {
            //if(knl==null)this.knowledge = null;
            this.question.Kn_Uid = knl == null ? '' : knl.Kn_Uid;
            this.knl_uid = this.question.Kn_Uid;
            if (this.knl_uid == '') this.knowledge = null;
        },
        //显示知识点详情
        showKnl: function (knl) {
            this.showKnowledge = knl;
            this.dialogVisible = true;
        }
    },
    template: `<div class="knowledge">
        <div :class="knowledge ? 'selected' : 'knl_noselected'">
            <icon>&#xe84d</icon>
            <template v-if="knowledge">               
                <span @click="showKnl(knowledge)">{{knowledge ? knowledge.Kn_Title : ''}} </span>
                <el-link type="danger" icon="el-icon-delete" @click="setcontact(null)">清除</el-link>
            </template>
            <span v-else>没有关联的知识点</span>
        </div>
        <div class="search">
            <el-cascader clearable v-model="sortuid" placeholder="请选择知识分类"
                :options="sorts" separator="／" :props="defaultSortsProps" filterable @change="getList">
                <template slot-scope="{ node, data }">
                    <span>{{ data.Kns_Name }}</span>    
                </template>
            </el-cascader>  
            <el-input v-model.trim="form.search" clearable style="width:245px" placeholder="搜索"></el-input>
            <el-button type="primary" v-on:click="handleCurrentChange(1)" :loading="loading"
                native-type="submit" plain class="el-icon-search">
                查询
            </el-button>
        </div>      
        <dl class="knl_list">
            <loading v-if="loading"></loading>
            <dd v-for="(knl,index) in datas" v-else-if="datas.length>0">
                <index>{{index+1}}</index>
                <a href="#" @click="showKnl(knl)">{{knl.Kn_Title}}</a>
                <el-popconfirm  title="关联该知识点？" @confirm="setcontact(knl)">
                    <el-link type="primary" slot="reference"><icon>&#xa029</icon>关联知识点</el-link>
                </el-popconfirm>
            </dd>
            <dt v-else>没有可选择的知识点</dt>
        </dl>
        <el-dialog  :title="showKnowledge.Kn_Title" :visible.sync="dialogVisible" class="showKnowledge">
            <span slot="title"><icon>&#xe84d</icon>{{showKnowledge.Kn_Title}}</span>
            <span v-html="showKnowledge.Kn_Details"></span>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="setcontact(showKnowledge)"><icon>&#xa029</icon>关联知识点</el-button>
            </span>
        </el-dialog>
        <div class="knl-pager-box">
            <el-pagination v-on:current-change="handleCurrentChange"  :disabled="loading" :current-page="form.index" :page-sizes="[1]"
                :page-size="form.size" :pager-count="12" layout="total, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>
    </div> `
});