$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            datas: [],
            current: '',     //当前采用的短信接口
            loading: false,
            loadingid: ''
        },
        watch: {
        },
        mounted: function () {
            this.loadDatas();
        },
        created: function () {
            let url = window.location.host;
            console.log(url);
        },
        methods: {
            //加载数据页
            loadDatas: function () {
                var th = this;
                th.loading = true;
                th.datas = [];
                $api.bat(
                    $api.post('Sms/ItemsFresh'),
                    $api.get("Sms/Current")
                ).then(([items, curr]) => {
                    //获取结果
                    th.datas = items.data.result;
                    th.current = curr.data.result;
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                }).finally(() => th.loading = false);
            },
            //设置当前使用的接口
            setcurrent: function (remark) {
                var th = this;
                th.loadingid = remark;
                $api.post('Sms/SetCurrent', { 'mark': remark }).then(function (req) {
                    if (req.data.success) {
                        th.$notify({
                            type: 'success',
                            message: '设置成功!',
                            center: true
                        });
                        th.loadDatas();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(() => th.loadingid = '');
            },
            //是否作为超链接
            //当前域名为https而要打开的域名为http时，会由于安全限制无法打在openbox中打开
            ishyperlink: function (url) {
                //当前网址是否有ssl证书
                let selfissl = window.location.href.substring(0, 8).toLowerCase() == 'https://';
                //要打开的域名是否有ssl证书
                let urlissl = url.substring(0, 8).toLowerCase() == 'https://';
                //return true;
                if (selfissl && !urlissl) return true;
                return false;
            },
            //重置密码的弹窗
            openbox: function (url, tag, title, width, height, ico) {
                if (this.ishyperlink(url)) return;
                var pattern = /^(http|https|\/\/).*/gi;
                if (!pattern.test(url)) {
                    var route = $dom('meta[view]').attr("route");
                    if (route.indexOf('/') > -1) route = route.substring(0, route.lastIndexOf('/') + 1);
                    url = route + url;
                }
                window.top.$pagebox.create({
                    width: width ? width : 400,
                    height: height ? height : 300,
                    url: url,
                    pid: window.name,
                    ico: ico,
                    id: window.name + '_' + tag,
                    title: $dom('title').text() + ' - ' + title
                }).open();
            }
        },
        components: {
            'sms_count': {
                props: ['mark', 'user', 'pw'],
                data: function () {
                    return {
                        count: -1,    //短信数量
                        error: '',       //错误提示
                        loading: false
                    }
                },
                mounted: function () {
                    this.getcount();
                },
                methods: {
                    getcount: function () {
                        var th = this;
                        th.loading = true;
                        $api.get('Sms/Count', { 'mark': th.mark, 'smsacc': th.user, 'smspw': th.pw }).then(function (req) {
                            if (req.data.success) {
                                th.count = req.data.result;
                            } else throw req.data.message;
                        }).catch(err => th.error = err)
                            .finally(() => th.loading = false);
                    }
                },
                template: `<span>
                    <loading v-if="loading"></loading>
                    <template v-else>
                         <span v-if="count>-1">{{count}}</span>
                         <alert v-else>{{error}}</alert>
                    </template>
                </span >`
            }
        }
    });

});