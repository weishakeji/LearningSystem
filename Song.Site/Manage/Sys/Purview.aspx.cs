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

namespace Song.Site.Manage.Sys
{
    public partial class Purview : Extend.CustomPage
    {
        string type = WeiSha.Common.Request.QueryString["type"].String;
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                switch (type.ToLower())
                {
                    case "posi":
                        Song.Entities.Position p = Business.Do<IPosition>().GetSingle(id);
                        ltName.Text = p.Posi_Name;
                        break;
                    case "group":
                        Song.Entities.EmpGroup e = Business.Do<IEmpGroup>().GetSingle(id);
                        ltName.Text = e.EGrp_Name;
                        break;
                    case "depart":
                        Song.Entities.Depart d = Business.Do<IDepart>().GetSingle(id);
                        ltName.Text = d.Dep_CnName;
                        break;
                    case "organ":
                        ltName.Text = "所有机构";
                        break;
                    case "orglevel":
                        Song.Entities.OrganLevel lv = Business.Do<IOrganization>().LevelSingle(id);
                        ltName.Text = "机构等级："+lv.Olv_Name;
                        break;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
