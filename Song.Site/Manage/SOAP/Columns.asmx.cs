using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using System.Reflection;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.SOAP
{
    /// <summary>
    /// Depart 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://yuefan.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Columns : System.Web.Services.WebService
    {

        [WebMethod]
        //获取所有可显示的栏目
        public Song.Entities.Columns[] All()
        {
            //所属机构的所有课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            return Business.Do<IColumns>().All(org.Org_ID, null);
        }
        [WebMethod]
        //获取单个栏目信息
        public Song.Entities.Columns Column(int id)
        {
            return Business.Do<IColumns>().Single(id);
        }
        [WebMethod]
        //获取单个栏目信息
        public string ColumnJson(int id)
        {
            Song.Entities.Columns nc = Business.Do<IColumns>().Single(id);
            Type info = nc.GetType();
            PropertyInfo[] properties = info.GetProperties();
            string node = "var node={";
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo pi = properties[i];
                //当前属性的值
                object obj = info.GetProperty(pi.Name).GetValue(nc, null);
                node += pi.Name + ":";
                if (obj == null)
                {
                    node += "\"\"";
                }
                else
                {
                    string type = obj.GetType().Name;
                    switch (obj.GetType().Name)
                    {
                        case "Boolean":
                            node += obj.ToString().ToLower();
                            break;
                        case "String":
                            node += "\"" + obj.ToString() + "\"";
                            break;
                        case "DateTime":
                            node += "\"" + obj.ToString() + "\"";
                            break;
                        default:
                            node += obj.ToString();
                            break;
                    }
                }
                if (i != properties.Length-1) node += ",";
                node += "\n";
            }
            node += "};";
            return node;
        }
        [WebMethod]
        //设置栏目顺序，并返回共显示的所有栏目
        public string Order(string result)
        {
            //保存顺序
            this._SaveOrder(Server.UrlDecode(result));
            return _getTreeHtml();
        }
        [WebMethod]
        //修改栏目，并返回共显示的所有栏目
        public string Update(string result)
        {
            this._Update(Server.UrlDecode(result));
            return _getTreeHtml();
        }
        [WebMethod]
        //添加栏目，并返回共显示的所有栏目
        public string Add(string result)
        {
            this._Add(Server.UrlDecode(result));
            return _getTreeHtml();
        }
        [WebMethod]
        //添加栏目，并返回共显示的所有栏目
        public string Del(string result)
        {
            this._Del(Server.UrlDecode(result));
            return _getTreeHtml();
        }
        #region 私有方法
        /// <summary>
        /// 返回树形菜单的HTML代码
        /// </summary>
        /// <returns></returns>
        private string _getTreeHtml()
        {
            //所属机构的所有课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Columns[] ncs = Business.Do<IColumns>().All(org.Org_ID, null);
            WeiSha.WebControl.MenuTree mt = new WeiSha.WebControl.MenuTree();
            mt.Title = "全部栏目";
            mt.DataTextField = "Col_Name";
            mt.IdKeyName = "Col_Id";
            mt.ParentIdKeyName = "Col_PID";
            mt.TaxKeyName = "Col_Tax";
            mt.SourcePath = "/manage/Images/tree";
            mt.TypeKeyName = "Col_Type";
            mt.IsUseKeyName = "Col_IsUse";
            //mt.IsShowKeyName = "Nc_IsShow";
            mt.DataSource = ncs;
            mt.DataBind();
            return mt.HTML;
        }
        /// <summary>
        /// 保存栏目的顺序状态
        /// </summary>
        /// <param name="result"></param>
        private void _SaveOrder(string result)
        {
            if (result == "" || result == null) return;
            try
            {
                Business.Do<IColumns>().SaveOrder(result);               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改栏目信息
        /// </summary>
        /// <param name="result"></param>
        private void _Update(string result)
        {
            if (result == "" || result == null) return;
            XmlDocument resXml = new XmlDocument();
            try
            {
                resXml.LoadXml(result, false);
                //resXml.Validate
                XmlNode node = resXml.SelectSingleNode("node");                
                int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
                Song.Entities.Columns nc = Business.Do<IColumns>().Single(id);
                if (nc == null) return;
                nc.Col_Name = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                nc.Col_ByName = ((XmlElement)node.SelectSingleNode("byname")).InnerText;
                nc.Col_PID = Convert.ToInt32(((XmlElement)node.SelectSingleNode("pid")).InnerText);
                nc.Col_Type = ((XmlElement)node.SelectSingleNode("type")).InnerText;
                nc.Col_Title = ((XmlElement)node.SelectSingleNode("title")).InnerText;
                nc.Col_Keywords = ((XmlElement)node.SelectSingleNode("keywords")).InnerText;
                nc.Col_Descr = ((XmlElement)node.SelectSingleNode("desc")).InnerText;
                nc.Col_Intro = ((XmlElement)node.SelectSingleNode("intro")).InnerText;
                nc.Col_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                nc.Col_IsNote = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsNote")).InnerText);
                //赋值
                Business.Do<IColumns>().Save(nc);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 添加栏目信息
        /// </summary>
        /// <param name="result"></param>
        private void _Add(string result)
        {
            if (result == "" || result == null) return;
            XmlDocument resXml = new XmlDocument();
            try
            {
                resXml.LoadXml(result, false);
                XmlNode node = resXml.SelectSingleNode("node");
                int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
                Song.Entities.Columns nc = new Song.Entities.Columns();
                nc.Col_Name = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                nc.Col_ByName = ((XmlElement)node.SelectSingleNode("byname")).InnerText;
                //父id
                int pid = 0;
                int.TryParse(((XmlElement)node.SelectSingleNode("pid")).InnerText, out pid);
                nc.Col_PID = pid;
                nc.Col_Type = ((XmlElement)node.SelectSingleNode("type")).InnerText;
                nc.Col_Title = ((XmlElement)node.SelectSingleNode("title")).InnerText;
                nc.Col_Keywords = ((XmlElement)node.SelectSingleNode("keywords")).InnerText;
                nc.Col_Descr = ((XmlElement)node.SelectSingleNode("desc")).InnerText;
                nc.Col_Intro = ((XmlElement)node.SelectSingleNode("intro")).InnerText;                
                nc.Col_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                nc.Col_IsNote = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsNote")).InnerText);
                //添加
                Business.Do<IColumns>().Add(nc);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 删除某点节
        /// </summary>
        /// <param name="nodexml"></param>
        private void _Del(string nodexml)
        {
            if (nodexml == "" || nodexml == null) return;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(nodexml, false);
            XmlNode node = resXml.SelectSingleNode("node");
            int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
            Business.Do<IColumns>().Delete(id);
        }
        #endregion
    }
}
