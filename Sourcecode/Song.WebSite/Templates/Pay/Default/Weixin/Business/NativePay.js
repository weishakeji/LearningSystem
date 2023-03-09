(function () {
    //扫码支付
    function NativePay() {

    };
    var fn = NativePay.prototype;
    /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
    fn.GetPrePayUrl = function (productId) {
        console.log(productId);
        /*
         Log.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

         WxPayData data = new WxPayData();
         data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
         data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
         data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
         data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
         data.SetValue("product_id", productId);//商品ID
         data.SetValue("sign", data.MakeSign(WxPayConfig.KEY));//签名
         string str = ToUrlParams(data.GetValues());//转换为URL串
         string url = "weixin://wxpay/bizpayurl?" + str;

         Log.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
         return url;
         */
    };
    /// <summary>
    /// 生成直接支付url，支付url有效期为2小时,模式二
    /// </summary>
    /// <param name="productId">商品ID</param>
    /// <param name="body">商品描述，此处为平台名称</param>
    /// <param name="out_trade_no">商户订单号</param>
    /// <param name="total_fee">充值金额，单位为分</param>
    /// <param name="appid">公众号id</param>
    /// <param name="mchid">微信商户id</param>
    /// <param name="paykey">微信商户支付密码</param>
    /// <param name="notify_url">回调地址</param>
    /// <param name="buyer">买家信息</param>
    /// <returns></returns>
    fn.GetPayUrl = function (productId, body, out_trade_no, total_fee, appid, mchid, paykey, notify_url, buyer) {
        /*
        Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            WxPayData data = new WxPayData();
        data.SetValue("body", body);//商品描述
        data.SetValue("attach", buyer);//附加数据
        data.SetValue("out_trade_no", out_trade_no);//商户订单号
        data.SetValue("total_fee", total_fee);//总金额
        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
        //data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
        data.SetValue("goods_tag", "null");//商品标记
        data.SetValue("trade_type", "NATIVE");//交易类型（JSAPI 公众号支付、NATIVE 扫码支付、APP APP支付）
        data.SetValue("product_id", productId);//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data, appid, mchid, paykey, WeiSha.Core.Server.IP, notify_url);
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

        Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
        return url;*/
    };
    /**
       * 参数数组转换为url格式
       * @param map 参数名与参数值的映射表
       * @return URL字符串
       */
    fn.ToUrlParams = function (map) {
        /*
          string buff = "";
          foreach (KeyValuePair<string, object> pair in map)
          {
              buff += pair.Key + "=" + pair.Value + "&";
          }
          buff = buff.Trim('&');
          return buff;*/
    };
    window.NativePay = new NativePay();
})();