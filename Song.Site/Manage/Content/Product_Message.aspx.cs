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

namespace Song.Site.Manage.Content
{
    public partial class Product_Message : Extend.CustomPage
    {
        //产品的ID
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null,null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                Song.Entities.ProductMessage[] eas = null;
                eas = Business.Do<IProduct>().GetProductMessagePager(id, this.tbSear.Text, null, null, Pager1.Size, Pager1.Index, out count);
                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Pm_id" };
                GridView1.DataBind();
                Pager1.RecordAmount = count;
                if (eas.Length > 0)
                {
                    //如果是第一次，则绑定列表的第一个
                    if (lbId.Text == "")
                    {
                        fillMessage(eas[0].Pm_Id);
                    }
                    else
                    {
                        //如果不是第一次绑定，例如修改数据后，取当前留言id
                        fillMessage(Convert.ToInt32(lbId.Text));
                    }
                }
                else
                {
                    btnEnter.Enabled = btnDell.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 填充产品留言的信息
        /// </summary>
        /// <param name="pmid">留言id</param>
        private void fillMessage(int pmid)
        {
            try
            {
                //获取当前留言对象
                Song.Entities.ProductMessage message = Business.Do<IProduct>().MessageSingle(pmid);
                if (message == null) return;
                //留言标题与内容
                lbId.Text = message.Pm_Id.ToString();
                tbText.Text = message.Pm_Title;
                tbContext.Text = message.Pm_Context;
                //时间与ip
                lbCrtTime.Text = message.Pm_CrtTime.ToString();
                lbIP.Text = message.Pm_IP;
                //邮箱与地址
                lbEmail.Text = message.Pm_Email;
                lbAddress.Text = message.Pm_Address;
                lbPhone.Text = message.Pm_Phone;
                //回复
                tbAnswer.Text = message.Pm_Answer;
                //是否显示在前台
                cbIsShow.Checked = message.Pm_IsShow;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }       
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.ProductMessage entity = Business.Do<IProduct>().MessageSingle(id);
                entity.Pm_IsShow = !entity.Pm_IsShow;
                Business.Do<IProduct>().MessageSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 将当前留言信息处于编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void showMsg_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                fillMessage(id);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }  
        /// <summary>
        /// 修改留言信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbId.Text == "") return;
                int pmid = Convert.ToInt32(lbId.Text);
                //获取当前留言对象
                Song.Entities.ProductMessage message = Business.Do<IProduct>().MessageSingle(pmid);
                if (message == null) return;
                //留言标题与内容
                message.Pm_Title = tbText.Text.Trim();
                message.Pm_Context = tbContext.Text.Trim();
                //回复
                message.Pm_Answer = tbAnswer.Text.Trim();
                //是否显示在前台
                message.Pm_IsShow = cbIsShow.Checked;
                //保存
                Business.Do<IProduct>().MessageSave(message);
                BindData(null, null);
                Master.Alert("操作成功");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<IProduct>().MessageDelete(id);
                lbId.Text = "";
                BindData(null, null);
                Master.Alert("操作成功");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
