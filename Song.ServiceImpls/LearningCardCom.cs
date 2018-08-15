using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Xml;
using NPOI.SS.UserModel;
using System.IO;



namespace Song.ServiceImpls
{
    public class LearningCardCom : ILearningCard
    {
        #region 学习卡设置管理
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetAdd(LearningCardSet entity)
        {
            entity.Lcs_CrtTime = DateTime.Now;
            Gateway.Default.Save<LearningCardSet>(entity);
            //生成学习卡
            LearningCard[] cards = CardGenerate(entity);
            foreach (LearningCard c in cards)
            {
                Gateway.Default.Save<LearningCard>(c);
            }
        }
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetSave(LearningCardSet entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Song.Entities.LearningCardSet rs = tran.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == entity.Lcs_ID).ToFirst<LearningCardSet>();
                    ////如果新增了充值码
                    ////if (rs.Lcs_Count < entity.Lcs_Count) _RechargeCodeBuilds(entity, entity.Rs_Count - rs.Rs_Count);
                    ////如果减少的充值码
                    //if (rs.Lcs_Count > entity.Lcs_Count)
                    //{
                    //    Song.Entities.RechargeCode[] rcs = tran.From<RechargeCode>().Where(RechargeCode._.Rs_ID == entity.Rs_ID && RechargeCode._.Rc_IsUsed == false).ToArray<RechargeCode>(rs.Rs_Count - entity.Rs_Count);
                    //    foreach (RechargeCode r in rcs)
                    //        tran.Delete<RechargeCode>(r);
                    //}
                    tran.Update<LearningCard>(new Field[] { LearningCard._.Lc_Price, LearningCard._.Lc_LimitStart, LearningCard._.Lc_LimitEnd },
                        new object[] { entity.Lcs_Price, entity.Lcs_LimitStart, entity.Lcs_LimitEnd },
                        LearningCard._.Lcs_ID == entity.Lcs_ID && LearningCard._.Lc_IsUsed == false);
                    tran.Save<LearningCardSet>(entity);
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
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetDelete(LearningCardSet entity)
        {
            int used = CardOfCount(entity.Org_ID, entity.Lcs_ID, true, true);
            if (used > 0) throw new Exception("当前设置项中涉及的学习卡已经存在使用记录，不能删除！可以选择禁用。");
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<LearningCardSet>(entity);
                    tran.Delete<LearningCard>(LearningCard._.Lcs_ID == entity.Lcs_ID);

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
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void SetDelete(int identify)
        {
            LearningCardSet set = SetSingle(identify);
            SetDelete(set);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LearningCardSet SetSingle(int identify)
        {
            return Gateway.Default.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == identify).ToFirst<LearningCardSet>();
        }
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public LearningCardSet[] SetCount(int orgid, bool? isEnable, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            return Gateway.Default.From<LearningCardSet>().Where(wc).OrderBy(LearningCardSet._.Lcs_CrtTime.Desc).ToArray<LearningCardSet>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public int SetOfCount(int orgid, bool? isEnable)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            return Gateway.Default.Count<LearningCardSet>(wc);
        }
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            if (!string.IsNullOrWhiteSpace(searTxt))
            {
                WhereClip like = new WhereClip();
                like &= LearningCardSet._.Lcs_Theme.Like("%" + searTxt + "%");
                like |= LearningCardSet._.Lcs_Intro.Like("%" + searTxt + "%");
                wc.And(like);
            }
            countSum = Gateway.Default.Count<LearningCardSet>(wc);
            return Gateway.Default.From<LearningCardSet>()
                .Where(wc).OrderBy(LearningCardSet._.Lcs_CrtTime.Desc).ToArray<LearningCardSet>(size, (index - 1) * size);
        }
        #endregion

