
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using WeiSha.Data;
using Song.Entities;
using Song.ServiceInterfaces;

namespace Song.DataQuery.SqlServer9
{
    public class CourseCom
    {
        /// <summary>
        /// 学员购买的该课程
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="sear">用于检索课程的字符</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry">是否试用，为null时取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry, int size, int index, out int countSum)
        {
            //当前学员
            Accounts student = Business.Do<IAccounts>().AccountsSingle(stid);
            if (student == null) throw new Exception("学员账号不存在");

            //试用课程，不包括学员组关联课程
            if (istry != null && (bool)istry)
            {
                WhereClip wc = Student_Course._.Ac_ID == stid;
                if (enable != null) wc.And(Student_Course._.Stc_IsEnable == (bool)enable);
                wc.And(Student_Course._.Stc_IsTry == (bool)istry);
                if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Contains("%" + sear));
                countSum = Gateway.Default.From<Course>()
                        .InnerJoin<Student_Course>(Student_Course._.Cou_ID == Course._.Cou_ID)
                        .Where(wc).Count();
                return Gateway.Default.From<Course>()
                        .InnerJoin<Student_Course>(Student_Course._.Cou_ID == Course._.Cou_ID)
                        .Where(wc).OrderBy(Student_Course._.Stc_StartTime.Desc).ToList<Course>(size, (index - 1) * size);
            }
            /* 
             * 时效内课程和所有课程，包括了学员组关联课程，相对复杂，用了sql语句实现
             */

            //学员购买课程记录中的课程
            string sql1 = @"select cou.* from Course as cou inner join  Student_Course as sc 
                            on cou.Cou_ID = sc.Cou_ID
                            where sc.Ac_ID = {{acid}} and sc.Stc_Type!=5 and {{enable}} and {{istry}}
                            and {{expired}} ";
            sql1 = sql1.Replace("{{acid}}", stid.ToString());
            //是否查询禁用课程（在课程购买中的禁用，不是课程禁用）
            if (enable != null) sql1 = sql1.Replace("{{enable}}", "sc.Stc_IsEnable = " + ((bool)enable ? 1 : 0));
            else
                sql1 = sql1.Replace("{{enable}}", "1=1");
            //购买时间内的
            if (state == 1)
                sql1 = sql1.Replace("{{expired}}", "(sc.Stc_StartTime < getdate() and  sc.Stc_EndTime > getdate())");
            //过期的
            else if (state == 2)
                sql1 = sql1.Replace("{{expired}}", "sc.Stc_EndTime<getdate()");
            else
                sql1 = sql1.Replace("{{expired}}", "1=1");
            //试用
            sql1 = sql1.Replace("{{istry}}", istry != null ? "sc.Stc_IsTry = " + ((bool)istry ? 1 : 0) : "1=1");

            //学员组关联的课程
            if (student.Sts_ID > 0)
            {
                StudentSort sort = Business.Do<IStudent>().SortSingle(student.Sts_ID);
                if (sort != null && sort.Sts_IsUse)
                {
                    string sql2 = @"select cou.* from Course as cou right join  StudentSort_Course as ssc
                                    on cou.Cou_ID = ssc.Cou_ID
                                    where ssc.Sts_ID = " + student.Sts_ID;
                    //如果取过期课程，则去除学员组关联的课程
                    if (state == 2)
                        sql1 += "except " + sql2;
                    //正常情况下，是学员购买课程+学员组关联课程
                    else
                        sql1 += "union " + sql2;
                }
            }
            //计算总数
            string sql_total = @"select count(*) from ({{sql}}) as r";
            if (!string.IsNullOrWhiteSpace(sear)) sql_total += " where Cou_Name like '%" + sear + "%'";
            sql_total = sql_total.Replace("{{sql}}", sql1);
            object o = Gateway.Default.FromSql(sql_total).ToScalar();
            countSum = o == null ? 0 : (int)o;

