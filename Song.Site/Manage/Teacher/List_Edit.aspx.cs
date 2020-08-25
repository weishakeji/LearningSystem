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
        //Ա���ϴ����ϵ�����·��
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
            //��������
            Th_Birthday.Text = th.Th_Birthday < DateTime.Now.AddYears(-100) ? "" : th.Th_Birthday.ToString("yyyy-MM-dd");
            //������Ƭ
            if (!string.IsNullOrEmpty(th.Th_Photo) && th.Th_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + th.Th_Photo;
            }
        }
        /// <summary>
        /// ���水ť�¼�
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
            //ͼƬ
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
            //���������
            if(Ths_ID.Items.Count>0)
                th.Ths_Name = Ths_ID.SelectedItem.Text;
            //�ж��˺��Ƿ����
            bool isHav = Business.Do<ITeacher>().IsTeacherExist(org.Org_ID, th);
            if (isHav)
            {
                Master.Alert(string.Format("��ǰ�˺� {0} �Ѿ�����", th.Th_AccName));
                return;
            }
            th.Th_PhoneMobi = th.Th_PhoneMobi.Trim();
            try
            {                
                if (id == 0)
                {
                    //��ȡ��ʦ�����Ļ����˺ţ�ѧԱ�˺ţ�
                    Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(th.Th_PhoneMobi, null, null);
                    if (acc != null)
                    {
                        acc.Org_ID = th.Org_ID;
                        th.Ac_ID = acc.Ac_ID;
                    }
                    else
                    {
                        //��������˺Ų����ڣ�
                        acc = new Song.Entities.Accounts();
                        acc.Org_ID = th.Org_ID;
                        acc.Ac_AccName = th.Th_PhoneMobi;   //�˺�Ϊ�ֻ���
                        acc.Ac_MobiTel1 = th.Th_PhoneMobi;
                        acc.Ac_Name = th.Th_Name;
                        th.Th_AccName = th.Th_PhoneMobi;
                        acc.Ac_IsPass = th.Th_IsPass = true;
                        th.Th_IsShow = true;
                        acc.Ac_IsUse = th.Th_IsUse = true;
                        acc.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(th.Th_Pw).MD5;    //����                
                        acc.Ac_Sex = th.Th_Sex;        //�Ա�
                        acc.Ac_Birthday = th.Th_Birthday;
                        acc.Ac_Qq = th.Th_Qq;
                        acc.Ac_Email = th.Th_Email;
                        acc.Ac_IDCardNumber = th.Th_IDCardNumber;  //���֤    
                        acc.Ac_IsTeacher = true;        //���˺��н�ʦ���
                        //����
                        th.Ac_ID = Business.Do<IAccounts>().AccountsAdd(acc);
                    }
                    id = Business.Do<ITeacher>().TeacherAdd(th);
                }
                else
                {
                    Business.Do<ITeacher>().TeacherSave(th);
                }

                Master.AlertCloseAndRefresh("�����ɹ���");
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
