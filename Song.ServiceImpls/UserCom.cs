using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class UserCom : IUser
    {


        #region 用户组部分

        public void AddGroup(UserGroup entity)
        {
            object obj = Gateway.Default.Max<UserGroup>(UserGroup._.UGrp_Tax, UserGroup._.UGrp_Id > -1);
            if (obj != DBNull.Value)
            {
                entity.UGrp_Tax = Convert.ToInt32(obj)+1;
            }
            else
            {
                entity.UGrp_Tax = 1;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<UserGroup>(entity);
        }

        public void SaveGroup(UserGroup entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<UserGroup>(entity);
                    //修改用户信息中的组名称
                    tran.Update<User>(new Field[] { User._.UGrp_Name }, new object[] { entity.UGrp_Name }, User._.UGrp_Id == entity.UGrp_Id);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
        }

        public int DeleteGroup(UserGroup entity)
        {
            object num = Gateway.Default.Count<User>(User._.UGrp_Id == entity.UGrp_Id);
            if (num != null && Convert.ToInt32(num) > 0)
            {
                return -1;
            }
            if (entity.UGrp_IsDefault)
            {
                return -2;
            }
            Gateway.Default.Delete<UserGroup>(UserGroup._.UGrp_Id == entity.UGrp_Id);
            return 0;
        }

        public int DeleteGroup(int identify)
        {
            UserGroup ug = this.GetGroupSingle(identify);
            return DeleteGroup(ug);
        }

        public UserGroup GetGroupSingle(int identify)
        {
            return Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_Id == identify).ToFirst<UserGroup>();
        }
        public UserGroup GetGroupDefault()
        {
            return Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_IsDefault == true).ToFirst<UserGroup>();
        }
        public UserGroup GetGroupSingle(string name)
        {
            return Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_Name == name).ToFirst<UserGroup>();
        }

        public UserGroup[] GetGroupAll()
        {
            return Gateway.Default.From<UserGroup>().OrderBy(UserGroup._.UGrp_Tax.Desc).ToArray<UserGroup>();
        }

        public UserGroup[] GetGroupAll(bool? isUse)
        {
            if (isUse == null)
            {
                return this.GetGroupAll();
            }
            return Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_IsUse == (bool)isUse).OrderBy(UserGroup._.UGrp_Tax.Desc).ToArray<UserGroup>();
        }

        public UserGroup GetGroup4User(int UserId)
        {
            return Gateway.Default.From<UserGroup>().InnerJoin<User>(UserGroup._.UGrp_Id == User._.UGrp_Id).Where(User._.User_Id == UserId).ToFirst<UserGroup>();
        }

        public User[] GetUser4Group(int grpId)
        {
            return Gateway.Default.From<User>().Where(User._.UGrp_Id==grpId).ToArray<User>();
        }

        public User[] GetUser4Group(int grpId, bool use)
        {
            return Gateway.Default.From<User>().Where(User._.UGrp_Id == grpId && User._.User_IsUse == use).ToArray<User>();
        }

        public bool IsGroupExist(string name)
        {
            object num = Gateway.Default.Count<UserGroup>(UserGroup._.UGrp_Name == name);
            if (num != null && Convert.ToInt32(num) > 0)
            {
                return true;
            }
            return false;
        }

        public bool GroupRemoveUp(int id)
        {
            Song.Entities.UserGroup current = this.GetGroupSingle(id);
            //当前对象排序号
            int orderValue = (int)current.UGrp_Tax; 
            //上一个对象，即兄长对象；存在当前优先级
            Song.Entities.UserGroup up = Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_Tax > orderValue).OrderBy(UserGroup._.UGrp_Tax.Asc).ToFirst<UserGroup>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.UGrp_Tax = up.UGrp_Tax;
            up.UGrp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<UserGroup>(current);
                    tran.Save<UserGroup>(up);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
            return true;
        }

        public bool GroupRemoveDown(int id)
        {
            //当前对象
            Song.Entities.UserGroup current = this.GetGroupSingle(id);
            //当前对象排序号
            int orderValue = (int)current.UGrp_Tax;
            //下一个对象，即弟弟对象；
            Song.Entities.UserGroup down = Gateway.Default.From<UserGroup>().Where(UserGroup._.UGrp_Tax < orderValue).OrderBy(UserGroup._.UGrp_Tax.Desc).ToFirst<UserGroup>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.UGrp_Tax = down.UGrp_Tax;
            down.UGrp_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<UserGroup>(current);
                    tran.Save<UserGroup>(down);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
            return true;
        }
        #endregion

        #region 网站用户信息
        public int AddUser(User entity)
        {
            //判断是否帐号重复
            object num = Gateway.Default.Count<User>(User._.User_AccName == entity.User_AccName);
            if (num != null && Convert.ToInt32(num) > 0)
            {
                return -1;
            }
            //设置默认组
            UserGroup group = this.GetGroupDefault();
            entity.UGrp_Id = group.UGrp_Id;
            entity.UGrp_Name = group.UGrp_Name;
            //
            return Gateway.Default.Save<User>(entity);
        }

        public void SaveUser(User entity)
        {
            Gateway.Default.Save<User>(entity);
        }

        public void DeleteUser(User entity)
        {
            Gateway.Default.Delete<User>(entity);
        }

        public void DeleteUser(int identify)
        {
            Gateway.Default.Delete<User>(User._.User_Id == identify);
        }

        public void DeleteUser(string accname)
        {
            Gateway.Default.Delete<User>(User._.User_AccName == accname);
        }

        public User GetUserSingle(int identify)
        {
            return Gateway.Default.From<User>().Where(User._.User_Id == identify).ToFirst<User>();
        }

        public User GetUserSingle(string accname)
        {
            return Gateway.Default.From<User>().Where(User._.User_AccName == accname).ToFirst<User>();
        }

        public User GetUserSingle(string accname, string pw)
        {
            return Gateway.Default.From<User>().Where(User._.User_AccName == accname && User._.User_Pw == pw).ToFirst<User>();
        }

        public bool LoginCheck(string accname, string pw)
        {
            //验证员工帐号与密码
            WhereClip wc = User._.User_AccName == accname && User._.User_Pw == pw;
            //员工必须在职
            wc.And(User._.User_IsUse == true);
            User ac = Gateway.Default.From<User>().Where(wc).ToFirst<User>();
            if (ac == null) return false;
            //判断用户组是否被禁用，如果禁用，同样返回false
            UserGroup grp = this.GetGroupSingle((int)ac.UGrp_Id);
            if (grp == null && grp.UGrp_IsUse == false) return false;
            return true;
        }

        public bool IsUserExist(string accname)
        {
            object num = Gateway.Default.Count<User>(User._.User_AccName == accname);
            if (num != null && Convert.ToInt32(num) > 0)
            {
                return true;
            }
            return false;
        }
        public User[] GetUserAll()
        {
            return Gateway.Default.From<User>().OrderBy(User._.User_RegTime.Desc).ToArray<User>();
        }

        public User[] GetUserAll(bool? isUse, string searName)
        {
            WhereClip wc = User._.User_Id != -1;
            if (isUse != null)
            {
                wc.And(User._.User_IsUse == isUse);
            }
            if (searName != "" && searName != String.Empty)
            {
                wc.And(User._.User_Name.Like("%" + searName + "%"));
            }
            return Gateway.Default.From<User>().Where(wc).OrderBy(User._.User_RegTime.Desc).ToArray<User>();
        }

        public User[] GetUserAll(int grpid, bool? isUse, string searName)
        {
            WhereClip wc = User._.UGrp_Id == grpid;
            if (isUse != null)
            {
                wc.And(User._.User_IsUse == isUse);
            }
            if (searName != "" && searName != String.Empty)
            {
                wc.And(User._.User_Name.Like("%" + searName + "%"));
            }
            return Gateway.Default.From<User>().Where(wc).OrderBy(User._.User_RegTime.Desc).ToArray<User>();
        }

        public User[] GetUserAll(int grpid, bool isUse)
        {
            WhereClip wc = User._.UGrp_Id == grpid;
            wc.And(User._.User_IsUse == isUse);          
            return Gateway.Default.From<User>().Where(wc).OrderBy(User._.User_RegTime.Desc).ToArray<User>();
        }

        public User[] GetUserPager(int size, int index, out int countSum)
        {
            countSum = Gateway.Default.Count<User>(User._.User_Id != -1);
            return Gateway.Default.From<User>().OrderBy(User._.User_RegTime.Desc).ToArray<User>(size, (index - 1) * size);
        }

        public User[] GetUserPager(int grpid, int size, int index, out int countSum)
        {
            WhereClip wc = User._.UGrp_Id == grpid;
            countSum = Gateway.Default.Count<User>(wc);
            Song.Entities.User[] ac = Gateway.Default.From<User>().Where(wc).OrderBy(User._.User_RegTime.Desc).ToArray<User>(size, (index - 1) * size);
            return ac;
        }

        public User[] GetUserPager(int? grpid, bool? isUse, string searName, int size, int index, out int countSum)
        {
            WhereClip wc = User._.User_Id != -1;
            if (grpid != null && grpid >-1)
            {
                wc.And(User._.UGrp_Id == grpid);
            }
            if (isUse != null)
            {
                wc.And(User._.User_IsUse == isUse);
            }
            if (searName != "" && searName != String.Empty)
            {
                wc.And(User._.User_Name.Like("%" + searName + "%"));
            }
            countSum = Gateway.Default.Count<User>(wc);
            Song.Entities.User[] ac = Gateway.Default.From<User>().Where(wc).OrderBy(User._.User_RegTime.Desc).ToArray<User>(size, (index - 1) * size);
            return ac;
        }
        #endregion
    }
}
