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
using System.Xml.Serialization;
using System.IO;


namespace Song.Site.Manage.Course
{
    public partial class OutlineEvent_Edit : Extend.CustomPage
    {
        //章节事件ID
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //所属课程的ID
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //所属章节的ID
        private int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        //所属章节的UID
        private string uid = WeiSha.Common.Request.QueryString["uid"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {               
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind(int type)
        {
            type = type <= 1 ? 1 : type;
            //内容页代码
            ContentPlaceHolder cpl1 = (ContentPlaceHolder)Master.FindControl("cphMain");  
            for (int i = 1; i <= 4; i++)
            {
                //System.Web.UI.WebControls.contr
                System.Web.UI.WebControls.Panel p = (System.Web.UI.WebControls.Panel)cpl1.FindControl("Panel" + i);
                if (p == null) continue;
                p.Visible = i == type;
            }
        }
        protected void tblTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type;
            int.TryParse(rblTypes.SelectedValue, out type);
            InitBind(type);
        }
        private void fill()
        {
            Song.Entities.OutlineEvent obj = id < 1 ? new Song.Entities.OutlineEvent() : Business.Do<IOutline>().EventSingle(id);
            if (id < 1)
            {
                InitBind(1);
                setEventQues(id);
                setEventFeedback(id);
            }
            if (id > 0 && obj != null)
            {
                InitBind(obj.Oe_EventType);
                rblTypes.Enabled = false;
                //基础信息
                tbTitle.Text = obj.Oe_Title;
                cbIsUse.Checked = obj.Oe_IsUse;
                tbWidth.Text = obj.Oe_Width.ToString();
                tbHeight.Text = obj.Oe_Height.ToString();
                tbPoint.Text = obj.Oe_TriggerPoint.ToString();
                ListItem liType = rblTypes.Items.FindByValue(obj.Oe_EventType.ToString());
                if (liType != null)
                {
                    rblTypes.SelectedIndex = -1;
                    liType.Selected = true;
                }
            }
            //如果是“提醒”
            if (obj.Oe_EventType == 1)
            {
                tbContext1.Text = obj.Oe_Context;
            }
            //如果是“知识展示”
            if (obj.Oe_EventType == 2)
            {
                tbContext2.Text = obj.Oe_Context;
            }
            //如果是“课程提问”
            if (obj.Oe_EventType == 3)
            {
                //试题题干
                tbQuesTit.Text = obj.Oe_Context;
                //试题选项
                setEventQues(id);
                //试题答案
                //tbAnswer.Text = obj.Oe_Answer;
            }
            //如果是“实时反馈”
            if (obj.Oe_EventType == 4)
            {
                tbQuesTit4.Text = obj.Oe_Context;
                setEventFeedback(id);
            }           
           
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.OutlineEvent obj = id < 1 ? new Song.Entities.OutlineEvent() : Business.Do<IOutline>().EventSingle(id);
            if (obj == null) return;
            //基础信息
            obj.Cou_ID = couid;
            obj.Ol_ID = olid;
            obj.Ol_UID = uid;
            obj.Oe_Title = tbTitle.Text.Trim();
            obj.Oe_IsUse = cbIsUse.Checked;
            int width, height, point, type;
            int.TryParse(tbWidth.Text, out width);
            int.TryParse(tbHeight.Text, out height);
            int.TryParse(tbPoint.Text, out point);
            int.TryParse(rblTypes.SelectedValue, out type);
            obj.Oe_Width = width;
            obj.Oe_Height = height;
            obj.Oe_TriggerPoint = point;
            obj.Oe_EventType = type;
            //如果是“提醒”
            if (obj.Oe_EventType == 1)
            {
                if (tbContext1.Text.Trim().Length > 300)
                    tbContext1.Text = tbContext1.Text.Substring(0, 300);
                obj.Oe_Context = tbContext1.Text.Trim();
            }
            //如果是“知识展示”
            if (obj.Oe_EventType == 2)
            {
                obj.Oe_Context = tbContext2.Text;
            }
            //如果是“课程提问”
            if (obj.Oe_EventType == 3)
            {
                //试题题干
                obj.Oe_Context = tbQuesTit.Text.Trim();
                DataTable dt = getEventQues();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                StringWriter sw = new StringWriter();
                xmlSerial.Serialize(sw, dt); // 序列化table
                obj.Oe_Datatable = sw.ToString();
                //试题答案
                //obj.Oe_Answer = tbAnswer.Text.Trim();
            }
            //如果是“实时反馈”
            if (obj.Oe_EventType == 4)
            {
                obj.Oe_Context = tbQuesTit4.Text.Trim();
                DataTable dt = getEventFeedback();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                StringWriter sw = new StringWriter();
                xmlSerial.Serialize(sw, dt); // 序列化table
                obj.Oe_Datatable = sw.ToString();
            }
            try
            {
                if (id < 1)
                {
                    //保存
                    Business.Do<IOutline>().EventAdd(obj);
                }
                else
                {
                    Business.Do<IOutline>().EventSave(obj);
                }
                Master.AlertCloseAndRefresh("操作成功");
            }
            catch
            {
                throw;
            }

        }

        #region 私有方法
        /// <summary>
        /// 设置试题的选项内容
        /// </summary>
        /// <returns></returns>
        private void setEventQues(int oeid)
        {
            DataTable dt = Business.Do<IOutline>().EventQues(oeid);
            if (dt == null)
            {
                dt = new DataTable("EventQues");
                dt.Columns.Add(new DataColumn("iscorrect", Type.GetType("System.Boolean")));
                dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
            }
            int rowcount = dt.Rows.Count;
            int maxLine = 4;
            for (int i = maxLine - rowcount; i > rowcount; i--)
            {
                DataRow dr = dt.NewRow();
                dr["item"] = "";
                dr["iscorrect"] = false; 
                dt.Rows.Add(dr);
            }
            gvAnswer.DataSource = dt;
            gvAnswer.DataBind();
        }
        /// <summary>
        /// 设置实时反馈的选项内容
        /// </summary>
        /// <returns></returns>
        private void setEventFeedback(int oeid)
        {
            DataTable dt = Business.Do<IOutline>().EventFeedback(oeid);
            if (dt == null)
            {
                dt = new DataTable("EventFeedback");
                dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("point", Type.GetType("System.Int32")));
            }
            int rowcount = dt.Rows.Count;
            int maxLine = 6;
            for (int i = maxLine - rowcount; i > rowcount; i--)
            {
                DataRow dr = dt.NewRow();
                dr["item"] = "";
                dr["point"] = 0;
                dt.Rows.Add(dr);
            }
            rptFeedback.DataSource = dt;
            rptFeedback.DataBind();
        }
        /// <summary>
        /// 获取试题的选项内容
        /// </summary>
        /// <returns></returns>
        private DataTable getEventQues()
        {
            DataTable dt = new DataTable("EventFeedback");
            dt.Columns.Add(new DataColumn("iscorrect", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));            
            //添加选择项
            for (int i = 0; i < gvAnswer.Rows.Count; i++)
            {
                //单选钮
                RadioButton rb = (RadioButton)gvAnswer.Rows[i].FindControl("rbAns");
                //选项文本框
                TextBox tb = (TextBox)gvAnswer.Rows[i].FindControl("itemTxt");
                DataRow dr = dt.NewRow();
                dr["iscorrect"] = rb.Checked;
                dr["item"] = tb.Text;                
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 获取实时反馈的选项内容
        /// </summary>
        /// <returns></returns>
        private DataTable getEventFeedback()
        {
            DataTable dt = new DataTable("EventFeedback");
            dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("point", Type.GetType("System.Int32")));
            foreach (Control c in rptFeedback.Controls)
            {
                string item = ((TextBox)c.FindControl("tbItem")).Text;
                int point;
                int.TryParse(((TextBox)c.FindControl("tbPoint")).Text,out point);
                DataRow dr = dt.NewRow();
                dr["item"] = item;
                dr["point"] = point;                
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion


    }
}
