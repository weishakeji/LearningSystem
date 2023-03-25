// 第三方登录的配置项
$dom.load.css(['/Utilities/OtherLogin/Styles/config.css']);
Vue.component('config', {
    props: ['isuse'],
    data: function () {
        return {
            items: [
                { name: 'QQ登录', tag: 'qq', icon: 'e82a', size: 16, width: 700, height: 400, obj: {} },
                { name: '微信登录', tag: 'weixin', icon: 'e730', size: 18, width: 700, height: 550, obj: {} },
                { name: '金蝶.云之家', tag: 'yunzhijia', icon: 'e726', size: 18, width: 600, height: 550, obj: {} },
                { name: '郑州工商学院', tag: 'zzgongshang', icon: 'a006', size: 18, width: 600, height: 500, obj: {} }
            ],
            //配置项的数据记录，记录在数据库
            entities: [],
            //可用的项
            usable_items: []
        }
    },
    watch: {},
    computed: {
    },
    created: function () {
        this.get_all_items();
    },
    methods: {
        //图标地址
        logosrc: function (item) {
            return '/Utilities/OtherLogin/Images/' + item.tag + '.png';
        },
        //图标
        icon: function (item) {
            return '<icon style="font-size:"' + item.size + 'px;">&#x' + item.icon + '</icon>';
        },
        //获取所有的项
        get_all_items: function () {
            var th = this;
            th.usable_items = [];
            $api.get('OtherLogin/GetAll', { 'isuse': th.isuse }).then(function (req) {
                if (req.data.success) {
                    th.entities = req.data.result;
                    th.usable_items = th.get_usable_items();

                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                console.error(err);
            }).finally(() => {
                th.usable_items = th.get_usable_items();
            });
        },
        //获取可用的项
        get_usable_items: function () {
            var items = [];
            //将数据库中的记录，保存到配置项的obj属性
            for (let i = 0; i < this.entities.length; i++) {
                const el = this.entities[i];
                for (let j = 0; j < this.items.length; j++) {
                    if (this.items[j].tag == el.Tl_Tag) {
                        this.items[j].obj = el;
                        items.push(this.items[j]);
                    }
                }
            }
            //显示所有
            if (this.isuse == undefined || this.isuse == null) {
                for (let j = 0; j < this.items.length; j++) {
                    const el = this.items[j];
                    let index = this.entities.findIndex(t => t.Tl_Tag == el.tag);
                    if (index < 0) items.push(this.items[j]);
                }
            }
            //如果简要名称没有，则显示配置项的名称
            for (let i = 0; i < items.length; i++) {
                if (items[i].obj.Tl_Name == '')
                    items[i].obj.Tl_Name = items[i].name;
            }
            this.$emit('load', items);
            return items;
        },
        //刷新
        fresh: function (tag) {
            this.get_all_items();
        },
        //弹窗
        openbox: function (url, title, icon, width, height) {
            var obj = {};
            obj = {
                'url': url, 'ico': icon, 'title': title,
                'pid': window.name, 'showmask': true, 'min': false, 'max': false,
                'width': width ? width : 600, 'height': height ? height : 400
            }
            $pagebox.create(obj).open();
        },
        /**
         *        
         */
        //点击事件，作为跳转项
        eventClick: function (item) {
            if (JSON.stringify(item.obj) == '{}' || item.obj == undefined || item.obj == null) return;
            var evt = eval('this.event_' + item.tag + '');
            if (evt != null) evt(item);
        },
        //qq登录
        event_qq: function (item) {
            var url = 'https://graph.qq.com/oauth2.0/authorize';
            url = $api.url.set(url, {
                'client_id': item.obj.Tl_APPID,
                'response_type': 'code',
                'scope': 'all',
                'state': item.obj.Tl_Tag,
                'redirect_uri': encodeURIComponent(item.obj.Tl_Returl + '/web/sign/qq')
            });
            if ($api.ismobi()) window.location.href = url;
            else
                this.openbox(url, item.name, item.icon);
        },
        //微信登录
        event_weixin: function (item) {
            var ismobi = $api.ismobi();
            var isweixin = $api.isWeixin();//是否处于微信中
            if (ismobi && !isweixin) return alert("请在微信中打开");
            if (isweixin) {
                if (item.obj.Tl_Config != '') {
                    try {
                        var conf = eval('(' + item.obj.Tl_Config + ')');
                        for (k in conf) item.obj[k] = conf[k];
                    } catch (err) { }
                }
                var url = 'https://open.weixin.qq.com/connect/oauth2/authorize';
                url = $api.url.set(url, {
                    'appid': item.obj.pubAppid,
                    'redirect_uri': encodeURIComponent(item.obj.pubReturl + '/mobi/sign/weixin'),
                    'response_type': 'code',
                    'scope': 'snsapi_base',
                    'state': item.obj.Tl_Tag
                }) + '#wechat_redirect';
                window.location.href = url;
            } else {
                //web端
                var url = 'https://open.weixin.qq.com/connect/qrconnect';
                url = $api.url.set(url, {
                    'appid': item.obj.Tl_APPID,
                    'redirect_uri': encodeURIComponent(item.obj.Tl_Returl + '/web/sign/weixin'),
                    'response_type': 'code',
                    'scope': 'snsapi_login',
                    'state': item.obj.Tl_Tag,
                    'style': 'black',
                }) + '#wechat_redirect';
                this.openbox('/web/sign/weixinQrcode?tag=' + item.obj.Tl_Tag, item.name, item.icon);
            }

        },
        //金蝶云之家
        event_yunzhijia: function (item) {
            //是否处在云之家平台中
            var isYzjApp = navigator.userAgent.match(/Qing\/.*;(iPhone|Android).*/) ? true : false;
            if (!isYzjApp) return alert("当前应用不在云之家App中");
        },
        //郑州工商学院
        event_zzgongshang: function (item) {
            var url = 'http://172.16.31.55/auth/oauth2/authorize';
            url = $api.url.set(url, {
                'client_id': item.obj.Tl_APPID,
                'redirect_uri': encodeURIComponent(item.obj.Tl_Returl),
                'response_type': 'code',
                'state': item.obj.Tl_Tag
            });
            console.log(url);
            //http://172.16.31.55/auth/oauth2/authorize?client_id=NmMyNzcwZTAwYTAxNDliZGI2ZWI0NTI2ZjlmNzA5ZTY&redirect_uri=http%3A%2F%2Fwww.51ixuejiao.com&response_type=code&state=your_state_code
        }
    },
    template: `<div v-if="usable_items.length>0">
        <slot name="title" :items="usable_items"></slot>
        <slot v-for="(item,index) in usable_items" 
            name="item" :item="item" :index="index" :img="logosrc(item)" :icon="icon(item)">
        </slot>
    </div>
    <slot name="null" v-else></slot>`
});
