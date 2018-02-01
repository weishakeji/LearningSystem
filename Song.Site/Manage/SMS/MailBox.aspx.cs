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


namespace Song.Site.Manage.SMS
{
    public partial class MailBox : Extend.CustomPage
    {
        //短信信息分类，1为草稿箱，2为已发送，3为垃圾箱
        private int box = WeiSha.Common.Request.QueryString["box"].Int16 ?? 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
       
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            string search = this.tbSear.Text.Trim();
            Song.Entities.SmsMessage[] eas = null;
            eas = Business.Do<ISMS>().MessagePager(null,box,null, search, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "SMS_Id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string getType(string type)
        {
            switch (Convert.ToInt16(type))
            {
                case 1:
                    return "通讯录";
                case 2:
                    return "个人";
                case 3:
                    return "员工";
            }
            return "";
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            //如果是垃圾箱，直接删除
            if (box == 3)
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<ISMS>().MessageDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            //如果是已发送，或草稿
            if (box == 1 || box == 2)
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Song.Entities.SmsMessage sms = Business.Do<ISMS>().GetSingle(Convert.ToInt16(id));
                    sms.Sms_MailBox = 3;
                    Business.Do<ISMS>().MessageSave(sms);
                }
                BindData(null, null);
            }
        }
    }
}
