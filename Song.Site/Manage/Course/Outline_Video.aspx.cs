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
using WeiSha.WebControl;
using System.Collections.Generic;


namespace Song.Site.Manage.Course
{
    public partial class Outline_Video : Extend.CustomPage
    {
        //章节ID
        protected int olid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //课程ID
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //UID，即全局唯一ID
        protected string uid = WeiSha.Common.Request.QueryString["uid"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //附件与视频
                VideoBind();
                EventBindData(null, null);
            }
        }
        #region 视频操作
        /// <summary>
        /// 附件绑定
        /// </summary>
        protected void VideoBind()
        {
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(uid, this.Uploader1.UploadPath);
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
            Song.Entities.OutlineEvent[] events = Business.Do<IOutline>().EventAll(-1, uid, -1, null);
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
    }
}
