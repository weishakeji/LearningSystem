using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
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
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            Gateway.Default.Save<RechargeSet>(entity);
            //生成充值卡
            RechargeCode[] cards = CardGenerate(entity);
            if (cards != null)
            {
                foreach (RechargeCode c in cards)
                    Gateway.Default.Save<RechargeCode>(c);
            }
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
                    RechargeCode[] cards = CardGenerate(entity, tran);
                    if (cards != null)
                    {
                        foreach (RechargeCode c in cards)
                            Gateway.Default.Save<RechargeCode>(c);
                    }
                    tran.Update<RechargeCode>(new Field[] { RechargeCode._.Rc_Price, RechargeCode._.Rc_LimitStart, RechargeCode._.Rc_LimitEnd },
                        new object[] { entity.Rs_Price, entity.Rs_LimitStart, entity.Rs_LimitEnd }, 
                        RechargeCode._.Rs_ID == entity.Rs_ID && RechargeCode._.Rc_IsUsed == false);
                    tran.Save<RechargeSet>(entity);
                    tran.Commit();

                    //记算实际卡数
                    int cardtotal = Gateway.Default.Count<RechargeCode>(RechargeCode._.Rs_ID == entity.Rs_ID);
                    entity.Rs_Count = cardtotal;
                    Gateway.Default.Save<RechargeSet>(entity);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
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
        /// 判断学习卡名称是否重复
        /// </summary>
        /// <param name="name">学习卡名称</param>
        /// <param name="id">学习卡id</param>
        /// <returns></returns>
        public bool RechargeSetIsExist(string name, int id)
        {
            WhereClip wc = new WhereClip();
            //如果是一个已经存在的对象，则不匹配自己
            if (id > 0) wc.And(RechargeSet._.Rs_ID != id);
            int count = Gateway.Default.Count<RechargeSet>(wc && RechargeSet._.Rs_Theme == name);
            return count > 0;
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
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public int RechargeSetOfCount(int orgid, bool? isEnable)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeSet._.Org_ID == orgid;
            if (isEnable != null) wc &= RechargeSet._.Rs_IsEnable == isEnable;
            return Gateway.Default.Count<RechargeSet>(wc);
        }
        /// <summary>
        /// 充值卡的最小长度，取机构id最大数、设置项id最大值
        /// </summary>
        /// <returns></returns>
        public int MinLength()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            object max_setid = Gateway.Default.Max<RechargeSet>(RechargeSet._.Rs_ID, new WhereClip());
            int min_len = (max_setid == null ? org.Org_ID.ToString().Length : (org.Org_ID.ToString() + max_setid.ToString()).Length) + 1;
            return min_len;
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
                like &= RechargeSet._.Rs_Theme.Contains(searTxt);
                like |= RechargeSet._.Rs_Intro.Contains(searTxt);
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
        /// 生成充值卡卡号（不重复）
        /// </summary>
        /// <param name="set">充值卡设置项</param>
        /// <param name="count">要生成的个数，小于等于0，取设置项中的数量</param>
        /// <returns></returns>
        public string[] _generateCode(RechargeSet set, int count)
        {
            //要生成的数量
            count = count <= 0 ? set.Rs_Count : count;
            //卡号前缀，来自机构id和设置项id
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == set.Org_ID).ToFirst<Organization>();
            object org_max = Gateway.Default.Max<Organization>(Organization._.Org_ID, new WhereClip());
            object set_max = Gateway.Default.Max<RechargeSet>(RechargeSet._.Rs_ID, new WhereClip());
            string prefix = _lenFormatstring(org.Org_ID, org_max) + _lenFormatstring(set.Rs_ID, set_max);


            //生成不重复的序列，采用洗牌算法
            List<int> temp = new List<int>();
            for (int i = 0; i < count; i++)
                temp.Add(i);
            int[] arr = new int[count];
            for (int i = 0; i < arr.Length; i++)
            {
                System.Random random = new System.Random(((int)DateTime.Now.Ticks) + i);
                int index = random.Next(0, temp.Count);
                arr[i] = temp[index];
                temp.RemoveAt(index);
            }

            //生成指定数量的卡号（不重复）
            string[] reslut = new string[arr.Length];
            //卡号数量的位数
            int len = count.ToString().Length;
            string format = string.Empty;
            while (len-- > 0) format += "0";
            //剩余卡号长度位数
            int surplus = set.Rs_CodeLength - count.ToString().Length - prefix.Length;
            for (int i = 0; i < arr.Length; i++)
            {
                reslut[i] = prefix + arr[i].ToString(format);
                if (surplus > 0)
                    reslut[i] += _cardBuildCode(string.Empty, i, surplus);
            }
            return reslut;
        }
        /// <summary>
        /// 格式化数值为指定位数字符串
        /// </summary>
        /// <param name="number">要格式化的数值</param>
        /// <param name="max">字符最大宽度，例如1234，宽度为4</param>
        /// <returns></returns>
        private string _lenFormatstring(int number, object max)
        {
            int len = max.ToString().Length;
            string format = string.Empty;
            while (len-- > 0) format += "0";
            return number.ToString(format);
        }
        /// <summary>
        /// 生成随机数字字符（有可能重复）
        /// </summary>
        /// <param name="scretKey">密钥</param>
        /// <param name="factor">随机因子</param>
        /// <param name="length">充值码的长度</param>
        private string _cardBuildCode(string scretKey, int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            //充值码基础值（来自时间）
            string baseCode = DateTime.Now.ToString("yyMMddhhmmssfff");
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            int rdNumber = rd.Next(0, int.MaxValue);
            int len = length - baseCode.Length;
            string format = string.Empty;
            while (len-- > 0) format += "0";
            baseCode += rdNumber.ToString(format);
            //充值码加密值（来自密钥中随机字符）
            if (string.IsNullOrWhiteSpace(scretKey))
                scretKey = WeiSha.Core.Request.UniqueID();
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
            return str.Length > length ? str.Substring(str.Length - length) : str;
        }
        /// <summary>
        /// 生成单个充值卡的密码
        /// </summary>
        /// <param name="factor">随机因子</param>
        /// <param name="length">密码的长度</param>
        /// <returns></returns>
        private string _cardBuildPw(int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) + factor);
            string lenstr = "9";
            while (lenstr.Length < length) lenstr += "9";
            int max = Convert.ToInt32(lenstr);
            int rdNumber = rd.Next(0, max);
            return rdNumber.ToString(lenstr.Replace("9", "0"));
        }
        /// <summary>
        /// 生成充值卡对象
        /// </summary>
        /// <param name="set">充值卡的设置项</param>
        /// <param name="code">卡号</param>
        /// <returns></returns>
        public RechargeCode CardGenerateObject(RechargeSet set, string code)
        {
            Song.Entities.RechargeCode card = new RechargeCode();
            card.Rc_Price = set.Rs_Price;   //面额
            card.Rc_IsEnable = true;    //是否启用
            card.Rc_CrtTime = DateTime.Now;  //创建时间                
            card.Rs_ID = set.Rs_ID;   //设置项id                
            card.Org_ID = set.Org_ID;   //机构id              
            card.Rc_LimitStart = set.Rs_LimitStart;   //时效
            card.Rc_LimitEnd = set.Rs_LimitEnd;
            //卡值码与其密码
            card.Rc_Code = code;
            int factor = 0;
            if (code.Length < 10) int.TryParse(code, out factor);
            else
                int.TryParse(code.Substring(0, 9), out factor);
            card.Rc_Pw = _cardBuildPw(factor, set.Rs_PwLength);
            return card;
        }
        /// <summary>
        /// 批量生成充值卡
        /// </summary>
        /// <param name="set">充值卡的设置项</param>
        /// <returns></returns>
        public RechargeCode[] CardGenerate(RechargeSet set)
        {
            return CardGenerate(set, null);
        }
        /// <summary>
        /// </summary>
        /// <param name="set"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public RechargeCode[] CardGenerate(RechargeSet set, DbTrans tran)
        {
            int count = 0;  //要生成的数量
            int realcount = Gateway.Default.Count<RechargeCode>(RechargeCode._.Rs_ID == set.Rs_ID);   //实际数量
            //如果实际数量小于要生成的数量，则只生成差额
            if (realcount < set.Rs_Count) count = set.Rs_Count - realcount;
            //如果实际数量小于要生成的数量，则删除多余（如果已经使用的过的，不可删除）
            if (realcount > set.Rs_Count)
            {
                bool isNull = false;
                if (tran == null)
                {
                    tran = Gateway.Default.BeginTrans();
                    isNull = true;
                }
                Song.Entities.RechargeCode[] lcard = tran.From<RechargeCode>().Where(RechargeCode._.Rs_ID == set.Rs_ID && RechargeCode._.Rc_IsUsed == false)
                    .ToArray<RechargeCode>(realcount - set.Rs_Count);
                foreach (RechargeCode r in lcard)
                    tran.Delete<RechargeCode>(r);
                if (isNull) tran.Commit();
                return null;
            }
            if (count <= 0) return null;

            //生成指定数量的充值卡
            List<string> list = new List<string>();
            while (list.Count < count)
            {
                string[] arr = _generateCode(set, count - list.Count);
                RechargeCode[] original = Gateway.Default.From<RechargeCode>().Where(RechargeCode._.Rs_ID == set.Rs_ID).ToArray<RechargeCode>();
                for (int i = 0; i < arr.Length; i++)
                {
                    bool isexist = false;
                    foreach (RechargeCode c in original)
                    {
                        if (c.Rc_Code.Equals(arr[i]))
                        {
                            isexist = true;
                            break;
                        }
                    }
                    if (!isexist) list.Add(arr[i]);
                }
            }
            //生成充值卡
            RechargeCode[] cards = new RechargeCode[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                cards[i] = CardGenerateObject(set, list[i]);
            }
            return cards;
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
        /// 通过充值码获取对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RechargeCode RechargeCodeSingle(string code)
        {
            code = Regex.Replace(code, @"[^\d-]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (code.IndexOf("-") < 0)
            {
                return Gateway.Default.From<RechargeCode>().Where(RechargeCode._.Rc_Code == code)
               .ToFirst<RechargeCode>();
            }
            //取密码与充值码
            string pw = code.Substring(code.IndexOf("-") + 1);
            code = code.Substring(0, code.IndexOf("-"));
            return this.RechargeCodeSingle(code, pw);
        }
        /// <summary>
        /// 通过充值码和密钥获取对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public RechargeCode RechargeCodeSingle(string code, string pw)
        {
            return Gateway.Default.From<RechargeCode>().Where(RechargeCode._.Rc_Code == code && RechargeCode._.Rc_Pw == pw)
               .ToFirst<RechargeCode>();
        }
        /// <summary>
        /// 校验充值码是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public RechargeCode CouponCheckCode(string code)
        {
            code = Regex.Replace(code, @"[^\d-]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (code.IndexOf("-") < 0) throw new Exception("充值码不正确！缺少破折号");
            //取密码与充值码
            string pw = code.Substring(code.IndexOf("-") + 1);
            code = code.Substring(0, code.IndexOf("-"));
            //验证是否正确
            WhereClip wc = new WhereClip();
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //wc &= RechargeCode._.Org_ID == org.Org_ID;
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
        public CouponAccount CouponUseCode(RechargeCode entity)
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
            ca.Ca_Info = string.Format("充值码({0})充值", entity.Rc_Code + "-" + entity.Rc_Pw);
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
                    return ca;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
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
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "充值码.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
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
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(RechargeCode._.Org_ID == orgid);
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
        public RechargeCode[] RechargeCodePager(int orgid, int rsid, string code, string account, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= RechargeCode._.Org_ID == orgid;
            if (rsid > 0) wc &= RechargeCode._.Rs_ID == rsid;
            if (!string.IsNullOrWhiteSpace(code)) wc &= RechargeCode._.Rc_Code.Contains(code);
            if (!string.IsNullOrWhiteSpace(account)) wc &= RechargeCode._.Ac_AccName.Contains(account);
            if (isEnable != null) wc &= RechargeCode._.Rc_IsEnable == isEnable;
            if (isUsed != null) wc &= RechargeCode._.Rc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<RechargeCode>(wc);
            return Gateway.Default.From<RechargeCode>()
                .Where(wc).OrderBy(RechargeCode._.Rc_CrtTime.Desc).ToArray<RechargeCode>(size, (index - 1) * size);
        }
        #endregion
    }
}
