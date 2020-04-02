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
        //��ǰѧԱ��ѧϰ���������Լ�ʹ����
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
                    //��ǰѧԱ��ѧϰ������
                    int accid = Extend.LoginState.Accounts.UserID;
                    if (accid > 0)
                    {
                        cardcount = Business.Do<ILearningCard>().AccountCardOfCount(accid);
                        usecount = cardcount - Business.Do<ILearningCard>().AccountCardOfCount(accid, 0);
                        //ѧԱ��ѧϰ��
                        Song.Entities.LearningCard[] cards = Business.Do<ILearningCard>().AccountCards(accid);
                        rptCards.DataSource = cards;
                        rptCards.DataBind();
                    }
                }

            }
            //
            //��ҳ���ajax�ύ��ȫ��������POST��ʽ
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    //ʹ��ѧϰ��
                    case "useCode":
                        useCode(this.Context);
                        break;
                    //����ѧϰ��
                    case "getCode":
                        getCode(this.Context);
                        break;

                }
            }
        }
        #region ѧϰ��
        /// <summary>
        /// ʹ��ѧϰ��
        /// </summary>
        /// <param name="context"></param>
        private void useCode(HttpContext context)
        {
            //ѧϰ���ı�������Կ
            string code = WeiSha.Common.Request.Form["card"].String;
            string state = "\"state\":{0},\"info\":\"{1}\",";
            string json = "\"items\":[";
            //û�д����ֵ��
            if (!string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    //��ʼ��֤
                    Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardCheck(code);
                    if (card != null)
                    {
                        Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                        if (st != null)
                        {
                            Business.Do<ILearningCard>().CardUse(card, st);
                            Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                            //��������Ŀγ�
                            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesForCard(card.Lc_Code, card.Lc_Pw);
                            for (int i = 0; i < courses.Length; i++)
                            {
                                Song.Entities.Course c = courses[i];
                                json += c.ToJson("Cou_ID,Cou_Name", null, null) + ",";
                            }
                            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                        }
                    }
                    state = string.Format(state, 1, "�ɹ�");
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
        /// ��ѧϰ���ݴ�����
        /// </summary>
        /// <param name="context"></param>
        private void getCode(HttpContext context)
        {
            //ѧϰ���ı�������Կ
            string code = WeiSha.Common.Request.Form["card"].String;
            string state = "\"state\":{0},\"info\":\"{1}\",";
            string json = "\"items\":[";
            //û�д����ֵ��
            if (!string.IsNullOrWhiteSpace(code))
            {
                //��ʼ��֤
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
                            state = string.Format(state, 1, "�ɹ�");
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
        /// ���ӵ�ַ�Ĳ���
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
