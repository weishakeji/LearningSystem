using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;

namespace UnitTest.Helper
{
    /// <summary>
    /// 数据库相关
    /// </summary>
    public class DbProvider
    {
        /// <summary>
        /// 设置数据库访问接口
        /// </summary>
        public static void SetDbGateway()
        {
            string name=string.Empty, connStr = string.Empty, providerName = string.Empty;

            string path = Helper.Path.WebSitePath();
            string dbconfig = System.IO.Path.Combine(path, "db.config");
            if (System.IO.File.Exists(dbconfig))
            {
                System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                xml.Load(dbconfig);
                System.Xml.XmlNode nodeconn = xml.LastChild;
                foreach (System.Xml.XmlNode node in nodeconn.ChildNodes)
                {
                    if (node.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        name = node.Attributes["name"]?.Value;
                        connStr = node.Attributes["connectionString"]?.Value;
                        providerName = node.Attributes["providerName"]?.Value;
                        break;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(connStr)) throw new Exception("未找到数据库连接字符串");
            //
            WeiSha.Data.DbProvider provider = WeiSha.Data.DbProviderFactory.CreateDbProvider(providerName, connStr);
            WeiSha.Data.Gateway.SetDefault(provider);

        }
    }
}
