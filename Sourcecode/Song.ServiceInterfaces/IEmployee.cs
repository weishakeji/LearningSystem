using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系员工的管理
    /// </summary>
    public interface IEmployee : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(EmpAccount entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(EmpAccount entity);
        /// <summary>
        /// 修改某个字段
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        void Update(int acid, Field[] fiels, object[] objs);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(EmpAccount entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        EmpAccount GetSingle(int identify);
        /// <summary>
        /// 管理账号登录
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid">管理员所在的机构id，如果小于等于零，取所有机构的管理员</param>
        /// <param name="posid">岗位的id</param>
        /// <returns></returns>
        EmpAccount EmpLogin(string acc, string pw, int orgid,int posid);
        /// <summary>
        /// 用于记录每次登录生成的验证码，用于：同一账号登录时，当前账号下线
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool RecordLoginCode(int accid, string code);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accid">账号</param>
        /// <param name="oldpw"></param>
        /// <param name="newpw"></param>
        /// <returns></returns>
        bool ChangePw(int accid, string oldpw, string newpw);
        /// <summary>
        /// 根据公司id获取本公司的管理员
        /// </summary>
        /// <param name="orgid">公司id</param>
        /// <returns></returns>
        EmpAccount GetAdminByOrgId(int orgid);
        /// <summary>
        /// 获取单一实体对象，按员工手机号码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        EmpAccount GetSingleByPhone(string phoneNumber);
        /// <summary>
        /// 获取单一实体对象，按员工名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        EmpAccount GetSingleByName(string name);
        /// <summary>
        /// 获取单一实体对象，按员工帐号名称与密码
        /// </summary>
        /// <param name="acc">员工帐号名称</param>
        /// <param name="pw">员工密码,MD5加密字符串</param>
        /// <returns></returns>
        EmpAccount GetSingle(string acc, string pw);
        EmpAccount GetSingle(int orgid, string acc, string pw);
        /// <summary>
        /// 获取当前员工所在的院系
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Depart Get4Depart(int identify);
        /// <summary>
        /// 当前员工是否为超级管理员
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsSuperAdmin(int identify);
        /// <summary>
        /// 当前员工是否为根机构员工
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsForRoot(int identify);
        /// <summary>
        /// 当前用户是否为超级管理员
        /// </summary>
        /// <param name="acc">当前用户对象</param>
        /// <returns></returns>
        bool IsSuperAdmin(EmpAccount acc);
        /// <summary>
        /// 当前员工是否为管理员
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsAdmin(int identify);
        /// <summary>
        /// 当前员工是否存在（通过帐号判断）
        /// </summary>
        /// <param name="accname">账号名称</param>
        /// <param name="accid">账号id</param>
        /// <returns>如果已经存在，则返回true</returns>
        bool IsExists(string accname, int accid);
        /// <summary>
        /// 验证能否登录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accname">员工帐号</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        bool LoginCheck(int orgid, string accname, string pw);
        /// <summary>
        /// 通过手机号码验证，当前员工是否为在职员工
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        bool IsOnJob(string phoneNumber);
        /// <summary>
        /// 获取对象；即所有员工；
        /// </summary>
        /// <returns></returns>
        EmpAccount[] GetAll(int orgid);        

        EmpAccount[] GetAll(int orgid, int depId, bool? isUse, string searTxt);
        /// <summary>
        /// 获取某个分厂的所有员工帐号；
        /// </summary>
        /// <param name="orgid">分厂id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt">员工名称</param>
        /// <returns></returns>
        EmpAccount[] GetAll4Org(int orgid, bool? isUse, string searTxt);

        /// <summary>
        /// 分页获取所有的员工帐号；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="posi">岗位id</param>
        /// <param name="name"></param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        EmpAccount[] GetPager(int orgid,int posi,string name, int size, int index, out int countSum);        

        #region 职务（头衔）管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TitileAdd(EmpTitle entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TitleSave(EmpTitle entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TitleDelete(EmpTitle entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void TitleDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        EmpTitle TitleSingle(int identify);
        /// <summary>
        /// 获取对象；即所有职位；
        /// </summary>
        /// <returns></returns>
        EmpTitle[] TitleAll(int orgid);
        EmpTitle[] TitleAll(int orgid, bool? isUse);
        /// <summary>
        /// 分页获取职务信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        EmpTitle[] TitlePager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
        /// <summary>
        /// 获取当前职务的所有员工
        /// </summary>
        /// <param name="titleid">职务Id</param>
        /// <param name="isUse">是否在职</param>
        /// <returns></returns>
        EmpAccount[] Title4Emplyee(int titleid, bool? isUse);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        bool TitleIsExist(string name, int id, int orgid);
        /// <summary>
        /// 更改职务的排序
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateTitleTaxis(EmpTitle[] entities);      
        #endregion


    }
}
