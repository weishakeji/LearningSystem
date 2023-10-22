//日期区间的选择

Vue.component('date_range', {
    //start:开始时间
    //end：结束时间
    props: ['start', 'end'],
    data: function () {
        var setTimeInterval = function (subtract) {
            let end = new Date();           // 获取当前时间     
            let month = end.getMonth();     // 获取当前月份
            let year = end.getFullYear();    //当前年份
            // 计算要减去的月份后的目标月份            
            month = month - subtract;
            year = month < 0 ? end.getFullYear() - 1 : end.getFullYear();
            if (month < 0) month += 12;
            // 设置目标日期为当前日期
            let start = new Date(end);
            start.setFullYear(year, month); // 设置目标年份和月份
            return [start, end];
        };
        return {
            //当前选择的日期
            selectDate: [this.start, this.end],
            pickerOptions: {
                shortcuts: [{
                    text: '最近一周',
                    onClick(p) {
                        const start = new Date();
                        start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
                        p.$emit('pick', [start, new Date()]);
                    }
                }, {
                    text: '最近一个月',
                    onClick: (p) => p.$emit('pick', setTimeInterval(1))
                }, {
                    text: '本月', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        var yy = start.getFullYear();
                        var mm = start.getMonth() + 1;
                        if (mm > 12) {
                            mm = 1;
                            yy = yy + 1;
                        }
                        var end = new Date(yy, mm, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick: (p) => p.$emit('pick', setTimeInterval(3))
                }, {
                    text: '本季度', onClick(picker) {
                        const start = new Date();
                        var yy = start.getFullYear();
                        var mm = start.getMonth();
                        if (mm >= 1 && mm <= 3) mm = 0;
                        if (mm >= 4 && mm <= 6) mm = 3;
                        if (mm >= 7 && mm <= 9) mm = 6;
                        if (mm >= 10 && mm <= 12) mm = 9;
                        start.setDate(1);
                        start.setMonth(mm);
                        const end = new Date(yy, mm + 3, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近半年',
                    onClick: (p) => p.$emit('pick', setTimeInterval(6))
                }, {
                    text: '最近一年',
                    onClick: (p) => p.$emit('pick', setTimeInterval(12))
                }, {
                    text: '本年', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        start.setMonth(0);
                        const end = new Date(start.getFullYear(), 12, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三年',
                    onClick: (p) => p.$emit('pick', setTimeInterval(36))
                }, {
                    text: '最近五年',
                    onClick: (p) => p.$emit('pick', setTimeInterval(60))
                }]
            },
        }
    },
    watch: {

    },
    created: function () {

    },
    methods: {
        //选择变动时触发事件
        evt_change: function () {
            this.$emit('change', this.selectDate[0], this.selectDate[1]);
        }
    },
    template: ` <el-date-picker v-model="selectDate" type="daterange" unlink-panels
        @change="evt_change" style="width: 220px;" range-separator="至"
        start-placeholder="开始日期" end-placeholder="结束日期" :picker-options="pickerOptions"
        :default-time="['00:00:00', '23:59:59']">
    </el-date-picker>`
});