using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Outline : Extend.CustomPage
    {
        //课程id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //当前章节id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected string UID
        {
            get { return this.getUID(); }
        }
        //上传资料的所有路径
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //隐藏确定按钮
                EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
                btnEnter.Visible = false;               
                BindData(null, null);
                outlineFill(olid);
            }
        }        
        #region 章节管理
        /// <summary>
        /// 绑定章节列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //绑定之前先清理
            Business.Do<IOutline>().OutlineCleanup(couid);
            //
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = Business.Do<IOutline>().OutlineTree(outlines);
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Ol_ID" };
            GridView1.DataBind();
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IOutline>().OutlineDelete(id);
            BindData(null, null);
            DataTable dt = (DataTable)GridView1.DataSource;
            olineInitBind(couid);
            int tmid = 0;
            if (dt != null && dt.Rows.Count > 0)
            {                
                int.TryParse(dt.Rows[0]["Ol_ID"].ToString(), out tmid);
                Response.Redirect("Courses_Outline.aspx?couid=" + couid + "&olid=" + tmid);
            }
            else
            {
                Response.Redirect("Courses_Outline.aspx?couid=" + couid + "&olid=" + tmid);
            }
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineUp(couid, id))
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineDown(couid, id))
            {
                BindData(null, null);
            }
        }
        #endregion

        #region 章节编辑
        /// <summary>
        /// 章节界面的初始绑定
        /// </summary>
        private void olineInitBind(int couid)
        {
            Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outline);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Ol_ID";
            tree.ParentIdKeyName = "Ol_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            ddlOutline.DataSource = dt;
            this.ddlOutline.DataTextField = "Ol_Name";
            this.ddlOutline.DataValueField = "Ol_ID";
            ddlOutline.DataBind();
            this.ddlOutline.Items.Insert(0, new ListItem("-- 顶级章节 --", "0"));
            //
            plAddShow.Visible = olid == 0;
            plOtherEdit.Visible = olid != 0;
        }
        /// <summary>
        /// 填充章节信息
        /// </summary>
        /// <param name="olid"></param>
        private void outlineFill(int olid)
        {
            olineInitBind(couid);
            Song.Entities.Outline mm;
            if (olid > 0)
            {
                mm = Business.Do<IOutline>().OutlineSingle(olid);
                //是否显示
                cbIsUse.Checked = mm.Ol_IsUse;
                //上级章节
                ListItem li = ddlOutline.Items.FindByValue(mm.Ol_PID.ToString());
                if (li != null)
                {
                    ddlOutline.SelectedIndex = -1;
                    li.Selected = true;
                }
                //唯一标识
                ViewState["UID"] = mm.Ol_UID;
                //附件与视频
                VideoBind();
                EventBindData(null, null);
                AccessoryBind();
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Outline();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
            }
            //标题
            Ol_Name.Text = mm.Ol_Name;
            //简介
            Ol_Intro.Text = mm.Ol_Intro;            
            lbOutlineID.Text = olid.ToString();
            //上传控件
            Uploader1.UID = this.getUID();

        }
        /// <summary>
        /// 保存章节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            int.TryParse(lbOutlineID.Text, out olid);

            Song.Entities.Outline ol = olid < 1 ? new Song.Entities.Outline() : Business.Do<IOutline>().OutlineSingle(olid);
            if (ol == null) return;
            //名称
            ol.Ol_Name = Ol_Name.Text.Trim();
            //简介
            ol.Ol_Intro = Ol_Intro.Text;
            //上级章节
            int pid = 0;
            int.TryParse(ddlOutline.SelectedValue, out pid);
            ol.Ol_PID = pid;
            //是否启用
            ol.Ol_IsUse = cbIsUse.Checked;
            //所属课程
            ol.Cou_ID = couid;
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(couid);
            if (cou != null) ol.Sbj_ID = cou.Sbj_ID;
            ////全局唯一ID
            //ol.Ol_UID = getUID();
            try
            {
                if (olid < 1)
                {
                    //新增
                    Business.Do<IOutline>().OutlineAdd(ol);
                }
                else
                {
                    Business.Do<IOutline>().OutlineSave(ol);
                }
                Response.Redirect("Courses_Outline.aspx?couid=" + couid + "&olid=" + ol.Ol_ID);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region 视频操作
        /// <summary>
        /// 附件绑定
        /// </summary>
        protected void VideoBind()
        {
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(this.getUID(), this.Uploader1.UploadPath);
            foreach (Accessory ac in acs)
            {
                if (ac.As_IsOuter) continue;             
                ac.As_FileName = Upload.Get[this.Uploader1.UploadPath].Virtual + ac.As_FileName;
            }
            dlVideo.DataSource = acs;
            dlVideo.DataKeyField = "As_Id";
            dlVideo.DataBind();
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_VideoDelClick(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<IAccessory>().Delete(id);
            VideoBind();
        }
        #endregion

        #region 视频事件
        /// <summary>
        /// 绑定章节事件列表
        /// </summary>
        protected void EventBindData(object sender, EventArgs e)
        {
            Song.Entities.OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, this.getUID(), -1, null);
            gvEventList.DataSource = events;
            gvEventList.DataKeyNames = new string[] { "Oe_ID" };
            gvEventList.DataBind();
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbEventUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvEventList.DataKeys[index].Value.ToString());
            //
            Song.Entities.OutlineEvent entity = Business.Do<IOutline>().EventSingle(id);
            entity.Oe_IsUse = !entity.Oe_IsUse;
            Business.Do<IOutline>().EventSave(entity);
            EventBindData(null, null);
        }
        /// <summary>
        /// 视频事件的单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEventDel_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int id = int.Parse(img.CommandArgument);
            Business.Do<IOutline>().EventDelete(id);
            EventBindData(null, null);           
        }
        #endregion

        #region 附件操作
        protected void btn_Click(object sender, EventArgs e)
        {
            Song.Entities.Accessory dd = new Accessory();
            //图片
            if (fuLoad.HasFile)
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.SaveAndDeleteOld(dd.As_Name);
                    dd.As_Name = fuLoad.FileName;
                    dd.As_FileName = fuLoad.File.Server.FileName;
                    dd.As_Size = fuLoad.PostedFile.ContentLength;
                    dd.As_Extension = fuLoad.File.Server.Extension;
                    dd.As_Uid = this.getUID();
                    dd.As_Type = _uppath;
                    Business.Do<IAccessory>().Add(dd);
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            AccessoryBind();
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<IAccessory>().Delete(id);
            AccessoryBind();

        }
        /// <summary>
        /// 附件绑定
        /// </summary>
        protected void AccessoryBind()
        {
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(this.getUID(), _uppath);
            foreach (Accessory ac in acs)
            {
                ac.As_FileName = Upload.Get[_uppath].Virtual + ac.As_FileName;
            }
            dlAcc.DataSource = acs;
            dlAcc.DataKeyField = "As_Id";
            dlAcc.DataBind();
        }
        #endregion



    }
}