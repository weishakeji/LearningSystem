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
        }
    },
    computed: {

    },
    mounted: function () { },
    methods: {
        //判断答题是否正确
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

    },
    template: `<dl :class="{'quesArea':true}" :style="'width:'+ques4display.length*100+'vw'">
           <question v-for="(qid,i) in ques4display" :qid="qid" :state="{}" :index="initindex+i" :total="list.length" :types="types"
            :mode="mode" :current="initindex+i==index"></question>
        </dl>`
});
