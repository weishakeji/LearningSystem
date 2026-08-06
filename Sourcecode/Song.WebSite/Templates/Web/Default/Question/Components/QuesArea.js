//试题练习的区域
//事件
//change:当答题信息变更时触发，返回一些统计数据
Vue.component('quesarea', {
    //ques:试题列表，只有试题类型与id
    //mode:练习模式，练题还是背题
    //state:答对记录，它不是一个记录项，而是管理记录的对象
    props: ['ques', 'types', 'mode', 'account', 'state', 'fontsize'],
    data: function () {
        return {
            list: [],         //所有试题，与ques不同，它是一维数组，方便后续计算            
            currid: '',         //当前试题id            
            index: 0,            //当前试题索引 

            currques: {},          //当前试题

            hoverState: false,      // 鼠标悬停状态
        }
    },
    watch: {
        //初始加载的简要试题信息，只有试题类型与id
        'ques': {
            handler(nv, ov) {
                if ($api.isnull(nv) || this.list.length > 0) return;
                this.list = Object.values(nv).flat();
            }, immediate: true
        },

        //滑动试题，滑动到指定试题索引
        'index': {
            handler: function (nv, ov) {
                if (nv > this.list.length - 1 || nv < 0) return;
                //设置当前练习的试题
                if (nv != null && this.list.length > 0)
                    this.state.last(this.list[nv], nv);
                //更新答题状态（不推送到服务器）
                this.state.update(false);
            }, immediate: true
        }
    },
    computed: {
        //是否处于第一道题
        isfirst: t => t.index == 0,
        //是否处于最后一道题
        islast: t => t.index == t.list.length - 1,
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
            if (index == null || (index <0 || index >= this.list.length))return;
            this.index = index;
            let qid = this.getid(index);
            if (qid != null || qid >= 0) this.currid = qid;          
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
            if (e.direction == 2 && this.index < this.list.length - 1) this.index++;
            //向右滑动
            if (e.direction == 4 && this.index > 0) this.index--;
            this.setindex(this.index, true, Math.abs(e.velocityX));
        },
        //键盘事件，实现上下键切换试题
        handleKeyDown: function (e) {
            if (!this.hoverState) return

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
        //试题答题状态变更时
        answer: function (state, ques) {
            this.state.data.current = state;
            //更新答题状态
            let index = this.state.data.items.findIndex(item => item.qid === state.qid);
            this.state.data.items[index] = state;
            //更新数据到服务器
            this.state.update(true);
        },
        //通过索引获取试题id
        getid: function (index) {
            if (index < 0) return null;
            if (index > this.list.length - 1) return null;
            return this.list[index];
        },
        //清除指定的试题
        cleanup: function (index) {
            if (index == null) index = this.index;
            //当前试题id
            let qid = this.getid(index);
            if (qid == null) return;
            //清除页面中的试题
            this.list.splice(index, 1);
            //index += this.list.length > index ? 1 : -1;
            if (index >= this.list.length) index = this.list.length - 1;
            if (index < 0) index = 0;

            //清理父级组件试题列表
            for (let k in this.ques) {
                let arr = this.ques[k];
                let idx = arr.indexOf(qid);
                if (idx >= 0) arr.splice(idx, 1);
            }
            this.$parent.state.del(qid);
            this.setindex(index);
        }
    },
    template: `<div :class="{'quesArea':true}">
        <div v-if="!$parent.loading && list.length<1" class="noques"><icon>&#xe849</icon>没有试题</div> 
         <template v-else>
            <info no-font-size>
                <span>
                    <i>{{index+1}}/{{list.length}}</i>
                    [ {{types[currques.Qus_Type - 1]}}题 ] 
                </span>
                <quesbuttons :question="currques" :account="account" :index="index"></quesbuttons> 
            </info>   
            <dl :style="'width:'+(list.length<=1 ? 1 : list.length)*100+'%'"  @mouseenter="hoverState = true" @mouseleave="hoverState = false">             
                <question ref="questions"  v-for="(qid,i) in list" :qid="qid" :state="state.getitem(qid,i)" :index="i" :curindex="index"
                    :total="list.length" :types="types" :account="account" :fontsize="fontsize" v-swipe="swipe"
                    :mode="mode" :iscurrent="i==index" @answer="answer" @current="q=>currques=q">                       
                </question>           
            </dl>
        </template>       
    </div>`
});
