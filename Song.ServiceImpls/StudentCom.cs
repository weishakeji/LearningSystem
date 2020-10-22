using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using WeiSha.Common;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Net;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace Song.ServiceImpls
{
    public class StudentCom : IStudent
    {

        #region 学员分类
        public void SortAdd(StudentSort entity)
        {           
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<StudentSort>(StudentSort._.Sts_Tax, StudentSort._.Org_ID == entity.Org_ID);
            entity.Sts_Tax = obj is int ? (int)obj + 1 : 0;
            Gateway.Default.Save<StudentSort>(entity);
        }

        public void SortSave(StudentSort entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<StudentSort>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Sts_Name }, new object[] { entity.Sts_Name }, Accounts._.Sts_ID == entity.Sts_ID);
                    if (entity.Sts_IsDefault)
                    {
                        tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { false },
                            StudentSort._.Sts_ID != entity.Sts_ID && StudentSort._.Org_ID == entity.Org_ID);
                        tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { true },
                            StudentSort._.Sts_ID == entity.Sts_ID && StudentSort._.Org_ID == entity.Org_ID);
                    }
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
            Accounts st = Gateway.Default.From<Accounts>().Where(Accounts._.Sts_ID == identify).ToFirst<Accounts>();
            if (st != null) throw new WeiSha.Common.ExceptionForAlert("当前学员分类下有学员信息");
            return Gateway.Default.Delete<StudentSort>(StudentSort._.Sts_ID == identify);
        }

        public StudentSort SortSingle(int identify)
        {
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == identify).ToFirst<StudentSort>();
        }

        public StudentSort SortDefault(int orgid)
        {
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Org_ID == orgid && StudentSort._.Sts_IsDefault == true).ToFirst<StudentSort>();
        }

        public void SortSetDefault(int orgid, int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { false }, StudentSort._.Org_ID == orgid);
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { true }, StudentSort._.Sts_ID == identify);
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

        public StudentSort[] SortAll(int orgid, bool? isUse)
        {
            WhereClip wc = StudentSort._.Org_ID == orgid;
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == isUse);
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToArray<StudentSort>();
        }

        public StudentSort[] SortCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = StudentSort._.Org_ID == orgid;
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToArray<StudentSort>(count);
        }

        public StudentSort Sort4Student(int studentId)
        {
            Accounts st = this.StudentSingle(studentId);
            if (st == null) return null;
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == st.Sts_ID).ToFirst<StudentSort>();
        }

        public Accounts[] Student4Sort(int sortid, bool? isUse)
        {
            WhereClip wc = Accounts._.Sts_ID == sortid;
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            return Gateway.Default.From<Accounts>().Where(wc).ToArray<Accounts>();
        }

        public bool SortIsExist(StudentSort entity)
        {
            //如果是一个已经存在的对象，则不匹配自己
            StudentSort mm = Gateway.Default.From<StudentSort>()
                   .Where(StudentSort._.Org_ID == entity.Org_ID && StudentSort._.Sts_Name == entity.Sts_Name && StudentSort._.Sts_ID != entity.Sts_ID)
                   .ToFirst<StudentSort>();
            return mm != null;
        }

        public bool SortRemoveUp(int orgid, int id)
        {
            //当前对象
            StudentSort current = Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == id).ToFirst<StudentSort>();
            //当前对象排序号
            int orderValue = (int)current.Sts_Tax; ;
            //上一个对象，即兄长对象；
            StudentSort up = Gateway.Default.From<StudentSort>()
                .Where(StudentSort._.Org_ID == orgid && StudentSort._.Sts_Tax < orderValue)
                .OrderBy(StudentSort._.Sts_Tax.Desc).ToFirst<StudentSort>();
            if (up == null) return false;
            //交换排序号
            current.Sts_Tax = up.Sts_Tax;
            up.Sts_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<StudentSort>(current);
                    tran.Save<StudentSort>(up);
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

        public bool SortRemoveDown(int orgid, int id)
        {
            //当前对象
            StudentSort current = Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == id).ToFirst<StudentSort>();
            //当前对象排序号
            int orderValue = (int)current.Sts_Tax;
            //下一个对象，即弟弟对象；
            StudentSort down = Gateway.Default.From<StudentSort>()
                .Where(StudentSort._.Org_ID == orgid && StudentSort._.Sts_Tax > orderValue)
                .OrderBy(StudentSort._.Sts_Tax.Asc).ToFirst<StudentSort>();
            if (down == null) return false;
            //交换排序号
            current.Sts_Tax = down.Sts_Tax;
            down.Sts_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<StudentSort>(current);
                    tran.Save<StudentSort>(down);
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
        /// <summary>
        /// 分页获取学员组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="name">分组名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public StudentSort[] SortPager(int orgid, bool? isUse, string name, int size, int index, out int countSum)
        {
            WhereClip wc = StudentSort._.Org_ID == orgid;            
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(name) && name.Trim() != "") wc.And(StudentSort._.Sts_Name.Like("%" + name + "%"));
            countSum = Gateway.Default.Count<StudentSort>(wc);
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToArray<StudentSort>(size, (index - 1) * size);
        }
        #endregion

        #region 学员管理
        public int StudentAdd(Accounts entity)
        {
            entity.Ac_RegTime = DateTime.Now;
            entity.Ac_IsUse = true;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
            }
            //如果账号为空
            if (string.IsNullOrWhiteSpace(entity.Ac_AccName))
            {
                if (!string.IsNullOrWhiteSpace(entity.Ac_MobiTel1))
                {
                    entity.Ac_AccName = entity.Ac_MobiTel1;
                }
                else
                {
                    entity.Ac_AccName = WeiSha.Common.Request.UniqueID();
                }
            }
            //如果密码为空
            if (string.IsNullOrWhiteSpace(entity.Ac_Pw))
                entity.Ac_Pw = WeiSha.Common.Login.Get["Accounts"].DefaultPw.MD5;
            else
                entity.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(entity.Ac_Pw).MD5;
            Gateway.Default.Save<Accounts>(entity);
            return entity.Ac_ID;
        }

        public void StudentSave(Accounts entity)
        {
            //如果密码不为空
            //if (!string.IsNullOrWhiteSpace(entity.Ac_Pw))
            //    entity.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(entity.Ac_Pw).MD5;   
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Accounts>(entity);
                    tran.Update<ExamResults>(new Field[] { ExamResults._.Sts_ID }, new object[] { entity.Sts_ID }, ExamResults._.Ac_ID == entity.Ac_ID);
                    //留言信息中学员信息
                    tran.Update<MessageBoard>(new Field[] { MessageBoard._.Ac_Name }, new object[] { entity.Ac_Name }, MessageBoard._.Ac_ID == entity.Ac_ID);
                    tran.Update<MessageBoard>(new Field[] { MessageBoard._.Ac_Photo }, new object[] { entity.Ac_Photo }, MessageBoard._.Ac_ID == entity.Ac_ID);
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
            //Extend.LoginState.Accounts.Refresh(entity);
        }
        #region 与其它系统同步
        /// <summary>
        /// 获取加密码字串
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string getSha1(Accounts entity)
        {
            //账号，密码，token
            string[] arr = new string[3] { entity.Ac_AccName, entity.Ac_Pw, "token" };
            Array.Sort(arr);
            string sha1 = "";
            for (int i = 0; i < arr.Length; i++)
                sha1 += arr[i];
            sha1 = new WeiSha.Common.Param.Method.ConvertToAnyValue(sha1).SHA1;
            //请求对方接口，参数：$uname、$upwd、$signature
            WebClient wc = new WebClient();
            string uri = "http://127.0.0.1/rss/sina.aspx?uname={0}&upwd={1}&signature={2}";
            uri = string.Format(uri, entity.Ac_AccName, entity.Ac_Pw, sha1);
            byte[] bResponse = wc.DownloadData(uri);
            //返回值。0：用户创建成功；	-1：用户创建失败（已存在）；-2：用户创建失败（未知原因）
            return Encoding.ASCII.GetString(bResponse);
        }
        #endregion
        public void StudentDelete(int identify)
        {
            Song.Entities.Accounts st = this.StudentSingle(identify);
            if (st == null) return;
            this.StudentDelete(st);
        }        
        public void StudentDelete(string accname, int orgid)
        {
            Song.Entities.Accounts st = this.StudentSingle(accname, orgid);
            if (st == null) return;
            this.StudentDelete(st);
        }

        public void StudentDelete(Song.Entities.Accounts entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                StudentDelete(entity, tran);
            }
        }
        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        public void StudentDelete(Song.Entities.Accounts entity, DbTrans tran)
        {
            if (tran == null) tran = Gateway.Default.BeginTrans();
            try
            {
                tran.Delete<Accounts>(Accounts._.Ac_ID == entity.Ac_ID);
                tran.Delete<Student_Ques>(Student_Ques._.Ac_ID == entity.Ac_ID);
                tran.Delete<Student_Notes>(Student_Notes._.Ac_ID == entity.Ac_ID);
                tran.Delete<Student_Course>(Student_Course._.Ac_ID == entity.Ac_ID);
                tran.Delete<TestResults>(TestResults._.Ac_ID == entity.Ac_ID);
                tran.Delete<MoneyAccount>(MoneyAccount._.Ac_ID == entity.Ac_ID);
                tran.Commit();
                if (!string.IsNullOrWhiteSpace(entity.Ac_Photo))
                    WeiSha.WebControl.FileUpload.Delete("Accounts", entity.Ac_Photo);
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
        public Accounts StudentSingle(int identify)
        {
            Song.Entities.Accounts st= Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == identify).ToFirst<Accounts>();
            if (st != null)
            {
                st.Ac_Age = (int)((DateTime.Now - st.Ac_Birthday).TotalDays / (365 * 3 + 366) * 4);
                st.Ac_Age = st.Ac_Age > 150 ? 0 : st.Ac_Age;
            }
            return st;
        }
        /// <summary>
        /// 获取单一实体，通过id与验证码
        /// </summary>
        /// <param name="id">学员Id</param>
        /// <param name="uid">学员登录时产生随机字符，用于判断同一账号不同人登录的问题</param>
        /// <returns></returns>
        public Accounts StudentSingle(int id, string uid)
        {
            return Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == id && Accounts._.Ac_CheckUID == uid).ToFirst<Accounts>();
        }

        public Accounts StudentSingle(string accname, int orgid)
        {
            WhereClip wc = Accounts._.Org_ID == orgid;
            Song.Entities.Accounts entity = null;
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_AccName == accname).ToFirst<Accounts>();
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_MobiTel1 == accname).ToFirst<Accounts>();
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_IDCardNumber == accname).ToFirst<Accounts>();
            return entity;
        }

        public Accounts StudentSingle(string accname, string pw, int orgid)
        {
            pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5;
            WhereClip wc = Accounts._.Org_ID == orgid;
            wc.And(Accounts._.Ac_AccName == accname);
            wc.And(Accounts._.Ac_Pw == pw);
            return Gateway.Default.From<Accounts>().Where(wc).ToFirst<Accounts>();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public Accounts StudentLogin(string acc, string pw, int orgid, bool? isPass)
        {
            WhereClip wc = Accounts._.Org_ID == orgid && Accounts._.Ac_IsUse == true;
            if (isPass != null) wc.And(Accounts._.Ac_IsPass == (bool)isPass);
            wc.And(Accounts._.Ac_Pw == new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5);
            Song.Entities.Accounts entity = null;
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_AccName == acc).ToFirst<Accounts>();
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_MobiTel1 == acc).ToFirst<Accounts>();
            if (entity == null) entity = Gateway.Default.From<Accounts>().Where(wc && Accounts._.Ac_IDCardNumber == acc).ToFirst<Accounts>();
            return entity;
        }
        public Accounts StudentLogin(int accid, string pw, int orgid, bool? isPass)
        {
            WhereClip wc = Accounts._.Org_ID == orgid && Accounts._.Ac_IsUse == true;
            if (isPass != null) wc.And(Accounts._.Ac_IsPass == (bool)isPass);
            wc.And(Accounts._.Ac_ID==accid && Accounts._.Ac_Pw == pw);
            Song.Entities.Accounts entity = Gateway.Default.From<Accounts>().Where(wc).ToFirst<Accounts>();          
            return entity;
        }
        public bool IsStudentExist(int orgid, string accname)
        {
            WhereClip wc = new WhereClip();
            wc |= Accounts._.Ac_AccName == accname;
            wc |= Accounts._.Ac_MobiTel1 == accname;
            Accounts mm = Gateway.Default.From<Accounts>()
                .Where(Accounts._.Org_ID == orgid && wc).ToFirst<Accounts>();
            return mm != null;
        }

        public bool IsStudentExist(int orgid, Accounts enity)
        {
            if (enity.Org_ID > 0) orgid = enity.Org_ID;
            int obj = Gateway.Default.Count<Accounts>(Accounts._.Org_ID == orgid && Accounts._.Ac_AccName == enity.Ac_AccName && Accounts._.Ac_ID != enity.Ac_ID);
            if (obj > 0) return true;
            obj = Gateway.Default.Count<Accounts>(Accounts._.Org_ID == orgid && Accounts._.Ac_MobiTel1 == enity.Ac_MobiTel1 && Accounts._.Ac_ID != enity.Ac_ID);
            if (obj > 0) return true;
            return false;
        }

        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accname"></param>
        /// <param name="answer">安全问题答案</param>
        /// <returns></returns>
        public bool IsStudentExist(int orgid, string accname, string answer)
        {
            if (string.IsNullOrWhiteSpace(answer)) return false;
            if (answer.Trim() == "") return false;
            Accounts mm = Gateway.Default.From<Accounts>()
               .Where(Accounts._.Org_ID == orgid && Accounts._.Ac_AccName == accname && Accounts._.Ac_Ans == answer).ToFirst<Accounts>();
            return mm != null;
        }

        public Accounts[] StudentAll(int orgid, bool? isUse)
        {
            WhereClip wc = Accounts._.Org_ID == orgid;
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            return Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
        }

        public Accounts[] StudentCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(count);
        }
        public int StudentOfCount(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            return Gateway.Default.Count<Accounts>(wc);
        }
        /// <summary>
        /// 导出Excel格式的学员信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">学员分组id，小于0为全部</param>
        /// <returns></returns>
        public string StudentExport4Excel(string path, int orgid, int sortid)
        {                     
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "学生信息.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("学生信息");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            WhereClip wc = Accounts._.Org_ID == orgid;
            if (sortid >= 0) wc.And(Accounts._.Sts_ID == sortid);  
            Accounts[] students = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
            for (int i = 0; i < students.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = students[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(students[i], null);
                    if (obj != null)
                    {
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
                                string f = s.Substring(s.LastIndexOf("=")+1);
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

        public Accounts[] StudentPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Accounts._.Org_ID == orgid;
            countSum = Gateway.Default.Count<Accounts>(wc);
            return Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(size, (index - 1) * size);
        }

        public Accounts[] StudentPager(int orgid, int? sortid, bool? isUse, string name, string phone, int size, int index, out int countSum)
        {
            WhereClip wc = Accounts._.Org_ID == orgid;
            if (sortid != null && sortid > 0) wc.And(Accounts._.Sts_ID == sortid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(name) && name.Trim() != "") wc.And(Accounts._.Ac_Name.Like("%" + name + "%"));
            if (!string.IsNullOrWhiteSpace(phone) && phone.Trim() != "") wc.And(Accounts._.Ac_MobiTel1.Like("%" + phone + "%"));
            countSum = Gateway.Default.Count<Accounts>(wc);
            return Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(size, (index - 1) * size);
        }
        #endregion

        #region 学员登录与在线记录
        /// <summary>
        /// 添加登录记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void LogForLoginAdd(Accounts st)
        {
            Song.Entities.LogForStudentOnline entity = new LogForStudentOnline();
            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            //学员信息
            if (st != null)
            {
                entity.Ac_ID = st.Ac_ID;
                entity.Ac_AccName = st.Ac_AccName;
                entity.Ac_Name = st.Ac_Name;
                entity.Lso_UID = st.Ac_CheckUID;
            }
            //登录相关时间
            entity.Lso_LoginDate = DateTime.Now.Date;
            entity.Lso_LoginTime = DateTime.Now;
            entity.Lso_CrtTime = DateTime.Now;
            entity.Lso_LastTime = DateTime.Now;
            entity.Lso_LogoutTime = DateTime.Now.AddMinutes(1);
            entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
            //登录信息
            entity.Lso_IP = WeiSha.Common.Browser.IP;
            entity.Lso_OS = WeiSha.Common.Browser.OS;
            entity.Lso_Browser = WeiSha.Common.Browser.Name + " " + WeiSha.Common.Browser.Version;
            entity.Lso_Platform = WeiSha.Common.Browser.IsMobile ? "Mobi" : "PC";

            Gateway.Default.Save<LogForStudentOnline>(entity);
        }
        /// <summary>
        /// 修改登录记录
        /// </summary>
        public void LogForLoginFresh(int interval, string plat)
        {
            //学员信息
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            if (st == null) return;
            //取当前登录记录
            int acid = st.Ac_ID;
            string uid = Extend.LoginState.Accounts.UID;
            Song.Entities.LogForStudentOnline entity = this.LogForLoginSingle(acid, uid, plat);
            if (entity == null) return;
            //记录时间
            //如果时间跨天了，重新生成记录
            if (entity.Lso_LoginTime.Date < DateTime.Now.Date)
            {
                //Extend.LoginState.Accounts.Write(st);
                this.LogForLoginAdd(st);
            }
            else
            {
                entity.Lso_LogoutTime = DateTime.Now.AddMinutes(1);
                entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
                entity.Lso_BrowseTime += interval;
                Gateway.Default.Save<LogForStudentOnline>(entity);
            }
        }
        /// <summary>
        /// 退出登录之前的记录更新
        /// </summary>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        public void LogForLoginOut(string plat)
        {
            //学员信息
            Song.Entities.Accounts st = null;
            int acid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            st = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (st == null) return;
            //取当前登录记录
            acid = st.Ac_ID;
            string uid = Extend.LoginState.Accounts.UID;
            Song.Entities.LogForStudentOnline entity = this.LogForLoginSingle(acid, uid, plat);
            if (entity == null) return;
            //记录时间
            entity.Lso_LogoutTime = DateTime.Now;
            entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
            Gateway.Default.Save<LogForStudentOnline>(entity);
        }
        /// <summary>
        /// 根据学员id与登录时生成的Uid返回实体
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="stuid">登录时生成的随机字符串，全局唯一</param>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        /// <returns></returns>
        public LogForStudentOnline LogForLoginSingle(int acid, string stuid, string plat)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentOnline._.Ac_ID == acid;
            wc &= LogForStudentOnline._.Lso_UID == stuid;
            if (!string.IsNullOrWhiteSpace(plat))
            {
                wc &= LogForStudentOnline._.Lso_Platform == plat;
            }
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginTime.Desc).ToFirst<LogForStudentOnline>();
        }
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        public LogForStudentOnline LogForLoginSingle(int identify)
        {
            return Gateway.Default.From<LogForStudentOnline>().Where(LogForStudentOnline._.Lso_ID == identify).ToFirst<LogForStudentOnline>();
        }
        /// <summary>
        /// 删除学员在线记录
        /// </summary>
        /// <param name="identify"></param>
        public void StudentOnlineDelete(int identify)
        {
            Gateway.Default.Delete<LogForStudentOnline>(LogForStudentOnline._.Lso_ID == identify);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="acid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>
        /// <param name="start">统计的开始时间</param>
        /// <param name="end">统计的结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LogForStudentOnline[] LogForLoginPager(int orgid, int acid, string platform, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            WhereClip wc = LogForStudentOnline._.Org_ID == orgid;
            if (acid > 0) wc.And(LogForStudentOnline._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentOnline._.Lso_Platform == platform);
                }
            }
            if (start != null) wc.And(LogForStudentOnline._.Lso_LoginDate >= (DateTime)start);
            if (end != null) wc.And(LogForStudentOnline._.Lso_LoginDate <= (DateTime)end);
            countSum = Gateway.Default.Count<LogForStudentOnline>(wc);
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginDate.Desc).ToArray<LogForStudentOnline>(size, (index - 1) * size);
        }
        public LogForStudentOnline[] LogForLoginPager(int orgid, int acid, string platform, DateTime? start, DateTime? end, string stname, string stmobi, int size, int index, out int countSum)
        {
            WhereClip wc = LogForStudentOnline._.Org_ID == orgid;
            if (acid > 0) wc.And(LogForStudentOnline._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentOnline._.Lso_Platform == platform);
                }
            }
            if (start != null) wc.And(LogForStudentOnline._.Lso_LoginDate >= (DateTime)start);
            if (end != null) wc.And(LogForStudentOnline._.Lso_LoginDate <= (DateTime)end);
            if (!string.IsNullOrWhiteSpace(stname) && stname.Trim() != "") wc.And(LogForStudentOnline._.Ac_Name.Like("%" + stname + "%"));
            if (!string.IsNullOrWhiteSpace(stmobi) && stmobi.Trim() != "") wc.And(LogForStudentOnline._.Ac_AccName.Like("%" + stmobi + "%"));

            countSum = Gateway.Default.Count<LogForStudentOnline>(wc);
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginDate.Desc).ToArray<LogForStudentOnline>(size, (index - 1) * size);
        }
        #endregion

        #region 学员在线学习的记录 
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="st"></param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyInterval">学习时间，此为时间间隔，每次提交学习时间加这个数</param>
        /// <param name="totalTime">视频总长度</param>
        public void LogForStudyFresh(int couid, int olid, Accounts st, int playTime, int studyInterval, int totalTime)
        {
            if (st == null) return;
            //当前章节的学习记录
            Song.Entities.LogForStudentStudy entity = this.LogForStudySingle(st.Ac_ID, olid);
            if (entity == null)
            {
                LogForStudyUpdate(couid, olid, st, playTime, studyInterval, totalTime);
            }
            else
            {
                LogForStudyUpdate(couid, olid, st, playTime, entity.Lss_StudyTime + studyInterval, totalTime);
            }
        }
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">章节id</param>
        /// <param name="st">学员账户</param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyTime">学习时间，此为累计时间</param>
        /// <param name="totalTime">视频总长度</param>
        /// <returns>学习进度百分比（相对于总时长）</returns>
        public double LogForStudyUpdate(int couid, int olid, Accounts st, int playTime, int studyTime, int totalTime)
        {
            if (st == null) return -1;
            //当前章节的学习记录
            //Song.Entities.LogForStudentStudy entity = this.LogForStudySingle(st.Ac_ID, olid);
            string sql = "SELECT *  FROM [LogForStudentStudy] where Ol_ID={0} and Ac_ID={1}";
            sql = string.Format(sql, olid, st.Ac_ID);
            Song.Entities.LogForStudentStudy entity = Gateway.Default.FromSql(sql).ToFirst<LogForStudentStudy>();
            if (entity == null)
            {
                //当前机构
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                entity = new LogForStudentStudy();
                entity.Lss_UID = WeiSha.Common.Request.UniqueID();
                entity.Lss_CrtTime = DateTime.Now;
                entity.Cou_ID = couid;
                entity.Ol_ID = olid;
                if (org != null) entity.Org_ID = org.Org_ID;
                //学员信息
                entity.Ac_ID = st.Ac_ID;
                entity.Ac_AccName = st.Ac_AccName;
                entity.Ac_Name = st.Ac_Name;
                //视频长度
                entity.Lss_Duration = totalTime;
            }
            //登录相关时间
            entity.Lss_LastTime = DateTime.Now;
            entity.Lss_PlayTime = playTime;
            entity.Lss_StudyTime = studyTime;
            if (entity.Lss_Duration < totalTime) entity.Lss_Duration = totalTime;
            //登录信息
            entity.Lss_IP = WeiSha.Common.Browser.IP;
            entity.Lss_OS = WeiSha.Common.Browser.OS;
            entity.Lss_Browser = WeiSha.Common.Browser.Name + " " + WeiSha.Common.Browser.Version;
            entity.Lss_Platform = WeiSha.Common.Browser.IsMobile ? "Mobi" : "PC";
            //保存到数据库
            Gateway.Default.Save<LogForStudentStudy>(entity);
            //计算完成度的百分比
            double per = (double)studyTime*1000 / (double)totalTime;
            per = Math.Floor(per * 10000) / 100;
            return per;
        }
        /// <summary>
        /// 根据学员id与登录时生成的Uid返回实体
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public LogForStudentStudy LogForStudySingle(int acid, int olid)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentStudy._.Ac_ID == acid;
            wc &= LogForStudentStudy._.Ol_ID == olid;
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).ToFirst<LogForStudentStudy>();
        }
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        public LogForStudentStudy LogForStudySingle(int identify)
        {
            return Gateway.Default.From<LogForStudentStudy>().Where(LogForStudentStudy._.Lss_ID == identify).ToFirst<LogForStudentStudy>();
        }
        /// <summary>
        /// 返回学习记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="acid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public LogForStudentStudy[] LogForStudyCount(int orgid, int couid, int olid, int acid, string platform, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(LogForStudentStudy._.Org_ID == orgid);
            if (couid > 0) wc.And(LogForStudentStudy._.Cou_ID == couid);
            if (olid > 0) wc.And(LogForStudentStudy._.Ol_ID == olid);
            if (acid > 0) wc.And(LogForStudentStudy._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentStudy._.Lss_Platform == platform);
                }
            }
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).OrderBy(LogForStudentStudy._.Lss_LastTime.Desc).ToArray<LogForStudentStudy>(count);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="acid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>
        /// <param name="start">统计的开始时间</param>
        /// <param name="end">统计的结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LogForStudentStudy[] LogForStudyPager(int orgid, int couid, int olid, int acid, string platform, int size, int index, out int countSum)
        {
            WhereClip wc = LogForStudentStudy._.Org_ID == orgid;
            if (couid > 0) wc.And(LogForStudentStudy._.Cou_ID == couid);
            if (olid > 0) wc.And(LogForStudentStudy._.Ol_ID == olid);
            if (acid > 0) wc.And(LogForStudentStudy._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentStudy._.Lss_Platform == platform);
                }
            }
            countSum = Gateway.Default.Count<LogForStudentStudy>(wc);
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).OrderBy(LogForStudentStudy._.Lss_LastTime.Desc).ToArray<LogForStudentStudy>(size, (index - 1) * size);
        }
        /// <summary>
        /// 学员的学习记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="acid">学员id</param>
        /// <returns>datatable中,LastTime:最后学习时间； studyTime：累计学习时间，complete：完成度百分比</returns>
        public DataTable StudentStudyCourseLog(int acid)
        {
            Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (student == null) throw new Exception("当前学员不存在！");
            Organization org= Gateway.Default.From<Organization>().Where(Organization._.Org_ID == student.Org_ID).ToFirst<Organization>();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;

            ////清理掉不需要的数据，包括：“章节不存在，章节没有视频，章节禁用或未完成”的学习记录，全部删除
            //WhereClip wc = LogForStudentStudy._.Ac_ID == acid;
            //SourceReader lfs = Gateway.Default.FromSql(string.Format("select Ol_ID from [LogForStudentStudy] where Ac_ID={0} group by Ol_ID",acid)).ToReader();
            //while (lfs.Read())
            //{
            //    long olid = lfs.GetInt64("Ol_ID");
            //    Outline ol = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
            //    if (ol == null || ol.Ol_IsVideo == false || ol.Ol_IsUse == false || ol.Ol_IsFinish == false)
            //    {
            //        Gateway.Default.Delete<LogForStudentStudy>(LogForStudentStudy._.Ol_ID == olid);
            //    }
            //} ;
            //开始计算
            string sql = @"
select c.Cou_ID,Cou_Name,Sbj_ID,lastTime,studyTime,complete from course as c inner join 
	(select cou_id, max(lastTime) as 'lastTime',sum(studyTime) as 'studyTime',		
		sum(case when complete>=100 then 100 else complete end) as 'complete'
		from 
			(SELECT top 90000 ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
				 sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
				 (case  when max(Lss_Duration)>0 then
					 cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
					 else 0 end
				  ) as 'complete'
			 FROM [LogForStudentStudy]  where {acid}  group by ol_id 
			 ) as s where s.totalTime>0 group by s.cou_id
	) as tm on c.cou_id=tm.cou_id  ";
            //sql = sql.Replace("{orgid}", orgid > 0 ? "org_id=" + orgid : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            try
            {
                DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ///* 不要删除
                    //*****如果没有购买的，则去除
                    //购买的课程(含概试用的）
                    List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseForStudent(acid, null, 0, null, -1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool isExist = false;
                        for (int j = 0; j < cous.Count; j++)
                        {
                            if (dt.Rows[i]["Cou_ID"].ToString() == cous[j].Cou_ID.ToString())
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            dt.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    // * */
                    //计算完成度                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        //课程的累计完成度
                        double complete = Convert.ToDouble(dr["complete"].ToString());
                        //课程id
                        int couid = Convert.ToInt32(dr["Cou_ID"].ToString());
                        int olnum = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true);
                        //完成度
                        double peracent = Math.Floor(complete / olnum * 100) / 100;
                        dr["complete"] = peracent >= (100 - tolerance) ? 100 : peracent;
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 学员指定学习课程的记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couids">课程id,逗号分隔</param>
        /// <returns></returns>
        public DataTable StudentStudyCourseLog(int stid, string couids)
        {
            DataTable dt = this.StudentStudyCourseLog(stid);
            if (dt == null) return dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //课程id
                int couid = Convert.ToInt32(dr["Cou_ID"].ToString());
                bool isexist = false;
                foreach (string id in couids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(id)) continue;
                    int sid = 0;
                    int.TryParse(id,out sid);
                    if (sid == 0) continue;
                    if (couid == sid)
                    {
                        isexist = true;
                        break;
                    }
                }
                if (!isexist)
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            return dt;
        }
        /// <summary>
        /// 学员学习某一门课程的完成度
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public DataTable StudentStudyCourseLog(int acid,int couid)
        {
            //开始计算
            string sql = @"
select * from course as c inner join 
	(select cou_id, max(lastTime) as 'lastTime',sum(studyTime) as 'studyTime',		
		sum(case when complete>=100 then 100 else complete end) as 'complete'
		from 
			(SELECT top 90000 ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
				 sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
				 (case  when max(Lss_Duration)>0 then
					 cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
					 else 0 end
				  ) as 'complete'
			 FROM [LogForStudentStudy]  where {couid} and {acid}  group by ol_id 
			 ) as s where s.totalTime>0 group by s.cou_id
	) as tm on c.cou_id=tm.cou_id  ";
            //sql = sql.Replace("{orgid}", orgid > 0 ? "org_id=" + orgid : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            sql = sql.Replace("{couid}", couid > 0 ? "Cou_ID=" + couid : "1=1");
            try
            {
                DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //课程的累计完成度
                        double complete = Convert.ToDouble(dr["complete"].ToString());
                        //课程id
                        couid = Convert.ToInt32(dr["Cou_ID"].ToString());
                        int olnum = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true);
                        //完成度
                        double peracent = Math.Floor(complete / olnum * 100) / 100;
                        dr["complete"] = peracent;
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 学员学习某一课程下所有章节的记录
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员账户id</param>
        /// <returns>datatable中，LastTime：最后学习时间；totalTime：视频时间长；playTime：播放进度；studyTime：学习时间，complete：完成度百分比</returns>
        public DataTable StudentStudyOutlineLog(int couid, int acid)
        {
            Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (student == null) throw new Exception("当前学员不存在！");
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == student.Org_ID).ToFirst<Organization>();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;
            //读取学员学习记录
            string sql = @"select * from outline as c left join 
                        (SELECT top 90000 ol_id, MAX(Lss_LastTime) as 'lastTime', 
	                        sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
                            cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100 as 'complete'

                          FROM [LogForStudentStudy] where {acid}  and Lss_Duration>0
                        group by ol_id ) as s
                        on c.ol_id=s.ol_id where {couid} order by ol_tax asc";
            sql = sql.Replace("{couid}", "cou_id=" + couid);
            sql = sql.Replace("{acid}", "ac_id=" + acid);
            try
            {
                DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
                //计算学习时度，因为没有学习的章节没有记录，也要计算进去
                DataTable dt= ds.Tables[0];
                //计算完成度                   
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrWhiteSpace(dr["complete"].ToString())) continue;
                    //课程的累计完成度
                    double complete = Convert.ToDouble(dr["complete"].ToString());                   
                    dr["complete"] = complete >= (100 - tolerance) ? 100 : complete;
                }
                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 学员的错题回顾
        /// <summary>
        /// 添加添加学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void QuesAdd(Student_Ques entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Cou_ID = qus.Cou_ID;
            entity.Sbj_ID = qus.Sbj_ID;
            entity.Squs_Level = qus.Qus_Diff;
            entity.Qus_Diff = qus.Qus_Diff;
            //
            WhereClip wc = Student_Ques._.Qus_ID == entity.Qus_ID && Student_Ques._.Ac_ID == entity.Ac_ID;
            Student_Ques sc = Gateway.Default.From<Student_Ques>().Where(wc).ToFirst<Student_Ques>();
            if (sc != null)
            {
                entity.Squs_ID = sc.Squs_ID;
                entity.Attach();
            }
            else
            {
                entity.Squs_CrtTime = DateTime.Now;
            }
            Gateway.Default.Save<Student_Ques>(entity);
        }
        /// <summary>
        /// 修改学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void QuesSave(Student_Ques entity)
        {
            Gateway.Default.Save<Student_Ques>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void QuesDelete(int identify)
        {
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Squs_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void QuesDelete(int quesid, int acid)
        {
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Qus_ID == quesid && Student_Ques._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public void QuesClear(int couid, int stid)
        {
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Cou_ID == couid && Student_Ques._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Ques QuesSingle(int identify)
        {
            return Gateway.Default.From<Student_Ques>().Where(Student_Ques._.Squs_ID == identify).ToFirst<Student_Ques>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] QuesAll(int acid, int sbjid, int couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_CrtTime.Desc).ToArray<Questions>();
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] QuesCount(int acid, int sbjid, int couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 高频错题
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="type">题型</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Questions[] QuesOftenwrong(int couid, int type, int count)
        {
            string sql = @"select {top} sq.count as Qus_Errornum,c.* from Questions as c inner join 
(SELECT qus_id,COUNT(qus_id) as 'count'  FROM [Student_Ques] where {couid} and {type} group by qus_id) as sq
on c.qus_id=sq.qus_id order by sq.count desc";
            sql = sql.Replace("{couid}", couid > 0 ? "cou_id=" + couid : "1=1");
            sql = sql.Replace("{type}", type > 0 ? "Qus_Type=" + type : "1=1");
            sql = sql.Replace("{top}", count > 0 ? "top " + count : "");
            return Gateway.Default.FromSql(sql).ToArray<Questions>();
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Questions[] QuesPager(int acid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            if (diff > 0) wc.And(Student_Ques._.Qus_Diff == diff);
            countSum = Gateway.Default.Count<Student_Ques>(wc);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_CrtTime.Desc).ToArray<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 学员的收藏
        /// <summary>
        /// 添加添加学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CollectAdd(Student_Collect entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Qus_Title = qus.Qus_Title;
            entity.Qus_Diff = qus.Qus_Diff;
            entity.Cou_ID = qus.Cou_ID;
            entity.Sbj_ID = qus.Sbj_ID;
            //
            WhereClip wc = Student_Collect._.Qus_ID == entity.Qus_ID && Student_Collect._.Ac_ID == entity.Ac_ID;
            Student_Collect sc = Gateway.Default.From<Student_Collect>().Where(wc).ToFirst<Student_Collect>();
            if (sc != null)
            {
                entity.Stc_ID = sc.Stc_ID;
                entity.Attach();
            }
            else
            {
                entity.Stc_CrtTime = DateTime.Now;
            }
            Gateway.Default.Save<Student_Collect>(entity);
        }
        /// <summary>
        /// 修改学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CollectSave(Student_Collect entity)
        {
            Gateway.Default.Save<Student_Collect>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void CollectDelete(int identify)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Stc_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void CollectDelete(int quesid, int acid)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Qus_ID == quesid && Student_Collect._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public void CollectClear(int couid, int stid)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Cou_ID == couid && Student_Collect._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Collect CollectSingle(int identify)
        {
            return Gateway.Default.From<Student_Collect>().Where(Student_Collect._.Stc_ID == identify).ToFirst<Student_Collect>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] CollectAll4Ques(int acid, int sbjid, int couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>();
        }
        public Student_Collect[] CollectAll(int acid, int sbjid, int couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Student_Collect>().Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Student_Collect>();
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] CollectCount(int acid, int sbjid, int couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Questions[] CollectPager(int acid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (diff > 0) wc.And(Student_Collect._.Qus_Diff == diff);
            countSum = Gateway.Default.Count<Student_Collect>(wc);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 学员的笔记
        /// <summary>
        /// 添加添加学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NotesAdd(Student_Notes entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Qus_Title = qus.Qus_Title;
            entity.Cou_ID = qus.Cou_ID;
            //
            WhereClip wc = Student_Notes._.Qus_ID == entity.Qus_ID && Student_Notes._.Ac_ID == entity.Ac_ID;
            Student_Notes sn = Gateway.Default.From<Student_Notes>().Where(wc).ToFirst<Student_Notes>();
            if (sn != null)
            {
                entity.Stn_Title = entity.Stn_Title;
                entity.Stn_Context = sn.Stn_Context + "\n" + entity.Stn_Context;
                entity.Stn_ID = sn.Stn_ID;
                entity.Attach();
            }
            else
            {
                entity.Stn_CrtTime = DateTime.Now;
                //entity.Stn_Context = "------ " + DateTime.Now.ToString() + " ------\n" + entity.Stn_Context;
            }
            Gateway.Default.Save<Student_Notes>(entity);
        }
        /// <summary>
        /// 修改学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NotesSave(Student_Notes entity)
        {
            Gateway.Default.Save<Student_Notes>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void NotesDelete(int identify)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Stn_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void NotesDelete(int quesid, int acid)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Qus_ID == quesid && Student_Notes._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public void NotesClear(int couid, int stid)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Cou_ID == couid && Student_Notes._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Notes NotesSingle(int identify)
        {
            return Gateway.Default.From<Student_Notes>().Where(Student_Notes._.Stn_ID == identify).ToFirst<Student_Notes>();
        }
        /// <summary>
        /// 获取单一实体对象，按试题id、学员id
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public Student_Notes NotesSingle(int quesid, int acid)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (quesid > 0) wc.And(Student_Notes._.Qus_ID == quesid);
            return Gateway.Default.From<Student_Notes>().Where(wc).ToFirst<Student_Notes>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Student_Notes[] NotesAll(int acid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Student_Notes>()
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Student_Notes>();
        }
        /// <summary>
        /// 取当前学员的笔记
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Questions[] NotesCount(int stid, int couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc.And(Student_Notes._.Ac_ID == stid);          
            if (couid > 0) wc.And(Student_Notes._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Notes>(Questions._.Qus_ID == Student_Notes._.Qus_ID)
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="quesid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] NotesCount(int acid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);           
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Notes>(Questions._.Qus_ID == Student_Notes._.Qus_ID)
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="quesid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Student_Notes[] NotesPager(int acid, int quesid, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (quesid > 0) wc.And(Student_Notes._.Qus_ID == quesid);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Student_Notes._.Stn_Context.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Student_Notes>(wc);
            return Gateway.Default.From<Student_Notes>()
               .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Student_Notes>(size, (index - 1) * size);
        }
        #endregion
    }
}
