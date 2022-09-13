using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Xml;

using WeiSha.Data;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;



namespace Song.ServiceImpls
{
    public class TeacherCom : ITeacher
    {

        #region 教师管理
        public int TeacherAdd(Teacher entity)
        {
            entity.Th_CrtTime = DateTime.Now;
            entity.Th_IsUse = true;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //教师职称
            Song.Entities.TeacherSort title = this.SortSingle(entity.Ths_ID);
            if (title == null) title = this.SortDefault(entity.Org_ID);
            if (title != null)
            {
                entity.Ths_ID = title.Ths_ID;
                entity.Ths_Name = title.Ths_Name;
            }
            //如果账号为空
            if (string.IsNullOrWhiteSpace(entity.Th_AccName))
            {
                if (!string.IsNullOrWhiteSpace(entity.Th_PhoneMobi))
                {
                    entity.Th_AccName = entity.Th_PhoneMobi;
                }
                else
                {
                    entity.Th_AccName = WeiSha.Core.Request.UniqueID();
                }
            }
            //如果密码不为空
            if (!string.IsNullOrWhiteSpace(entity.Th_Pw))                
                entity.Th_Pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(entity.Th_Pw).MD5;
            return Gateway.Default.Save<Teacher>(entity);
        }
        public bool TeacherCreateAccount(Teacher entity)
        {
            Song.Entities.Accounts acc = null;
            if (entity.Ac_ID > 0) acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
            if (acc==null && !string.IsNullOrWhiteSpace(entity.Th_AccName)) 
                acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_AccName == entity.Th_AccName).ToFirst<Accounts>();
            if (acc == null && !string.IsNullOrWhiteSpace(entity.Ac_UID))
                acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_UID == entity.Ac_UID).ToFirst<Accounts>();
            if (acc != null)
            {
                Gateway.Default.Update<Teacher>(new Field[] { 
                    Teacher._.Ac_ID, Teacher._.Ac_UID }, 
                    new object[] { acc.Ac_ID,acc.Ac_UID }, Teacher._.Th_AccName == acc.Ac_AccName);
                return true;
            }
            //创建学员账号
            acc = new Accounts();
            acc.Ac_AccName = entity.Th_AccName;
            acc.Ac_Name = entity.Th_Name;
            acc.Ac_IsTeacher = true;
            acc.Ac_IsUse = true;
            acc.Ac_IsPass = true;
            acc.Ac_MobiTel1 = entity.Th_PhoneMobi;
               
