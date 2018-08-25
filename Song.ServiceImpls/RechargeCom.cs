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
    public class RechargeCom : IRecharge
    {
        #region 充值码设置管理
        /// <summary>
        /// 添加充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeSetAdd(RechargeSet entity)
        {
            entity.Rs_CrtTime = DateTime.Now;
            Gateway.Default.Save<RechargeSet>(entity);
            _RechargeCodeBuilds(entity, entity.Rs_Count);
        }
        /// <summary>
        /// 修改充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeSetSave(RechargeSet entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Song.Entities.RechargeSet rs = tran.From<RechargeSet>().Where(RechargeSet._.Rs_ID == entity.Rs_ID).ToFirst<RechargeSet>();
                    //如果新增了充值码
                    if (rs.Rs_Count < entity.Rs_Count) _RechargeCodeBuilds(entity, entity.Rs_Count - rs.Rs_Count);
                    //如果减少的充值码
                    if (rs.Rs_Count > entity.Rs_Count)
                    {
                        Song.Entities.RechargeCode[] rcs = tran.From<RechargeCode>().Where(RechargeCode._.Rs_ID == entity.Rs_ID && RechargeCode._.Rc_IsUsed == false).ToArray<RechargeCode>(rs.Rs_Count - entity.Rs_Count);
                        foreach (RechargeCode r in rcs)
                            tran.Delete<RechargeCode>(r);
                    }
                    tran.Update<RechargeCode>(new Field[] { RechargeCode._.Rc_Price, RechargeCode._.Rc_LimitStart, RechargeCode._.Rc_LimitEnd },
                        new object[] { entity.Rs_Price, entity.Rs_LimitStart, entity.Rs_LimitEnd }, 
                        RechargeCode._.Rs_ID == entity.Rs_ID && RechargeCode._.Rc_IsUsed == false);
                    tran.Save<RechargeSet>(entity);
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
        /// 删除充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeSetDelete(RechargeSet entity)
        {
            int used = RechargeCodeOfCount(entity.Org_ID, entity.Rs_ID, true, true);
            if (used > 0) throw new Exception("当前设置项中涉及的充值码已经存在消费记录，不能删除！可以选择禁用。");
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<RechargeSet>(entity);
                    tran.Delete<RechargeCode>(RechargeCode._.Rs_ID == entity.Rs_ID);

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
        public void RechargeSetDelete(int identify)
        {
            RechargeSet set = RechargeSetSingle(identify);
            RechargeSetDelete(set);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public RechargeSet RechargeSetSingle(int identify)
        {
            return Gateway.Default.From<RechargeSet>().Where(RechargeSet._.Rs_ID == identify).ToFirst<RechargeSet>();
        }
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public RechargeSet[] RechargeSetCount(int orgid, bool? isEnable, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeSet._.Org_ID == orgid;
            if (isEnable != null) wc &= RechargeSet._.Rs_IsEnable == isEnable;
            return Gateway.Default.From<RechargeSet>().Where(wc).OrderBy(RechargeSet._.Rs_CrtTime.Desc).ToArray<RechargeSet>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public int RechargeSetOfCount(int orgid, bool? isEnable)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeSet._.Org_ID == orgid;
            if (isEnable != null) wc &= RechargeSet._.Rs_IsEnable == isEnable;
            return Gateway.Default.Count<RechargeSet>(wc);
        }
        /// <summary>
        /// 分页获取充值码设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public RechargeSet[] RechargeSetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeSet._.Org_ID == orgid;
            if (isEnable != null) wc &= RechargeSet._.Rs_IsEnable == isEnable;
            if (!string.IsNullOrWhiteSpace(searTxt))
            {
                WhereClip like = new WhereClip();
                like &= RechargeSet._.Rs_Theme.Like("%" + searTxt + "%");
                like |= RechargeSet._.Rs_Intro.Like("%" + searTxt + "%");
                wc.And(like);
            }
            countSum = Gateway.Default.Count<RechargeSet>(wc);
            return Gateway.Default.From<RechargeSet>()
                .Where(wc).OrderBy(RechargeSet._.Rs_CrtTime.Desc).ToArray<RechargeSet>(size, (index - 1) * size);
        }
        #endregion

        #region 充值码管理
        /// <summary>
        /// 添加充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeCodeAdd(RechargeCode entity)
        {
            entity.Rc_CrtTime = DateTime.Now;
            Gateway.Default.Save<RechargeCode>(entity);
        }
        /// <summary>
        /// 修改充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeCodeSave(RechargeCode entity)
        {
            Gateway.Default.Save<RechargeCode>(entity);
        }
        #region 充值码生成
        /// <summary>
        /// 生成充值码
        /// </summary>
        /// <param name="setEntity"></param>
        private void _RechargeCodeBuilds(RechargeSet setEntity, int count)
        {

            for (int i = 0; i < count; i++)
            {
                Song.Entities.RechargeCode code = new RechargeCode();
                //面额
                code.Rc_Price = setEntity.Rs_Price;
                //是否启用
                code.Rc_IsEnable = true;
                //创建时间
                code.Rc_CrtTime = DateTime.Now;
                //类型
                code.Rc_Type = 0;
                //设置项id
                code.Rs_ID = setEntity.Rs_ID;
                //机构id
                code.Org_ID = setEntity.Org_ID;
                //时间效
                code.Rc_LimitStart = setEntity.Rs_LimitStart;
                code.Rc_LimitEnd = setEntity.Rs_LimitEnd;
                //卡值码与其密码
                code.Rc_Code = _RechargeCodeBuildCoder(setEntity.Rs_Pw, i, setEntity.Rs_CodeLength);
                code.Rc_Pw = _RechargeCodeBuildPw(i, setEntity.Rs_PwLength);
                Gateway.Default.Save<RechargeCode>(code);
            }
        }
        /// <summary>
        /// 生成单个充值码
        /// </summary>
        /// <param name="pw">密钥</param>
        /// <param name="factor">随机因子</param>
        /// <param name="length">充值码的长度</param>
        private string _RechargeCodeBuildCoder(string pw, int factor, int length)
        {
            //充值码基础值（来自时间）
            string baseCode = DateTime.Now.ToString("yyMMddhhmmssfff");
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            int rdNumber = rd.Next(0, 1000);
            baseCode += rdNumber.ToString("000");
            //充值码加密值（来自密钥中随机字符）
            if (string.IsNullOrWhiteSpace(pw))
                pw = WeiSha.Common.Request.UniqueID();
            string pwstr = "";
            while (pwstr.Length < baseCode.Length)
            {
                System.Random rdpw = new System.Random(pwstr.Length * factor);
                int tm = rd.Next(0, pw.Length - 1);
                pwstr += pw.Substring(tm, 1);
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
        /// 生成充值卡密码
        /// </summary>
        /// <param name="factor">随机因子</param>
        /// <param name="length">密码的长度</param>
        /// <returns></returns>
        private string _RechargeCodeBuildPw(int factor, int length)
        {
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            string lenstr = "9";
            while (lenstr.Length < length) lenstr += "9";
            int max = Convert.ToInt32(lenstr);
            int rdNumber = rd.Next(0, max);
            return rdNumber.ToString(lenstr.Replace("9", "0"));
        }
        #endregion
        /// <summary>
        /// 删除充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void RechargeCodeDelete(RechargeCode entity)
        {
            Gateway.Default.Delete<RechargeCode>(RechargeCode._.Rc_ID == entity.Rc_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void RechargeCodeDelete(int identify)
        {
            Gateway.Default.Delete<RechargeCode>(RechargeCode._.Rc_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public RechargeCode RechargeCodeSingle(int identify)
        {
            return Gateway.Default.From<RechargeCode>().Where(RechargeCode._.Rc_ID == identify).ToFirst<RechargeCode>();
        }
        /// <summary>
        /// 校验充值码是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RechargeCode CouponCheckCode(string code)
        {
            code = Regex.Replace(code, @"[^\d-]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (code.IndexOf("-") < 0) throw new Exception("该充值码不正确！");
            //取密码与充值码
            string pw = code.Substring(code.IndexOf("-") + 1);
            code = code.Substring(0, code.IndexOf("-"));
            //验证是否正确
            WhereClip wc = new WhereClip();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            wc &= RechargeCode._.Org_ID == org.Org_ID;
            wc &= RechargeCode._.Rc_Code == code.Trim();
            wc &= RechargeCode._.Rc_Pw == pw.Trim();
            wc &= RechargeCode._.Rc_LimitStart < DateTime.Now;
            wc &= RechargeCode._.Rc_LimitEnd > DateTime.Now;
            RechargeCode single = Gateway.Default.From<RechargeCode>().Where(wc).ToFirst<RechargeCode>();
            if (single == null) throw new Exception("该充值码不存在，或已经过期！");
            if (single.Rc_IsUsed) throw new Exception("该充值码已经使用过！");
            return single;
        }
        /// <summary>
        /// 使用该充值码
        /// </summary>
        /// <param name="entity"></param>
        public void CouponUseCode(RechargeCode entity)
        {
            //是否被禁用
            Song.Entities.RechargeSet set = Gateway.Default.From<RechargeSet>().Where(RechargeSet._.Rs_ID == entity.Rs_ID).ToFirst<RechargeSet>();
            if (set == null || set.Rs_IsEnable == false) throw new Exception("该充值码已经被禁用");
            //是否过期
            if (!(DateTime.Now > entity.Rc_LimitStart && DateTime.Now < entity.Rc_LimitEnd.Date.AddDays(1)))
            {
                throw new Exception("该充值码已经过期");
            }
            //标注已经使用
            entity.Rc_IsUsed = true;
            entity.Rc_UsedTime = DateTime.Now;
            //产生流水
            CouponAccount ca = new CouponAccount();
            ca.Ca_Value = entity.Rc_Price;
            ca.Ac_ID = entity.Ac_ID;
            ca.Ca_From = 2;
            ca.Rc_Code = entity.Rc_Code + "-" + entity.Rc_Pw;
            ca.Ca_Source = "充值码充值";
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Business.Do<IAccounts>().CouponAdd(ca);
                    tran.Save<RechargeCode>(entity);
                    RechargeSet setEnity = tran.From<RechargeSet>().Where(RechargeSet._.Rs_ID == entity.Rs_ID).ToFirst<RechargeSet>();
                    if (setEnity != null)
                    {                      
                        setEnity.Rs_UsedCount++;
                        tran.Save<RechargeSet>(setEnity);
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
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public RechargeCode[] RechargeCodeCount(int orgid, int rsid, bool? isEnable, bool? isUsed, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeCode._.Org_ID == orgid;
            if (rsid > 0) wc &= RechargeCode._.Rs_ID == rsid;
            if (isEnable != null) wc &= RechargeCode._.Rc_IsEnable == isEnable;
            if (isUsed != null) wc &= RechargeCode._.Rc_IsUsed == isUsed;
            return Gateway.Default.From<RechargeCode>().Where(wc).OrderBy(RechargeCode._.Rc_CrtTime.Desc).ToArray<RechargeCode>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        public int RechargeCodeOfCount(int orgid, int rsid, bool? isEnable, bool? isUsed)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeCode._.Org_ID == orgid;
            if (rsid > 0) wc &= RechargeCode._.Rs_ID == rsid;
            if (isEnable != null) wc &= RechargeCode._.Rc_IsEnable == isEnable;
            if (isUsed != null) wc &= RechargeCode._.Rc_IsUsed == isUsed;
            return Gateway.Default.Count<RechargeCode>(wc);
        }
        /// <summary>
        /// 导出Excel格式的充值码信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充值码设置项的id</param>
        /// <returns></returns>
        public string RechargeCode4Excel(string path, int orgid, int rsid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "充值码.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //创建工作簿对象
            Song.Entities.RechargeSet rs = Gateway.Default.From<RechargeSet>().Where(RechargeSet._.Rs_ID == rsid).ToFirst<RechargeSet>();
            ISheet sheet = hssfworkbook.CreateSheet(rs.Rs_Theme);
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            WhereClip wc = RechargeCode._.Org_ID == orgid;
            if (rsid >= 0) wc.And(RechargeCode._.Rs_ID == rsid);
            RechargeCode[] rcodes = Gateway.Default.From<RechargeCode>().Where(wc).OrderBy(RechargeCode._.Rc_CrtTime.Desc).ToArray<RechargeCode>();
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
        /// 分页获取充值码设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public RechargeCode[] RechargeCodePager(int orgid, int rsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeCode._.Org_ID == orgid;
            if (rsid > 0) wc &= RechargeCode._.Rs_ID == rsid;
            if (isEnable != null) wc &= RechargeCode._.Rc_IsEnable == isEnable;
            if (isUsed != null) wc &= RechargeCode._.Rc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<RechargeCode>(wc);
            return Gateway.Default.From<RechargeCode>()
                .Where(wc).OrderBy(RechargeCode._.Rc_CrtTime.Desc).ToArray<RechargeCode>(size, (index - 1) * size);
        }
        public RechargeCode[] RechargeCodePager(int orgid, int rsid, string code, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeCode._.Org_ID == orgid;
            if (rsid > 0) wc &= RechargeCode._.Rs_ID == rsid;
            if (!string.IsNullOrWhiteSpace(code)) wc &= RechargeCode._.Rc_Code.Like("%" + code + "%");
            if (isEnable != null) wc &= RechargeCode._.Rc_IsEnable == isEnable;
            if (isUsed != null) wc &= RechargeCode._.Rc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<RechargeCode>(wc);
            return Gateway.Default.From<RechargeCode>()
                .Where(wc).OrderBy(RechargeCode._.Rc_CrtTime.Desc).ToArray<RechargeCode>(size, (index - 1) * size);
        }
        #endregion
    }
}
