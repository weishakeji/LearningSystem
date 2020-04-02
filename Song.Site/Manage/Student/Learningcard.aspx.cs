using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.IO;

namespace Song.Site.Manage.Student
{
    public partial class Learningcard : Extend.CustomPage
    {
        Song.Entities.Organization org;
        //当前学员的学习卡数量，以及使用数
        protected int cardcount, usecount;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            if (st == null) return;

            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                org = Business.Do<IOrganization>().OrganCurrent();
                if (!this.IsPostBack)
                {
                    //当前学员的学习卡数量
                    int accid = Extend.LoginState.Accounts.UserID;
                    if (accid > 0)
                    {
                        cardcount = Business.Do<ILearningCard>().AccountCardOfCount(accid);
                        usecount = cardcount - Business.Do<ILearningCard>().AccountCardOfCount(accid, 0);
                        //学员的学习卡
                        Song.Entities.LearningCard[] cards = Business.Do<ILearningCard>().AccountCards(accid);
                        rptCards.DataSource = cards;
                        rptCards.DataBind();
                    }
                }

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
                        useCode(this.Context);
                        break;
                    //领用学习卡
                    case "getCode":
                        getCode(this.Context);
                        break;

                }
            }
        }
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
