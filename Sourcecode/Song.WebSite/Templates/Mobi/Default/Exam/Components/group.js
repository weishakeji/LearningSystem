//试题大项分类
Vue.component('group', {
    //item,试题分类项
    //state:状态，如正确、错误、未做； 
    props: ['item', 'index', 'types', 'state'],
    data: function () {
        return {
            loading: false
        }
    },
    watch: {
        'state': function (nv, ov) {
            //console.log(nv);
        }
    },
    computed: {},
    mounted: function () {
        $dom.load.css([$dom.pagepath() + 'Components/Styles/group.css']);
    },
    methods: {
        //显示分类的信息栏
        showIndex: function () {
            let changeNum = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九'];
            return changeNum[this.index + 1];
        },
        //显示题型
        showType: function () {
            return this.types[this.item.type - 1];
        },
        //计算得分
        score: function () {
            var num = 0;
            for (var i = 0; i < this.item.ques.length; i++) {
                var q = this.item.ques[i];
                if (q.success) num += q.score;
            }
            return Math.floor(num * 100) / 100;
        }
    },
    template: `<div> 
    <div class="type_title">{{showIndex()}}、 {{showType()}}   
        <div class="type_info">               
            <van-tag type="warning">{{item.count}}道题，共{{item.number}}分</van-tag>    
            <van-tag type="success">得分{{score()}}</van-tag>           
        </div>
    </div>
    <slot></slot>
    </div>`
});