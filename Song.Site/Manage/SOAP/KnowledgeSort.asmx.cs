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
    public class KnowledgeSort : System.Web.Services.WebService
    {

        [WebMethod]
        //获取所有可显示的栏目
        public Song.Entities.KnowledgeSort[] All()
        {
            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            return Business.Do<IKnowledge>().GetSortAll(org.Org_ID, -1,null);
        }
        [WebMethod]
        //获取单个栏目信息
        public Song.Entities.KnowledgeSort Single(int id)
        {
            return Business.Do<IKnowledge>().SortSingle(id);
        }
        [WebMethod]
        //获取单个栏目信息
        public string SingleJson(int id)
        {
            Song.Entities.KnowledgeSort nc = Business.Do<IKnowledge>().SortSingle(id);
            string json = nc.ToJson();
            return json;
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
            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.KnowledgeSort[] ncs = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, -1,null);
            WeiSha.WebControl.MenuTree mt = new WeiSha.WebControl.MenuTree();
            mt.Title = "全部分类";
            mt.DataTextField = "Kns_Name";
            mt.IdKeyName = "Kns_Id";
            mt.ParentIdKeyName = "Kns_PID";
            mt.TaxKeyName = "Kns_Tax";
            mt.SourcePath = "/manage/Images/tree";
            //mt.TypeKeyName = "Kns_Type";
            mt.IsUseKeyName = "Kns_IsUse";
            //mt.IsShowKeyName = "Nc_IsShow";
            mt.Root = 0;
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
                Business.Do<IKnowledge>().SortSaveOrder(result);               
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
                XmlNode node = resXml.SelectSingleNode("node");                
                int id = Convert.ToInt32(((XmlElement)node).Attributes["id"].Value);
                Song.Entities.KnowledgeSort nc = Business.Do<IKnowledge>().SortSingle(id);
                if (nc == null) return;
                nc.Kns_Name = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                nc.Kns_ByName = ((XmlElement)node.SelectSingleNode("byname")).InnerText;
                nc.Kns_PID = Convert.ToInt32(((XmlElement)node.SelectSingleNode("pid")).InnerText);
                nc.Kns_Intro = ((XmlElement)node.SelectSingleNode("intro")).InnerText;
                nc.Kns_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                //赋值
                Business.Do<IKnowledge>().SortSave(nc);
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
                Song.Entities.KnowledgeSort nc = new Song.Entities.KnowledgeSort();
                nc.Kns_Name = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                nc.Kns_ByName = ((XmlElement)node.SelectSingleNode("byname")).InnerText;
                nc.Kns_PID = Convert.ToInt32(((XmlElement)node.SelectSingleNode("pid")).InnerText);
                nc.Kns_Intro = ((XmlElement)node.SelectSingleNode("intro")).InnerText;                
                nc.Kns_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                //添加
                Business.Do<IKnowledge>().SortAdd(nc);
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
            Business.Do<IKnowledge>().SortDelete(id);
        }
        #endregion
    }
}
