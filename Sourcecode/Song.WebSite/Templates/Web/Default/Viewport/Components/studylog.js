
//视频学习记录
$dom.load.css([$dom.pagepath() + 'Components/Styles/studylog.css'])
Vue.component('studylog', {
    props: ["number", "icon", 'title', 'unit', 'show'],
    data: function () {
        return {
            header: ['列1', '列2', '列3'],

            index: true,
            columnWidth: [50],
            align: ['center'],
            config: {},
            //获取记录的查询条件
            query: { 'orgid': -1, 'couid': -1, 'size': 100, 'index': 1 },
            datas: [],
            loading: true
        }
    },
    watch: {
        'number': {
            handler: function (nv, ov) {

            }, immediate: true, deep: true
        }
    },
    computed: {
    },
    mounted: function () {
        var th = this;
        this.getStudyLogPager(1);
        window.setInterval(function () {
            th.getStudyLogPager();
        }, 30 * 1000);
    },
    methods: {
        getStudyLogPager: function (index) {
            var th = this;
            if (index != null) th.query.index = index;
            else th.query.index++;
            $api.get('Course/StudyLogPager', th.query).then(function (req) {
                if (req.data.success) {
                    var result = req.data.result;
                    console.error(result);
                    for (let i = 0; i < result.length; i++) {
                        const el = result[i];
                        var d = th.datas.findIndex(v => el.Ac_ID == v.Ac_ID);
                        if (d < 0) th.datas.push(el);
                    }
                    th.buildData(th.datas);
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        //创建图表所需data
        buildData: function (datas) {
            var data = [];
            for (let i = 0; i < datas.length; i++) {
                const d = datas[i];
                var progress = d.Lss_Duration <= 0 ? 0 : Math.round(d.Lss_StudyTime / (d.Lss_Duration / 1000) * 100);
                progress = progress >= 100 ? 100 : progress;
                if (d.Ac_Name.length > 1) d.Ac_Name = this.asterisk(d.Ac_Name, 1, 2);
                //if (d.Ac_IDCardNumber.length > 1) d.Ac_IDCardNumber = this.asterisk(d.Ac_IDCardNumber, 6, 14);
                if (d.Ac_AccName.length > 1) d.Ac_AccName = this.asterisk(d.Ac_AccName, 6, 14);
                var arr = [d.Ac_Name, d.Ac_AccName, progress + " %"];
                data.push(arr);
            }
            this.config = {
                data: data,
                rowNum: 9
            }
        },
        //替换星号
        asterisk: function (str, start, end) {
            var len = str.length;
            end = end >= len ? len : end;
            start = start > len || start < 0 ? 0 : start;

            var bStr = str.substr(0, start);
            var eStr = str.substr(end, len);

            var asterisk = "";
            //var size=
            while (end - start > asterisk.length) asterisk += "*";
            return bStr + asterisk + eStr;
        }
    },

    template: `<div class="studylog">
    <dv-loading v-if="loading">Loading...</dv-loading>
    <dv-scroll-board v-else :config="config" :header="header" style="width:100%;height:100%" />
    </div>`
});
