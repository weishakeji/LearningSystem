//试题的区域
//事件
//answer:答题状态变更时触发，返回答题状态与试题信息
//swipe:试题滑动时触发，返回当前试题索引
Vue.component('quesarea', {
    //ques:试题列表
    //state:答对记录，它不是一个记录项，而是管理记录的对象
    props: ['ques', 'types', 'account', 'state', 'fontsize'],
    data: function () {
        return {
            list: [],         //所有试题，与ques不同，它是一维数组，方便后续计算            
            currid: '',         //当前试题id            
            index: 0,            //当前试题索引    

            currques: {},          //当前试题
        }
    },
    watch: {
        //初始加载的简要试题信息，只有试题类型与id
        'ques': {
            handler(nv, ov) {
console.log("ques:", nv);
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
        //屏幕宽度
        screenWidth: function () {
            let el = this.$parent.$el;
            return $dom(el).width();
        },
        //试题总数
        questotal: t => t.ques.reduce((total, item) => total + (item.count || 0), 0),
    },
    mounted: function () { },
    methods: {
        //设置当前试题的id与索引
        //index:试题索引
        //effects:是否有滑动特效
        //speed:滑动速度，单位px/ms
        setindex: function (index, effects, speed) {
            let qid = this.getid(index);
            if (qid != null || qid >= 0) this.currid = qid;
            if (index != null && (index >= 0 || index < this.list.length)) this.index = index;
            //触发滑动事件,返回当前索引
            this.$emit('swipe', index);

            //设置试题的滑动位置
            var dl = $dom("div.quesArea dl");
            speed = speed == null ? 0.5 : 0.7 - speed / 10;
            if (effects == null || effects == true) dl.css('transition', 'left ' + speed + 's ease-in-out');
            else dl.css('transition', 'none');
            var left = -this.screenWidth * this.index;
            this.$nextTick(function () {
                window.setTimeout(() => dl.css('left', left + 'px'), 50);
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
        //通过索引获取试题的id
        getid: function (index) {
            if (index < 0) return null;
            if (index > this.list.length - 1) return null;
            return this.list[index];
        },
    },
    template: `<div :class="{'quesArea':true}" remark="试题区域">
        <div v-if="!$parent.loading && ques.length<1" class="noques"><icon>&#xe849</icon>没有试题</div>
        <template v-else>
            <info no-font-size>
                <span>
                    <i>{{index+1}}/{{questotal}}</i>
                    [ {{types[currques.Qus_Type - 1]}}题 ] 
                </span>             
            </info>   
            <dl :style="'width:'+(questotal<=1 ? 1 : questotal)*screenWidth+'px'">
            <template v-for="(group,gindex) in ques">
                <question ref="questions"  v-for="(q,i) in group.ques" :ques="q" :index="i" :curindex="index"
                    :total="questotal" :types="types" :account="account" :fontsize="fontsize" v-swipe="swipe"
                    :iscurrent="i==index" @current="q=>currques=q">                       
                </question>   
            </template>
                       
            </dl>
        </template>
    </div>`
});
