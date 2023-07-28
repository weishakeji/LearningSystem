//试题练习的区域
$dom.load.css([$dom.pagepath() + 'Components/Styles/QuesArea.css']);
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

            //异步加载的试题id,为了加快试题显示，
            //在练习中，异步加载当前试题的前后试题
            asynclist: [],
            asynccount: 6,       //异步加载多少道试题
            asyncloading: false      //异步加载中
        }
    },
    watch: {
        //初始加载的简要试题信息，只有试题类型与id
        'ques': {
            handler(nv, ov) {
                if ($api.isnull(nv) || this.list.length > 0) return;
                const list = [];
                for (let k in nv) {
                    for (let i = 0; i < nv[k].length; i++)
                        list.push(nv[k][i]);
                }
                this.list = list;
                //console.log(list);
            },
            immediate: true
        },
        'mode': function (nv, ov) {
            console.log(nv);
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
                this.$nextTick(function () {
                    window.setTimeout(function () {
                        $dom("dl.quesArea").css('left', -100 * nv + 'vw');
                    }, 50);
                });
                //计算当前试题的前后试题
                this.asyncload_list(nv);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () { },
    methods: {
        //设置当前试题的id与索引
        setindex: function (qid, index) {
            if (qid != null || qid >= 0) this.currid = qid;
            if (index != null && (index >= 0 || index < this.list.length)) this.index = index;
            //触发滑动事件,返回当前索引
            this.$emit('swipe', index);
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
            //触发滑动事件,返回当前索引
            this.$emit('swipe', this.index);
        },
        //试题答题状态变更时
        answer: function (state, ques) {
            this.state.data.current = state;
            this.state.update(true);
        },
        //计算要异步加载的试题id
        asyncload_list: function (index) {
            //获取当前试题的前后试题的id，           
            let arr = [];
            let half = Math.floor(this.asynccount / 3);
            let init = index - half <= 0 ? 0 : index - half;       //取值的起始索引   
            let max = init + this.asynccount >= this.list.length ? this.list.length : init + this.asynccount;   //最大索引
            for (let i = init; i <= max && i < this.list.length; i++) {
                if (i == index) continue;
                arr.push(this.list[i]);
            }
            //要加载的试题放到列表中，（等待后续异步加载）
            let n = 0;
            for (let i = 0; i < arr.length; i++) {
                if (this.asynclist.indexOf(arr[i]) !== -1)
                    continue;
                if (i < half) this.asynclist.push(arr[i]);
                else
                    this.asynclist.splice(n++, 0, arr[i]);
            }
            //console.log(this.asynclist);
            if (!this.asyncloading) this.asyncload();
        },
        //异步加载当前试题临近的试题
        asyncload: function () {
            var th = this;
            th.asyncloading = th.asynclist.length > 0;
            if (!th.asyncloading) return;
            $api.cache('Question/ForID:43200', { 'id': th.asynclist[0] }).then(function (req) {
                if (req.data.success) {
                    if (th.asynclist.length > 0)
                        th.asynclist.splice(0, 1);
                    th.asyncload();
                } else {
                    throw req;
                }
            }).catch(err => console.error(err))
                .finally(() => { });
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
            this.setindex(qid,index);
            return;
            var th = this;
            this.$nextTick(function () {
                th.index = index;
            });
            //this.$parent.data = this.$parent.state.data.count;
            console.log(index);
        }
    },
    template: `<dl :class="{'quesArea':true}" :style="'width:'+(list.length<=1 ? 1 : list.length)*100+'vw'" v-swipe="swipe">
           <div v-if="!$parent.loading && list.length<1" class="noques"><icon>&#xe849</icon>没有试题</div>
           <question v-else v-for="(qid,i) in list" :qid="qid" :state="state.getitem(qid,i)" :index="i"
            :total="list.length" :types="types" :account="account" :fontsize="fontsize"
            :mode="mode" :current="i==index" @answer="answer">
                <template v-slot:buttons="btn">
                    <quesbuttons :question="btn.ques" :account="account" :couid="0" :current="i==index"></quesbuttons>
                </template>
            </question>
        </dl>`
});
