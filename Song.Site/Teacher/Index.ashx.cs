using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.Extend;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site.Teacher
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : BasePage
    {
        //用于标识布局的值
        protected string loyout = WeiSha.Common.Request.QueryString["loyout"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            //界面状态          
            this.Document.SetValue("loyout", loyout);
            //是否允许注册
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            this.Document.SetValue("IsRegTeacher", config["IsRegTeacher"].Value.Boolean ?? true);
            //老师列表
            Tag thTag = this.Document.GetChildTagById("teachers");
            if (thTag != null)
            {
                int thSize = int.Parse(thTag.Attributes.GetValue("size", "4"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                index = index > 0 ? index : 1;
                int sum = 0;
                Song.Entities.Teacher[] teachers = Business.Do<ITeacher>().TeacherPager(this.Organ.Org_ID, -1, true, true, "", thSize, index, out sum);
                foreach (Song.Entities.Teacher c in teachers)
                {
                    c.Th_Photo = Upload.Get["Teacher"].Virtual + c.Th_Photo;
                }
                this.Document.SetValue("teachers", teachers);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)thSize);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
            }
        }
    }
}