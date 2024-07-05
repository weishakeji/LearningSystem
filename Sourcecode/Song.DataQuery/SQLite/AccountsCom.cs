using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Data;

namespace Song.DataQuery.SQLite
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
            //支持sqlserver,sqlite
            string sql = @"select interval*{interval} as 'group',COUNT(0) as 'count' from
                            (select FLOOR(age / {interval}) as interval, age  from
                            (select * from
                            (select  {year} - Ac_Age as 'age' from Accounts where {orgid}) as agedata where age < 100 and age > 0) as tt
                            ) as result group by interval order by interval asc";
            sql = sql.Replace("{interval}", interval.ToString());
            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
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
            string sql = @"SELECT STRFTIME('{d}', Ac_RegTime) AS ""group"",
                               COUNT(*) AS ""count""
                        FROM Accounts
                        WHERE {orgid}
                        AND Ac_RegTime > '{start}' AND Ac_RegTime <= '{end}'
                        GROUP BY STRFTIME('{d}', Ac_RegTime)
                        ORDER BY STRFTIME('{d}', Ac_RegTime); ";
            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
            //时间区间
            sql = sql.Replace("{start}", start.ToString("yyyy-MM-dd"));
            sql = sql.Replace("{end}", end.ToString("yyyy-MM-dd"));
            //按时间间隔
            if ("y".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%Y");
            else if ("d".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%Y-%m-%d");
            else if ("w".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%w");
            else
                sql = sql.Replace("{d}", "%Y-%m");

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
            //string sql = @"select CONVERT(varchar, dt, 23) as 'group', COUNT(*) as 'count' from
            //        (
            //         select DATEADD({d}, DATEDIFF({d}, 0, Ac_LastTime), 0) AS dt from Accounts 
            //         where {orgid} and Ac_LastTime>'{start}' and Ac_LastTime<='{end}'
            //        ) as ym
            //        group by dt order by dt asc";
            string sql = @"SELECT STRFTIME('{d}', Ac_LastTime) AS ""group"",
                               COUNT(*) AS ""count""
                        FROM Accounts
                        WHERE {orgid}
                        AND Ac_LastTime > '{start}' AND Ac_LastTime <= '{end}'
                        GROUP BY STRFTIME('{d}', Ac_LastTime)
                        ORDER BY STRFTIME('{d}', Ac_LastTime); ";

            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid : "1=1");
            //时间区间
            sql = sql.Replace("{start}", start.ToString("yyyy-MM-dd"));
            sql = sql.Replace("{end}", end.ToString("yyyy-MM-dd"));
            //按时间间隔
            if ("y".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%Y");
            else if ("d".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%Y-%m-%d");
            else if ("w".Equals(interval, StringComparison.OrdinalIgnoreCase))
                sql = sql.Replace("{d}", "%w");
            else
                sql = sql.Replace("{d}", "%Y-%m");

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
            //string sql = @"select datevalue, SUM(Ma_Money) as 'money' from
            //        (select DATEADD({interval}, DATEDIFF({interval}, 0, Ma_CrtTime), 0) AS 'datevalue',Ma_Money from 
            //        MoneyAccount where Ma_IsSuccess=1 and {type} and {from} and {acid} and {orgid} and
            //        {start} and {end}
            //        ) as ym
            //        group by datevalue order by datevalue desc";

            string sql = @"SELECT STRFTIME('{d}', Ma_CrtTime) AS datevalue, SUM(Ma_Money) AS 'money'
                    FROM (SELECT STRFTIME('{d}', Ma_CrtTime) AS datevalue, Ma_Money,Ma_CrtTime
                          FROM MoneyAccount
                          WHERE Ma_IsSuccess = 1 AND {type} AND {from} AND {orgid}
                          AND {start} AND Ma_CrtTime <{end}
                         ) AS ym
                    GROUP BY datevalue
                    ORDER BY datevalue DESC;";
            //日:Day，月：MONTH,年:YEAR，周:WEEK
            string unit = "%Y-%m";
            if (interval == "y") unit = "%Y";
            if (interval == "w") unit = "%w";
            if (interval == "d") unit = "%Y-%m-%d";
            sql = sql.Replace("{d}", unit.ToString());
            //机构id
            sql = sql.Replace("{orgid}", orgid > 0 ? "Org_ID=" + orgid.ToString() : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? "Ac_ID=" + acid.ToString() : "1=1");
            //
            sql = sql.Replace("{type}", type > 0 ? "Ma_Type=" + type.ToString() : "1=1");
            sql = sql.Replace("{from}", from > 0 ? "Ma_From=" + from.ToString() : "1=1");
            //
            sql = sql.Replace("{start}", start != null ? "Ma_CrtTime>='" + ((DateTime)start).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
            sql = sql.Replace("{end}", end != null ? "Ma_CrtTime<'" + ((DateTime)end).ToString("yyyy-MM-dd HH:mm:ss") + "'" : "1=1");
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
