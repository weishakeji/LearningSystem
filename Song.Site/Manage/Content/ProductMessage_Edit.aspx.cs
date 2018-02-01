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
    public partial class ProductMessage_Edit : Extend.CustomPage
    {
        //产品留言的ID
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string _uppath = "Product";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fillMessage();
            }
        }
        /// <summary>
        /// 填充产品留言的信息
        /// </summary>
        private void fillMessage()
        {
            //获取当前留言对象
            Song.Entities.ProductMessage message = Business.Do<IProduct>().MessageSingle(id);
            if (message == null) return;
            fillProduct(message);
            //留言标题与内容
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
        /// <summary>
        /// 设置初始产品留言相联的产品信息
        /// </summary>
        /// <param name="message">产品留言的实体</param>
        private void fillProduct(Song.Entities.ProductMessage message)
        {
            Song.Entities.Product p = Business.Do<IContents>().ProductSingle((int)message.Pd_Id);
            if (p == null) return;
            //产品名称
            lbName.Text = p.Pd_Name;
            //产品分类
            //lbColumn.Text = p.Ps_Name;
            //型号
            lbModel.Text = p.Pd_Model;
            //编号
            lbCode.Text = p.Pd_Code;
            //是否使用,是否新产品，是否推荐
            lbIsUse.Visible = !p.Pd_IsUse;
            lbIsNew.Visible = p.Pd_IsNew;
            lbIsRec.Visible = p.Pd_IsRec;
            //上市时间
            lbPushTime.Text = ((DateTime)p.Pd_PushTime).ToString("yyyy-MM-dd");
            //图片
            this.imgShow.Src = Upload.Get[_uppath].Virtual + p.Pd_Logo;
            //编辑
            lbAccName.Text = p.Acc_Name; 
            //价格
            lbPrise.Text = p.Pd_Prise.ToString();
            //重量
            lbWeight.Text = p.Pd_Weight.ToString();
            //库存
            lbStocks.Text = p.Pd_Stocks.ToString();
            lbUnit.Text = p.Pd_Unit;
           //简介
            lbIntro.Text = p.Pd_Intro;
            //内容
            lbDetails.Text = p.Pd_Details;
            //关键字
            lbKeywords.Text = p.Pd_KeyWords;           
            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //获取当前留言对象
                Song.Entities.ProductMessage message = Business.Do<IProduct>().MessageSingle(id);
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
                Master.AlertCloseAndRefresh("操作成功！");
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
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
