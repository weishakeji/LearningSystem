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
    /// Purview 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://yuefan.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Purview : System.Web.Services.WebService
    {

        [WebMethod]
        //用于权限管理的功能菜单，禁用的不显示
        public Song.Entities.ManageMenu[] PurViewMenu()
        {
            //获取功能菜单树的所有菜单项
            return Business.Do<IManageMenu>().GetTree("func", true, null);
        }
        [WebMethod]
        //员工的所有权限，包括所在院系、所属角色、所在员工组的所有权限
        public Song.Entities.ManageMenu[] GetAll4Emplyee(int empId)
        {
            //获取功能菜单树的所有菜单项
            return Business.Do<IPurview>().GetAll4Emplyee(empId);
        }
        [WebMethod]
        //员工的所有权限，包括所在院系、所属角色、所在员工组的所有权限,输出菜单id,如“12,34,22”
        public string GetAll4Array(int empId)
        {
            //获取功能菜单树的所有菜单项
            Song.Entities.ManageMenu[] mm = Business.Do<IPurview>().GetAll4Emplyee(empId);
            string ManageId = "";
            foreach (Song.Entities.ManageMenu m in mm)
            {
                ManageId += "," + m.MM_Id;
            }
            if (ManageId != "")
            {
                ManageId = ManageId.Substring(1);
            }
            return ManageId;
        }
        [WebMethod]
        //用于权限管理的功能菜单
        public Song.Entities.Purview[] GetPurView(int memberId, string type)
        {
            //获取功能菜单树的所有菜单项
            return Business.Do<IPurview>().GetAll(memberId, type);
        }
        [WebMethod]
        //接收权限信息，成功后返回1
        public int AcceptPurView(string xml)
        {
            xml = Server.UrlDecode(xml);
            return _Accept(xml);
        }
        #region 私有方法
        /// <summary>
        /// 记录权限
        /// </summary>
        /// <param name="result">客户端提交的权限信息</param>
        /// <returns></returns>
        private int _Accept(string result)
        {
            if (result == "" || result == null) return 0;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(result, false);
            //取类别等
            XmlNode nodes = resXml.SelectSingleNode("nodes");
            XmlElement xenodes = (XmlElement)nodes;
            int memberId = Convert.ToInt32(xenodes.Attributes["memberId"].Value);
            string type = xenodes.Attributes["type"].Value;
            XmlNodeList nodeList = resXml.SelectSingleNode("nodes").ChildNodes;
            //遍历所有子节点 
            string mmids = "";
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                string mmid = xe.Attributes["MM_Id"].Value;
                string state = xe.Attributes["state"].Value;
                mmids += mmid + "|" + state + ",";
            }
            Business.Do<IPurview>().AddBatch(memberId, mmids, type);
            return 1;
        }
        #endregion
    }
}
