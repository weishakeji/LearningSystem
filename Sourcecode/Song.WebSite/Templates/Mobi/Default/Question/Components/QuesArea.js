//试题练习的区域
$dom.load.css([$dom.pagepath() + 'Components/Styles/QuesArea.css']);
Vue.component('quesarea', {
    //ques:试题列表，只有试题类型与id
    //mode:练习模式，练题还是背题
    props: ['ques', 'types', 'mode'],
    data: function () {
        return {
            list: [],         //所有试题，与ques不同，它是一维数组，方便后续计算    
            ques4display: [],     //用于显示的试题，   
            currid: -1,         //当前试题id            
            index: 0,            //当前试题索引
            initindex: 0,         //试题列表的初始索引

            swipeIndex: 0,

            maxvalue: 4,       //每次显示的最多试题数，即ques4display的最大长度
        }
    },
    watch: {
        //初始加载的简要试题信息，只有试题类型与id
        'ques': {
            handler(nv, ov) {
                if ($api.isnull(nv)) return;
                const list = [];
                for (let k in nv) {
                    for (let i = 0; i < nv[k].length; i++)
                        list.push(nv[k][i]);
                }
                this.list = list;
                console.log(list);
            },
            immediate: true
        },
        //滑动试题，滑动到指定试题索引
        'swipeIndex': function (nv, ov) {
            if (nv > this.ques4display.length - 1 || nv < 0) return;
            //设置当前练习的试题
            if (nv != null && this.ques4display.length > 0) {
                var ques = this.ques4display[nv];
                //this.state.last(ques.Qus_ID, nv);
            }
            //更新答题状态（不推送到服务器）
            //this.state.update(false);
            this.$nextTick(function () {
                window.setTimeout(function () {
                    $dom("dl.quesArea").css('left', -($dom("#vapp").width() * nv) + 'px');
                }, 50);
            });
        }
    },
    computed: {

    },
    mounted: function () { },
    methods: {
        //设置当前试题的id与索引
        setindex: function (qid, index) {
            if (qid != null || qid >= 0) this.currid = qid;
            if (index != null || index > 0) this.index = index - 1;
            //生成用于显示的试题，           
            let arr = [];
            let half = Math.floor(this.maxvalue / 3);
            let init = this.index - half <= 0 ? 0 : this.index - half;       //取值的起始索引   
            let max = init + this.maxvalue >= this.list.length ? this.list.length : init + this.maxvalue;   //最大索引
            for (let i = init; i < max; i++) {
                arr.push(this.list[i]);
            }
            this.ques4display = [];
            this.ques4display = arr;
            this.initindex = init;
            console.log(arr);
            //console.log(qid);
            //console.log(index);
        },
        //试题滑动 
        swipe: function (e) {
            if (e && e.preventDefault) {
                e.preventDefault();
                var node = $dom(e.target ? e.target : e.srcElement);
                if (node.hasClass("van-overlay") || node.hasClass("van-popup"))
                    return;
            }
            //向左滑动
            if (e.direction == 2 && this.swipeIndex < this.ques4display.length - 1) this.swipeIndex++;
            //向右滑动
            if (e.direction == 4 && this.swipeIndex > 0) this.swipeIndex--;
        },
    },
    template: `<dl :class="{'quesArea':true}" :style="'width:'+ques4display.length*100+'vw'" v-swipe="swipe">
           <question v-for="(qid,i) in ques4display" :qid="qid" :state="{}" :index="initindex+i" :total="list.length" :types="types"
            :mode="mode" :current="initindex+i==index"></question>
        </dl>`
});
