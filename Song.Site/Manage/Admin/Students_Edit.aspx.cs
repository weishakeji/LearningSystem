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



namespace Song.Site.Manage.Admin
{
    public partial class Students_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //员工上传资料的所在路径
        private string _uppath = "Student";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void init()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.StudentSort[] sort = Business.Do<IStudent>().SortAll(org.Org_ID, true);
            Sts_ID.DataSource = sort;
            Sts_ID.DataBind();
            foreach (Song.Entities.StudentSort ts in sort)
            {
                if (ts.Sts_IsDefault)
                {
                    ListItem li = Sts_ID.Items.FindByValue(ts.Sts_ID.ToString());
                    if (li != null) li.Selected = true;
                }
            }
        }
        private void fill()
        {
            Song.Entities.Accounts th = id == 0 ? new Song.Entities.Accounts() : Business.Do<IAccounts>().AccountsSingle(id);
            if (th == null) return;
            this.EntityBind(th);
            //出生年月
            Ac_Birthday.Text = th.Ac_Birthday < DateTime.Now.AddYears(-100) ? "" : th.Ac_Birthday.ToString("yyyy-MM-dd");
            //个人照片
            if (!string.IsNullOrEmpty(th.Ac_Photo) && th.Ac_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + th.Ac_Photo;
            }
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {            
            //
            Song.Entities.Accounts st = id == 0 ? new Song.Entities.Accounts() : Business.Do<IAccounts>().AccountsSingle(id);
            st = this.EntityFill(st) as Song.Entities.Accounts;
            if (st.Ac_Birthday > DateTime.Now.AddYears(-100))
            {
                st.Ac_Age = Convert.ToInt32((DateTime.Now - st.Ac_Birthday).TotalDays / 365);
            }
            if (id == 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org == null) throw new WeiSha.Common.ExceptionForAlert("当前机构不存在");
                st.Org_ID = org.Org_ID;
            }
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(st.Ac_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    st.Ac_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //分类的名称
            if (Sts_ID.Items.Count > 0)
                st.Sts_Name = Sts_ID.SelectedItem.Text;
            //判断是否存在
            Song.Entities.Accounts t = Business.Do<IAccounts>().IsAccountsExist(-1, st);
            if (t!=null)
            {
                Master.Alert(string.Format("当前学员账号 {0} 已经存在", st.Ac_AccName));
                return;
            }
            try
            {
                if (id == 0)
                {
                    id = Business.Do<IAccounts>().AccountsAdd(st);
                }
                else
                {
                    Business.Do<IAccounts>().AccountsSave(st);
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
