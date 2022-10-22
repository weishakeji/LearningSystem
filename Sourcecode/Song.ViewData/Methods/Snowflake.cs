using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Newtonsoft.Json.Linq;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 雪花id的相关处理，实际业务中用不到
    /// </summary>
    public class Snowflake : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 未经处理的课程
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Course[] CourseUntreated()
        {
            string sql = "SELECT *  FROM Course where Cou_ID<100000";
            return WeiSha.Data.Gateway.Default.FromSql(sql).ToArray<Song.Entities.Course>();
        }
        /// <summary>
        /// 将之前的课程，Cou_ID转为雪花id,原Cou_ID值记录到Cou_PID
        /// </summary>
        /// <returns></returns>
        public int CourseGenerate()
        {
            string sql = "SELECT *  FROM Course where Cou_ID<100000";
            Song.Entities.Course[] courses = WeiSha.Data.Gateway.Default.FromSql(sql).ToArray<Song.Entities.Course>();
            int i = 0;
            foreach(Song.Entities.Course c in courses)
            {
                long couid = c.Cou_ID;             
                long snowid = WeiSha.Core.Request.SnowID();
                Business.Do<ICourse>().CourseUpdate(couid,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Course._.Cou_ID,
                        Song.Entities.Course._.Cou_PID },
                    new object[] { snowid, couid });
                i++;
            }
            return i;
        }
        public JObject Generate(int count)
        {
            JObject jo = new JObject();
            for (int i = 0; i < count; i++)
            {
                long snowid = WeiSha.Core.Request.SnowID();             
                jo.Add("snow_"+ snowid, snowid.ToString());               
            }
            return jo;
        }
    }
}
