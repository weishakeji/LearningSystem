using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Song.ViewData.Helper
{
    public class AreaCode
    {
        private static readonly Dictionary<int, string> _areas = new Dictionary<int, string>();
        private static readonly object _lock = new object();
        /// <summary>
        /// 行政区域的键值表，key为区号，value为区划名称
        /// </summary>
        public static Dictionary<int, string> Areas
        {
            get
            {
                lock (_lock)
                {
                    if (_areas.Count < 1)
                    {
                        string file = HostingEnvironment.MapPath("~/Utilities/AreaCodeInfo.csv");
                        //string file = "E:\\SourceCode\\02_LearningSystem_dev\\Sourcecode\\Song.WebSite\\Utilities\\AreaCodeInfo.csv";
                        try
                        {
                            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                            {
                                string line;
                                int code;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    line = line.Trim();
                                    if (string.IsNullOrWhiteSpace(line)) continue;
                                    if (line.IndexOf(",") < 0) continue;
                                    int.TryParse(line.Substring(0, line.IndexOf(",")), out code);
                                    if (!_areas.ContainsKey(code))
                                        _areas.Add(code, line.Substring(line.IndexOf(",") + 1));
                                }
                            }
                        }
                        catch { }
                    }
                    return _areas;
                }
            }
        }
        /// <summary>
        /// 所有省级单位信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> Provinces()
        {
            Dictionary<int, string> province = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> kvp in Areas)
            {
                if (kvp.Key % 10000 > 0) continue;
                province.Add(kvp.Key, kvp.Value);              
            }
            return province;
        }
        /// <summary>
        /// 城市信息
        /// </summary>
        /// <param name="province">省级单位编码</param>
        /// <returns></returns>
        public static Dictionary<int, string> Cities(int province)
        {
            while (province > 99) province /= 10;   //不管省级代码是几位，最终减为两位
            Dictionary<int, string> cities = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> kvp in Areas)
            {
                if (kvp.Key / 10000 != province) continue;  //不是本省的跳过
                if (kvp.Key % 10000 == 0) continue;         //省级单位名称，跳过
                if (kvp.Key % 100 != 0) continue;
                cities.Add(kvp.Key, kvp.Value);
            }
            return cities;
        }
        /// <summary>
        /// 区县信息
        /// </summary>
        /// <param name="city">市级单位编码</param>
        /// <returns></returns>
        public static Dictionary<int, string> Districts(int city)
        {
            while (city > 9999) city /= 10;   //不管市级代码是几位，最终减为4位
            Dictionary<int, string> districts = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> kvp in Areas)
            {
                if (kvp.Key / 100 != city) continue;  //不是本市的跳过
                if (kvp.Key % 100 == 0) continue;         //当前市级单位，跳过           
                districts.Add(kvp.Key, kvp.Value);
            }
            return districts;
        }
        /// <summary>
        /// 通过行政区划名称，获取代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetCode(string name)
        {
            foreach (KeyValuePair<int, string> kvp in Areas)
                if (kvp.Value.Equals(name)) return kvp.Key;       
            return -1;
        }
        /// <summary>
        /// 通过代码获取区划名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetArea(int code)
        {
            while (code.ToString().Length < 6) code *= 10;
            while (code.ToString().Length > 6) code /= 10;
            foreach (KeyValuePair<int, string> kvp in Areas)           
                if (kvp.Key==code) return kvp.Value;           
            return string.Empty;
        }
    }
}
