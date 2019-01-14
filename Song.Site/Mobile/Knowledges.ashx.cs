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
            this.Document.Variables.SetValue("couid", couid);
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
                if (course == null) return;
                this.Document.Variables.SetValue("course", course);
                //是否学习当前课程
                if (this.Account != null)
                {
                    //是否购买当前课程
                    bool isBuy =  Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
                    this.Document.Variables.SetValue("isBuy", isBuy);
                    //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
                    bool canStudy = isBuy || course.Cou_IsFree || course.Cou_IsLimitFree;
                    this.Document.Variables.SetValue("canStudy", canStudy);
                }
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

                int sumcount = 0;
                //信息列表
                Song.Entities.Knowledge[] kls = null;
                kls = Business.Do<IKnowledge>().KnowledgePager(couid, sorts, search, size, index, out sumcount);
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