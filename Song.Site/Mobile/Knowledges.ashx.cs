using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;
using System.Data;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 当前课程的知识库
    /// </summary>
    public class Knowledges : BasePage
    {
        public int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //知识库栏目
                Song.Entities.KnowledgeSort[] kns = Business.Do<IKnowledge>().GetSortAll(-1, couid, -1, true);
                DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(kns);
                WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
                tree.IdKeyName = "Kns_ID";
                tree.ParentIdKeyName = "Kns_PID";
                tree.TaxKeyName = "Kns_Tax";
                tree.Root = 0;
                dt = tree.BuilderTree(dt);
                this.Document.Variables.SetValue("kns", dt);
                this.Document.Variables.SetValue("couid", couid);
                //当前知识库栏目
                int sorts = WeiSha.Common.Request.QueryString["sorts"].Int32 ?? 0;  //栏目分类id
                Song.Entities.KnowledgeSort sort = Business.Do<IKnowledge>().SortSingle(sorts);
                this.Document.Variables.SetValue("sort", sort);
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页    
                string sorts = WeiSha.Common.Request.Form["sorts"].String;  //栏目分类id
                string search = WeiSha.Common.Request.Form["sear"].String;  //要检索的字符
                Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();

                int sumcount = 0;
                //信息列表
                Song.Entities.Knowledge[] kls = null;
                kls = Business.Do<IKnowledge>().KnowledgePager(currCourse.Cou_ID, sorts, search, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int n = 0; n < kls.Length; n++)
                {
                    json += kls[n].ToJson("Kn_ID,Kn_Title", "Kn_Details") + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}