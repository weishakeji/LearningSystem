//试题的区域
//事件
//answer:答题状态变更时触发，返回答题状态与试题信息
//swipe:试题滑动时触发，返回当前试题索引
Vue.component('quesarea', {
    //paperques:所有试题列表，按题型划分
    //state:答对记录，它不是一个记录项，而是管理记录的对象
    props: ['paperques', 'types', 'account', 'state', 'fontsize'],
    data: function () {
        return {
            list: [],         //所有试题，与ques不同，它是一维数组，方便后续计算            
            currid: '',         //当前试题id      
            currgroup: null,    //当前试题组
            index: 0,            //当前试题的全局索引  

            hoverState: false,      // 鼠标悬停状态
        }
    },
    watch: {
        //初始加载的简要试题信息，只有试题类型与id
        'paperques': {
            handler: function (nv, ov) {
                if (nv != null && nv.length > 0) {
                    for (let i = 0; i < this.paperques.length; i++)
                        this.$set(this.paperques[i], 'index', i);
                    this.setindex(0, false);
                }
            },
            immediate: true
        },
        //滑动试题，滑动到指定试题索引
        'index': {
            handler: function (nv, ov) {

            }, immediate: true
        }
    },
    computed: {
        //试题总数
        questotal: t => $api.isnull(t.paperques) ? 0 : t.paperques.reduce((total, item) => total + (item.count || 0), 0),
    },
    mounted: function () {
        window.addEventListener('keydown', this.handleKeyDown)
    },
    methods: {
        //设置当前试题的id与索引
        //index:试题索引
        //effects:是否有滑动特效
        //speed:滑动速度，单位px/ms
        setindex: function (index, effects, speed) {
            if (index == null || (index < 0 || index >= this.questotal)) return;
            this.index = index;
            let qid = this.getid(index);
            if (qid != null || qid >= 0) this.currid = qid;
            //获取当前试题组
            let group = this.getgroup(index);
            if (group) this.currgroup = group;
            //触发滑动事件,返回当前索引
            this.$emit('swipe', index);

            //设置试题的滑动位置
            var dl = $dom("div.quesArea dl");
            speed = speed == null || isNaN(speed) ? 0.5 : 0.7 - speed / 10;
            if (effects == null || effects == true) dl.css('transition', 'left ' + speed + 's ease-in-out');
            else dl.css('transition', 'none');
            var left = -100 * this.index;
            this.$nextTick(function () {
                window.setTimeout(() => dl.css('left', left + '%'), 50);
            });
        },
        //试题滑动 
        swipe: function (e) {
            if (e) {
                if (e.preventDefault) e.preventDefault();
                let node = $dom(e.target ? e.target : e.srcElement);
                if (node.length > 0 && (node.hasClass("van-overlay") || node.hasClass("van-popup"))) return;
            }
            //向左滑动
            if (e.direction == 2 && this.index < this.questotal - 1) this.index++;
            //向右滑动
            if (e.direction == 4 && this.index > 0) this.index--;
            this.setindex(this.index, true, Math.abs(e.velocityX));
        },
        //键盘事件，实现上下键切换试题
        handleKeyDown: function (e) {
            if (!this.hoverState) return;
            switch (e.key) {
                case 'ArrowUp':
                case 'ArrowLeft':
                    e.preventDefault()
                    this.setindex(this.index - 1);
                    break
                case 'ArrowDown':
                case 'ArrowRight':
                    e.preventDefault()
                    this.setindex(this.index + 1);
                    break
            }
        },
        //答题事件
        answer: function (ques) {
            if (ques.Qus_Type == 1 || ques.Qus_Type == 3) this.setindex(this.index + 1);
            this.$emit('answer', ques);
        },
        //通过索引获取试题的id
        getid: function (idx) {
            if (idx < 0) return null;
            let index = idx;
            for (let i = 0; i < this.paperques.length; i++) {
                let group = this.paperques[i];
                if (index < group.count) return group.ques[index].Qus_ID;
                index -= group.count;
            }
            return null;
        },
        //获取试题组
        getgroup: function (idx) {
            let index = idx;
            for (let i = 0; i < this.paperques.length; i++) {
                let group = this.paperques[i];
                if (index < group.count) return group;
                index -= group.count;
            }
            return null;
        },
        //显示题型名称
        showtype: function () {
            let group = this.currgroup;
            if ($api.isnull(group)) return '';
            const map = ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];
            let index = map[group.index] + '、';
            if (group.byname && group.byname != '' && group.byname != 'null') return index + group.byname;
            return index + this.types[group.type - 1] + '题';
        },
    },
    template: `<div :class="{'quesArea':true}" remark="试题区域">
        <div v-if="questotal<1" class="noques"><icon>&#xe849</icon>没有试题</div>
        <template v-else>
            <info no-font-size v-if="currgroup">
                <span>
                    {{showtype()}} 
                    <span>（{{currgroup.count}} 道题, {{currgroup.number}} 分）</span>
                </span>
                <span>
                    {{index+1 }} / {{questotal}}      
                </span> 
            </info>   
            <dl :style="'width:'+(questotal<=1 ? 1 : questotal)*100+'%;'" 
             @mouseenter="hoverState = true" @mouseleave="hoverState = false">
                <template v-for="(group,gindex) in paperques"> 
                    <question ref="question" v-for="(q,i) in group.ques" :ques="q" :groups="paperques" :groupindex="gindex" :quesindex="i" :currindex="index" 
                        :fontsize="fontsize" v-swipe="swipe" @answer="answer">
                    </question>   
                </template>                       
            </dl>
        </template>
    </div>`
});
