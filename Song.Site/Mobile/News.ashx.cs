using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Text.RegularExpressions;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 新闻频道页
    /// </summary>
    public class News : BasePage
    {
        int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;  //栏目id
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前栏目
            Song.Entities.Columns col = Business.Do<IColumns>().Single(colid);
            this.Document.Variables.SetValue("column", col);
            //获取所有栏目
            Song.Entities.Columns[] cols = Business.Do<IColumns>().ColumnCount(this.Organ.Org_ID, colid, "news", true, -1);
            this.Document.SetValue("cols", cols);

            this.Document.RegisterGlobalFunction(this.isColumns);
        }
        /// <summary>
        /// 是否有子级
        /// </summary>
        /// <param name="para">参数一个，取几个栏目</param>
        /// <returns></returns>
        protected string isColumns(object[] para)
        {
            int colid = 0;
            if (para.Length > 0 && para[0] is int)
                int.TryParse(para[0].ToString(), out colid);
            bool isChild= Business.Do<IColumns>().IsChildren(colid, true);
            return isChild.ToString();
        }

    }
}