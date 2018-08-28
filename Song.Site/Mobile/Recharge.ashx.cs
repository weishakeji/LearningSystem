using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.IO;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 充值界面
    /// </summary>
    public class Recharge : BasePage
    {
        
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //支付接口
                interFaceList();   
            }
            //
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                        //验证充值码
                    case "paycard":
                        veriMoneyCode(context);
                        break;
                    case "decode_qrcode":
                        decode_qrcode(context);
                        break;
                }
            }   
                
           
        }
        #region 解析二维码
        /// <summary>
        /// 解析二维码
        /// </summary>
        /// <param name="context"></param>
        private void decode_qrcode(HttpContext context)
        {
            string ret = string.Empty;
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFile file = context.Request.Files[0];
                    //文件流转二进制
                    Stream stream = file.InputStream;
                    byte[] photo = new byte[file.ContentLength];
                    stream.Read(photo, 0, file.ContentLength);
                    stream.Close();
                    //二进制转图片对象
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(photo);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    //解析二维码
                    ret = WeiSha.Common.QrcodeHepler.Decode(img);
                }
                catch
                {
                }
            }
            //
            context.Response.Write(ret);
            context.Response.End();

        }
        #endregion

        #region 验证充值码
        /// <summary>
        /// 校验充值码
        /// </summary>
        /// <param name="context"></param>
        private void veriMoneyCode(HttpContext context)
        {            
            //充值码
            string card = WeiSha.Common.Request.Form["card"].String;
            //没有传入充值码
            if (string.IsNullOrWhiteSpace(card))
            {
                this.Response.Write("0");
                this.Response.End();
                return;
            }           
            //开始验证
            try
            {
                Song.Entities.RechargeCode code = Business.Do<IRecharge>().CouponCheckCode(card);
                if (code != null)
                {
                    Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                    code.Ac_ID = st.Ac_ID;
                    code.Ac_AccName = st.Ac_AccName;
                    Business.Do<IRecharge>().CouponUseCode(code);
                    Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                    this.Response.Write("1");
                }
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);
            }
            this.Response.End();
        }
        /// <summary>
        /// 增加地址的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private string addPara(string url, params string[] para)
        {
            return WeiSha.Common.Request.Page.AddPara(url, para);
        }
        #endregion

        #region 在线支付接口
        private void interFaceList()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
            Song.Entities.PayInterface[] pis = Business.Do<IPayInterface>().PayAll(org.Org_ID, "mobi", true);
            this.Document.Variables.SetValue("pis", pis);
        }
        #endregion
    }
}