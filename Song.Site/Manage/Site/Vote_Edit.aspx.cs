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

namespace Song.Site.Manage.Site
{
    public partial class Vote_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string _uppath = "Vote";
        //调查子项的显示行数
        private int itemNumber = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fillTheme();
                fillItem();
            }
        }
        #region 页面初始化
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fillTheme()
        {
            try
            {
                //设置调查主题
                Song.Entities.Vote mm;
                if (id != 0)
                {
                    mm = Business.Do<IVote>().GetSingle(id);
                    cbIsShow.Checked = mm.Vt_IsShow;
                    cbIsUse.Checked = mm.Vt_IsUse;
                    cbIsAllowSee.Checked = mm.Vt_IsAllowSee;
                    //唯一标识
                    ViewState["UID"] = mm.Vt_UniqueId;
                    //调查类别，单选还是多选
                    ListItem li = rbSelType.Items.FindByValue(mm.Vt_SelectType.ToString());
                    if (li != null) li.Selected = true;
                    //图片
                    if (mm.Vt_IsImage && !string.IsNullOrEmpty(mm.Vt_Image))
                    {
                        this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Vt_ImageSmall;
                    }
                }
                else
                {
                    mm = new Song.Entities.Vote();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                }
                tbName.Text = mm.Vt_Name;
                tbIntro.Text = mm.Vt_Intro;
                //是否为图片调查
                cbIsImg.Checked = mm.Vt_IsImage;
                tdImageArea.Visible = mm.Vt_IsImage;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
         }
        /// <summary>
        /// 填充子项目
        /// </summary>
        private void fillItem()
        {
            try
            {
                //调查选择项的填充
                string uid = this.getUID();
                DataTable dt = Business.Do<IVote>().GetVoteItem(uid);
                if (this.itemNumber - dt.Rows.Count < 0) itemNumber = dt.Rows.Count;
                int tm = this.itemNumber - dt.Rows.Count;
                for (int i = 0; i < tm; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Vt_id"] = "-1";
                    dr["Vt_Number"] = 0;
                    dt.Rows.Add(dr);
                }
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Vt_id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //设置调查主题
            Song.Entities.Vote mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IVote>().GetSingle(id);
                }
                else
                {
                    mm = new Song.Entities.Vote();
                }
                mm.Vt_Name = tbName.Text;
                mm.Vt_Intro = tbIntro.Text;
                mm.Vt_IsShow = cbIsShow.Checked;
                mm.Vt_IsUse = cbIsUse.Checked;
                mm.Vt_Type = 1;
                //是否允许查看结果
                mm.Vt_IsAllowSee = cbIsAllowSee.Checked;
                //是否为图片调查
                mm.Vt_IsImage = cbIsImg.Checked;
                //唯一标识
                mm.Vt_UniqueId = this.getUID();
                //调查类别，单选还是多选
                mm.Vt_SelectType = Convert.ToInt32(rbSelType.SelectedValue);
                //图片
                if (cbIsImg.Checked && fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = true;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SmallHeight = 300;
                        fuLoad.SmallWidth = 400;
                        fuLoad.SaveAndDeleteOld(mm.Vt_Image);
                        //记录新图片文件，以及缩略图文件
                        mm.Vt_Image = fuLoad.File.Server.FileName;
                        mm.Vt_ImageSmall = fuLoad.File.Server.SmallFileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
                if (id != 0)
                {
                    Business.Do<IVote>().ItemSave(mm);
                }
                else
                {
                    Business.Do<IVote>().ThemeAdd(mm);
                }
                //保存子项
                DataTable dt = Business.Do<IVote>().GetVoteItem(mm.Vt_UniqueId);
                foreach (System.Web.UI.WebControls.GridViewRow gvr in GridView1.Rows)
                {
                    TextBox newName = (TextBox)(gvr.FindControl("tbItemName"));
                    if (newName.Text.Trim() == "") continue;
                    //Label lbNumber = (Label)(gvr.FindControl("tbItemNumber"));
                    //int iNumber = Convert.ToInt32(lbNumber.Text);
                    int itemId = Convert.ToInt32(GridView1.DataKeys[gvr.RowIndex].Value);
                    if (itemId > -1)
                    {
                        //修改子项的记录
                        Song.Entities.Vote mod = Business.Do<IVote>().GetSingle(itemId);
                        mod.Vt_UniqueId = this.getUID();
                        mod.Vt_Name = newName.Text;
                        mod.Vt_Tax = gvr.RowIndex;
                        Business.Do<IVote>().ItemSave(mod);
                        //当前操作的子项id,如果已经存在于数据库，则在dt中清除，清到最后，剩下的将是不需保留的。
                        int index = -1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int oid = Convert.ToInt32(dt.Rows[i]["Vt_Id"].ToString());
                            if (itemId == oid)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index > -1) dt.Rows.RemoveAt(index);
                    }
                    else
                    {
                        //新增的子项记录
                        Song.Entities.Vote add = new Song.Entities.Vote();
                        add.Vt_UniqueId = this.getUID();
                        add.Vt_Name = newName.Text;
                        add.Vt_Tax = gvr.RowIndex;
                        Business.Do<IVote>().ItemAdd(add);
                    }
                }
                //删除剩余的
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int oid = Convert.ToInt32(dt.Rows[i]["Vt_Id"].ToString());
                    Business.Do<IVote>().ItemDelete(oid);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }

        #region 增加、减少编辑行
        /// <summary>
        /// 增加行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable("DataBase");
                dt.Columns.Add("Vt_Id", Type.GetType("System.Int32"));
                dt.Columns.Add("Vt_Name", Type.GetType("System.String"));
                dt.Columns.Add("Vt_Number", Type.GetType("System.Int32"));
                //记录界面中的数据
                foreach (System.Web.UI.WebControls.GridViewRow gvr in GridView1.Rows)
                {
                    TextBox tbName = (TextBox)(gvr.FindControl("tbItemName"));
                    Literal ltNumber = (Literal)(gvr.FindControl("ltItemNumber"));
                    int id = Convert.ToInt32(GridView1.DataKeys[gvr.RowIndex].Value);
                    //
                    DataRow dr = dt.NewRow();
                    dr["Vt_id"] = id;
                    dr["Vt_Name"] = tbName.Text;
                    dr["Vt_Number"] = ltNumber.Text;
                    dt.Rows.Add(dr);
                }
                //增加新行
                DataRow addr = dt.NewRow();
                addr["Vt_id"] = -1;
                addr["Vt_Name"] = "";
                addr["Vt_Number"] = 0;
                dt.Rows.Add(addr);
                //绑定
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Vt_id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }

        protected void lbDel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable("DataBase");
                dt.Columns.Add("Vt_Id", Type.GetType("System.Int32"));
                dt.Columns.Add("Vt_Name", Type.GetType("System.String"));
                dt.Columns.Add("Vt_Number", Type.GetType("System.Int32"));
                //记录界面中的数据
                foreach (System.Web.UI.WebControls.GridViewRow gvr in GridView1.Rows)
                {
                    TextBox tbName = (TextBox)(gvr.FindControl("tbItemName"));
                    Literal ltNumber = (Literal)(gvr.FindControl("ltItemNumber"));
                    int id = Convert.ToInt32(GridView1.DataKeys[gvr.RowIndex].Value);
                    //
                    DataRow dr = dt.NewRow();
                    dr["Vt_id"] = id;
                    dr["Vt_Name"] = tbName.Text;
                    dr["Vt_Number"] = ltNumber.Text;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 1)
                {
                    //去除末行
                    dt.Rows.RemoveAt(dt.Rows.Count - 1);
                }
                //绑定
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Vt_id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #endregion
        /// <summary>
        /// 当选择，是否为图片调查时，显示图片上传功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsImg_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            tdImageArea.Visible = cb.Checked;
        }
    }
}
