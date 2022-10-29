//课程学习界面的顶部选项卡
//事件：switch选项卡切换
Vue.component('study_tabs', {
    props: ['course', 'state', 'account'],
    data: function () {
        return {
            loading: true,
            tabs: [
                { 'name': '视频', 'tag': 'existVideo', 'icon': 'e83a', 'show': false },
                { 'name': '直播', 'tag': 'isLive', 'icon': 'e6bf', 'show': false },
                { 'name': '内容', 'tag': 'isContext', 'icon': 'e6cb', 'show': false },
                { 'name': '附件', 'tag': 'isAccess', 'icon': 'e853', 'show': false },
                { 'name': '习题', 'tag': 'isQues', 'icon': 'e75e', 'show': false }],
            tabActive: 'existVideo',
            tabindex: -1,
            show: false     //是否显示,只要有一个tabs是show，此处即为false
        }
    },
    watch: {
        //根据当前章节状态，显示不同的选项卡
        'state': function (val, old) {
            this.loading = false;
            this.show = false;
            this.tabindex = -1;
            for (let i = 0; i < this.tabs.length; i++) {
                const item = this.tabs[i];
                item.show = val[item.tag];
                if (item.tag == 'isLive') {
                    //item.show = true;
                }
                if (item.show) this.show = true;
            }

            this.$nextTick(function () {
                for (let i = 0; i < this.tabs.length; i++) {
                    if (this.tabs[i].show) {
                        this.tabActive = this.tabs[i].tag;
                        this.tabindex = i;
                        break;
                    }
                }
                if (this.tabindex < 0) {
                    this.tabActive = this.state.isLogin ? 'isNull' : 'noLogin';
                }
                this.$emit('switch', this.tabActive, this.tabindex);
                //console.error(this.tabActive);
            });
        },
        'islogin': function (nv, ov) {
            this.state.isLogin = nv;
            if (!this.state.islogin) {
                this.tabActive = 'nologin';
                this.tabindex = -1;
                this.$emit('switch', this.tabActive, this.tabindex);
            }
        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        },
        //课程是否存在
        couexist: function () {
            return JSON.stringify(this.course) != '{}' && this.course != null;
        }
    },
    mounted: function () {
        var css = $dom.path() + 'course/Components/Styles/study_tabs.css';
        $dom.load.css([css]);
    },
    methods: {
        //选项卡的点击事件
        tabClick: function (item, index) {
            this.tabActive = item.tag;
            this.tabindex = index;
            this.$emit('switch', this.tabActive, this.tabindex);
        },
        //知识库的点击事件
        knlClick: function () {
            $pagebox.create({
                'url': $api.url.set('Knowledges', { 'couid': this.course.Cou_ID }),
                'title': '课程知识库', 'ico': 'f0085',
                'width': '80%', 'height': '80%',
                'min': false, 'full': true, 'showmask': true
            }).open();
        },
    },
    template: `<div id="study_tabs">    
        <div class="courseBox">
            <icon course></icon>
            <template v-if="couexist && course.Cou_Name!=''" remark="课程名称">
                课程：<a :href="'detail.'+course.Cou_ID">《{{course.Cou_Name}}》</a>
            </template>
            <template v-else>
                当前课程好像不存在，如果是正在加载，请等待！
            </template>
            <template v-if="course.Sbj_Name!=''" remark="专业">
                - <a :href="'index?sbjid='+course.Sbj_ID">{{course.Sbj_Name}}</a>
            </template>
        </div>    
        <dl>
            <dd v-if="loading" class="tabCurrent"><loading>...</loading></dd>
            <dd v-if="tab.show && islogin" v-for="(tab,i) in tabs" @click="tabClick(tab,i)"
            :class="{'tabCurrent':tab.tag==tabActive}">
                <icon v-html="'&#x'+tab.icon"></icon>
                {{tab.name}}
            </dd>
           <dd v-if="!show || !islogin" class="tabCurrent"> <icon>&#xe61f</icon>提示</dd>
       </dl>
       <div id="btnKnowledge" v-if="couexist && state.isKnl" v-on:click="knlClick">知识库</div>
    </div>`
});