using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Data;
using WxPayAPI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 支付接口
    /// </summary>
    [HttpPut, HttpGet]
    public class Pay : ViewMethod, IViewAPI
    {
        #region 增删改查
        /// <summary>
        /// 支付接口实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.PayInterface ForID(int id)
        {
            return Business.Do<IPayInterface>().PaySingle(id);
        }
        /// <summary>
        /// 计算某一个支付接口的收入
        /// </summary>
        /// <param name="id">支付接口的id</param>
        /// <returns></returns>
        public decimal Summary(int id)
        {
            return Business.Do<IPayInterface>().Summary(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Add(Song.Entities.PayInterface entity)
        {
            try
            {
                PayInterface pi = Business.Do<IPayInterface>().PayIsExist(-1, entity, 2);
                if (pi != null) throw new Exception("支付接口名称重复");
                Business.Do<IPayInterface>().PayAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(PayInterface entity)
        {
            PayInterface pi = Business.Do<IPayInterface>().PayIsExist(-1, entity, 2);
            if (pi != null) throw new Exception("支付接口名称重复");

            Song.Entities.PayInterface old = Business.Do<IPayInterface>().PaySingle(entity.Pai_ID);
            if (old == null) throw new Exception("Not found entity for PayInterface！");
            old.Copy<Song.Entities.PayInterface>(entity);
            Business.Do<IPayInterface>().PaySave(old);
            return true;
        }
        /// <summary>
        /// 接口是否存在（接口名称不得重复）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="scope">查询范围，1查询所有接口；2同一类型（Pai_InterfaceType），同一设备端（Pai_Platform），不重名</param>
        /// <returns>重名返回true</returns>
        [HttpPost]
        public bool IsExist(PayInterface entity, int scope)
        {
            PayInterface pi = Business.Do<IPayInterface>().PayIsExist(-1, entity, scope);
            return pi != null;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IPayInterface>().PayDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 所有接口
        /// </summary>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <returns>专业列表</returns>
        [SuperAdmin]
        public Song.Entities.PayInterface[] List(string platform)
        {
            return Business.Do<IPayInterface>().PayAll(-1, platform, null);
        }
        /// <summary>
        /// 所有可用接口
        /// </summary>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <returns></returns>
        public Song.Entities.PayInterface[] ListEnable(string platform)
        {
            return Business.Do<IPayInterface>().PayAll(-1, platform, true);
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool ModifyTaxis(Song.Entities.PayInterface[] items)
        {
            try
            {
                Business.Do<IPayInterface>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 支付
        /// <summary>
        /// 生成资金流水的记录
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="payif">支付接口配置的实体对象</param>
        /// <returns>资金流水的记录项，Ma_IsSuccess默认为false</returns>
        [Student][HttpPost]
        public MoneyAccount MoneyIncome(double money,Song.Entities.PayInterface payif)
        {
            if (money <= 0) return null;

            Song.Entities.Accounts acc = this.User;
            //产生支付流水号
            MoneyAccount ma = new MoneyAccount();
            ma.Ma_Money = (decimal)money;
            ma.Ac_ID = acc.Ac_ID;
            ma.Ma_Source = payif.Pai_Pattern;
            ma.Pai_ID = payif.Pai_ID;
            ma.Ma_From = 3;
            ma.Ma_IsSuccess = false;
            ma = Business.Do<IAccounts>().MoneyIncome(ma);
            return ma;
        }
        /// <summary>
        /// 通过流水号获取资金记录
        /// </summary>
        /// <param name="serial">流水号</param>
        /// <returns></returns>
        public MoneyAccount MoneyAccount(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial)) return null;
            return Business.Do<IAccounts>().MoneySingle(serial);          
        }
        /// <summary>
        /// 通过id获取支付接口(启用的）,相较于ForID方法，此处取启用的接口
        /// </summary>
        /// <param name="id">支付接口id</param>
        /// <returns>支持接口</returns>
        public Song.Entities.PayInterface Interface(int id)
        {
            Song.Entities.PayInterface pi= Business.Do<IPayInterface>().PaySingle(id);
            return pi.Pai_IsEnable ? pi : null;
        }

        #endregion

        #region 微信支付
        /// <summary>
        /// 微信支付订单查询
        /// </summary>
        /// <param name="serial">资金流水号，由我方系统生成的订单号</param>
        /// <param name="appid">公众号id</param>
        /// <param name="mchid">微信商户id</param>
        /// <param name="paykey">支付密钥</param>
        /// <returns></returns>
        public JObject WxOrderQuery(string serial, string appid, string mchid, string paykey)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", serial);  //商户订单号
            WxPayData result = WxPayApi.OrderQuery(data, appid, mchid, paykey);//提交订单查询请求给API，接收返回数据
            JObject jo= result.ToJObject();
            WxPayAPI.Log.Debug("WxPayApi", "OrderQuery trade_state : " + jo["trade_state"].ToString());
            //订单成功
            if (jo["trade_state"].ToString()== "SUCCESS")
            {
                Song.Entities.MoneyAccount macc = Business.Do<IAccounts>().MoneySingle(serial);
                if(macc != null && !macc.Ma_IsSuccess)
                {
                    //付款方与收款方（商户id)
                    macc.Ma_Buyer = jo["attach"].ToString();
                    macc.Ma_Seller = jo["mch_id"].ToString();
                    Business.Do<IAccounts>().MoneyConfirm(macc);
                    //刷新当前登录的学员信息
                    Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(macc.Ac_ID);
                    Song.ViewData.LoginAccount.Fresh(acc);
                }
                //WxPayAPI.Log.Debug("WxPayApi", "OrderQuery response : " + response);
            }
            return result.ToJObject();
        }
        /// <summary>
        /// 生成扫码支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="body">商品描述，此处为平台名称</param>
        /// <param name="serial">商户订单号</param>
        /// <param name="total_fee">充值金额，单位为分</param>
        /// <param name="appid">公众号id</param>
        /// <param name="mchid">微信商户id</param>
        /// <param name="paykey">微信商户支付密码</param>
        /// <param name="notify_url">回调地址</param>
        /// <param name="buyer">买家信息</param>
        /// <returns></returns>
        public string WxNativePayUrl(string productId, string body, string serial, int total_fee, string appid, string mchid, string paykey, string notify_url, string buyer)
        {
            NativePay nativePay = new NativePay();
            return nativePay.GetPayUrl(productId, body, serial, total_fee, appid, mchid, paykey, notify_url, buyer);          
        }
        /// <summary>
        /// 获取token和openid
        /// </summary>
        /// <returns>access_token,openid</returns>
        public JObject WxOpenidAndAccessTokenFromCode(string appid, string secret, string code)
        {
            try
            {
                //构造获取openid及access_token的url
                WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();
                data.SetValue("appid", appid);
                data.SetValue("secret", secret);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();
                //请求url以获取数据
                string result = WxPayAPI.HttpService.Get(url);
                return JObject.Parse(result);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayAPI.WxPayException(ex.ToString());
            }
        }
        /// <summary>
        /// 生成html5支付的对象
        /// </summary>
        /// <param name="tracetype">支付方式，（JSAPI 公众号支付、NATIVE 扫码支付、APP APP支付、MWEB H5支付）</param>
        /// <param name="body">商品描述，此处为平台名称</param>
        /// <param name="serial">商户订单号</param>
        /// <param name="openid"></param>
        /// <param name="total_fee">充值金额，单位为分</param>
        /// <param name="appid">公众号id</param>
        /// <param name="mchid">微信商户id</param>
        /// <param name="paykey">微信商户支付密码</param>
        /// <param name="notify_url">回调地址</param>
        /// <param name="buyer">买家信息</param>
        /// <returns></returns>
        public JObject WxJsApiPay(string tracetype, string body, string serial, string openid, int total_fee, string appid, string mchid, string paykey, string notify_url, string buyer)
        {
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            jsApiPay.openid = openid;
            jsApiPay.total_fee = total_fee;
            //JSAPI支付预处理
            try
            {
                //统一下单
                WxPayData result = jsApiPay.GetUnifiedOrderResult(tracetype, body, serial, appid, mchid, paykey, notify_url, buyer);
                if (result == null) throw new Exception();
                WxPayAPI.Log.Info(this.GetType().ToString(), tracetype + "回调地址 : " + notify_url);
                WxPayAPI.Log.Info(this.GetType().ToString(), tracetype + "支付下单 : " + result.ToJson());
                //如果是公众号支付
                if (tracetype == "JSAPI")
                {
                    string wxJsApiParam = jsApiPay.GetJsApiParameters(paykey);// 用于前端js调用
                    JObject jo = JObject.Parse(wxJsApiParam);
                    return jo;
                }
                // if (tracetype == "MWEB")
                return result.ToJObject();
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), "支付下单失败 : " + ex.Message);
                WxPayAPI.Log.Error(this.GetType().ToString(), "支付下单失败 : " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
    }
}