        #region 学习卡管理
        /// <summary>
        /// 生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <param name="factor">随机因子</param>
        /// <returns></returns>
        public LearningCard CardGenerate(LearningCardSet set, int factor=-1)
        {
            Song.Entities.LearningCard card = new LearningCard();
            card.Lc_Price = set.Lcs_Price;   //面额
            card.Lc_IsEnable = true;    //是否启用
            card.Lc_CrtTime = DateTime.Now;  //创建时间                
            card.Lcs_ID = set.Lcs_ID;   //设置项id                
            card.Org_ID = set.Org_ID;   //机构id              
            card.Lc_LimitStart = set.Lcs_LimitStart;   //时间效
            card.Lc_LimitEnd = set.Lcs_LimitEnd;
            //卡值码与其密码
            card.Lc_Code = _CardBuildCoder(set.Lcs_SecretKey, factor, set.Lcs_CodeLength);
            card.Lc_Pw = _CardBuildPw(factor, set.Lcs_PwLength);
            return card;
        }
        /// <summary>
        /// 批量生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <returns></returns>
        public LearningCard[] CardGenerate(LearningCardSet set)
        {
            int count = set.Lcs_Count;
            //生成学习卡
            LearningCard[] cards = new LearningCard[count];
            for (int i = 0; i < count; i++)
                cards[i] = CardGenerate(set, i); 
            //判断是否重复
            for (int i = 0; i < cards.Length; i++)
            {
                for (int j = 0; j < cards.Length; j++)
                {
                    if (j <= i) continue;
                    if (cards[i].Lc_Code == cards[j].Lc_Code)
                    {
                        cards[i] = CardGenerate(set, i * j + DateTime.Now.Millisecond);
                        i = j = 0;
                    }
                }
            }
            return cards;
        }
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardAdd(LearningCard entity)
        {
            entity.Lc_CrtTime = DateTime.Now;
            Gateway.Default.Save<LearningCard>(entity);
        }
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardSave(LearningCard entity)
        {
            Gateway.Default.Save<LearningCard>(entity);
        }
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardDelete(LearningCard entity)
        {
            Gateway.Default.Delete<LearningCard>(LearningCard._.Lc_ID == entity.Lc_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CardDelete(int identify)
        {
            Gateway.Default.Delete<LearningCard>(LearningCard._.Lc_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LearningCard CardSingle(int identify)
        {
            return Gateway.Default.From<LearningCard>().Where(LearningCard._.Lc_ID == identify).ToFirst<LearningCard>();
        }
        /// <summary>
        /// 校验学习卡是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public LearningCard CardCheck(string code)
        {
            code = Regex.Replace(code, @"[^\d-]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (code.IndexOf("-") < 0) throw new Exception("该学习卡不正确！");
            //取密码与充值码
            string pw = code.Substring(code.IndexOf("-") + 1);
            code = code.Substring(0, code.IndexOf("-"));
            //验证是否正确
            WhereClip wc = new WhereClip();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            wc &= LearningCard._.Org_ID == org.Org_ID;
            wc &= LearningCard._.Lc_Code == code.Trim();
            wc &= LearningCard._.Lc_Pw == pw.Trim();
            wc &= LearningCard._.Lc_LimitStart < DateTime.Now;
            wc &= LearningCard._.Lc_LimitEnd > DateTime.Now;
            LearningCard single = Gateway.Default.From<LearningCard>().Where(wc).ToFirst<LearningCard>();
            if (single == null) throw new Exception("该学习卡不存在，或已经过期！");
            if (single.Lc_IsUsed) throw new Exception("该学习卡已经使用过！");
            return single;
        }
        /// <summary>
        /// 使用该学习卡
        /// </summary>
        /// <param name="entity"></param>
        public void CardUse(LearningCard entity)
        {
            if (entity.Lc_State != 0) throw new Exception("该学习卡已经使用");
            LearningCardSet set = this.SetSingle(entity.Lcs_ID);
            if (set == null || set.Lcs_IsEnable == false) throw new Exception("该学习卡不可使用");
            //是否过期
            if (!(DateTime.Now > entity.Lc_LimitStart && DateTime.Now < entity.Lc_LimitEnd.Date.AddDays(1)))            
                throw new Exception("该学习卡已经过期");
            //标注已经使用
            entity.Lc_IsUsed = true;
            entity.Lc_UsedTime = DateTime.Now;
            entity.Lc_State = 1;    //状态，0为初始，1为使用，-1为回滚
            //
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Course[] courses = this.CoursesGet(set.Lcs_RelatedCourses);
                    foreach (Course cou in courses)
                    {
                        Song.Entities.Student_Course sc = new Student_Course();
                        sc.Ac_ID = entity.Ac_ID;
                        sc.Cou_ID = cou.Cou_ID;
                        sc.Stc_Money = 0;
                        sc.Stc_StartTime = entity.Lc_LimitStart;
                        sc.Stc_EndTime = entity.Lc_LimitEnd;
                        sc.Org_ID = entity.Org_ID;
                        sc.Stc_CrtTime = DateTime.Now;
                        tran.Save<Student_Course>(sc);
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
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        public void CardRollback(LearningCard entity)
        {
            if (entity.Lc_State == 1)
            {
                LearningCardSet set = this.SetSingle(entity.Lcs_ID);
                Course[] courses = this.CoursesGet(set.Lcs_RelatedCourses);
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        foreach (Course cou in courses)
                        {
                            tran.Delete<Student_Course>(Student_Course._.Ac_ID == entity.Ac_ID && Student_Course._.Cou_ID == cou.Cou_ID);
                            tran.Update<Accounts>(new Field[] { Accounts._.Ac_CurrCourse }, new object[] { -1 },
                                Accounts._.Ac_ID == entity.Ac_ID && Accounts._.Ac_CurrCourse == cou.Cou_ID);                            
                        }
                        tran.Commit();
                        Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
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
        }
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public LearningCard[] CardCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            return Gateway.Default.From<LearningCard>().Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        public int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            return Gateway.Default.Count<LearningCard>(wc);
        }
        /// <summary>
        /// 导出Excel格式的学习卡信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <returns></returns>
        public string Card4Excel(string path, int orgid, int lcsid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "学习卡.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //创建工作簿对象
            LearningCardSet rs = Gateway.Default.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == lcsid).ToFirst<LearningCardSet>();
            ISheet sheet = hssfworkbook.CreateSheet(rs.Lcs_Theme);
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            WhereClip wc = LearningCard._.Org_ID == orgid;
            if (lcsid >= 0) wc.And(LearningCard._.Lcs_ID == lcsid);
            LearningCard[] rcodes = Gateway.Default.From<LearningCard>().Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>();
            for (int i = 0; i < rcodes.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = rcodes[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(rcodes[i], null);
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
                                string f = s.Substring(s.LastIndexOf("=") + 1);
                                if (value.ToLower() == h.ToLower()) value = f;
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
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<LearningCard>(wc);
            return Gateway.Default.From<LearningCard>()
                .Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>(size, (index - 1) * size);
        }
        #endregion

        #region 充值码生成
        /// <summary>
        /// 生成单个学习卡的编码
        /// </summary>
        /// <param name="scretKey">密钥</param>
        /// <param name="factor">随机因子</param>
        /// <param name="length">充值码的长度</param>
        private string _CardBuildCoder(string scretKey, int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            //充值码基础值（来自时间）
            string baseCode = DateTime.Now.ToString("yyMMddhhmmssfff");
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            int rdNumber = rd.Next(0, 1000);
            baseCode += rdNumber.ToString("000");
            //充值码加密值（来自密钥中随机字符）
            if (string.IsNullOrWhiteSpace(scretKey))
                scretKey = WeiSha.Common.Request.UniqueID();
            string pwstr = "";
            while (pwstr.Length < baseCode.Length)
            {
                System.Random rdpw = new System.Random(pwstr.Length * factor);
                int tm = rd.Next(0, scretKey.Length - 1);
                pwstr += scretKey.Substring(tm, 1);
            }
            //基础值与加密值运算
            byte[] typeBase = System.Text.Encoding.Default.GetBytes(baseCode);
            byte[] typePw = System.Text.Encoding.Default.GetBytes(pwstr);
            string str = "";
            for (int i = 0; i < typeBase.Length; i++)
            {
                typeBase[i] ^= typePw[i];
                str += typeBase[i].ToString();
            }
            return str.Substring(str.Length - length);
        }
        /// <summary>
        /// 生成单个学习卡的密码
        /// </summary>
        /// <param name="factor">随机因子</param>
        /// <param name="length">密码的长度</param>
        /// <returns></returns>
        private string _CardBuildPw(int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            string lenstr = "9";
            while (lenstr.Length < length) lenstr += "9";
            int max = Convert.ToInt32(lenstr);
            int rdNumber = rd.Next(0, max);
            return rdNumber.ToString(lenstr.Replace("9", "0"));
        }
        #endregion

        #region 关联课程
        /// <summary>
        /// 获取关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Course[] CoursesGet(LearningCardSet set)
        {
            return CoursesGet(set.Lcs_RelatedCourses);
        }
        public Course[] CoursesGet(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml, false);
            XmlNodeList items = xmlDoc.SelectNodes("Items/item");
            List<Course> list = new List<Course>();
            for (int i = 0; i < items.Count; i++)
            {
                string couid = ((XmlElement)items[i]).GetAttribute("Cou_ID");
                if (string.IsNullOrWhiteSpace(couid)) continue;
                int id = 0;
                int.TryParse(couid, out id);
                if (id <= 0) continue;
                Course cou = Business.Do<ICourse>().CourseSingle(id);
                if (cou == null) continue;
                list.Add(cou);                
            }
            return list.ToArray();
        }
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet对象中的Lcs_RelatedCourses将记录关联信息</returns>
        public LearningCardSet CoursesSet(LearningCardSet set, Course[] courses)
        {
            if (courses == null || courses.Length <= 0) return set;
            int[] couid = new int[courses.Length];
            for (int i = 0; i < courses.Length; i++)
                couid[i] = courses[i].Cou_ID;
            return CoursesSet(set, couid);
        }
        public LearningCardSet CoursesSet(LearningCardSet set, int[] couid)
        {
            if (couid == null || couid.Length < 1) return null;
            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("Items");
            xmlDoc.AppendChild(root);
            for (int i = 0; i < couid.Length; i++)
            {
                XmlElement item = xmlDoc.CreateElement("item");
                item.SetAttribute("Cou_ID", couid[i].ToString());
                root.AppendChild(item);
            }
            set.Lcs_RelatedCourses = root.OuterXml;
            return set;
        }
        #endregion
    }
}
