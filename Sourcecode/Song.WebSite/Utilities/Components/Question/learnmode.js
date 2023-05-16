//试题练习的模式，练习模式与背题模式
$dom.load.css(['/Utilities/Components/Question/Styles/learnmode.css']);
Vue.component('learnmode', {
    //state:当前的状态值，偶数为答题模式，奇数为背景模式
    props: ['state'],
    data: function () {
        return {
            value: 0
        }
    },
    watch: {
        'state': {
            handler(nv, ov) {
                if (nv != null) this.value = nv;
            },
            immediate: true
        }
    },
    computed: {},
    mounted: function () {

    },
    methods: {
        click_event: function () {
            this.value++;
            this.$emit('change', this.value);
        }
    },
    template: `<div class="ws_ques_learnmode" @click="click_event" :even="value%2==0" noview>
        <div :current="value%2==0" noview>答题</div>
        <div :current="value%2==1" noview>背题</div>      
    </div>`
});