// 课程购买信息
Vue.component('purchase_data', {
    props: ['couid', 'account'],
    data: function () {
        return {
            //课程购买信息，来自表Student_Course的记录
            data: {},
            existtosort: false,      //是否存在于学员组关联的课程
            loading: false
        }
    },
    watch: {
        'couid': {
            handler: function (nv, ov) {
                this.onload();
            }, immediate: true
        }

    },
    computed: {
        //是否有购买记录
        isnull: function () {
            return JSON.stringify(this.data) == '{}' || this.data == null;
        }
    },
    mounted: function () { },
    methods: {
        onload: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Course/Purchaselog:5', { 'couid': th.couid, 'stid': this.account.Ac_ID }),
                $api.get('Course/ExistStudentSort', { 'couid': th.couid, 'stsid': this.account.Sts_ID })
            ).then(([purchase, exist]) => {
                //获取结果
                th.data = purchase.data.result;
                th.existtosort = exist.data.result;
                console.log(th.existtosort);
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        }
    },
    //forever:如果课程存在于学员组的关联，则是不限时课程，即永远
    template: `<slot :data="data" :isnull="isnull" :forever="existtosort"></slot>`
});