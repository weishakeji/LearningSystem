using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;


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
    public class Depart : System.Web.Services.WebService
    {

        [WebMethod]
        //获取所有可显示的院系
        public Song.Entities.Depart[] Departs()
        {
            Song.Entities.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            return Business.Do<IDepart>().GetAll(org.Org_ID, true, true);
        }
        [WebMethod]
        //设置院系顺序，并返回共显示的所有院系
        public Song.Entities.Depart[] DepartOrder(string result)
        {
            this._SaveOrder(Server.UrlDecode(result));
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Song.Entities.Depart[] deps= Business.Do<IDepart>().GetAll(orgid);
            return deps;
        }
        [WebMethod]
        //修改院系，并返回共显示的所有院系
        public Song.Entities.Depart[] DepartUpdate(string result)
        {
            this._Update(Server.UrlDecode(result));
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Business.Do<IDepart>().GetAll(orgid);
        }
        [WebMethod]
        //添加院系，并返回共显示的所有院系
        public Song.Entities.Depart[] DepartAdd(string result)
        {
            this._Add(Server.UrlDecode(result));
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Business.Do<IDepart>().GetAll(orgid);
        }
        [WebMethod]
        //添加院系，并返回共显示的所有院系
        public Song.Entities.Depart[] DepartDel(string result)
        {
            this._Del(Server.UrlDecode(result));
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Business.Do<IDepart>().GetAll(orgid);
        }
        #region 私有方法
        /// <summary>
        /// 保存院系的顺序状态
        /// </summary>
        /// <param name="result"></param>
        private void _SaveOrder(string result)
        {
            if (result == "" || result == null) return;
            try
            {
                Business.Do<IDepart>().SaveOrder(result);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 修改院系信息
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
                Song.Entities.Depart dep = Business.Do<IDepart>().GetSingle(id);
                if (dep == null) return;
                dep.Dep_CnName = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                dep.Dep_CnAbbr = ((XmlElement)node.SelectSingleNode("CnAbbr")).InnerText;
                dep.Dep_EnName = ((XmlElement)node.SelectSingleNode("enname")).InnerText;
                dep.Dep_EnAbbr = ((XmlElement)node.SelectSingleNode("EnAbbr")).InnerText;
                dep.Dep_Code = ((XmlElement)node.SelectSingleNode("code")).InnerText;
                dep.Dep_IsShow = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsShow")).InnerText);
                dep.Dep_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                dep.Dep_Func = ((XmlElement)node.SelectSingleNode("func")).InnerText;
                dep.Dep_Phone = ((XmlElement)node.SelectSingleNode("phone")).InnerText;
                dep.Dep_Fax = ((XmlElement)node.SelectSingleNode("fax")).InnerText;
                dep.Dep_Email = ((XmlElement)node.SelectSingleNode("email")).InnerText;
                dep.Dep_Msn = ((XmlElement)node.SelectSingleNode("msn")).InnerText;
                dep.Dep_WorkAddr = ((XmlElement)node.SelectSingleNode("WorkAddr")).InnerText;
                //赋值
                Business.Do<IDepart>().Save(dep);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 添加院系信息
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
                Song.Entities.Depart dep = new Song.Entities.Depart();
                dep.Dep_CnName = ((XmlElement)node.SelectSingleNode("name")).InnerText;
                string pid = ((XmlElement)node.SelectSingleNode("pid")).InnerText;
                dep.Dep_PatId = string.IsNullOrWhiteSpace(pid) ? 0 : Convert.ToInt32(pid);
                dep.Dep_CnAbbr = ((XmlElement)node.SelectSingleNode("CnAbbr")).InnerText;
                dep.Dep_EnName = ((XmlElement)node.SelectSingleNode("enname")).InnerText;
                dep.Dep_EnAbbr = ((XmlElement)node.SelectSingleNode("EnAbbr")).InnerText;
                dep.Dep_Code = ((XmlElement)node.SelectSingleNode("code")).InnerText;
                dep.Dep_IsShow = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsShow")).InnerText);
                dep.Dep_IsUse = Convert.ToBoolean(((XmlElement)node.SelectSingleNode("IsUse")).InnerText);
                dep.Dep_Func = ((XmlElement)node.SelectSingleNode("func")).InnerText;
                dep.Dep_Phone = ((XmlElement)node.SelectSingleNode("phone")).InnerText;
                dep.Dep_Fax = ((XmlElement)node.SelectSingleNode("fax")).InnerText;
                dep.Dep_Email = ((XmlElement)node.SelectSingleNode("email")).InnerText;
                dep.Dep_Msn = ((XmlElement)node.SelectSingleNode("msn")).InnerText;
                dep.Dep_WorkAddr = ((XmlElement)node.SelectSingleNode("WorkAddr")).InnerText;
                //所属机构
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                dep.Org_ID = orgid;
                //赋值
                Business.Do<IDepart>().Add(dep);
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
            Business.Do<IDepart>().Delete(id);
        }
        #endregion
    }
}
