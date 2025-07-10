//课程购买或学习的按钮
Vue.component('largebutton', {
    //当前课程，当前学员
    props: ["course", "account", "studied", "finaltest", "purchase", "forever", "loading"],
    data: function () {
        return {
            urls: [
                { 'type': 'login', 'link': '/web/sign/in' },
                //学习有多个链接，full指全功能（学练考）课程学习页,question为题库型课程学习页
                {
                    'type': 'learn', 'link': {
                        full: '/web/course/study.{couid}',
                        question: '/web/question/course.{couid}'
                    }
                },
                { 'type': 'buy', 'link': '/web/course/buy.{couid}' },
                { 'type': 'test', 'link': '/web/test/paper.{tpid}' },
            ]
        }
    },
    watch: {
        'urls': {
            handler: function (nv, ov) {
                let urls = this.urls;
                //添加来源页的参数
                let referrer = url => $api.url.set(url, { 'referrer': encodeURIComponent(location.href) });
                for (let i = 0; i < urls.length; i++) {
                    if ($api.getType(urls[i].link) == 'Object') {
                        for (let k in urls[i].link)
                            urls[i].link[k] = referrer(urls[i].link[k]);
                    } else {
                        urls[i].link = referrer(urls[i].link);
                    }
                }
            }, immediate: true
        },
        //当课程变更时
        'course': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                this.urls = this.handlelink('couid', nv.Cou_ID);
            }, immediate: true, deep: true
        },
        'finaltest': {
            handler: function (nv, ov) {
                if ($api.isnull(nv)) return;
                this.urls = this.handlelink('tpid', nv.Tp_Id);
            }, immediate: true, deep: true
        },
    },
    computed: {
        //是否登录
        islogin: t => !$api.isnull(t.account),
        //是否存在结课考试
        istest: t => !$api.isnull(t.finaltest),
        //课程为空,或课程被禁用
        nullcourse: t => $api.isnull(t.course) || !this.course.Cou_IsUse,
        //是否购买记录
        purchased: t => !$api.isnull(t.purchase),
        //可以学习
        canstudy: t => t.studied && (t.purchased && t.purchase.Stc_IsEnable)
    },
    mounted: function () { },
    methods: {
        //处理urls的链接
        handlelink: function (tag, val) {
            let urls = this.urls;
            let replace = url => url.replace(new RegExp('\{' + tag + '\}'), val);;
            for (let i = 0; i < urls.length; i++) {
                if ($api.getType(urls[i].link) == 'Object') {
                    for (let k in urls[i].link)
                        urls[i].link[k] = replace(urls[i].link[k]);
                } else {
                    urls[i].link = replace(urls[i].link);
                }
            }
            return urls;
        },
        //跳转
        url: function (type) {
            let item = this.urls.find(function (item) {
                return item.type == type;
            });
            let url = '';
            if (type == 'learn') {
                if (this.course.Cou_Type == 0) url = item.link.full;
                if (this.course.Cou_Type == 2) url = item.link.question;
            } else url = item.link;
            //if (this.course.Cou_Type == 0) return window.location.href = url;
            if (this.course.Cou_Type == 2 && type == 'learn') {
                var obj = {
                    'url': url,
                    'ico': 'e731', 'min': false, 'showmask': true,
                    'title': '试题练习 - ' + this.course.Cou_Name,
                    'width': '1200',
                    'height': '80%'
                }
                let pbox = top.$pagebox.create(obj);
                pbox.open();
            } else
                window.location.href = url;
        }
    },
    template: `<div class="couBtnBox">
        <loading v-if="loading && islogin"></loading>
        <template v-else>
            <button v-if="!islogin" @click="url('login')">登录学习</button> 
            <template v-else-if="canstudy || forever || course.Cou_IsFree">
                <button @click="url('learn')">开始学习</button>
                <button @click="url('test')" v-if="istest" class="finaltest"><icon>&#xe810</icon>结课考试</button>
            </template>             
            <template v-else-if="course.Cou_IsLimitFree" remark="限时免费">
                <button @click="url('learn')">开始学习</button>
                <button @click="url('buy')" class="buy">选修该课程</button>
            </template>
            <template v-else-if="course.Cou_IsTry" remark="可以试学">
                <button @click="url('learn')">试学</button>
                <button @click="url('buy')" class="buy">选修该课程</button>
            </template>
            <button v-else @click="url('buy')" class="buy">选修该课程</button> 
        </template>
    </div>`
});