
//转向支付平台的方法
Vue.component('topayment', {
    //interface:接口对象
    //moneyrecord: 资金流水的对象
    props: ['interface', 'moneyrecord'],
    data: function () {
        return {

        }
    },
    watch: {
        'moneyrecord': {
            'handler': function (nv, ov) {
                //调用转向支付平台的方法
                if (this.ifexist) {
                    var type = this.interface.Pai_InterfaceType;
                    if (type == '' || type == null) return;
                    type = type.toLowerCase();
                    //用Pai_InterfaceType属性，作为方法名，传递接口与流水号的对象，生成相应参数，转向支付平台
                    var func = eval('this.' + type);
                    if (func == null) return;
                    func(this.interface, nv);

                }
            }, 'immediate': true
        }
    },
    computed: {
        //支付接口是否存在
        ifexist: function () {
            return JSON.stringify(this.interface) != '{}' && this.interface != null && this.interface.Pai_IsEnable == true
                && this.interface.Pai_InterfaceType != '';
        }
    },
    mounted: function () { },
    methods: {
        //微信公众号支付
        weixinpubpay: function (pi, ma) {
            //回调路径
            var redirect_uri = $api.url.host() + "Pay/Weixin/PublicPay.aspx";
            console.log(redirect_uri);
            redirect_uri= encodeURIComponent(redirect_uri);
            //返回的状态值，接口id、流水号        
            var state = "pi:{0},serial:{1}";
            state=state.format(pi.Pai_ID,ma.Ma_Serial);          
            //构建参数
            var data = {
                "appid": pi.Pai_ParterID,
                "redirect_uri": redirect_uri,
                "response_type": "code",
                "scope": "snsapi_base",
                "state": state + "#wechat_redirect"
            };
            var url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + this.tourl(data);
            window.location.href = url;
            console.log(url);
        },
        //微信扫码支付
        weixinnativepay: function (pi, ma) {
            /*
            string host = System.Web.HttpContext.Current.Request.Url.Host + ":" + WeiSha.Core.Server.Port + "/";
            if (!string.IsNullOrWhiteSpace(pi.Pai_Returl)) host = pi.Pai_Returl;
            if (!host.EndsWith("/")) host += "/";
            //用于生成支付二维码的URL          
            string url = "Pay/Weixin/NativePayPage.aspx?pi={0}&serial={1}";
            url = host + string.Format(url.ToString(), pi.Pai_ID, ma.Ma_Serial);
            System.Web.HttpContext.Current.Response.Redirect(url);
            */

        },
        //微信小程序支付
        weixinapppay: function (pi, ma) {
            /*
            string url = "/pay/Weixin/miniProgramPay.aspx?piid={0}&serial={1}&money={2}&org={3}";
            url = string.Format(url, pi.Pai_ID, ma.Ma_Serial, money * 100, ma.Org_ID);
            
            //调试
            string msg = string.Format("小程序支付，第一步（启动）：\r\n接口ID：{0}，流水号：{1}，金额：{2}元", pi.Pai_ID, ma.Ma_Serial, money);
            WxPayAPI.Log.Info(this.GetType().FullName, msg);

            System.Web.HttpContext.Current.Response.Redirect(url);
            */
        },
        //微信Html5支付
        weixinh5pay: function (pi, ma) {
            /*
            string url = "Weixin/Html5Pay.aspx?piid={0}&serial={1}&money={2}";
            url = string.Format(url, pi.Pai_ID, ma.Ma_Serial, money*100);
            System.Web.HttpContext.Current.Response.Redirect(url);
            */
        },

        tourl: function (obj) {
            var buff = "";
            for (let key in obj)
                buff += key + "=" + obj[key] + "&";
            if (buff.substring(buff.length - 1) == '&')
                buff = buff.substring(0, buff.length - 1);
            return buff;
        },
        /* 支付宝支付 */
        //支付宝手机支付
        alipaywap: function (pi, ma) {

        },
        //支付宝网页直付
        alipayweb: function (pi, ma) {

        },
    },
    template: `<loading>正在转向支付平台..</loading>`
});
