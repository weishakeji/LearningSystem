//课程试题练习的通过率
$dom.load.css([$dom.pagepath() + 'Components/Styles/ques_progress.css']);
Vue.component('ques_progress', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    //purchase:购买记录
    props: ['course', 'stid', 'config', 'purchase'],
    data: function () {
        return {
            data: {},        //进度信息
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            loading: false
        }
    },
    watch: {
        'purchase': {
            handler: function (nv, ov) {
                this.percent = this.ispurchase ? nv.Stc_QuesScore : 0;
            }, immediate: true
        }
    },
    computed: {
        'color': function () {
            if (this.progress == 0) return '#909399';
            if (this.progress < 30) return '#F56C6C';
            if (this.progress < 60) return '#E6A23C';
            if (this.progress < 90) return '#67C23A';
            if (this.progress < 100) return 'rgb(106 179 255)';
            return '#409EFF';
        },
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
        //是否有购买记录
        ispurchase: function () {
            return JSON.stringify(this.purchase) != '{}' && this.purchase != null;
        }
    },
    mounted: function () { },
    methods: {
        //是否完成
        finished: function (percentage) {
            return percentage >= 100;
        },
        //进度条显示的数值样式
        format(percentage) {
            percentage = Math.round(percentage * 100) / 100;
            return this.finished(percentage) ? '完成学习！100%' : '通过率：' + percentage + '%';
        },
        //查看课程学习记录详情
        viewDetail: function (item) {
            if (!window.top || !window.top.vapp) return;
            if (!window.top.vapp.open) {
                this.$message({
                    message: '无法打开页面',
                    type: 'warning'
                });
                return;
            }
            var url = '/web/Question/course';
            url = $api.url.set(url, { 'couid': item.Cou_ID, 'stid': this.stid });
            var obj = {
                'url': url,
                'pid': window.name,
                'ico': 'e731', 'min': false,
                'title': '试题练习 - ' + item.Cou_Name,
                'width': '600',
                'height': '80%'
            }
            window.top.vapp.open(obj);

        }
    },
    template: `<div class="ques_progress">
        <div><span><icon>&#xe75e</icon>试题练习</span>        
        </div>
        <div class="progress">   
            <el-tooltip effect="light" content="点击进度条，查看详情" placement="bottom-end"> 
                <el-progress :text-inside="true" :format="format" :stroke-width="20" :color="color"
                @click.native="viewDetail(course)" style="width:100%"
                :status="progress>=100 ? 'success' : ''" :percentage="progress"></el-progress>   
            </el-tooltip>
        </div> 
    </div>`
});