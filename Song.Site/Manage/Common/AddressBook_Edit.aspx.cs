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
using System.Collections.Generic;


namespace Song.Site.Manage.Common
{
    public partial class AddressBook_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bindSort();
                fill();
            }
        }
        private void bindSort()
        {
            try
            {
                List<Song.Entities.AddressSort> asort = Business.Do<IAddressList>().SortAll(true);
                ddlTpye.DataSource = asort;
                ddlTpye.DataTextField = "ads_name";
                ddlTpye.DataValueField = "ads_id";
                ddlTpye.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                Song.Entities.AddressList mm;
                if (id != 0)
                {
                    mm = Business.Do<IAddressList>().AddressSingle(id);
                    //生日
                    if (mm.Adl_Birthday > Convert.ToDateTime("1900-01-01"))
                    {
                        tbBirthday.Text = ((DateTime)mm.Adl_Birthday).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.AddressList();
                }
                tbName.Text = mm.Adl_Name;
                //分类
                ListItem li = ddlTpye.Items.FindByText(mm.Ads_Name);
                if (li != null) li.Selected = true;
                //性别
                ListItem liSex = rblSex.Items.FindByValue(mm.Adl_Sex.ToString());
                if (liSex != null) liSex.Selected = true;
                //电话
                tbTel.Text = mm.Adl_Tel;
                tbCoTel.Text = mm.Adl_CoTel;
                tbMobileTel.Text = mm.Adl_MobileTel;
                //公司，家庭地址，邮编
                tbCompany.Text = mm.Adl_Company;
                tbAddress.Text = mm.Adl_Address;
                tbZip.Text = mm.Adl_Zip;
                //
                tbEmail.Text = mm.Adl_Email;
                tbQQ.Text = mm.Adl_QQ;
                tbMsn.Text = mm.Adl_Msn;
                tbBlog.Text = mm.Adl_Blog;
                //爱好
                if (mm.Adl_Like != "" && mm.Adl_Like != null)
                {
                    if (mm.Adl_Like.IndexOf(",") > -1)
                    {
                        foreach (string str in mm.Adl_Like.Split(','))
                        {
                            ListItem liLike = cbLike.Items.FindByText(str);
                            if (liLike != null)
                            {
                                liLike.Selected = true;
                            }
                        }
                    }
                }
                //简介
                tbIntro.Text = mm.Adl_Intro;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
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
                Song.Entities.AddressList mm;
                if (id != 0)
                {
                    mm = Business.Do<IAddressList>().AddressSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.AddressList();
                }
                mm.Adl_Name = tbName.Text;
                //分类
                mm.Ads_Name = ddlTpye.SelectedItem.Text;
                //性别
                mm.Adl_Sex = Convert.ToInt16(rblSex.SelectedItem.Value);
                //电话
                mm.Adl_Tel = tbTel.Text;
                mm.Adl_CoTel = tbCoTel.Text;
                mm.Adl_MobileTel = tbMobileTel.Text;
                //公司，家庭地址，邮编
                mm.Adl_Company = tbCompany.Text;
                mm.Adl_Address = tbAddress.Text;
                mm.Adl_Zip = tbZip.Text;
                //
                mm.Adl_Email = tbEmail.Text;
                mm.Adl_QQ = tbQQ.Text;
                mm.Adl_Msn = tbMsn.Text;
                mm.Adl_Blog = tbBlog.Text;
                //爱好
                string str = "";
                foreach (ListItem liLike in cbLike.Items)
                {
                    if (liLike.Selected)
                    {
                        str += liLike.Text + ",";
                    }
                }
                mm.Adl_Like = str;
                //生日
                if (tbBirthday.Text.Trim() == "")
                {
                    mm.Adl_Birthday = Convert.ToDateTime("1900-01-01");
                }
                else
                {
                    mm.Adl_Birthday = Convert.ToDateTime(tbBirthday.Text);
                }
                //简介
                mm.Adl_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IAddressList>().AddressAdd(mm);
                }
                else
                {
                    Business.Do<IAddressList>().AddressSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
