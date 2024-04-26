//日期区间的选择
$dom.load.css(['/Utilities/Components/Styles/date_range.css']);
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
                    text: '本周',
                    onClick(p) {
                        const today = new Date();   //当前时间
                        let dayOfWeek = today.getDay(); //当前星期几                      
                        // 计算本周的起始时间
                        let startDate = new Date(today);
                        startDate.setDate(today.getDate() - (dayOfWeek > 0 ? dayOfWeek - 1 : 6));
                        // 计算本周的结束时间
                        let endDate = new Date(today);
                        endDate.setDate(today.getDate() + (dayOfWeek > 0 ? 7 - dayOfWeek : 0));
                        p.$emit('pick', [startDate, endDate]);
                    }
                }, {
                    text: '最近一个月',
                    onClick: (p) => p.$emit('pick', setTimeInterval(1))
                }, {
                    text: '本月', onClick(picker) {
                        const start = new Date();
                        start.setDate(1);
                        let yy = start.getFullYear();
                        let mm = start.getMonth() + 1;
                        if (mm > 12) {
                            mm = 1;
                            yy = yy + 1;
                        }
                        let end = new Date(yy, mm, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近三个月',
                    onClick: (p) => p.$emit('pick', setTimeInterval(3))
                }, {
                    text: '本季度', onClick(picker) {
                        const start = new Date();
                        let yy = start.getFullYear();
                        let mm = start.getMonth() + 1;
                        mm = Math.floor(mm % 3 == 0 ? mm / 3 : mm / 3 + 1);
                        start.setDate(1);
                        start.setMonth((mm - 1) * 3);
                        const end = new Date(yy, start.getMonth() + 3, 0);
                        picker.$emit('pick', [start, end]);
                    }
                }, {
                    text: '最近半年',
                    onClick: (p) => p.$emit('pick', setTimeInterval(6))
                }, {
                    text: '本学期',
                    onClick: (p) => {
                        const curr = new Date();
                        let mm = curr.getMonth() + 1;
                        let yy = curr.getFullYear();
                        let start, end;
                        if (mm <= 6) {//上半年
                            start = new Date(yy, 0, 1);
                            end = new Date(yy, 6, 0);
                        } else {  //下半年
                            start = new Date(yy, 6, 1);
                            end = new Date(yy, 12, 0);
                        }
                        p.$emit('pick', [start, end]);
                    }
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
                }]
            },
        }
    },
    watch: {
        'start': {
            handler: function (nv, ov) {
                this.$set(this.selectDate, 0, nv);
            }, immediate: true
        },
        'end': {
            handler: function (nv, ov) {
                this.$set(this.selectDate, 1, nv);
            }, immediate: true
        }
    },
    created: function () {

    },
    methods: {
        //选择变动时触发事件
        evt_change: function () {
            //起始时间只保留日期部分，例如 2023-10-01 00:00:00
            let start = this.selectDate != null ? this.todate(this.selectDate[0]) : '';
            //结束时间由当前日期加一，即查询条件中包含结束那一天的当天
            let end = this.selectDate != null ? this.todate(this.selectDate[1]) : '';
            if (end != null && end != '') end.setDate(end.getDate() + 1);
            this.$emit('change', start, end);
        },
        //只保留日期部分
        todate: function (time) {
            if (time == null) return null;
            this.selectDate[1]
            // 获取日期的年、月、日
            let year = time.getFullYear();
            let month = time.getMonth() + 1;
            let day = time.getDate();
            return new Date(year + '/' + month + '/' + day);
        }
    },
    template: ` <el-date-picker class="date_range" v-model="selectDate" type="daterange" unlink-panels
        @change="evt_change" style="width: 220px;" range-separator="至"
        start-placeholder="开始日期" end-placeholder="结束日期" :picker-options="pickerOptions"
        :default-time="['00:00:00', '23:59:59']">
    </el-date-picker>`
});