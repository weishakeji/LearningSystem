using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 单点登录的管理
    /// </summary>
    public class Sso : ViewMethod, IViewAPI
    {
        #region 增删改查
        /// <summary>
        /// 所有配置项
        /// </summary>
        /// <param name="use"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleSignOn[] All(bool? use)
        {
            return Business.Do<ISSO>().GetAll(use);
        }
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SingleSignOn ForID(int id)
        {
            return Business.Do<ISSO>().GetSingle(id);
        }
        /// <summary>
        /// 通过APPID获取登录接口对象,用于前端登录时使用，如果登录接口被禁用，则返回null
        /// </summary>
        /// <param name="appid">链接分类的id</param>
        /// <returns>如果登录接口被禁用，则返回null</returns>
        public SingleSignOn ForAPPID(string appid)
        {
            SingleSignOn sso= Business.Do<ISSO>().GetSingle(appid);
            if (sso == null) return null;
            return sso.SSO_IsUse ? sso : null;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(string name, int id)
        {
            return Business.Do<ISSO>().Exist(name,id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<ISSO>().Delete(idval);               
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Add(SingleSignOn entity)
        {
            //域名全部转小写
            if (!string.IsNullOrWhiteSpace(entity.SSO_Domain))
                entity.SSO_Domain = entity.SSO_Domain.ToLower();
            Business.Do<ISSO>().Add(entity);
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Modify(SingleSignOn entity)
        {
            Song.Entities.SingleSignOn old = Business.Do<ISSO>().GetSingle(entity.SSO_ID);
            if (old == null) throw new Exception("Not found entity for SingleSignOn！");
            //域名全部转小写
            if (!string.IsNullOrWhiteSpace(entity.SSO_Domain))
                entity.SSO_Domain = entity.SSO_Domain.ToLower();
            old.Copy<Song.Entities.SingleSignOn>(entity);
            Business.Do<ISSO>().Save(old);
            return true;
        }
        #endregion

        #region 登录与退出
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="appid">单点登录的接口标识</param>
        /// <param name="user">账号</param>
        /// <param name="name">账号的姓名</param>
        /// <param name="sort">学员组名称</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts Login(string appid, string user, string name, string sort)
        {
            if (string.IsNullOrWhiteSpace(appid) || string.IsNullOrWhiteSpace(user)) return null;
            //单点登录接口是否存在或启用
            Song.Entities.SingleSignOn sso = Business.Do<ISSO>().GetSingle(appid);
            if (sso == null || !sso.SSO_IsUse) return null;
            //账号是否存在或禁用
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(user, -1);
            if (acc != null && (!acc.Ac_IsUse || !acc.Ac_IsPass))
                throw VExcept.Verify("账号：" + user + " 被禁用或未通过审核", 103);

            string PathKey = "Accounts";
            string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
            string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
            //如果账号存在，直接返回登录状态
            if (acc != null)
            {
                acc = Business.Do<IAccounts>().AccountsLogin(acc);
                acc.Ac_Photo = System.IO.File.Exists(PhyPath + acc.Ac_Photo) ? VirPath + acc.Ac_Photo : "";
                acc.Ac_Pw = LoginAccount.Status.Generate_checkcode(acc);
                LoginAccount.Status.Fresh(acc);
                return acc;
            } else if (sso.SSO_IsAdd)   //登录接口，允许添加学员
            {
                acc = new Accounts();
                acc.Ac_AccName = user;
                if (!string.IsNullOrWhiteSpace(name)) acc.Ac_Name = name;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                acc.Org_ID = org.Org_ID;
             
                if (!string.IsNullOrWhiteSpace(sort))
                {
                    Song.Entities.StudentSort stsort = Business.Do<IStudent>().SortSingle(sort, org.Org_ID);
                    if (stsort != null)
                    {
                        acc.Sts_ID = stsort.Sts_ID;
                        acc.Sts_Name = stsort.Sts_Name;
                    }
                    else if(sso.SSO_IsAddSort)
                    {
                        stsort = new StudentSort();
                        stsort.Sts_Name = sort;
                        stsort.Org_ID = org.Org_ID;
                        stsort.Org_Name = org.Org_Name;
                        stsort.Sts_IsUse = true;
                        Business.Do<IStudent>().SortAdd(stsort);
                        acc.Sts_ID = stsort.Sts_ID;
                        acc.Sts_Name = stsort.Sts_Name;
                    }
                }
                acc.Ac_IsPass = acc.Ac_IsUse = true;
                Business.Do<IAccounts>().AccountsAdd(acc);

                //acc = Business.Do<IAccounts>().AccountsSingle(user, -1);
                acc = Business.Do<IAccounts>().AccountsLogin(acc);
                acc.Ac_Pw = LoginAccount.Status.Generate_checkcode(acc);
                LoginAccount.Status.Fresh(acc);
                return acc;
            }
            else
            {
                throw VExcept.Verify("账号：" + user + " 不存在，且没有创建账号的权限", 104);
            }           
        }
        #endregion
    }
}
