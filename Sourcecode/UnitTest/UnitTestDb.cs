using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Song.Entities;
using WeiSha.Data;
using System.Collections.Generic;

namespace UnitTest
{
    /// <summary>
    /// 数据库测试
    /// </summary>
    [TestClass]
    public class UnitTestDb
    {
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        [TestMethod]
        public void DbTest()
        {
            string name = string.Empty, connStr = string.Empty, providerName = string.Empty;
            string path = Helper.Path.WebSitePath();
            string dbconfig = System.IO.Path.Combine(path, "db.config");
            if (System.IO.File.Exists(dbconfig))
            {
                System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                xml.Load(dbconfig);
                System.Xml.XmlNode nodeconn = xml.LastChild;
                foreach (System.Xml.XmlNode node in nodeconn.ChildNodes)
                {
                    if (node.NodeType != System.Xml.XmlNodeType.Element) continue;

                    name = node.Attributes["name"]?.Value;
                    connStr = node.Attributes["connectionString"]?.Value;
                    providerName = node.Attributes["providerName"]?.Value;
                }
            }

            Assert.AreEqual(name, "Song");
        }
        /// <summary>
        /// 测试数据库操作
        /// </summary>
        [TestMethod]
        public void DbHandler()
        {
            Helper.DbProvider.SetDbGateway();
            Organization org = Gateway.Default.From<Organization>().ToFirst<Organization>();
            Assert.IsNotNull(org);
        }
        [TestMethod]
        public void DataTypes()
        {
            Helper.DbProvider.SetDbGateway();
            Song.ServiceImpls.DataBaseCom dbcom = new Song.ServiceImpls.DataBaseCom();
            List<string> types=dbcom.DataTypes();
            Assert.IsNotNull(types);
        }
    }
}