            //综合sql1和sql2,主要是查询
            string sql3 = @"select ROW_NUMBER() OVER(Order by  Stc_EndTime desc, Ssc_ID desc) AS 'rowid', * from 
	                    (
		                    select muster.*, sc.Stc_EndTime,ssc.Ssc_ID from 
		                    (
			                    {{sql}}
		                    ) as muster 
		                     left join 	
		                     (
			                    select * from  Student_Course as sc
			                    where sc.Ac_ID={{acid}} and sc.Stc_IsEnable=1 and sc.Stc_Type!=5
			                    and (sc.Stc_StartTime<getdate() and  sc.Stc_EndTime>getdate())
			                    and sc.Cou_ID not in (select Cou_ID from  StudentSort_Course where Sts_ID={{stsid}})			
		                     ) as sc on muster.Cou_ID = sc.Cou_ID
		                     left join  
		                     (
			                    select * from  StudentSort_Course where Sts_ID={{stsid}}
		                     ) as ssc  on muster.Cou_ID = ssc.Cou_ID
		
	                    )as t";
            if (!string.IsNullOrWhiteSpace(sear)) sql3 += " where Cou_Name like '%" + sear + "%'";
            sql3 = sql3.Replace("{{sql}}", sql1);
            sql3 = sql3.Replace("{{acid}}", stid.ToString());
            sql3 = sql3.Replace("{{stsid}}", student.Sts_ID.ToString());

            //分页
            string sql4 = "select * from ({{sql}}) as p where rowid > {{start}} and rowid<={{end}}";
            sql4 = sql4.Replace("{{sql}}", sql3);
            int start = (index - 1) * size;
            int end = (index - 1) * size + size;
            sql4 = sql4.Replace("{{start}}", start.ToString());
            sql4 = sql4.Replace("{{end}}", end.ToString());

