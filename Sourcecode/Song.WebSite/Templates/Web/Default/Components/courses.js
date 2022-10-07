
//获取多个课程
Vue.component('courses', {
    //sbjid:专业id
    //couid:当前课程，在获取的课程中去除该课程
    props: ["sbjid", "couid", "org", "count"],
    data: function () {
        return {
            path: $dom.path(),   //模板路径
            show: false,         //是否显示，  
            datas: [],
            surplus: -1,         //获取课程的剩余次数
        }
    },
    watch: {
        'sbjid': {
            handler: function (nv, ov) {
                this.surplus = this.count;
                this.getcourses(nv, this.count);
            }, immediate: true
        }
    },
    computed: {},
    mounted: function () {
        var css = $dom.path() + 'Components/Styles/courses.css';
        $dom.load.css([css]);
    },
    methods: {
        //跳转
        godetail: function (id) {
            var url = $api.url.dot(id, '/web/Course/Detail');
            window.location.href = url;
        },
        seturl: function () {
            return $api.url.dot(this.item.Cou_ID, '/web/Course/Detail');
        },
        //从指专业内获取课程列表
        getcourses: function (sbjid, count) {
            var th = this;
            var orgid = th.org.Org_ID;        
            $api.get('Course/ShowCount',
                { 'sbjid': sbjid, 'orgid': orgid, 'search': '', 'order': 'rec', 'count': count })
                .then(function (req) {
                    if (req.data.success) {
                        th.datas = th.remove_self(req.data.result);
                        //如果没有达到指定数量，则继续获取
                        if (th.datas.length < th.count && th.surplus >= 0)
                            th.getone(1);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
        },
        getone: function (index) {
            var th = this;
            th.surplus--;
            var orgid = th.org.Org_ID;
            $api.get('Course/ShowPager', { 'orgid': orgid, 'sbjids': '', 'search': '', 'order': '', 'size': 1, 'index': index })
                .then(function (req) {
                    if (req.data.success) {
                        var result = th.remove_self(req.data.result);
                        if (result.length > 0) {
                            for (let i = 0; i < result.length; i++) {
                                const element = result[i];
                                th.$set(th.datas, th.datas.length + i, element);
                            }
                        }
                        //如果没有达到指定数量，则继续获取
                        if (th.datas.length < th.count && th.surplus >= 0)
                            th.getone(index + 1);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    //alert(err);
                    //Vue.prototype.$alert(err);
                    console.error(err);
                });
        },
        //去除自身课程
        remove_self: function (arr) {
            for (let i = 0; i < arr.length; i++) {
                if (arr[i].Cou_ID == this.couid) {
                    arr.splice(i, 1);
                    i--;
                }
            }
            return arr;
        }
    },

    template: `<weisha_courses v-if="datas && datas.length>0">          
        <slot v-for="(item,index) in datas" name="item" :data="item" :index="index">
            {{index+1}}.{{item.Cou_Name}}<br/>
        </slot>        
    </weisha_courses>`
});
