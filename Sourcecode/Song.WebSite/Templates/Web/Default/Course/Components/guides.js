//课程通知公告的列表
$dom.load.css([$dom.pagepath() + 'Components/Styles/guides.css']);
Vue.component('guides', {
    //课程id，分类uid
    props: ["couid", "gcuid"],
    data: function () {
        return {
            guides: [],             //课程公告
            form: { 'couid': '', 'uid': '', 'show': '', 'use': true, 'search': '', 'size': 10, 'index': 1 },
            total: 1, //总记录数
            totalpages: 1, //总页数
            loading: false,

            dtailShow: false,        //显示内容
            detailObj: {}
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                this.form.couid = this.couid;
                this.handleCurrentChange(1);
            },
            immediate: true
        },
        'gcuid': {
            handler: function (nv, ov) {
                this.form.uid = nv;
                this.handleCurrentChange(1);
            },
            immediate: false
        }
    },
    computed: {

    },
    mounted: function () {

    },
    methods: {
        handleCurrentChange: function (index) {
            if (index != null) this.form.index = index;
            var th = this;
            th.loading = true;
            //每页多少条，通过界面高度自动计算
            var area = document.documentElement.clientHeight - 100;
            th.form.size = Math.floor(area / 41);
            $api.get("Guide/Pager", th.form).then(function (d) {
                if (d.data.success) {
                    th.guides = d.data.result;
                    th.totalpages = Number(d.data.totalpages);
                    th.total = d.data.total;
                } else {
                    console.error(d.data.exception);
                    throw d.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
    },
    template: `<div class="weisha_guides">
    <template v-if="guides.length>0">
        <div v-for="(item,index) in guides" @click="dtailShow=true;detailObj=item;">
            <index>{{(form.index - 1) * form.size + index + 1}} . </index>{{item.Gu_Title}}
        </div>    
        <div id="pager-box">
            <el-pagination v-on:current-change="handleCurrentChange" :current-page="form.index" 
                :page-size="form.size" :pager-count="12" layout="total, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>
    </template>
    <div v-else class="guide_null">
        没有可查阅的信息
    </div>
    <el-dialog  :title="detailObj.Gu_Title"  :visible.sync="dtailShow" custom-class="guide_dialog" append-to-body>
        <div v-html="detailObj.Gu_TitleFull" v-if="detailObj.Gu_TitleFull!=''" class="Gu_TitleFull"></div>
        <template v-if="detailObj.Gu_Details!=''">
            <div v-html="detailObj.Gu_Details"  class="Gu_Details"></div>
            <div  v-if="detailObj.Gu_Source!=''">来源 ： {{detailObj.Gu_Source}}</div>
        </template>        
        <div v-else>(没有内容)</div>
    </el-dialog>
    </div>`
});

