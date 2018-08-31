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
    /// 学习卡使用界面
    /// </summary>
    public class LearningCard : BasePage
    {
        
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                  
            }
            //
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                        //使用学习卡
                    case "useCode":
                        useCode(context);
                        break;
                        //领用学习卡
                    case "getCode":
                        getCode(context);
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

        #region 学习卡
        /// <summary>
        /// 使用学习卡
        /// </summary>
        /// <param name="context"></param>
        private void useCode(HttpContext context)
        {            
            //学习卡的编码与密钥
            string code = WeiSha.Common.Request.Form["card"].String;
            string state = "\"state\":{0},\"info\":\"{1}\",";
            string json = "\"items\":[";
            //没有传入充值码
            if (!string.IsNullOrWhiteSpace(code))
            {               
                try
                {
                    //开始验证
                    Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardCheck(code);
                    if (card != null)
                    {
                        Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                        if (st != null)
                        {
                            Business.Do<ILearningCard>().CardUse(card, st);
                            Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                            //输出关联的课程
                            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesForCard(card.Lc_Code, card.Lc_Pw);
                            for (int i = 0; i < courses.Length; i++)
                            {
                                Song.Entities.Course c = courses[i];
                                json += c.ToJson("Cou_ID,Cou_Name", null, null) + ",";
                            }
                            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                        }                       
                    }
                    state = string.Format(state, 1, "成功");
                }
                catch (Exception ex)
                {
                    state = string.Format(state, 0, ex.Message);                   
                }
            }
            json += "]";
            Response.Write("({" + state + json + "})");
            this.Response.End();
        }
        /// <summary>
        /// 将学习卡暂存名下
        /// </summary>
        /// <param name="context"></param>
        private void getCode(HttpContext context)
        {
            //学习卡的编码与密钥
            string code = WeiSha.Common.Request.Form["card"].String;
            string state = "\"state\":{0},\"info\":\"{1}\",";
            string json = "\"items\":[";
            //没有传入充值码
            if (!string.IsNullOrWhiteSpace(code))
            {
                //开始验证
                try
                {
                    Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardCheck(code);
                    if (card != null)
                    {
                        Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                        if (st != null)
                        {
                            Business.Do<ILearningCard>().CardGet(card, st);
                            Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                            state = string.Format(state, 1, "成功");
                        }

                    }
                }
                catch (Exception ex)
                {
                    state = string.Format(state, 0, ex.Message);     
                }
            }
           json += "]";
           Response.Write("({" + state + json + "})");
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
  
    }
}