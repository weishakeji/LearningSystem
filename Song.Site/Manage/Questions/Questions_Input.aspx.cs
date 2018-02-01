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

namespace Song.Site.Manage.Questions
{
    public partial class Questions_Input : Extend.CustomPage
    {
        //试题id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //试题类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        //参数
        protected string query = string.Empty;  
        protected void Page_Load(object sender, EventArgs e)
        {
            query = this.ClientQueryString;            

            //if (string.IsNullOrWhiteSpace(query))
            //如果是新增（id大于0）,但又没有指定试题类型，则查询当前试题的类型
            if (string.IsNullOrWhiteSpace(type) && id > 0)
            {
                Song.Entities.Questions entity = Business.Do<IQuestions>().QuesSingle(id);
                type = entity.Qus_Type.ToString();
            }
            //如果类型指定了，则转向编辑页
            if (!string.IsNullOrWhiteSpace(type))
            {
                string goUrl = "Questions_Input{0}.aspx?type={0}&" + this.ClientQueryString;
                this.Response.Redirect(string.Format(goUrl, type, id));
            }
            //如果不是新增（id小于1）,但又没有指定试题类型，输出类型，让用户自己选择
            if (string.IsNullOrWhiteSpace(type) && id < 1)
            {
                //题型分类汉字名称
                string[] typeStr = App.Get["QuesType"].Split(',');
                rptTypes.DataSource = typeStr;
                rptTypes.DataBind();
            }
        }        
    }
}
