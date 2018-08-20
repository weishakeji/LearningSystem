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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Card
{
    public partial class Learningcard_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            tbStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tbEnd.Text = DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd");
            tbPw.Enabled = id < 1;
        }
        void fill()
        {
            Song.Entities.RechargeSet mm;
            if (id != 0)
            {
                mm = Business.Do<IRecharge>().RechargeSetSingle(id);
                cbIsEnable.Checked = mm.Rs_IsEnable;
                tbPw.Text = mm.Rs_Pw;
                //有效期
                tbStart.Text = mm.Rs_LimitStart.ToString("yyyy-MM-dd");
                tbEnd.Text = mm.Rs_LimitEnd.ToString("yyyy-MM-dd");
                //主题
                tbTheme.Text = mm.Rs_Theme;
                //数量，面额
                tbCount.Text = mm.Rs_Count.ToString();
                tbPrice.Text = mm.Rs_Price.ToString();
                //简介
                tbIntro.Text = mm.Rs_Intro;
                //充值码长度
                tbCodeLength.Text = mm.Rs_CodeLength.ToString();
                tbPwLength.Text = mm.Rs_PwLength.ToString();
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.RechargeSet();
                tbPw.Text = getUID();
            }
            
            
        }       
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.RechargeSet mm = id != 0 ? Business.Do<IRecharge>().RechargeSetSingle(id) : new Song.Entities.RechargeSet();           
            //主题
            mm.Rs_Theme = tbTheme.Text.Trim();
            //数量，面额
            int count = 0, price = 0;
            int.TryParse(tbCount.Text, out count);
            int.TryParse(tbPrice.Text, out price);
            mm.Rs_Count = count;
            mm.Rs_Price = price;            
            //有效期
            DateTime start = DateTime.Now, end = DateTime.Now;
            DateTime.TryParse(tbStart.Text, out start);
            DateTime.TryParse(tbEnd.Text, out end);
            mm.Rs_LimitStart = start;
            mm.Rs_LimitEnd = end;
            //简介
            mm.Rs_Intro = tbIntro.Text.Trim();
            //密钥，是否禁用
            mm.Rs_Pw = tbPw.Text.Trim();
            mm.Rs_IsEnable = cbIsEnable.Checked;
            //充值码长度
            mm.Rs_CodeLength = Convert.ToInt32(tbCodeLength.Text);
            mm.Rs_PwLength = Convert.ToInt32(tbPwLength.Text);
            //确定操作
            try
            {
                if (id == 0)
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    mm.Org_ID = org.Org_ID;
                    Business.Do<IRecharge>().RechargeSetAdd(mm);
                }
                else
                {
                    Business.Do<IRecharge>().RechargeSetSave(mm);
                }
                
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
       
    }
}