            return Gateway.Default.FromSql(sql4).ToList<Course>();

        }
        /// <summary>
        /// 学员购买的该课程,以及学员组关联的课程,不分页，不排序
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sear"></param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry"></param>
        /// <returns></returns>
        public List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry)
        {
            //当前学员
            Accounts student = Business.Do<IAccounts>().AccountsSingle(stid);
            if (student == null) throw new Exception("学员账号不存在");

            //试用课程，不包括学员组关联课程
            if (istry != null && (bool)istry)
            {
                WhereClip wc = Student_Course._.Ac_ID == stid;
                if (enable != null) wc.And(Student_Course._.Stc_IsEnable == (bool)enable);
                wc.And(Student_Course._.Stc_IsTry == (bool)istry);
                if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Contains("%" + sear));

                return Gateway.Default.From<Course>()
                        .InnerJoin<Student_Course>(Student_Course._.Cou_ID == Course._.Cou_ID)
                        .Where(wc).OrderBy(Student_Course._.Stc_StartTime.Desc).ToList<Course>();
            }
            /* 
             * 时效内课程和所有课程，包括了学员组关联课程，相对复杂，用了sql语句实现
             */

            //学员购买课程记录中的课程
            string sql1 = @"select cou.* from Course as cou inner join  Student_Course as sc 
                            on cou.Cou_ID = sc.Cou_ID
                            where sc.Ac_ID = {{acid}} and sc.Stc_Type!=5 and {{enable}} and {{istry}}
                            and {{expired}} ";
            sql1 = sql1.Replace("{{acid}}", stid.ToString());
            //是否查询禁用课程（在课程购买中的禁用，不是课程禁用）
            if (enable != null) sql1 = sql1.Replace("{{enable}}", "sc.Stc_IsEnable = " + ((bool)enable ? 1 : 0));
            else
                sql1 = sql1.Replace("{{enable}}", "1=1");
            //购买时间内的
            if (state == 1)
                sql1 = sql1.Replace("{{expired}}", "(sc.Stc_StartTime < getdate() and  sc.Stc_EndTime > getdate())");
            //过期的
            else if (state == 2)
                sql1 = sql1.Replace("{{expired}}", "sc.Stc_EndTime<getdate()");
            else
                sql1 = sql1.Replace("{{expired}}", "1=1");
            //试用
            sql1 = sql1.Replace("{{istry}}", istry != null ? "sc.Stc_IsTry = " + ((bool)istry ? 1 : 0) : "1=1");

            //学员组关联的课程
            if (student.Sts_ID > 0)
            {
                StudentSort sort = Business.Do<IStudent>().SortSingle(student.Sts_ID);
                if (sort != null && sort.Sts_IsUse)
                {
                    string sql2 = @"select cou.* from Course as cou right join  StudentSort_Course as ssc
                                    on cou.Cou_ID = ssc.Cou_ID
                                    where ssc.Sts_ID = " + student.Sts_ID;
                    //如果取过期课程，则去除学员组关联的课程
                    if (state == 2)
                        sql1 += "except " + sql2;
                    //正常情况下，是学员购买课程+学员组关联课程
                    else
                        sql1 += "union " + sql2;
                }
            }
            return Gateway.Default.FromSql(sql1).ToList<Course>();
        }
        /// <summary>
        /// 获取收入最多的课程
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> RankIncome(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            countSum = this.CourseOfCount(orgid, sbjid, -1, null, null);       
            string sql = @"select count as Cou_TryNum, * from (
                        select top {end} ROW_NUMBER() OVER(Order by count desc ) AS 'rowid',* from 
                        (select ISNULL(b.count,0) as 'count', c.* from course as c left join 
                                                    (SELECT cou_id, sum(Stc_Money) as 'count'
                                                      FROM [Student_Course] where  {begin} and {over}  group by cou_id ) as b
                                                      on c.cou_id=b.cou_id where org_id={orgid} and {sbjid}  
                         ) as t ) paper	 where  rowid > {start} ";

            sql = sql.Replace("{orgid}", orgid.ToString());
            sql = sql.Replace("{begin}", start != null ? "Stc_CrtTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            sql = sql.Replace("{over}", start != null ? "Stc_CrtTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            //按专业选取（包括专业的下级专业）
            string sbjWhere = string.Empty;
            if (sbjid > 0)
            {
                List<long> sbjids = Business.Do<ISubject>().TreeID(sbjid, orgid);
                for (int i = 0; i < sbjids.Count; i++)
                {
                    sbjWhere += "sbj_id=" + sbjids[i] + " ";
                    if (i < sbjids.Count - 1) sbjWhere += " or ";
                }
            }
            sql = sql.Replace("{sbjid}", sbjid > 0 ? "(" + sbjWhere + ")" : "1=1");
            //分页
            sql = sql.Replace("{end}", (size * index).ToString());
            sql = sql.Replace("{start}", ((index - 1) * size).ToString());
            return Gateway.Default.FromSql(sql).ToList<Course>();
        }
        /// <summary>
        /// 获取选学人数最多的课程列表，从多到少
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public List<Course> RankHot(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            countSum = this.CourseOfCount(orgid, sbjid, -1, null, null);
            string sql = @"select count as Cou_TryNum, * from (
                        select top {end} ROW_NUMBER() OVER(Order by count desc ) AS 'rowid',* from 
                        (select ISNULL(b.count,0) as 'count', c.* from course as c left join 
                                                    (SELECT cou_id, count(cou_id) as 'count'
                                                      FROM [Student_Course]  where  {begin} and {over}   group by cou_id ) as b
                                                      on c.cou_id=b.cou_id where org_id={orgid} and {sbjid}  
                         ) as t ) paper	 where  rowid > {start} ";

            sql = sql.Replace("{orgid}", orgid.ToString());
            sql = sql.Replace("{begin}", start != null ? "Stc_CrtTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            sql = sql.Replace("{over}", start != null ? "Stc_CrtTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            //按专业选取（包括专业的下级专业）
            string sbjWhere = string.Empty;
            if (sbjid > 0)
            {
                List<long> sbjids = Business.Do<ISubject>().TreeID(sbjid, orgid);
                for (int i = 0; i < sbjids.Count; i++)
                {
                    sbjWhere += "sbj_id=" + sbjids[i] + " ";
                    if (i < sbjids.Count - 1) sbjWhere += " or ";
                }
            }
            sql = sql.Replace("{sbjid}", sbjid > 0 ? "(" + sbjWhere + ")" : "1=1");
            //分页
            sql = sql.Replace("{end}", (size * index).ToString());
            sql = sql.Replace("{start}", ((index - 1) * size).ToString());
            return Gateway.Default.FromSql(sql).ToList<Course>();
        }
        /// <summary>
        /// 分页获取当前课程的学员（即学习该课程的学员），并计算出完成度
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stsid"></param>
        /// <param name="acc">学员账号或姓名</param>
        /// <param name="name">学员的姓名</param>
        /// <param name="idcard"></param>
        /// <param name="mobi"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public DataTable StudentLogPager(long couid, long stsid, string acc, string name, string idcard, string mobi,
            DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            //计算总数的脚本
            string sqlsum = @"select COUNT(*) as total from 
                     (select * from Student_Course where {{where4sc}} and ({{start}} and {{end}}) ) as sc  inner join
                     Accounts as a on sc.Ac_ID = a.Ac_ID {{where4acc}}"; ;
            //购买记录的条件
            string where4sc = "{{couid}} and {{stsid}}";
            where4sc = where4sc.Replace("{{couid}}", couid > 0 ? "Student_Course.Cou_ID =" + couid.ToString() : "1=1");
            where4sc = where4sc.Replace("{{stsid}}", stsid > 0 ? "Student_Course.Sts_ID =" + stsid.ToString() : "1=1");
            //学员查询条件
            string where4acc = "where {{acc}} and {{name}} and {{idcard}} and {{mobi}}";
            where4acc = where4acc.Replace("{{acc}}", string.IsNullOrWhiteSpace(acc) ? "1=1" : "a.Ac_AccName LIKE '%" + acc + "%'");
            where4acc = where4acc.Replace("{{name}}", string.IsNullOrWhiteSpace(name) ? "1=1" : "a.Ac_Name LIKE '%" + name + "%'");
            where4acc = where4acc.Replace("{{idcard}}", string.IsNullOrWhiteSpace(idcard) ? "1=1" : "a.Ac_IDCardNumber LIKE '%" + idcard + "%'");
            where4acc = where4acc.Replace("{{mobi}}", string.IsNullOrWhiteSpace(mobi) ? "1=1" : "a.Ac_MobiTel1 LIKE '%" + mobi + "%'");

            //计算满足条件的记录总数          
            sqlsum = sqlsum.Replace("{{start}}", start == null ? "1=1" : "Stc_StartTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqlsum = sqlsum.Replace("{{end}}", end == null ? "1=1" : "Stc_StartTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqlsum = sqlsum.Replace("{{where4acc}}", where4acc);
            sqlsum = sqlsum.Replace("{{where4sc}}", where4sc);
            object o = Gateway.Default.FromSql(sqlsum).ToScalar();
            countSum = o == null ? 0 : Convert.ToInt32(o);

            //分页查询的脚本
            string sqljquery = string.Empty;
            sqljquery = @"select * from
(select * from
                       (select a.*,sc.Cou_ID,sc.Stc_QuesScore,sc.Stc_StudyScore,sc.Stc_ExamScore,ROW_NUMBER() OVER(Order by a.ac_id desc ) AS rowid from 
                         (select * from Student_Course where {{where4sc}} and ({{start}} and {{end}})  ) as sc  inner join      
                         Accounts as a on sc.Ac_ID=a.Ac_ID {{where4acc}}) as pager  where  rowid > {{startindex}} and rowid<={{endindex}} 
   ) as acc
   left join
  (SELECT ac_id, MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
                     sum(Lss_StudyTime) as 'studyTime', sum(Lss_Duration)/1000 as 'totalTime', MAX([Lss_PlayTime])/100 as 'playTime',
                     (case  when max(Lss_Duration)> 0 then
                      cast(convert(decimal(18, 4), 1000 * sum(cast(Lss_StudyTime as float)) / sum(cast(Lss_Duration AS float))) as float) * 100
                      else 0 end
                      ) as 'complete'
                    FROM[LogForStudentStudy]  where {{couid}} group by ac_id) as lss on acc.Ac_ID = lss.Ac_ID and acc.Cou_ID = lss.cou_id";

            sqljquery = sqljquery.Replace("{{couid}}", couid > 0 ? "Cou_ID =" + couid : "1=1");
            sqljquery = sqljquery.Replace("{{start}}", start == null ? "1=1" : "Stc_StartTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqljquery = sqljquery.Replace("{{end}}", end == null ? "1=1" : "Stc_StartTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqljquery = sqljquery.Replace("{{where4acc}}", where4acc);
            sqljquery = sqljquery.Replace("{{where4sc}}", where4sc);
            int startindex = (index - 1) * size;
            int endindex = (index - 1) * size + size;
            sqljquery = sqljquery.Replace("{{startindex}}", startindex.ToString());
            sqljquery = sqljquery.Replace("{{endindex}}", endindex.ToString());
            DataSet ds = Gateway.Default.FromSql(sqljquery).ToDataSet();
            //完成度大于100，则等于100
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "complete")
                    {
                        if (dr[i].ToString() != "")
                        {
                            double complete = 0;
                            double.TryParse(dr[i].ToString(), out complete);
                            complete = complete >= 100 ? 100 : complete;
                            dr[i] = complete;
                        }

                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 课程收益汇总
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public decimal Income(int orgid, long sbjid, DateTime? start, DateTime? end)
        {
            //按专业选取（包括专业的下级专业）
            string sql = @"select SUM(total) from (select ISNULL(b.total,0) as 'total' from course as c left join 
                            (SELECT cou_id, sum(Stc_Money) as 'total'
                              FROM[Student_Course] where  {start} and {end} group by cou_id) as b
                              on c.cou_id = b.cou_id where  org_id={orgid} and {sbjid} ) as tm ";
            sql = sql.Replace("{orgid}", orgid.ToString());
            sql = sql.Replace("{start}", start != null ? "Stc_CrtTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            sql = sql.Replace("{end}", start != null ? "Stc_CrtTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            string sbjWhere = string.Empty;
            if (sbjid > 0)
            {
                List<long> sbjids = Business.Do<ISubject>().TreeID(sbjid, orgid);
                for (int i = 0; i < sbjids.Count; i++)
                {
                    sbjWhere += "sbj_id=" + sbjids[i] + " ";
                    if (i < sbjids.Count - 1) sbjWhere += " or ";
                }
            }
            sql = sql.Replace("{sbjid}", sbjid > 0 ? "(" + sbjWhere + ")" : "1=1");
            object obj = Gateway.Default.FromSql(sql).ToScalar();
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }
        #region 相关方法
        /// <summary>
        /// 课程数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isuse">是否包括启用的课程,null取所有，true取启用的，false取未启用的</param>
        /// <param name="isfree">是否免费</param>
        /// <returns></returns>
        public int CourseOfCount(int orgid, long sbjid, int thid, bool? isuse, bool? isfree)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (isuse != null) wc.And(Course._.Cou_IsUse == (bool)isuse);
            if (isfree != null) wc.And(Course._.Cou_IsFree == (bool)isfree);
            return Gateway.Default.Count<Course>(wc);
        }
        #endregion
    }
}
