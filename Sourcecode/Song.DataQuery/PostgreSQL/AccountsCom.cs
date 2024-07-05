using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Data;

namespace Song.DataQuery.PostgreSQL
{
    public class AccountsCom
    {
        /// <summary>
        /// 统计各个年龄段的学员
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">年龄间隔，即某个年龄段</param>
        /// <returns></returns>
        public DataTable AgeGroup(int orgid, int interval)
        {
            if (interval <= 0) interval = 10;
            //支持postgresql
            string sql = @"select interval*{interval} as group,COUNT(0) as count from
                            (select FLOOR(age / {interval}) as interval, age  from
                            (select * from
                            (select  {year} - ""Ac_Age"" as age from ""Accounts"" where {orgid}) as agedata where age < 100 and age > 0) as tt
                            ) as result group by interval order by interval asc";
            sql = sql.Replace("{interval}", interval.ToString());
            sql = sql.Replace("{orgid}", orgid > 0 ? @"""Org_ID""=" + orgid : "1=1");
            sql = sql.Replace("{year}", DateTime.Now.Year.ToString());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 统计学员注册的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">间隔单位，y为年,m为月,d为日</param>
        /// <param name="start">统计区间的起始时间</param>
        /// <param name="end">统计区间的结束时间</param>
        /// <returns></returns>
        public DataTable RegTimeGroup(int orgid, string interval, DateTime start, DateTime end)
        {
            string sql = @"select dt as group, COUNT(*) as count from
                    (
	                    select TO_CHAR(""Ac_RegTime"", '{d}') AS dt from ""Accounts""
                        where ""Org_ID"" = 5 and ""Ac_RegTime"" > '2023-06-09' and ""Ac_RegTime"" <= '2024-06-09'
                    ) as ym
                    group by dt order by dt asc";
            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
            //时间区间
            sql = sql.Replace("{start}", start.ToString("yyyy-MM-dd"));
            sql = sql.Replace("{end}", end.ToString("yyyy-MM-dd"));
            //按时间间隔
            if ("y".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-01-01");
            else if ("d".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-MM-DD");
            else if ("w".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "IW");
            else
                sql = sql.Replace("{d}", "YYYY-MM-01");

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 统计学员登录情况
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">间隔单位，y为年,m为月,d为日</param>
        /// <param name="start">统计区间的起始时间</param>
        /// <param name="end">统计区间的结束时间</param>
        /// <returns></returns>
        public DataTable LoginTimeGroup(int orgid, string interval, DateTime start, DateTime end)
        {
            string sql = @"select dt as group, COUNT(*) as count from
                    (
	                    select TO_CHAR(""Ac_LastTime"", '{d}')  AS dt from ""Accounts"" 

                        where ""Org_ID"" = 5 and ""Ac_LastTime"" > '2023-06-09' and ""Ac_LastTime"" <= '2024-06-09'
                    ) as ym
                    group by dt order by dt asc";
            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
            //时间区间
            sql = sql.Replace("{start}", start.ToString("yyyy-MM-dd"));
            sql = sql.Replace("{end}", end.ToString("yyyy-MM-dd"));
            //按时间间隔
            if ("y".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-01-01");
            else if ("d".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-MM-DD");
            else if ("w".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "IW");
            else
                sql = sql.Replace("{d}", "YYYY-MM-01");

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 按日期统计资金
        /// </summary>
        /// <param name="interval">时间间隔，YEAR:按年统计,MONTH:按月，WEEK:按周，Day:按日</param>
        /// <param name="orgid">机构id</param>
        /// <param name="acid">账号id</param>
        /// <param name="type">1支出，2收入（包括充值、分润等）</param>
        /// <param name="from">类型，来源，1为管理员操作，2为充值码充值；3这在线支付；4购买课程,5分润</param>
        /// <param name="start">时间区间的起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public Dictionary<string, double> MoneyStatistics(string interval, int orgid, int acid, int type, int from, DateTime? start, DateTime? end)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            string sql = @"select datevalue, SUM(""Ma_Money"") as money from
                    (select TO_CHAR(""Ma_CrtTime"", '{d}') AS datevalue,""Ma_Money"" from 
                    ""MoneyAccount"" where ""Ma_IsSuccess""=true and {type} and {from} and {acid} and {orgid} and
                    {start} and {end}
                    ) as ym
                    group by datevalue order by datevalue desc";
            //日:Day，月：MONTH,年:YEAR，周:WEEK
            //按时间间隔
            if ("y".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-01-01");
            else if ("d".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "YYYY-MM-DD");
            else if ("w".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "IW");
            else
                sql = sql.Replace("{d}", "YYYY-MM-01");
            //机构id
            sql = sql.Replace("{orgid}", orgid > 0 ? @"""Org_ID""=" + orgid.ToString() : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? @"""Ac_ID""=" + acid.ToString() : "1=1");
            //
            sql = sql.Replace("{type}", type > 0 ? @"""Ma_Type""=" + type.ToString() : "1=1");
            sql = sql.Replace("{from}", from > 0 ? @"""Ma_From""=" + from.ToString() : "1=1");
            //
            sql = sql.Replace("{start}", start != null ? @"""Ma_CrtTime"">='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            sql = sql.Replace("{end}", end != null ? @"""Ma_CrtTime""<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                while (reader.Read())
                {
                    object dvalue = reader["datevalue"];
                    DateTime datevalue = Convert.ToDateTime(dvalue);

                    object dmoney = reader["money"];
                    double money = Convert.ToDouble(dmoney);
                    dic.Add(datevalue.ToString("yyyy-MM-dd HH:mm:ss"), (double)money);
                }
                reader.Close();
                reader.Dispose();

            }
            return dic;
        }
    }
}
