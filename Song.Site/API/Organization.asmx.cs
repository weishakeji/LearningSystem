using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Song.Site.SOAP
{
    /// <summary>
    /// Organization 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [XmlInclude(typeof(OrginDataInfo))]
    [ToolboxItem(false)]
    public class Organization : System.Web.Services.WebService
    {
        /// <summary>
        /// 当前机构的名称
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string Name()
        {
            //机构中文全名
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            return org.Org_Name;
        }
        /// <summary>
        /// 当前机构的详细信息（来自机构表的属性）
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.Organization Details()
        {
            //机构完整信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            string path = WeiSha.Common.Upload.Get["Org"].Virtual;
            org.Org_LicensePic = path + "Org/" + org.Org_LicensePic;
            org.Org_Logo = path + "Org/" + org.Org_Logo;
            return org;
        }
        /// <summary>
        /// 所有机构的信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public Song.Entities.Organization[] All()
        {
            //所有机构完整信息
            Song.Entities.Organization[] org = Business.Do<IOrganization>().OrganAll(null, -1);
            string path = WeiSha.Common.Upload.Get["Org"].Virtual;
            foreach (Song.Entities.Organization o in org)
            {
                o.Org_LicensePic = path + "Org/" + o.Org_LicensePic;
                o.Org_Logo = path + "Org/" + o.Org_Logo;
            }
            return org;
        }
        /// <summary>
        /// 机构的数据信息，如学员数等
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [WebMethod]
        public object DataInfo(int orgid)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) org = Business.Do<IOrganization>().OrganCurrent();        
            // 
            OrginDataInfo info = new OrginDataInfo();
            info.OrgName = org.Org_Name;
            info.PlateName = org.Org_PlatformName;
            info.Subject = Business.Do<ISubject>().SubjectOfCount(org.Org_ID, true, -1);
            info.Course = Business.Do<ICourse>().CourseOfCount(org.Org_ID, -1, -1);
            info.Questions = Business.Do<IQuestions>().QuesOfCount(org.Org_ID, -1, -1, -1, -1, true);
            info.TestPaper = Business.Do<ITestPaper>().PagerOfCount(org.Org_ID, -1, -1, -1, true);
            info.Student = Business.Do<IAccounts>().AccountsOfCount(org.Org_ID, null);
            info.Article = Business.Do<IContents>().ArticleOfCount(org.Org_ID, -1);
            info.Teacher = Business.Do<ITeacher>().TeacherOfCount(org.Org_ID, null);
            info.Knowledge = Business.Do<IKnowledge>().KnowledgeOfCount(org.Org_ID, -1, null);
            info.Notice = Business.Do<INotice>().OfCount(org.Org_ID, null);
            return info;
        }
    }
    /// <summary>
    /// 系统中各项数据
    /// </summary>
    [Serializable]
    public class OrginDataInfo
    {
        public string OrgName { get; set; }
        public string PlateName { get; set; }
        public int Subject { get; set; }
        public int Course { get; set; }
        public int Questions { get; set; }
        public int TestPaper { get; set; }
        public int Student { get; set; }
        public int Article { get; set; }
        public int Teacher { get; set; }
        public int Knowledge { get; set; }
        public int Notice { get; set; }
    }
}
