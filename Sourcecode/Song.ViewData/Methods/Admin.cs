using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace Song.ViewData.Methods
{

    /// <summary>
    /// 管理员
    /// </summary> 
    [HttpPost]
    public class Admin : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "Employee";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="acc">验证</param>
        /// <param name="pw">密码</param>
        /// <param name="vcode">提交的验证码</param>
        /// <param name="vmd5">用于验证码的加密字符串</param>
        /// <returns></returns>
        public Song.Entities.EmpAccount Login(string acc, string pw, string vcode, string vmd5)
        {
            string val = new Song.ViewData.ConvertToAnyValue(acc + vcode).MD5;
            if (!val.Equals(vmd5, StringComparison.CurrentCultureIgnoreCase))
                throw VExcept.Verify("验证码错误", 101);
            ////当前机构等于管理员所在机构
            //Song.Entities.Organization curr = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.EmpAccount EmpAccount = Business.Do<IEmployee>().EmpLogin(acc, pw, -1);
            if (EmpAccount == null) throw VExcept.Verify("密码错误", 102);
            if(!(bool)EmpAccount.Acc_IsUse) throw VExcept.Verify("当前账号被禁用", 103);
            //克隆当前对象
            EmpAccount = EmpAccount.DeepClone<Song.Entities.EmpAccount>();
            if (!string.IsNullOrWhiteSpace(EmpAccount.Acc_Photo))
            {
                EmpAccount.Acc_Photo = string.IsNullOrWhiteSpace(EmpAccount.Acc_Photo) ? "" : VirPath + EmpAccount.Acc_Photo;
                if (!System.IO.File.Exists(WeiSha.Core.Server.MapPath(EmpAccount.Acc_Photo))) EmpAccount.Acc_Photo = "";
            }            //登录，密码被设置成加密状态值
            EmpAccount.Acc_Pw = LoginAdmin.Status.Login(EmpAccount);
            return EmpAccount;
        }
        ///// <summary>
        ///// 获取当前登录的用户
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public Song.Entities.EmpAccount User()
        //{
        //    Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
        //    return acc;
        //}
        /// <summary>
        /// 当前登录的普通管理员
        /// </summary>
        /// <returns></returns>
        public Song.Entities.EmpAccount General()
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            //当前机构等于管理员所在机构         
            Song.Entities.Organization curr = Business.Do<IOrganization>().OrganCurrent();
            if (curr.Org_ID != acc.Org_ID) return null;
            ////当前登录对象的岗位
            //Song.Entities.Position posi = Business.Do<IPosition>().GetSingle((int)acc.Posi_Id);
            //if (posi == null) return null;

            //bool issuper=LoginAdmin.Status.IsSuperAdmin(acc);
            //return issuper ? null : acc;
            return acc;
        }
        /// <summary>
        /// 当前登录的超级管理员
        /// </summary>
        /// <returns></returns>
        public Song.Entities.EmpAccount Super()
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            bool issuper = LoginAdmin.Status.IsSuperAdmin(acc);
            return !issuper ? null : acc;
        }
        /// <summary>
        /// 当前登录用户所在的机构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Organization Organ()
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) return null;
            return Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
        }
        /// <summary>
        /// 当前登录账号是不是超级管理员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool IsSuper()
        {
            return LoginAdmin.Status.IsSuperAdmin(this.Letter);
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        /// <returns>返回登录状态信息，包含登录账号与时效，以加密方式</returns>
        public string Fresh()
        {
            return LoginAdmin.Status.Fresh(this.Letter);
        }
        /// <summary>
        /// 更改当前管理的登录密码
        /// </summary>
        /// <param name="oldpw">原密码</param>
        /// <param name="newpw">新密码</param>    
        [Admin]
        public bool ChangePw(string oldpw, string newpw)
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) throw new ExceptionForNoLogin();
            return Business.Do<IEmployee>().ChangePw(acc.Acc_Id, oldpw, newpw);
        }
        /// <summary>
        /// 更新安全问题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ChangeSafeQustion(string ques, string ans)
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) throw new ExceptionForNoLogin();
            try
            {
                Business.Do<IEmployee>().Update(acc.Acc_Id, new WeiSha.Data.Field[] {
                    Song.Entities.EmpAccount._.Acc_Qus, Song.Entities.EmpAccount._.Acc_Ans,
                }, new object[] { ques, ans });
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 重置管理员密码
        /// </summary>
        /// <param name="accid">管理员账号Id</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        [SuperAdmin, Admin]
        [HttpPut]
        public bool ResetPw(int accid, string pw)
        {
            Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(accid);
            if (acc == null) throw new ExceptionForPrompt("账号不存在");
            acc.Acc_Pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            try
            {
                Business.Do<IEmployee>().Save(acc);
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        /// <summary>
        /// 根据id获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.EmpAccount ForID(int id)
        {
            Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(id);
            acc.Acc_Pw = string.Empty;
            acc.Acc_Photo = string.IsNullOrWhiteSpace(acc.Acc_Photo) ? "" : Upload.Get[PathKey].Virtual + acc.Acc_Photo;
            if (!System.IO.File.Exists(WeiSha.Core.Server.MapPath(acc.Acc_Photo))) acc.Acc_Photo = "";
            return acc;
        }
        /// <summary>
        /// 根据id删除账号，可以有多个id，用逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost(Ignore = true), HttpDelete, Admin, SuperAdmin]
        public int Delete(string id)
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            bool issuper = this.IsSuper();    //是否超管登录
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    if (acc != null && acc.Acc_Id == idval) throw new Exception("不可删除自身账号");
                    Song.Entities.EmpAccount emp = Business.Do<IEmployee>().GetSingle(idval);
                    if (emp == null) continue;
                    //如果用户属于超管角色，则不允许删除
                    Song.Entities.Position posi = Business.Do<IPosition>().GetSingle(emp.Posi_Id);
                    if (posi != null)
                    {
                        if (posi.Posi_IsAdmin == true && !issuper) throw new Exception("管理员不可以删除！");
                    }
                    Business.Do<IEmployee>().Delete(emp);
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
        /// 添加管理员
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        [HttpPost, Admin]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool Add(Song.Entities.EmpAccount acc)
        {
            try
            {
                //图片
                string filename = _uploadLogo();
                if (!string.IsNullOrWhiteSpace(filename)) acc.Acc_Photo = filename;

                Business.Do<IEmployee>().Add(acc);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="acc">员工账号的实体</param>
        /// <returns></returns>
        [HttpPost, Admin]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool Modify(EmpAccount acc)
        {
            Song.Entities.EmpAccount old = Business.Do<IEmployee>().GetSingle(acc.Acc_Id);
            if (old == null) throw new Exception("Not found entity for EmpAccount！");
            //图片
            string filename = _uploadLogo();
            //如果有上传的图片，且之前也有图片,则删除原图片
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(acc.Acc_Photo)) && !string.IsNullOrWhiteSpace(old.Acc_Photo))
                WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Acc_Photo);

            //账号，密码，登录状态值，不更改
            if (this.IsSuper())
                old.Copy<Song.Entities.EmpAccount>(acc, "Acc_Pw,Acc_CheckUID");
            else
                old.Copy<Song.Entities.EmpAccount>(acc, "Acc_AccName,Acc_Pw,Acc_CheckUID");
            if (!string.IsNullOrWhiteSpace(filename)) old.Acc_Photo = filename;
            Business.Do<IEmployee>().Save(old);
            return true;
        }
        /// <summary>
        ///  图片上传的处理方法
        /// </summary>
        /// <returns></returns>
        private string _uploadLogo()
        {
            string filename = string.Empty;
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);               
                break;
            }
            //转jpg
            if (!string.IsNullOrWhiteSpace(filename))
            {
                if (!".jpg".Equals(Path.GetExtension(filename), StringComparison.CurrentCultureIgnoreCase))
                {
                    string old = filename;
                    using (System.Drawing.Image image = WeiSha.Core.Images.FileTo.ToImage(PhyPath + filename))
                    {
                        filename = Path.ChangeExtension(filename, "jpg");
                        image.Save(PhyPath + Path.ChangeExtension(filename, "jpg"), ImageFormat.Jpeg);
                    }
                    System.IO.File.Delete(PhyPath + old);
                }
            }
            return filename;
        }
        /// <summary>
        /// 上传修改账号的照片
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public string UpPhoto(int accid)
        {
            Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(accid);
            if (acc == null) throw new Exception("Not found entity for EmpAccount！");

            string filename = string.Empty;
            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                WeiSha.Core.Images.FileTo.Zoom(PhyPath + filename, 200, 200, false);              
                break;
            }
            //转jpg
            if (!".jpg".Equals(Path.GetExtension(filename), StringComparison.CurrentCultureIgnoreCase))
            {
                string old = filename;
                using (System.Drawing.Image image = WeiSha.Core.Images.FileTo.ToImage(PhyPath + filename))
                {
                    filename = Path.ChangeExtension(filename, "jpg");
                    image.Save(PhyPath + Path.ChangeExtension(filename, "jpg"),ImageFormat.Jpeg);
                }
                System.IO.File.Delete(PhyPath + old);              
            }
            if (!string.IsNullOrWhiteSpace(acc.Acc_Photo))
            {
                string filehy = PhyPath + acc.Acc_Photo;             
                try
                {
                    //删除原图
                    if (System.IO.File.Exists(filehy))
                        System.IO.File.Delete(filehy);
                    //删除缩略图，如果有
                    string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                    if (System.IO.File.Exists(smallfile))
                        System.IO.File.Delete(smallfile);
                }
                catch { }
            }
            Business.Do<IEmployee>().Update(acc.Acc_Id, new WeiSha.Data.Field[] {
                    Song.Entities.EmpAccount._.Acc_Photo
                }, new object[] { filename });
            return VirPath + filename;
        }
        /// <summary>
        /// 获取管理员列表(限当前机构）
        /// </summary>
        /// <param name="name">姓名，为空则返回所有</param>
        /// <param name="size">每页多少条记录</param>
        /// <param name="index">第几页</param>
        /// <returns>按页返回列表数据</returns>
        [HttpGet]
        [Admin]
        public ListResult List(string name, int size, int index)
        {
            Song.Entities.Organization org = LoginAdmin.Status.Organ(this.Letter);
            //总记录数
            int count = 0;
            EmpAccount[] eas = Business.Do<IEmployee>().GetPager(org.Org_ID, -1,name, size, index, out count);
            foreach (EmpAccount ea in eas) ea.Acc_Pw = string.Empty;
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取机构员工
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="posi">岗位id</param>
        /// <param name="name">按管理员名称检索</param>
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        [HttpGet]
        [Admin]
        public ListResult Pager(int orgid, int posi, string name, int index, int size)
        {
            //总记录数
            int count = 0;
            EmpAccount[] eas = Business.Do<IEmployee>().GetPager(orgid, posi, name, size, index, out count);
            foreach (EmpAccount ea in eas) ea.Acc_Pw = string.Empty;
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 当前管理员的菜单项
        /// </summary>
        /// <returns></returns>
        [Admin]
        [HttpGet]
        public string Menu()
        {
            Song.Entities.EmpAccount acc = LoginAdmin.Status.User(this.Letter);
            if (acc == null) throw new ExceptionForNoLogin();
            Song.Entities.ManageMenu[] menus = Business.Do<IPurview>().GetAll4Emplyee((int)acc.Posi_Id);
            string path = WeiSha.Core.Server.MapPath("/Utilities/datas/");
            string result = "[";
            if (LoginAdmin.Status.IsSuperAdmin(this.Letter))
            {
                result += File.ReadAllText(path + "SuperAdmin.json");
            }
            else
            {
                foreach (Song.Entities.ManageMenu m in menus)
                {
                    if (string.IsNullOrWhiteSpace(m.MM_Marker)) continue;
                    try
                    {
                        result += File.ReadAllText(path + m.MM_Marker + ".json") + ",";
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            result += "]";
            return result;
        }
        /// <summary>
        /// 查询员工信息
        /// </summary>
        /// <param name="search">按名称索引</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.EmpAccount[] Search(string search)
        {
            EmpAccount[] eas = Business.Do<IEmployee>().GetAll(-1, -1, true, search);
            foreach (EmpAccount ea in eas) ea.Acc_Pw = string.Empty;
            return eas;
        }
        /// <summary>
        /// 所有员工，包括禁用的
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Song.Entities.EmpAccount[] All(string search)
        {
            EmpAccount[] eas = Business.Do<IEmployee>().GetAll(-1, -1, null, search);
            foreach (EmpAccount ea in eas) ea.Acc_Pw = string.Empty;
            return eas;
        }
        /// <summary>
        /// 员工账号是否存在
        /// </summary>
        /// <param name="acc">账号名称</param>
        /// <param name="id">账号id，如果账号已经存在，则不断判断自身</param>
        /// <returns></returns>
        [HttpGet]
        public bool IsExistAcc(string acc,int id)
        {
            return Business.Do<IEmployee>().IsExists(acc,id);
        }
        #region 职务/头衔的管理
        /// <summary>
        /// 通过id获取职务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.EmpTitle TitleForID(int id)
        {
            return Business.Do<IEmployee>().TitleSingle(id);
        }
        /// <summary>
        /// 添加头衔信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool TitleAdd(Song.Entities.EmpTitle entity)
        {
            try
            {
                Business.Do<IEmployee>().TitileAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改头衔信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool TitleModify(Song.Entities.EmpTitle entity)
        {
            try
            {
                Song.Entities.EmpTitle old = Business.Do<IEmployee>().TitleSingle(entity.Title_Id);
                if (old == null) throw new Exception("Not found entity for EmpTitle！");
                old.Copy<Song.Entities.EmpTitle>(entity);
                Business.Do<IEmployee>().TitleSave(old);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除职务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin]
        public int TitleDelete(string id)
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
                    Business.Do<IEmployee>().TitleDelete(idval);
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
        /// 机构下所有的职务信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.EmpTitle[] TitleList(int orgid)
        {
            return Business.Do<IEmployee>().TitleAll(orgid);
        }
        /// <summary>
        /// 机构下启用的职务信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.EmpTitle[] TitleEnabledList(int orgid)
        {
            return Business.Do<IEmployee>().TitleAll(orgid, true);
        }
        /// <summary>
        /// 分页获取职务信息（包含未启用的）
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="name">职务名称，模糊查询</param>
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        public ListResult TitlePager(int orgid,string name,int index, int size)
        {
            int sum = 0;
            Song.Entities.EmpTitle[] titles = Business.Do<IEmployee>().TitlePager(orgid, null, name, size, index, out sum);           
            Song.ViewData.ListResult result = new ListResult(titles);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 更改职务的排序
        /// </summary>
        /// <param name="items">职务信息数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool TitleTaxis(Song.Entities.EmpTitle[] items)
        {
            try
            {
                Business.Do<IEmployee>().UpdateTitleTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 工作组的管理
        /// <summary>
        /// 通过id获取工作组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.EmpGroup GroupForID(int id)
        {
            return Business.Do<IEmpGroup>().GetSingle(id);
        }
        /// <summary>
        /// 添加工作组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool GroupAdd(Song.Entities.EmpGroup entity)
        {
            try
            {
                Business.Do<IEmpGroup>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改工作组信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool GroupModify(Song.Entities.EmpGroup entity)
        {
            try
            {
                Song.Entities.EmpGroup old = Business.Do<IEmpGroup>().GetSingle(entity.EGrp_Id);
                if (old == null) throw new Exception("Not found entity for EmpGroup！");
                old.Copy<Song.Entities.EmpGroup>(entity);
                Business.Do<IEmpGroup>().Save(old);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除工作组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin]
        public int GroupDelete(string id)
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
                    Business.Do<IEmpGroup>().Delete(idval);
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
        /// 机构下所有的工作组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.EmpGroup[] GroupList(int orgid)
        {
            return Business.Do<IEmpGroup>().GetAll(orgid);
        }
        /// <summary>
        /// 机构下启用的职务信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.EmpGroup[] GroupEnabledList(int orgid)
        {
            return Business.Do<IEmpGroup>().GetAll(orgid, true);
        }
        /// <summary>
        /// 分页获取职务信息（包含未启用的）
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="name">职务名称，模糊查询</param>
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        public ListResult GroupPager(int orgid, string name, int index, int size)
        {
            int sum = 0;
            Song.Entities.EmpGroup[] titles = Business.Do<IEmpGroup>().Pager(orgid, null, name, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(titles);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 更改工作组的排序
        /// </summary>
        /// <param name="items">工作组的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool GroupTaxis(Song.Entities.EmpGroup[] items)
        {
            try
            {
                Business.Do<IEmpGroup>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}
