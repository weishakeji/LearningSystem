using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Song.ServiceInterfaces;
using WeiSha.Common;
using Song.ViewData.Attri;
using System.IO;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 平台信息
    /// </summary>
    [HttpGet]
    public class Platform : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        /// <returns></returns>
        public WeiSha.Common.License Version()
        {
            WeiSha.Common.License lic = WeiSha.Common.License.Value;
            return lic;
        }
        /// <summary>
        /// 版权信息，来自根路径下的copyright.xml文件
        /// </summary>
        /// <returns></returns>
        public Copyright_Item[] Copyright()
        {
            List<Copyright_Item> list = new List<Copyright_Item>();
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load(WeiSha.Common.Server.MapPath("~/Copyright.xml"));
            System.Xml.XmlNodeList nodes = xml.SelectNodes("Copyright/*");
            foreach (System.Xml.XmlNode n in nodes)
            {
                string remark = n.Attributes["remark"] != null ? n.Attributes["remark"].Value : string.Empty;
                string type = n.Attributes["type"] != null ? n.Attributes["type"].Value : string.Empty;
                list.Add(new Copyright_Item()
                {
                    Name = n.Name,
                    Remark = Microsoft.JScript.GlobalObject.escape(remark),
                    Type = type,
                    Text = Microsoft.JScript.GlobalObject.escape(n.InnerText.Trim())
                });
            }
            return list.ToArray<Copyright_Item>();
        }


        /// <summary>
        /// 数据库是否链接正常
        /// </summary>
        /// <returns></returns>
        public bool DbConnection()
        {
            bool isCorrect = Business.Do<ISystemPara>().DatabaseLinkTest();
            return isCorrect;
        }

        /// <summary>
        /// 数据库版本
        /// </summary>
        /// <returns></returns>
        public string DbVersion()
        {
            object version = Business.Do<ISystemPara>().ScalarSql("select @@version");
            if (version == null) return string.Empty;
            string str = version.ToString();
            str = str.Replace("\n", "").Replace("\t", "").Replace("\r", "");
            return str;
        }

        /// <summary>
        /// 数据库字段与表是否完成
        /// </summary>
        /// <returns></returns>
        public string[] DbCheck()
        {
            bool isCorrect = Business.Do<ISystemPara>().DatabaseLinkTest();
            if (!isCorrect)
            {
                throw new Exception("数据库链接不正常！");
            }
            List<string> error = Business.Do<ISystemPara>().DatabaseCompleteTest();
            if (error == null)
            {
                return new string[] { };
            }
            return error.ToArray<string>();
        }
        /// <summary>
        /// 机构公章信息
        /// </summary>
        /// <returns>path:公章图片路径;positon:位置</returns>
        public Dictionary<string, string> Stamp()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //公章
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //公章显示位置
            string positon = config["StampPosition"].Value.String;
            if (string.IsNullOrEmpty(positon)) positon = "right-bottom";
            dic.Add("positon", positon);
            //公章图像信息
            string stamp = config["Stamp"].Value.String;
            string filepath = Upload.Get["Org"].Physics + stamp;           
            dic.Add("path", !File.Exists(filepath) ? "" : Upload.Get["Org"].Virtual + stamp);
            return dic;
        }
        //其它基础信息
    }
    #region
    /// <summary>
    /// 版权信息的项
    /// </summary>
    public class Copyright_Item
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
    #endregion
}
