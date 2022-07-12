
//数值的展示
$dom.load.css([$dom.pagepath() + 'Components/Styles/number.css'])
Vue.component('number', {
    //number:要显示的值
    //icon:图标
    //unit:数值的单位
    props: ["number", "icon", 'title', 'unit', 'show'],
    data: function () {
        return {
            config: { number: [0], content: '{nt}' },
            real: 0      //记录number的值
        }
    },
    watch: {
        'number': {
            handler: function (nv, ov) {
                if (ov <= 0 || ov == null) this.real = nv;
                this.config = this.create_config(nv);
                //
                var th = this;
                window.setTimeout(function () {
                    th.number = Math.floor(th.real / 3);
                    window.setTimeout(function () {
                        th.number = th.real;
                    }, 500);
                }, 15 * 1000);
            }, immediate: true, deep: true
        }
    },
    computed: {
    },
    mounted: function () {


    },
    methods: {
        create_config: function (num) {
            var n = num==null  ||  isNaN(Number(num)) ? 0 : num;
            var config = { number: [0], content: '{nt}' };
            config.number = [num];
            config.content = '{nt} ' + (this.unit ? this.unit : '');
            return $api.clone(config);
        }
    },

    template: `<div class="number_show">
      <div class="title">
        <icon v-html="'&#x'+icon" v-if="icon!='' && icon!=null"></icon>
        <span v-html="title"></span>
      </div>  
      <dv-digital-flop :config="config" :animationFrame="12" animationCurve="easeInOutBack"></dv-digital-flop>
    </div>`
});
