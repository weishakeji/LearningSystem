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
    public class Employee : System.Web.Services.WebService
    {

        [WebMethod]
        //获取所有在职员工
        public Song.Entities.EmpAccount[] Employees()
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Business.Do<IEmployee>().GetAll(orgid, -1, true, "");
        }
        [WebMethod]
        //获取某院系所有在职员工
        public Song.Entities.EmpAccount[] Emp4Depart(int depid)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            return Business.Do<IEmployee>().GetAll(orgid, depid, true, null);
        }
        [WebMethod]
        //递交员工与角色的关联
        public void UpdateEmp4Posi(string xml)
        {
            if (xml == "" || xml == null) return;
            XmlDocument resXml = new XmlDocument();
            xml = Server.UrlDecode(xml);
            try
            {
                resXml.LoadXml(xml, false);
                XmlNodeList nodeList = resXml.SelectSingleNode("nodes").ChildNodes;
                //取rootid
                XmlNode nodes = resXml.SelectSingleNode("nodes");
                XmlElement xenodes = (XmlElement)nodes;
                int posi = Convert.ToInt32(xenodes.Attributes["posi"].Value);
                Business.Do<IPosition>().DeleteRelation4Emp(posi);
                //遍历所有子节点 
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;
                    int empid = Convert.ToInt32(xe.Attributes["empid"].Value);
                    Song.Entities.EmpAccount mm = Business.Do<IEmployee>().GetSingle(empid);
                    if (mm != null && mm.Acc_AccName.ToLower() != "admin")
                    {
                        mm.Posi_Id = posi;
                        Business.Do<IEmployee>().Save(mm);
                    }
                }
            }
            catch
            {
            }
        }
        [WebMethod]
        //递交员工与角色的关联
        public void UpdateEmp4Group(string xml)
        {
            if (xml == "" || xml == null) return;
            XmlDocument resXml = new XmlDocument();
            xml = Server.UrlDecode(xml);
            try
            {
                resXml.LoadXml(xml, false);
                XmlNodeList nodeList = resXml.SelectSingleNode("nodes").ChildNodes;
                //取rootid
                XmlNode nodes = resXml.SelectSingleNode("nodes");
                XmlElement xenodes = (XmlElement)nodes;
                int groupid = Convert.ToInt32(xenodes.Attributes["groupid"].Value);
                Business.Do<IEmpGroup>().DelRelation4Group(groupid);
                //遍历所有子节点 
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;
                    int empid = Convert.ToInt32(xe.Attributes["empid"].Value);
                    Business.Do<IEmpGroup>().AddRelation(empid, groupid);
                }
            }
            catch
            {
            }
        }
    }
}
