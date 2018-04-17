using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;
using System.Net;
using System.IO;
namespace Song.Site
{
    /// <summary>
    /// 系统启始页Default
    /// </summary>
    public class Default : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //是否允许注册
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            this.Document.SetValue("IsRegStudent", config["IsRegStudent"].Value.Boolean ?? true);

            //学员成绩排行，取最后一次考试
            //考试当前考试
            Tag exrTag = this.Document.GetChildTagById("exrTag");
            if (exrTag != null)
            {
                Song.Entities.Examination exam = Business.Do<IExamination>().ExamLast();
                if (exam != null)
                {
                    int count = int.Parse(exrTag.Attributes.GetValue("count", "5"));
                    Song.Entities.ExamResults[] exr = Business.Do<IExamination>().Results(exam.Exam_ID, count);
                    this.Document.SetValue("exr", exr);
                    //学员分组排行
                    DataTable dt = Business.Do<IExamination>().Result4StudentSort(exam.Exam_ID);
                    this.Document.SetValue("exrdt", dt);
                }
            }
        }
    }
}