            int acid = Business.Do<IAccounts>().AccountsAdd(acc);
            Gateway.Default.Update<Teacher>(new Field[] {
                    Teacher._.Ac_ID, Teacher._.Ac_UID },
                   new object[] { acc.Ac_ID, acc.Ac_UID }, Teacher._.Th_AccName == acc.Ac_AccName);
            return false;
        }
        public void TeacherSave(Teacher entity){

            //如果密码不为空
            //if (string.IsNullOrWhiteSpace(entity.Th_Pw))
            //    entity.Th_Pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(entity.Th_Pw).MD5;  

            //教师职称
            Song.Entities.TeacherSort title = this.SortSingle(entity.Ths_ID);
            if (title == null) title = this.SortDefault(entity.Org_ID);
            if (title != null)
            {
                entity.Ths_ID = title.Ths_ID;
                entity.Ths_Name = title.Ths_Name;
            }
            //如果身份证不为空，则解析生日
            if (!string.IsNullOrWhiteSpace(entity.Th_IDCardNumber))
            {
                try
                {
                    IDCardNumber card = IDCardNumber.Get(entity.Th_IDCardNumber);
                    entity.Th_Age = card.Birthday.Year;
                    entity.Th_Sex = card.Sex;
                    entity.Th_Birthday = card.Birthday;
                    entity.Th_Native = card.Province + "," + card.Area + "," + card.City;
                    entity.Th_IDCardNumber = card.CardNumber;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //如果用户自己设置了年龄，则记录出生年份
                if (entity.Th_Age > 0) entity.Th_Age = DateTime.Now.Year - entity.Th_Age;
                //计算年龄，如果设置了生日，则自动计算出生年月
                if (entity.Th_Birthday > DateTime.Now.AddYears(-100))
                    entity.Th_Age = entity.Th_Birthday.Year;
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Teacher>(entity);
                    //同步课程表中的教师名称
                    tran.Update<Course>(new Field[] { Course._.Th_Name }, new object[] { entity.Th_Name }, Course._.Th_ID == entity.Th_ID);
                    //同步教师评价中的名称
                    tran.Update<TeacherComment>(new Field[] { TeacherComment._.Th_Name }, new object[] { entity.Th_Name }, TeacherComment._.Th_ID == entity.Th_ID);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Sex, Accounts._.Ac_Birthday, Accounts._.Ac_IDCardNumber, Accounts._.Ac_Nation, Accounts._.Ac_Native },
                        new object[] { entity.Th_Sex, entity.Th_Birthday, entity.Th_IDCardNumber, entity.Th_Nation, entity.Th_Native }, Accounts._.Ac_ID == entity.Ac_ID);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        public int TeacherUpdate(int id, Field[] fiels, object[] objs)
        {
            return Gateway.Default.Update<Teacher>(fiels, objs, Teacher._.Th_ID == id);
        }
        public void TeacherDelete(int identify)
        {
            Song.Entities.Teacher th = this.TeacherSingle(identify);
            if (th == null) return;
            this.TeacherDelete(th);   
        }

        public void TeacherDelete(Teacher entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                TeacherDelete(entity, tran);
            }
        }
        public void TeacherDelete(Teacher entity, DbTrans tran)
        {
            if (tran == null) tran = Gateway.Default.BeginTrans();
            try
            {
                tran.Delete<Teacher>(Teacher._.Th_ID == entity.Th_ID);
                tran.Delete<TeacherHistory>(TeacherHistory._.Th_ID == entity.Th_ID);
                tran.Update<Accounts>(new Field[] { Accounts._.Ac_IsTeacher }, new object[] { false }, Accounts._.Ac_ID == entity.Ac_ID);
                tran.Commit();

                WeiSha.Core.Upload.Get["Teacher"].DeleteFile(entity.Th_Photo);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;

            }
            finally
            {
                tran.Close();
                tran.Dispose();
            }
        }
        public Teacher TeacherSingle(int identify)
        {
            return Gateway.Default.From<Teacher>().Where(Teacher._.Th_ID == identify).ToFirst<Teacher>();
        }

        /// <summary>
        /// 获取单一实体对象，按学员id；
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        public Teacher TeacherForAccount(int accid)
        {
            return Gateway.Default.From<Teacher>().Where(Teacher._.Ac_ID == accid).ToFirst<Teacher>();
        }

        public Teacher TeacherSingle(string accname, string pw, int orgid)
        {
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            wc.And(Teacher._.Th_AccName == accname || Teacher._.Th_IDCardNumber == accname);
            wc.And(Teacher._.Th_Pw == pw);
            return Gateway.Default.From<Teacher>().Where(wc).ToFirst<Teacher>();
        }
        /// <summary>
        /// 教师登录
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public Teacher TeacherLogin(string acc, string pw, int orgid)
        {
            WhereClip wc = Teacher._.Org_ID == orgid && Teacher._.Th_IsUse == true && Teacher._.Th_IsPass == true;
            wc.And(Teacher._.Th_Pw == new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5);
            Song.Entities.Teacher entity = null;
            if (entity == null) entity = Gateway.Default.From<Teacher>().Where(wc && Teacher._.Th_AccName == acc).ToFirst<Teacher>();
            if (entity == null) entity = Gateway.Default.From<Teacher>().Where(wc && Teacher._.Th_PhoneMobi == acc).ToFirst<Teacher>();
            if (entity == null) entity = Gateway.Default.From<Teacher>().Where(wc && Teacher._.Th_IDCardNumber == acc).ToFirst<Teacher>();            
            return entity;
        }
        public bool IsTeacherExist(int orgid, string accname)
        {
            Teacher mm = Gateway.Default.From<Teacher>()
                .Where(Teacher._.Org_ID == orgid && Teacher._.Th_AccName == accname).ToFirst<Teacher>();
            if (mm == null) return false;
            //如果是一个已经存在的对象，则不匹配自己
            mm = Gateway.Default.From<Teacher>()
                   .Where(Teacher._.Org_ID == orgid && Teacher._.Th_AccName == accname && Teacher._.Th_ID != mm.Th_ID)
                   .ToFirst<Teacher>();
            return mm != null;
        }
        public bool IsTeacherExist(int orgid, Teacher entity)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            wc &= Teacher._.Th_ID != entity.Th_ID;
            wc.And(Teacher._.Th_AccName == entity.Th_AccName);
            int obj = Gateway.Default.Count<Teacher>(wc);
            return obj > 0;
        }
        public Teacher[] TeacherAll(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            if (isUse != null) wc.And(Teacher._.Th_IsUse == isUse);
            return Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_RegTime.Desc).ToArray<Teacher>();
        }

        public Teacher[] TeacherCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            if (isUse != null) wc.And(Teacher._.Th_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_RegTime.Desc).ToArray<Teacher>(count);
        }

        public int TeacherOfCount(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            if (isUse != null) wc.And(Teacher._.Th_IsUse == isUse);          
            return Gateway.Default.Count<Teacher>(wc);
        }
        /// <summary>
        /// 导出Excel格式的教师信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">教师分组id，小于0为全部</param>
        /// <returns></returns>
        public string TeacherExport4Excel(string path, int orgid, int sortid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "教师信息.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("教师信息");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
            {
                string exExport = nodes[i].Attributes["export"] != null ? nodes[i].Attributes["export"].Value : ""; //是否导出
                if (exExport.ToLower() == "false") continue;
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            }
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            WhereClip wc = Teacher._.Org_ID == orgid;
            if (sortid >= 0) wc.And(Teacher._.Ths_ID == sortid);
            Teacher[] Teachers = Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_RegTime.Desc).ToArray<Teacher>();
            for (int i = 0; i < Teachers.Length; i++)
                Teachers[i].Th_Intro = WeiSha.Core.HTML.ClearTag(Teachers[i].Th_Intro);
            for (int i = 0; i < Teachers.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = Teachers[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(Teachers[i], null);
                    if (obj != null)
                    {
                        string exExport = nodes[j].Attributes["export"] != null ? nodes[j].Attributes["export"].Value : ""; //是否导出
                        if (exExport.ToLower() == "false") continue;
                        string format = nodes[j].Attributes["Format"] != null ? nodes[j].Attributes["Format"].Value : "";
                        string datatype = nodes[j].Attributes["DataType"] != null ? nodes[j].Attributes["DataType"].Value : "";
                        string defvalue = nodes[j].Attributes["DefautValue"] != null ? nodes[j].Attributes["DefautValue"].Value : "";
                        string value = "";
                        switch (datatype)
                        {
                            case "date":
                                DateTime tm = Convert.ToDateTime(obj);
                                value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                                break;
                            default:
                                value = obj.ToString();
                                break;
                        }
                        if (defvalue.Trim() != "")
                        {
                            foreach (string s in defvalue.Split('|'))
                            {
                                string h = s.Substring(0, s.IndexOf("="));
                                string f = s.Substring(s.LastIndexOf("=") + 1);
                                if (value == h) value = f;
                            }
                        }
                        row.CreateCell(j).SetCellValue(value);
                    }
                }
            }
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        public Teacher[] TeacherPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            countSum = Gateway.Default.Count<Teacher>(wc);
            index = index > 0 ? index : 1;
            return Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_CrtTime.Desc).ToArray<Teacher>(size, (index - 1) * size);
        }

        public Teacher[] TeacherPager(int orgid, int thsid, bool? isUse, bool? isShow, string searName, string phone, string acc, string idcard, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Teacher._.Org_ID == orgid);
            if (thsid > 0) wc.And(Teacher._.Ths_ID == thsid);
            if (isUse != null) wc.And(Teacher._.Th_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Teacher._.Th_IsShow == (bool)isShow);
            if (!string.IsNullOrWhiteSpace(acc)) wc.And(Teacher._.Th_AccName.Like("%" + acc + "%"));
            if (!string.IsNullOrWhiteSpace(searName)) wc.And(Teacher._.Th_Name.Like("%" + searName + "%"));
            if (!string.IsNullOrWhiteSpace(idcard)) wc.And(Teacher._.Th_IDCardNumber.Like("%" + idcard + "%"));
            if (!string.IsNullOrWhiteSpace(phone))
            {
                WhereClip wcphone = new WhereClip();
                wcphone.Or(Teacher._.Th_Phone.Like("%" + phone + "%"));
                wcphone.Or(Teacher._.Th_PhoneMobi.Like("%" + phone + "%"));
                wc.And(wcphone);
            }
            countSum = Gateway.Default.Count<Teacher>(wc);
            index = index > 0 ? index : 1;
            return Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_CrtTime.Desc).ToArray<Teacher>(size, (index - 1) * size);
        }
        #endregion

        #region 教师分类
        public void SortAdd(TeacherSort entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();                    
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<TeacherSort>(TeacherSort._.Ths_Tax, TeacherSort._.Org_ID == org.Org_ID);
            entity.Ths_Tax = obj is int ? (int)obj + 1 : 0;    
            Gateway.Default.Save<TeacherSort>(entity);
        }

        public void SortSave(TeacherSort entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    if (entity.Ths_IsDefault)
                    {
                        tran.Update<TeacherSort>(new Field[] { TeacherSort._.Ths_IsDefault }, new object[] { false },
                            TeacherSort._.Ths_ID != entity.Ths_ID && TeacherSort._.Org_ID == entity.Org_ID);
                    }
                    tran.Save<TeacherSort>(entity);
                    tran.Update<Teacher>(new Field[] { Teacher._.Ths_Name }, new object[] { entity.Ths_Name }, Teacher._.Ths_ID == entity.Ths_ID);                    
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }

        public int SortDelete(int identify)
        {
            Teacher st = Gateway.Default.From<Teacher>().Where(Teacher._.Ths_ID == identify).ToFirst<Teacher>();
            if (st != null) throw new WeiSha.Core.ExceptionForAlert("请先删除其所属的教师信息");
            return Gateway.Default.Delete<TeacherSort>(TeacherSort._.Ths_ID == identify);
        }

        public TeacherSort SortSingle(int identify)
        {
            return Gateway.Default.From<TeacherSort>().Where(TeacherSort._.Ths_ID == identify).ToFirst<TeacherSort>();
        }

        public TeacherSort SortDefault(int orgid)
        {
            return Gateway.Default.From<TeacherSort>().Where(TeacherSort._.Org_ID == orgid && TeacherSort._.Ths_IsDefault == true).ToFirst<TeacherSort>();
        }

        public void SortSetDefault(int orgid, int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Update<TeacherSort>(new Field[] { TeacherSort._.Ths_IsDefault }, new object[] { false }, TeacherSort._.Org_ID == orgid);
                    tran.Update<TeacherSort>(new Field[] { TeacherSort._.Ths_IsDefault }, new object[] { true }, TeacherSort._.Ths_ID == identify);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }

        public TeacherSort[] SortAll(int orgid, string search, bool? isUse)
        {
            WhereClip wc = TeacherSort._.Org_ID == orgid;
            if (isUse != null) wc.And(TeacherSort._.Ths_IsUse == isUse);
            if(!string.IsNullOrWhiteSpace(search)) wc.And(TeacherSort._.Ths_Name.Like("%" + search + "%"));
            return Gateway.Default.From<TeacherSort>().Where(wc).OrderBy(TeacherSort._.Ths_Tax.Asc).ToArray<TeacherSort>();
        }

        public List<TeacherSort> SortCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = TeacherSort._.Org_ID == orgid;
            if (isUse != null) wc.And(TeacherSort._.Ths_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<TeacherSort>().Where(wc).ToList<TeacherSort>(count);
        }
        public int SortOfNumber(int sortid)
        {
            return Gateway.Default.Count<Teacher>(Teacher._.Ths_ID == sortid);
        }
        public TeacherSort Sort4Teacher(int studentId)
        {
            Teacher st = this.TeacherSingle(studentId);
            if (st == null) return null;
            return Gateway.Default.From<TeacherSort>().Where(TeacherSort._.Ths_ID == st.Ths_ID).ToFirst<TeacherSort>();
        }

        public Teacher[] Teacher4Sort(int sortid, bool? isUse)
        {
            WhereClip wc = Teacher._.Ths_ID == sortid;
            if (isUse != null) wc.And(Teacher._.Th_IsUse == isUse);
            return Gateway.Default.From<Teacher>().Where(wc).ToArray<Teacher>();
        }

        public bool SortIsExist(TeacherSort entity)
        {
            //如果是一个已经存在的对象，则不匹配自己
            TeacherSort mm = Gateway.Default.From<TeacherSort>()
                   .Where(TeacherSort._.Org_ID == entity.Org_ID && TeacherSort._.Ths_Name == entity.Ths_Name && TeacherSort._.Ths_ID != entity.Ths_ID)
                   .ToFirst<TeacherSort>();
            return mm != null;
        }

        /// <summary>
        /// 更改的排序
        /// </summary>
        /// <param name="items">实体数组</param>
        /// <returns></returns>
        public bool SortUpdateTaxis(Song.Entities.TeacherSort[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (TeacherSort item in items)
                    {
                        tran.Update<TeacherSort>(
                            new Field[] { TeacherSort._.Ths_Tax },
                            new object[] { item.Ths_Tax },
                            TeacherSort._.Ths_ID == item.Ths_ID);
                    }
                    tran.Commit();
                    return true;
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
        /// <summary>
        /// 学员账户号导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid"></param>
        /// <param name="sorts"></param>
        /// <param name="isall"></param>
        /// <param name="orgs">机构id,用逗号分隔</param>
        /// <returns></returns>
        public string Export4Excel(string path, int orgid, string sorts, bool isall)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "教师信息.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Teacher._.Org_ID == orgid;
            Teacher[] accounts = null;
            //如果没有分组信息，则取当前机构的所有
            if (string.IsNullOrWhiteSpace(sorts))
            {
                if (isall)
                {
                    accounts = Gateway.Default.From<Teacher>().Where(wc).OrderBy(Teacher._.Th_ID.Desc).ToArray<Teacher>();
                    _export4Excel_to_sheet(hssfworkbook, "教师信息", accounts, nodes);
                }
                else
                {
                    wc &= Accounts._.Org_ID == orgid;
                    accounts = Gateway.Default.From<Teacher>().Where(wc && Teacher._.Ths_ID <= 0).OrderBy(Teacher._.Th_ID.Desc).ToArray<Teacher>();
                    _export4Excel_to_sheet(hssfworkbook, "未设置职称", accounts, nodes);
                }
            }
            else
            {
                //按学员组生成工作簿
                foreach (string s in sorts.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    int sortid = 0;
                    int.TryParse(s, out sortid);
                    if (sortid == 0) continue;
                    //
                    Song.Entities.TeacherSort sts = Gateway.Default.From<TeacherSort>().Where(TeacherSort._.Ths_ID == sortid).ToFirst<TeacherSort>();
                    if (sts == null) continue;
                    accounts = Gateway.Default.From<Teacher>().Where(Teacher._.Org_ID == orgid && Teacher._.Ths_ID == sts.Ths_ID)
                        .OrderBy(Teacher._.Th_ID.Desc).ToArray<Teacher>();
                    _export4Excel_to_sheet(hssfworkbook, sts.Ths_Name, accounts, nodes);
                }               
            }

            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        private void _export4Excel_to_sheet(HSSFWorkbook hssfworkbook, string sheetName, Teacher[] accounts, XmlNodeList nodes)
        {
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);   //创建工作簿对象
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
            {
                string exExport = nodes[i].Attributes["export"] != null ? nodes[i].Attributes["export"].Value : ""; //是否导出
                if (exExport.ToLower() == "false") continue;
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            }
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;

            for (int i = 0; i < accounts.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = accounts[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(accounts[i], null);
                    if (obj != null)
                    {
                        string exExport = nodes[j].Attributes["export"] != null ? nodes[j].Attributes["export"].Value : ""; //是否导出
                        if (exExport.ToLower() == "false") continue;
                        string format = nodes[j].Attributes["Format"] != null ? nodes[j].Attributes["Format"].Value : "";
                        string datatype = nodes[j].Attributes["DataType"] != null ? nodes[j].Attributes["DataType"].Value : "";
                        string defvalue = nodes[j].Attributes["DefautValue"] != null ? nodes[j].Attributes["DefautValue"].Value : "";

                        string value = "";
                        switch (datatype)
                        {
                            case "date":
                                DateTime tm = Convert.ToDateTime(obj);
                                value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                                break;
                            default:
                                value = obj.ToString();
                                break;
                        }
                        if (defvalue.Trim() != "")
                        {
                            foreach (string s in defvalue.Split('|'))
                            {
                                string h = s.Substring(0, s.IndexOf("="));
                                string f = s.Substring(s.LastIndexOf("=") + 1);
                                if (value == h) value = f;
                            }
                        }
                        row.CreateCell(j).SetCellValue(value);
                    }
                }
            }
            return;
        }
        #endregion

        #region 教师履历管理
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该教师，则返回-1</returns>
        public int HistoryAdd(TeacherHistory entity)
        {
            entity.Thh_CrtTime = DateTime.Now;
            return Gateway.Default.Save<TeacherHistory>(entity);
        }
        /// <summary>
        /// 修改教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void HistorySave(TeacherHistory entity)
        {
            Gateway.Default.Save<TeacherHistory>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void HistoryDelete(int identify)
        {
            Gateway.Default.Delete<TeacherHistory>(TeacherHistory._.Thh_ID==identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public TeacherHistory HistorySingle(int identify)
        {
            return Gateway.Default.From<TeacherHistory>().Where(TeacherHistory._.Thh_ID == identify).ToFirst<TeacherHistory>();
        }
        /// <summary>
        /// 获取对象；即所有网站教师；
        /// </summary>
        /// <returns></returns>
        public TeacherHistory[] HistoryAll(int thid)
        {
            WhereClip wc = TeacherHistory._.Th_ID == thid;
            return Gateway.Default.From<TeacherHistory>().Where(wc).OrderBy(TeacherHistory._.Thh_StartTime.Asc).ToArray<TeacherHistory>();
        }
        /// <summary>
        /// 获取教师
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public TeacherHistory[] HistoryCount(int thid, int count)
        {
            if (count < 1) return this.HistoryAll(thid);
            WhereClip wc = TeacherHistory._.Th_ID == thid;
            return Gateway.Default.From<TeacherHistory>().Where(wc).OrderBy(TeacherHistory._.Thh_StartTime.Asc).ToArray<TeacherHistory>(count);
        }
        #endregion

        #region 教师评价管理
        /// <summary>
        /// 添加教师评价
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        public int CommentAdd(TeacherComment entity)
        {
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                entity.Org_ID = org.Org_ID;
            }
            entity.Thc_CrtTime = DateTime.Now;
            entity.Thc_IP = WeiSha.Core.Request.IP.IPAddress;
            entity.Thc_Device = WeiSha.Core.Browser.IsMobile ? "Mobi" : "PC";
            return Gateway.Default.Save<TeacherComment>(entity);
        }
        /// <summary>
        /// 修改教师评价信息
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CommentSave(TeacherComment entity)
        {
            Gateway.Default.Save<TeacherComment>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CommentDelete(int identify)
        {
            Gateway.Default.Delete<TeacherComment>(TeacherComment._.Thc_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public TeacherComment CommentSingle(int identify)
        {
            return Gateway.Default.From<TeacherComment>().Where(TeacherComment._.Thc_ID == identify).ToFirst<TeacherComment>();
        }
        /// <summary>
        /// 某天内，最近一次学员给教师的评价
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="accid">学员id</param>
        /// <param name="day">当前天数</param>
        /// <returns></returns>
        public TeacherComment CommentSingle(int thid, int accid, int day)
        {
            WhereClip wc = new WhereClip();
            if (thid > 0) wc.And(TeacherComment._.Th_ID == thid);
            if (accid > 0) wc.And(TeacherComment._.Ac_ID == accid);
            if (day > 0)
            {
                DateTime start = DateTime.Now.AddDays(-day);
                wc.And(TeacherComment._.Thc_CrtTime >= start);
            }
            return Gateway.Default.From<TeacherComment>().Where(wc).OrderBy(TeacherComment._.Thc_CrtTime.Desc).ToFirst<TeacherComment>();
        }
        /// <summary>
        /// 计算教师的评分，从当前日期之前的多少天内
        /// </summary>
        /// <param name="thid">评分教师的id</param>
        /// <param name="day">从当前日期之前的多少天内，小于或等于0表示取所有</param>
        /// <returns></returns>
        public double CommentScore(int thid, int day)
        {
            DateTime end = DateTime.Now;
            DateTime start = end.AddDays(-day);
            return CommentScore(thid, start, end);
        }
        /// <summary>
        /// 计算教师的评分，通过起始时间
        /// </summary>
        /// <param name="thid">评分教师的id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public double CommentScore(int thid, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            wc.And(TeacherComment._.Thc_IsUse == true);
            if (thid > 0) wc.And(TeacherComment._.Th_ID == thid);             
            if (start != null) wc.And(TeacherComment._.Thc_CrtTime > (DateTime)start);
            if (end != null) wc.And(TeacherComment._.Thc_CrtTime <= (DateTime)end);
            //
            object obj = Gateway.Default.Avg<TeacherComment>(TeacherComment._.Thc_Score, wc);
            if (obj == null) return 0;
            double avg = 0;
            double.TryParse(obj.ToString(), out avg);
            return avg;
        }
        /// <summary>
        /// 获取教师的评价
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public TeacherComment[] CommentCount(int thid, bool? isUse, bool? isShow, int count)
        {
            WhereClip wc = new WhereClip();
            if (thid > 0) wc.And(TeacherComment._.Th_ID == thid);
            if (isUse != null) wc.And(TeacherComment._.Thc_IsUse == (bool)isUse);
            if (isShow != null) wc.And(TeacherComment._.Thc_IsShow == (bool)isShow);
            return Gateway.Default.From<TeacherComment>().Where(wc).OrderBy(TeacherComment._.Thc_CrtTime.Desc).ToArray<TeacherComment>(count);
        }
        /// <summary>
        /// 教师评分排行
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="count">获取个数</param>
        /// <returns></returns>
        public TeacherComment[] CommentOrder(int orgid, bool? isUse, bool? isShow, int day, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TeacherComment._.Org_ID == orgid);
            if (isUse != null) wc.And(TeacherComment._.Thc_IsUse == (bool)isUse);
            if (isShow != null) wc.And(TeacherComment._.Thc_IsShow == (bool)isShow);
            //时间区间
            if (day > 0)
            {
                DateTime end = DateTime.Now;
                DateTime start = end.AddDays(-day);
                if (start != null) wc.And(TeacherComment._.Thc_CrtTime > (DateTime)start);
                if (end != null) wc.And(TeacherComment._.Thc_CrtTime <= (DateTime)end);
            }
            TeacherComment[] tcs=Gateway.Default.From<TeacherComment>()
                .Select(TeacherComment._.Th_Name, TeacherComment._.Thc_Score.Avg())
                .GroupBy(TeacherComment._.Th_Name.Group).Where(wc).OrderBy(new OrderByClip("Thc_Score desc")).ToArray<TeacherComment>(count);
            foreach (TeacherComment t in tcs)
            {
                t.Thc_Score = Math.Round(t.Thc_Score * 100) / 100;
            }
            return tcs;
        }
        /// <summary>
        /// 某天内，学员给教师的评价数
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="accid">学员id</param>
        /// <param name="day">当前天数</param>
        /// <returns></returns>
        public int CommentOfCount(int thid, int accid, int day)
        {           
            WhereClip wc = new WhereClip();
            if (thid > 0) wc.And(TeacherComment._.Th_ID == thid);
            if (accid > 0) wc.And(TeacherComment._.Ac_ID == accid);
            if (day > 0)
            {
                DateTime start = DateTime.Now.AddDays(-day);
                wc.And(TeacherComment._.Thc_CrtTime >= start);
            }
            return Gateway.Default.Count<TeacherComment>(wc);
        }
        public TeacherComment[] CommentPager(int orgid, bool? isUse, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TeacherComment._.Org_ID == orgid);
            if (isUse != null) wc.And(TeacherComment._.Thc_IsUse == (bool)isUse);
            if (isShow != null) wc.And(TeacherComment._.Thc_IsShow == (bool)isShow);
            countSum = Gateway.Default.Count<TeacherComment>(wc);
            index = index > 0 ? index : 1;
            return Gateway.Default.From<TeacherComment>().Where(wc).OrderBy(TeacherComment._.Thc_CrtTime.Desc).ToArray<TeacherComment>(size, (index - 1) * size);
        }
        /// <summary>
        /// 当前机构下某个教师的评价信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public TeacherComment[] CommentPager(int orgid, int thid, bool? isUse, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TeacherComment._.Org_ID == orgid);
            if (thid > 0) wc.And(TeacherComment._.Th_ID == thid);
            if (isUse != null) wc.And(TeacherComment._.Thc_IsUse == (bool)isUse);
            if (isShow != null) wc.And(TeacherComment._.Thc_IsShow == (bool)isShow);
            countSum = Gateway.Default.Count<TeacherComment>(wc);
            index = index > 0 ? index : 1;
            return Gateway.Default.From<TeacherComment>().Where(wc).OrderBy(TeacherComment._.Thc_CrtTime.Desc).ToArray<TeacherComment>(size, (index - 1) * size);
        }
        /// <summary>
        /// 当前机构下某个教师的评价信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thname">教师姓名</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public TeacherComment[] CommentPager(int orgid, string thname, bool? isUse, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TeacherComment._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(thname)) wc.And(TeacherComment._.Th_Name == thname);
            if (isUse != null) wc.And(TeacherComment._.Thc_IsUse == (bool)isUse);
            if (isShow != null) wc.And(TeacherComment._.Thc_IsShow == (bool)isShow);
            countSum = Gateway.Default.Count<TeacherComment>(wc);
            index = index > 0 ? index : 1;
            return Gateway.Default.From<TeacherComment>().Where(wc).OrderBy(TeacherComment._.Thc_CrtTime.Desc).ToArray<TeacherComment>(size, (index - 1) * size);
        }
        #endregion
    }
}
