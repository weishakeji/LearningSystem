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

namespace Song.Site.Manage.View
{
    public partial class News : Extend.CustomPage
    {
        //栏目分类
        int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
         
            Song.Entities.Article[] eas = null;
            //条件
            //是否置顶，如果没有选择，则为空
            bool? istop = !cbIsTop.Checked ? null : (bool?)true;
            bool? ishot = !cbIsHot.Checked ? null : (bool?)true;
            bool? isrec = !cbIsRec.Checked ? null : (bool?)true;
            bool? isimg = !cbIsImg.Checked ? null : (bool?)true;
            //
            //所属机构的所有课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            eas = Business.Do<IContents>().ArticlePager(org.Org_ID, colid, tbSear.Text.Trim(), null, false,
                istop, ishot, isrec, isimg,
                Pager1.Size, Pager1.Index, out count);
            rtpList.DataSource = eas;
            rtpList.DataBind();

            Pager1.RecordAmount = count;
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
    }
}
