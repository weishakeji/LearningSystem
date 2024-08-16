//课程视频的学习值
Vue.component('progress_value', {
    //config:机构的配置项，其实包括了视频完成度的容差值（VideoTolerance）
    props: ['course', 'stid', 'config'],
    data: function () {
        return {
            data: {},        //进度信息         
            percent: 0,     //完成的真实百分比
            tolerance: 10,       //容差，例如容差为10，则完成90%即可认为是100%
            purchase: {},        //购买记录，其中包含学习进度等
            score: 0,        //最终成绩
            loading: false
        }
    },
    watch: {
        'course': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true, deep: true
        },
        'config': {
            handler: function (nv, ov) {
                if (nv && nv.VideoTolerance) {
                    this.tolerance = Number(nv.VideoTolerance);
                    this.tolerance = isNaN(this.tolerance) ? 0 : this.tolerance;
                    this.tolerance = this.tolerance <= 0 ? 0 : this.tolerance;
                }
            }, immediate: true, deep: true
        },
    },
    computed: {
        //完成度，加了容差之后的
        'progress': function () {
            return this.percent + this.tolerance >= 100 ? 100 : this.percent;
        },
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.cache('Course/LogForVideo:5', { 'couid': th.course.Cou_ID, 'stid': th.stid }),
                $api.get('Course/Purchaselog', { 'stid': th.stid, 'couid': th.course.Cou_ID }),
            ).then(([cou, purchase]) => {
                var result = cou.data.result;
                if (result != null && result.length > 0) {
                    th.data = result[0];
                    th.data.lastTime = new Date(th.data.lastTime);
                    th.percent = th.data.complete;
                    console.log(th.data);
                } else {
                    th.data = null;
                    th.percent = 0;
                }
                //购买记录
                th.purchase = purchase.data.result;               
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
    },
    template: `<dd>
            <loading v-if="loading"></loading>
            <slot v-else :value='progress' :score="score" :course='data' :purchase="purchase"></slot>
    </dd>`
});