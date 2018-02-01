using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 网站用户的管理
    /// </summary>
    public interface IUser : WeiSha.Common.IBusinessInterface
    {
        #region 网站用户组管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void AddGroup(UserGroup entity);        
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SaveGroup(UserGroup entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果删除成功，返回0；如果组包括用户，返回-1；如果是默认组，返回-2</returns>
        int DeleteGroup(UserGroup entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns>如果删除成功，返回0；如果组包括用户，返回-1；如果是默认组，返回-2</returns>
        int DeleteGroup(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        UserGroup GetGroupSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按用户组名称
        /// </summary>
        /// <param name="name">用户组名称</param>
        /// <returns></returns>
        UserGroup GetGroupSingle(string name);
        /// <summary>
        /// 获取默认用户组
        /// </summary>
        /// <returns></returns>
        UserGroup GetGroupDefault();
        /// <summary>
        /// 获取对象；即所有用户组；
        /// </summary>
        /// <returns></returns>
        UserGroup[] GetGroupAll();
        UserGroup[] GetGroupAll(bool? isUse);
        /// <summary>
        /// 获取某网站用户所属的组；
        /// </summary>
        /// <param name="UserId">网站用户id</param>
        /// <returns></returns>
        UserGroup GetGroup4User(int UserId);
        /// <summary>
        /// 获取某个组的所有网站用户
        /// </summary>
        /// <param name="grpId">组id</param>
        /// <returns></returns>
        User[] GetUser4Group(int grpId);
        /// <summary>
        /// 获取某个组的所有网站用户
        /// </summary>
        /// <param name="grpId"></param>
        /// <param name="use">是否禁用</param>
        /// <returns></returns>
        User[] GetUser4Group(int grpId, bool use);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="name">组名称</param>
        /// <returns></returns>
        bool IsGroupExist(string name);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool GroupRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool GroupRemoveDown(int id);
        #endregion

        #region 用户
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该用户，则返回-1</returns>
        int AddUser(User entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SaveUser(User entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DeleteUser(User entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void DeleteUser(int identify);
        /// <summary>
        /// 删除，按网站用户帐号名
        /// </summary>
        /// <param name="name">网站用户名称</param>
        void DeleteUser(string accname);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        User GetUserSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按网站用户名称
        /// </summary>
        /// <param name="name">帐号名称</param>
        /// <returns></returns>
        User GetUserSingle(string accname);
        /// <summary>
        /// 获取单一实体对象，按网站用户帐号名称与密码
        /// </summary>
        /// <param name="acc">网站用户帐号名称</param>
        /// <param name="pw">网站用户密码,MD5加密字符串</param>
        /// <returns></returns>
        User GetUserSingle(string accname, string pw);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="acc">网站用户帐号</param>
        /// <param name="pw">登录密码</param>
        /// <returns></returns>
        bool LoginCheck(string accname, string pw);
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="name">用户帐号</param>
        /// <returns></returns>
        bool IsUserExist(string accname);
        /// <summary>
        /// 获取对象；即所有网站用户；
        /// </summary>
        /// <returns></returns>
        User[] GetUserAll();
        /// <summary>
        /// 获取所有网站用户
        /// </summary>
        /// <param name="isUse">是否在职</param>
        /// <param name="searTxt">按名称查询</param>
        /// <returns></returns>
        User[] GetUserAll(bool? isUse, string searName);
        /// <summary>
        /// 获取某个用户组的所有网站用户帐号；
        /// </summary>
        /// <param name="grpid">用户组id,-1取全部网站用户，0取所在不属于任何用户组的网站用户</param>
        /// <returns></returns>
        User[] GetUserAll(int grpid, bool? isUse, string searName);
        /// <summary>
        /// 获取某个用户组的所有网站用户帐号；
        /// </summary>
        /// <param name="grpid">用户组id,-1取全部网站用户，0取所在不属于任何用户组的网站用户</param>
        /// <param name="isUse">是否在职</param>
        /// <returns></returns>
        User[] GetUserAll(int grpid, bool isUse);
        /// <summary>
        /// 分页获取所有的网站用户帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        User[] GetUserPager(int size, int index, out int countSum);
        /// <summary>
        /// 分页获取某用户组，所有的网站用户帐号；
        /// </summary>
        /// <param name="grpid">用户组Id</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        User[] GetUserPager(int grpid, int size, int index, out int countSum);
        User[] GetUserPager(int? grpid, bool? isUse, string searName, int size, int index, out int countSum);
        #endregion
    }
}
