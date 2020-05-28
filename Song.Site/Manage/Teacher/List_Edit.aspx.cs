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



namespace Song.Site.Manage.Teacher
{
    public partial class List_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //员工上传资料的所有路径
        private string _uppath = "Teacher";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void init()
        {
            Song.Entities.TeacherSort[] sort = Business.Do<ITeacher>().SortAll(org.Org_ID, true);
            Ths_ID.DataSource = sort;
            Ths_ID.DataBind();
            foreach (Song.Entities.TeacherSort ts in sort)
            {
                if (ts.Ths_IsDefault)
                {
                    ListItem li = Ths_ID.Items.FindByValue(ts.Ths_ID.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
        }
        private void fill()
        {
            Song.Entities.Teacher th = id == 0 ? new Song.Entities.Teacher() : Business.Do<ITeacher>().TeacherSingle(id);
            if (id > 0) this.EntityBind(th);
            //出生年月
            Th_Birthday.Text = th.Th_Birthday < DateTime.Now.AddYears(-100) ? "" : th.Th_Birthday.ToString("yyyy-MM-dd");
            //个人照片
            if (!string.IsNullOrEmpty(th.Th_Photo) && th.Th_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + th.Th_Photo;
            }
        }
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {

            Song.Entities.Teacher th = id == 0 ? new Song.Entities.Teacher() : Business.Do<ITeacher>().TeacherSingle(id);
            th = this.EntityFill(th) as Song.Entities.Teacher;
            if (th.Th_Birthday > DateTime.Now.AddYears(-100))
            {
                th.Th_Age = Convert.ToInt32((DateTime.Now - th.Th_Birthday).TotalDays / 365);
            }
            th.Org_ID = org.Org_ID;
            th.Org_Name = org.Org_Name;
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(th.Th_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    th.Th_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //分类的名称
            if(Ths_ID.Items.Count>0)
                th.Ths_Name = Ths_ID.SelectedItem.Text;
            //判断账号是否存在
            bool isHav = Business.Do<ITeacher>().IsTeacherExist(org.Org_ID, th);
            if (isHav)
            {
                Master.Alert(string.Format("当前账号 {0} 已经存在", th.Th_AccName));
                return;
            }
            th.Th_PhoneMobi = th.Th_PhoneMobi.Trim();
            try
            {                
                if (id == 0)
                {
                    //获取教师关联的基础账号（学员账号）
                    Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(th.Th_PhoneMobi, null, null);
                    if (acc != null)
                    {
                        acc.Org_ID = th.Org_ID;
                        th.Ac_ID = acc.Ac_ID;
                    }
                    else
                    {
                        //如果基础账号不存在；
                        acc = new Song.Entities.Accounts();
                        acc.Org_ID = th.Org_ID;
                        acc.Ac_AccName = th.Th_PhoneMobi;   //账号为手机号
                        acc.Ac_MobiTel1 = th.Th_PhoneMobi;
                        acc.Ac_Name = th.Th_Name;
                        th.Th_AccName = th.Th_PhoneMobi;
                        acc.Ac_IsPass = th.Th_IsPass = true;
                        th.Th_IsShow = true;
                        acc.Ac_IsUse = th.Th_IsUse = true;
                        acc.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(th.Th_Pw).MD5;    //密码                
                        acc.Ac_Sex = th.Th_Sex;        //性别
                        acc.Ac_Birthday = th.Th_Birthday;
                        acc.Ac_Qq = th.Th_Qq;
                        acc.Ac_Email = th.Th_Email;
                        acc.Ac_IDCardNumber = th.Th_IDCardNumber;  //身份证    
                        acc.Ac_IsTeacher = true;        //该账号有教师身份
                        //保存
                        th.Ac_ID = Business.Do<IAccounts>().AccountsAdd(acc);
                    }
                    id = Business.Do<ITeacher>().TeacherAdd(th);
                }
                else
                {
                    Business.Do<ITeacher>().TeacherSave(th);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }            
        }
        private Song.Entities.Accounts _getAccount(string phone)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(phone, null, null);
            return acc;
        }
    }
}
