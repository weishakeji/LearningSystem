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
    public class StudentCom
    {
        /// <summary>
        /// 高频错题
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="type">题型</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Questions[] QuesOftenwrong(long couid, int type, int count)
        {
            string sql = @"
                    select {top} sq.count as Qus_Errornum,c.* 
                    from ""Questions"" as c 
                    inner join
                    (
                        SELECT ""Qus_ID"",COUNT(*) as count
                        FROM ""Student_Ques""
                        where {couid} and {type} group by ""Qus_ID""
                    ) as sq on c.""Qus_ID"" = sq.""Qus_ID""
                    order by sq.count desc ";
            sql = sql.Replace("{couid}", couid > 0 ? @"""Cou_ID""=" + couid : "1=1");
            sql = sql.Replace("{type}", type > 0 ? @"""Qus_Type""=" + type : "1=1");
            sql = sql.Replace("{top}", count > 0 ? "top " + count : "");          
            return Gateway.Default.FromSql(sql).ToArray<Questions>();
        }
        /// <summary>
        /// 登录日志的统计信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns>返回三列，area:行政区划名称,code:区划编码,count:登录人次</returns>
        public DataTable LoginLogsSummary(int orgid, DateTime? start, DateTime? end, string province, string city)
        {
            //支持sqlserver
            string sql = @"SELECT 
                        CASE WHEN {{field}} = '' THEN 'Other'       
                                ELSE {{field}}
                             END AS 'area'
                        ,MAX(lso_code) as 'code'
                        ,count(*) as 'count' 
                FROM LogForStudentOnline 
                WHERE {{orgid}} and {{start}} and {{end}} and {{parent}}='{{area}}'  
                GROUP BY {{field}} order by count desc";
            sql = sql.Replace("{{orgid}}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
            sql = sql.Replace("{{start}}", start == null ? "1=1" : "Lso_LoginTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sql = sql.Replace("{{end}}", end == null ? "1=1" : "Lso_LoginTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            if (string.IsNullOrWhiteSpace(province) && string.IsNullOrWhiteSpace(city))
            {
                sql = sql.Replace("{{field}}", "Lso_Province");
                sql = sql.Replace("{{parent}}", "''");
                sql = sql.Replace("{{area}}", "");
            }
            else if (!string.IsNullOrWhiteSpace(province))
            {
                sql = sql.Replace("{{field}}", "Lso_City");
                sql = sql.Replace("{{parent}}", "Lso_Province");
                sql = sql.Replace("{{area}}", province);
            }
            else if (!string.IsNullOrWhiteSpace(city))
            {
                sql = sql.Replace("{{field}}", "Lso_District");
                sql = sql.Replace("{{parent}}", "Lso_City");
                sql = sql.Replace("{{area}}", city);
            }

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 错题所属的课程
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couname">课程名称，可模糊查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Course[] QuesForCourse(int stid, string couname, int size, int index, out int countSum)
        {
            //计算满足条件的数量
            string sumSql = @"select COUNT(*) from course as c inner join 
                            (select cou_id from Student_Ques where Ac_ID={stid} group by cou_id) as q
                            on c.cou_id=q.cou_id where {course}";
            sumSql = sumSql.Replace("{stid}", stid.ToString());
            sumSql = sumSql.Replace("{course}", string.IsNullOrWhiteSpace(couname) ? "1=1" : "Cou_Name LIKE '%" + couname + "%'");
            countSum = Convert.ToInt32(Gateway.Default.FromSql(sumSql).ToScalar());

            //分页获取数据
            string sql = @"SELECT [TMP_TABLE].* FROM 
	                ( select c.*,q.count,ROW_NUMBER() OVER( ORDER BY q.sid desc ) AS TMP__ROWID  from course as c inner join 
                (select cou_id,max(Squs_ID) as 'sid',COUNT(*) as 'count' from Student_Ques where Ac_ID={stid} group by cou_id) as q
                on c.cou_id=q.cou_id where {course}  ) 
	                [TMP_TABLE] WHERE TMP__ROWID BETWEEN {start} AND {end}";
            sql = sql.Replace("{stid}", stid.ToString());
            sql = sql.Replace("{course}", string.IsNullOrWhiteSpace(couname) ? "1=1" : "Cou_Name LIKE '%" + couname + "%'");
            //RowNum BETWEEN (页码-1)*页大小+1 AND 页码*页大小
            sql = sql.Replace("{start}", ((index - 1) * size + 1).ToString());
            sql = sql.Replace("{end}", (index * size).ToString());

            return Gateway.Default.FromSql(sql).ToArray<Course>();

        }

        /// <summary>
        /// 购买的课程的学员，不重复
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="idcard"></param>
        /// <param name="mobi"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns>Ac_CurrCourse列为学员选修的课程数</returns>
        public List<Accounts> PurchasePager(int orgid, long stsid, long couid,
            string acc, string name, string idcard, string mobi,
           DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            //计算总数的脚本
            string sqlsum = @"select COUNT(*) as total from 
    (select ac_id,count(ac_id) as 'count' from 
		(select * from Student_Course where {{where4sc}} and ({{start}} and {{end}}) ) as ss   group by ac_id 
	) as sc  inner join
    Accounts as a on sc.Ac_ID = a.Ac_ID {{where4acc}}"; ;
            //购买记录的条件
            string where4sc = "{{orgid}} and {{couid}} and {{stsid}}";
            where4sc = where4sc.Replace("{{orgid}}", orgid > 0 ? "Student_Course.Org_ID =" + orgid.ToString() : "1=1");
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
            countSum = Convert.ToInt32(o);

            //分页查询的脚本
            string sqljquery = @"select Ac_ID,count as 'Ac_CurrCourse',Ac_AccName,Ac_Name,Ac_IDCardNumber,Ac_Age,Ac_Photo,
Ac_Money,Ac_Point,Ac_Coupon,Org_ID,Sts_ID,Sts_Name,Ac_Sex,Ac_MobiTel1,Ac_MobiTel2
                        from
                       (select a.*,count,ROW_NUMBER() OVER(Order by a.ac_id ) AS rowid from 
                         (select ac_id,count(ac_id) as 'count' from 
		(select * from Student_Course where {{where4sc}} and ({{start}} and {{end}}) ) as ss   group by ac_id 
	) as sc  inner join      
                         Accounts as a on sc.Ac_ID=a.Ac_ID {{where4acc}}) as pager  where  rowid > {{startindex}} and rowid<={{endindex}} ";
            sqljquery = sqljquery.Replace("{{start}}", start == null ? "1=1" : "Stc_StartTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqljquery = sqljquery.Replace("{{end}}", end == null ? "1=1" : "Stc_StartTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            sqljquery = sqljquery.Replace("{{where4acc}}", where4acc);
            sqljquery = sqljquery.Replace("{{where4sc}}", where4sc);
            int startindex = (index - 1) * size;
            int endindex = (index - 1) * size + size;
            sqljquery = sqljquery.Replace("{{startindex}}", startindex.ToString());
            sqljquery = sqljquery.Replace("{{endindex}}", endindex.ToString());

            return Gateway.Default.FromSql(sqljquery).ToList<Accounts>();
        }
        /// <summary>
        /// 学员的活跃情况
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="stsid">学员组id</param>
        /// <param name="acc">账号</param>
        /// <param name="name">姓名</param>
        /// <param name="mobi">手机号</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="code">学号</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="orderpattr">排序方式，asc或desc</param>
        /// <returns></returns>
        public DataTable Activation(int orgid, long stsid, string acc, string name, string mobi, string idcard, string code,
             string orderby, string orderpattr,
            int size, int index, out int countSum)
        {
            string sql = @"select *  from (
	                    select acc.Ac_ID,Ac_Name,Ac_AccName,Ac_Sex,Ac_IDCardNumber,Ac_MobiTel1,Ac_LastTime,Sts_ID,Sts_Name,Ac_Money,Ac_Photo
		                    ,logincount,logintime
		                    ,coursecount,rechargecount,lastrecharge,laststudy,lastexrcise,lasttest,lastexam
		                    ,ROW_NUMBER() OVER( ORDER BY {{orderby}} {{orderpattr}} ) AS rowid from Accounts as acc
	                    left join  --登录次数与最后登录时间
	                    (select Ac_id, COUNT(0) as 'logincount', max(Lso_CrtTime) as 'logintime' from LogForStudentOnline group by Ac_ID) as ol
		                    on acc.Ac_ID=ol.Ac_id
	                    left join --课程购买个数
	                    (select Ac_id, COUNT(0) as 'coursecount' from Student_Course group by Ac_ID) as buy
		                    on acc.Ac_ID=buy.Ac_id
	                    left join ----资金动向
	                    (select Ac_id, COUNT(*) as 'rechargecount',max(Ma_CrtTime) as 'lastrecharge'  from MoneyAccount where Ma_Type=2  group by Ac_ID) as recharge
		                    on acc.Ac_ID=recharge.Ac_ID			
	                    left join --视频学习记录
	                    (select Ac_id, max(Lss_LastTime) as 'laststudy' from LogForStudentStudy group by Ac_ID) as video
		                    on acc.Ac_ID=video.Ac_ID
	                    left join --试题练习记录
	                    (select Ac_id, max(Lse_LastTime) as 'lastexrcise' from LogForStudentExercise group by Ac_ID) as ques
		                    on acc.Ac_ID=ques.Ac_ID
	                    left join --测试成绩
	                    (select Ac_id, max(Tr_CrtTime) as 'lasttest' from TestResults group by Ac_ID) as test
		                    on acc.Ac_ID=test.Ac_ID
	                    left join --考试成绩
	                    (select Ac_id, max(Exr_CrtTime) as 'lastexam' from ExamResults group by Ac_ID) as exam
		                    on acc.Ac_ID=exam.Ac_ID
	                    --查询条件
	                    where {{where}}  	
                    ) as res where rowid BETWEEN {{start}} AND {{end}}";
            //查询条件
            string wheresql = @" {{orgid}} and {{stsid}} and {{acc}} and {{name}} and {{mobi}} and {{idcard}} and {{code}}";
            wheresql = wheresql.Replace("{{orgid}}", orgid <= 0 ? "1=1" : "Org_ID=" + orgid);
            wheresql = wheresql.Replace("{{stsid}}", stsid <= 0 ? "1=1" : "Sts_ID=" + stsid);
            wheresql = wheresql.Replace("{{acc}}", string.IsNullOrWhiteSpace(acc) ? "1=1" : "Ac_AccName like '%" + acc + "%'");
            wheresql = wheresql.Replace("{{name}}", string.IsNullOrWhiteSpace(name) ? "1=1" : "Ac_Name like '%" + name + "%'");
            wheresql = wheresql.Replace("{{mobi}}", string.IsNullOrWhiteSpace(mobi) ? "1=1" : "Ac_MobiTel1 like '%" + mobi + "%'");
            wheresql = wheresql.Replace("{{idcard}}", string.IsNullOrWhiteSpace(idcard) ? "1=1" : "Ac_IDCardNumber like '%" + idcard + "%'");
            wheresql = wheresql.Replace("{{code}}", string.IsNullOrWhiteSpace(code) ? "1=1" : "Ac_CodeNumber like '%" + code + "%'");
            //获取记录总数
            string sumSql = @"select COUNT(*) from Accounts where " + wheresql;
            countSum = Convert.ToInt32(Gateway.Default.FromSql(sumSql).ToScalar());

            //查询
            sql = sql.Replace("{{where}}", wheresql);
            //排序条件与方式
            sql = sql.Replace("{{orderby}}", string.IsNullOrWhiteSpace(orderby) ? "Ac_LastTime" : orderby);
            sql = sql.Replace("{{orderpattr}}", "asc".Equals(orderpattr, StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC");
            //RowNum BETWEEN(页码-1)*页大小 + 1 AND 页码*页大小
            sql = sql.Replace("{{start}}", ((index - 1) * size + 1).ToString());
            sql = sql.Replace("{{end}}", (index * size).ToString());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }

        /// <summary>
        /// 学员的学习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns>datatable中,LastTime:最后学习时间； studyTime：累计学习时间，complete：完成度百分比</returns>
        public DataTable StudentStudyCourseLog(int acid)
        {
            Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (student == null) throw new Exception("当前学员不存在！");
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == student.Org_ID).ToFirst<Organization>();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;

            ////清理掉不需要的数据，包括：“章节不存在，章节没有视频，章节禁用或未完成”的学习记录，全部删除
            //WhereClip wc = LogForStudentStudy._.Ac_ID == acid;
            //SourceReader lfs = Gateway.Default.FromSql(string.Format("select Ol_ID from [LogForStudentStudy] where Ac_ID={0} group by Ol_ID",acid)).ToReader();
            //while (lfs.Read())
            //{
            //    long olid = lfs.GetInt64("Ol_ID");
            //    Outline ol = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
            //    if (ol == null || ol.Ol_IsVideo == false || ol.Ol_IsUse == false || ol.Ol_IsFinish == false)
            //    {
            //        Gateway.Default.Delete<LogForStudentStudy>(LogForStudentStudy._.Ol_ID == olid);
            //    }
            //} ;
            //开始计算
            string sql = @"
select c.Cou_ID,Cou_Name,Sbj_ID,lastTime,studyTime,complete from course as c inner join 
	(select cou_id, max(lastTime) as 'lastTime',sum(studyTime) as 'studyTime',		
		sum(case when complete>=100 then 100 else complete end) as 'complete'
		from 
			(SELECT top 90000 ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
				 sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
				 (case  when max(Lss_Duration)>0 then
					 cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
					 else 0 end
				  ) as 'complete'
			 FROM [LogForStudentStudy]  where {acid}  group by ol_id 
			 ) as s where s.totalTime>0 group by s.cou_id
	) as tm on c.cou_id=tm.cou_id  ";
            //sql = sql.Replace("{orgid}", orgid > 0 ? "org_id=" + orgid : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            try
            {
                DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ///* 不要删除
                    //*****如果没有购买的，则去除
                    //购买的课程(含概试用的）
                    int count = 0;
                    List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseForStudent(acid, null, 0, null, null);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool isExist = false;
                        for (int j = 0; j < cous.Count; j++)
                        {
                            if (dt.Rows[i]["Cou_ID"].ToString() == cous[j].Cou_ID.ToString())
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            dt.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    // * */
                    //计算完成度                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        //课程的累计完成度
                        double complete = Convert.ToDouble(dr["complete"].ToString());
                        //课程id
                        long couid = Convert.ToInt64(dr["Cou_ID"].ToString());
                        int olnum = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true, null);
                        //完成度
                        double peracent = Math.Floor(complete / olnum * 100) / 100;
                        dr["complete"] = peracent >= (100 - tolerance) ? 100 : peracent;
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
    }
}
