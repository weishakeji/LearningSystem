using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;
using WeiSha.Data;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 课程学习首页
    /// </summary>
    public class CoursePage : BasePage
    {
        //当前课程的id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //是否购买当前课程
        bool isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {
            WeiSha.Data.Gateway.Default.RegisterLogger(new logger());
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null) return;
            //记录当前学习的课程
            Extend.LoginState.Accounts.Course(course);
            #region 创建与学员的关联
            if (this.Account != null)
            {
                int accid = this.Account.Ac_ID;
                bool istudy = Business.Do<ICourse>().Study(couid,accid);              
            }
            #endregion

            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            this.Document.SetValue("course", course);
            //课程资源路径
            this.Document.SetValue("coupath", Upload.Get["Course"].Virtual);
            //学习该课程的总人数，包括已经过期的
            int studyCount = Business.Do<ICourse>().CourseStudentSum(course.Cou_ID, null);
            this.Document.Variables.SetValue("studyCount", studyCount);
            //是否学习当前课程
            if (this.Account != null)
            {
                //是否购买
                isBuy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, course.Cou_ID);
                //没有购买，但处于限时免费中
                if (!isBuy && course.Cou_IsLimitFree)
                {

                }
                this.Document.Variables.SetValue("isBuy", isBuy);
                //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
                bool canStudy = isBuy || course.Cou_IsFree || course.Cou_IsLimitFree || course.Cou_IsTry;             
                this.Document.Variables.SetValue("canStudy", canStudy);
            }
            //树形章节输出
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(course.Cou_ID, true);
            if (outlines.Length > 0)
                this.Document.Variables.SetValue("olTree", Business.Do<IOutline>().OutlineTree(outlines));
            //课程公告
            Tag guidTag = this.Document.GetChildTagById("guides");
            int guidCount = 0;
            if (guidTag != null)
            {
                string tm = guidTag.Attributes.GetValue("count", "10");
                int.TryParse(tm, out guidCount);
            }
            Song.Entities.Guide[] guides = Business.Do<IGuide>().GuideCount(0, course.Cou_ID, 0, guidCount);
            this.Document.Variables.SetValue("guides", guides);  
            //购买课程的时间区间
            this.Document.RegisterGlobalFunction(this.getBuyInfo);
        }
        /// <summary>
        /// 获取课程的购买信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected object getBuyInfo(object[] id)
        {
            int couid = 0;
            if (id.Length > 0 && id[0] is int)
                int.TryParse(id[0].ToString(), out couid);
            if (this.Account != null)
            {
                return Business.Do<ICourse>().StudentCourse(this.Account.Ac_ID, couid);
            }
            return null;
        }   
    }

    public class logger : WeiSha.Data.Logger.IExecuteLog
    {
        public void Begin(IDbCommand command)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            string path = _context.Request.Url.AbsolutePath;

            string sql = command.CommandText;
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                System.Data.SqlClient.SqlParameter para = (System.Data.SqlClient.SqlParameter)command.Parameters[i];
                string vl = para.Value.ToString();
                string tp = para.DbType.ToString();
                if (tp.IndexOf("Int") > -1)
                    sql = sql.Replace("@p" + i.ToString(), vl);
                if (tp == "String")
                    sql = sql.Replace("@p" + i.ToString(), "'"+vl+"'");
                if (tp == "Boolean")
                    sql = sql.Replace("@p" + i.ToString(), vl == "True" ? "1" : "0");
                if (tp == "DateTime")
                    sql = sql.Replace("@p" + i.ToString(), "'" + ((DateTime)para.Value).ToString("yyyy/MM/dd HH:mm:ss") + "'");
            }
            //string t = command.Connection
            WeiSha.Common.Log.Info(path, sql);
            //WeiSha.Common.Log.Info(path, command.Connection.ConnectionString);
            //throw new NotImplementedException();
        }

        public void End(IDbCommand command, ReturnValue retValue, long elapsedTime)
        {
            //throw new NotImplementedException();
        }
    }
}