//试题的展示,用于试卷预览，只显示题不显示答案
Vue.component('question', {
    //ques:当前试题
    //index:当前试题的索引
    //groups:试题按题型分类的试题组
    //groupindex:当前试题题型索引
    props: ['ques', 'index', 'groups', 'groupindex', 'types'],
    data: function () {
        return {
            init: false,
            ques: {},       //试题        
            ismobi: $api.ismobi(),       //是否是手机端    

            accessory: {},           //试题附件
            loading: false,
            loading_file: false          //加载附件文件
        }
    },
    watch: {
        'ques': {
            handler(nv, ov) {
                this.$nextTick(function () {
                    var dom = $dom(".preview[qid='" + this.ques.Qus_ID + "']");
                    //清理空元素                
                    window.ques.clearempty(dom.find('header'));
                    window.ques.clearempty(dom.find('article>div'));
                    //公式渲染
                    this.$mathjax([dom[0]]);
                });
            },
            immediate: true
        },
    },
    computed: {
        //是否试题加载完成
        existques: function () {
            return JSON.stringify(this.ques) != '{}' && this.ques != null;
        }
    },
    mounted: function () {

    },
    methods: {
        //计算序号，整个试卷采用一个序号，跨题型排序
        calcIndex: function (index) {
            let gindex = this.groupindex - 1;
            let initIndex = 0;
            while (gindex >= 0) {
                initIndex += this.groups[gindex].ques.length;
                gindex--;
            };
            return initIndex + index;
        },
        //选项的序号转字母
        toletter: index => String.fromCharCode(65 + index),

    },
    template: `<div :qid="ques.Qus_ID" class="preview">        
        <header :index="calcIndex(index+1)" :num="ques.Qus_Number" v-html="ques.Qus_Title">          
            <span v-html="ques.Qus_Title"></span>
            <span>（{{ques.Qus_Number}} 分）</span>   
        </header>      
        
        <article v-if="ques.Qus_Type==1 || ques.Qus_Type==2" :questype="ques.Qus_Type">
            <div v-for="(ans,i) in ques.Qus_Items" :index="toletter(i)" v-html="ans.Ans_Context">               
            </div>
        </article>           
        <article  v-if="ques.Qus_Type==3" :questype="ques.Qus_Type">
            <div>正 确</div> <div>错 误</div>
        </article>
        <article v-if="ques.Qus_Type==4" :questype="ques.Qus_Type">
            <textarea rows="10"></textarea>
        </article>
        <article v-if="ques.Qus_Type==5" :questype="ques.Qus_Type">
            <div v-for="(ans,i) in ques.Qus_Items" :index="toletter(i)">              
            </div>
        </article> 
      </div>`
});