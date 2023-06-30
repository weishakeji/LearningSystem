using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Reflection;
using System.Collections;



namespace Song.ServiceImpls
{
    public class SystemParaCom : ISystemPara
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(SystemPara entity)
        {
            if (IsExists(entity)) throw new Exception("当前参数已经存在");
            Gateway.Default.Save<SystemPara>(entity);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        public void Save(string key, string value)
        {
            SystemPara Ps = Gateway.Default.From<SystemPara>().Where(SystemPara._.Sys_Key == key).ToFirst<SystemPara>();
            //如果是一个新对象
            if (Ps == null) Ps = new SystemPara();            
            Ps.Sys_Key = key;
            Ps.Sys_Value = value;
            Gateway.Default.Save<SystemPara>(Ps);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 修改，且是否直接刷新全局参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRefresh"></param>
        public void Save(string key, string value, bool isRefresh)
        {
            SystemPara Ps = Gateway.Default.From<SystemPara>().Where(SystemPara._.Sys_Key == key).ToFirst<SystemPara>();
            //如果是一个新对象
            if (Ps == null) Ps = new SystemPara();  
            Ps.Sys_Key = key;
            Ps.Sys_Value = value;
            Gateway.Default.Save<SystemPara>(Ps);
            if (isRefresh) this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        /// <param name="unit">参数的单位</param>
        public void Save(string key, string value, string unit)
        {
            SystemPara Ps = Gateway.Default.From<SystemPara>().Where(SystemPara._.Sys_Key == key).ToFirst<SystemPara>();
            //如果是一个新对象
            if (Ps == null)
            {
                Ps = new SystemPara();
            }
            Ps.Sys_Key = key;
            Ps.Sys_Value = value;
            Ps.Sys_Unit = unit;
            Gateway.Default.Save<SystemPara>(Ps);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 用实例保存
        /// </summary>
        /// <param name="entity"></param>
        public void Save(SystemPara entity)
        {
            if (IsExists(entity)) throw new Exception("当前参数已经存在");
            Gateway.Default.Save<SystemPara>(entity);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 当前参数是否存在（通过参数名判断）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>如果已经存在，则返回true</returns>
        public bool IsExists(SystemPara entity)
        {
            WhereClip wc = new WhereClip();
            wc.And(SystemPara._.Sys_Key == entity.Sys_Key);
            int obj = Gateway.Default.Count<SystemPara>(wc && SystemPara._.Sys_Id != entity.Sys_Id);
            return obj > 0;
        }
        /// <summary>
        /// 刷新全局参数
        /// </summary>
        public List<SystemPara> Refresh()
        {
            Cache.EntitiesCache.Clear<SystemPara>();
            List<SystemPara> syspara = Gateway.Default.From<SystemPara>().OrderBy(SystemPara._.Sys_Key.Asc).ToList<SystemPara>();
            Cache.EntitiesCache.Save<SystemPara>(syspara);
            return syspara;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(SystemPara entity)
        {
            Gateway.Default.Delete<SystemPara>(entity);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Gateway.Default.Delete<SystemPara>(SystemPara._.Sys_Id == identify);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 删除，按键值
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            Gateway.Default.Delete<SystemPara>(SystemPara._.Sys_Key == key);
            this.Refresh();     //重新构建系统参数的缓存
        }
        /// <summary>
        /// 根据键，获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            SystemPara curr = GetSingle(key);           
            if (curr == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(curr.Sys_Value))
                return string.IsNullOrWhiteSpace(curr.Sys_Default) ? string.Empty : curr.Sys_Default;
            return curr.Sys_Value.Trim();
        }
        /// <summary>
        /// 根据键，获取值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public WeiSha.Core.Param.Method.ConvertToAnyValue this[string key]
        {
            get
            {
                SystemPara curr = GetSingle(key);
                if (curr == null) return new WeiSha.Core.Param.Method.ConvertToAnyValue(null);
                string val = !string.IsNullOrWhiteSpace(curr.Sys_Value) ? curr.Sys_Value.Trim() : curr.Sys_Default;
                WeiSha.Core.Param.Method.ConvertToAnyValue p = new WeiSha.Core.Param.Method.ConvertToAnyValue(val);
                p.Unit = curr.Sys_Unit;
                return p;
            }
        }
        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SystemPara GetSingle(int id)
        {
            return Gateway.Default.From<SystemPara>().Where(SystemPara._.Sys_Id == id).ToFirst<SystemPara>();
        }
        /// <summary>
        /// 获取单个实例，通过键值获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SystemPara GetSingle(string key)
        {
            SystemPara curr = null;
            //从缓存中读取
            List<SystemPara> list = Cache.EntitiesCache.GetList<SystemPara>();
            if (list == null) list = this.Refresh();
            List<SystemPara> tm = (from l in list
                                   where l.Sys_Key == key
                                   select l).ToList<SystemPara>();
            if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<SystemPara>().Where(SystemPara._.Sys_Key == key).ToFirst<SystemPara>();
            return curr;
        }
        /// <summary>
        /// 获取所有参数
        /// </summary>
        /// <returns></returns>
        public List<SystemPara> GetAll()
        {
            //从缓存中读取
            List<SystemPara> list = Cache.EntitiesCache.GetList<SystemPara>();
            if (list == null) list = this.Refresh();
            return list;           
        }
        /// <summary>
        /// 查询获取参数
        /// </summary>
        /// <param name="searKey">键名</param>
        /// <param name="searIntro">参数说明</param>
        /// <returns></returns>
        public DataTable GetAll(string searKey, string searIntro)
        {
            WhereClip wc = SystemPara._.Sys_Id > -1;
            if (searKey != null && searKey != "")
            {
                wc.And(SystemPara._.Sys_Key.Like("%" + searKey + "%"));
            }
            if (searIntro != null && searIntro != "")
            {
                wc.And(SystemPara._.Sys_ParaIntro.Like("%" + searIntro + "%"));
            }
            DataSet ds = Gateway.Default.From<SystemPara>().Where(wc).OrderBy(SystemPara._.Sys_Key.Asc).ToDataSet();
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 生成资金流水号
        /// </summary>
        /// <returns></returns>
        public string Serial()
        {
            //前缀
            string pre = "";
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) pre = org.Org_TwoDomain;
            //二级域名前缀
            int len = 4;
            if (string.IsNullOrWhiteSpace(pre)) pre = "";
            pre = pre.Replace(".", "");
            while (pre.Length < len) pre += "X";  //小于指定长度，则补位
            if (pre.Length > len) pre = pre.Substring(0, len);  //大于指定长度，则截取
            //机构ID前缀
            string id = "";
            if (org != null) id = org.Org_ID.ToString("000");
            while (id.Length < 4) id = "0" + id;
            //序号
            string baseCode = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            System.Random rd = new System.Random((int)DateTime.Now.Ticks + org.Org_ID);
            int rdNumber = rd.Next(0, 99);
            //长度：4+18+4+2=28
            return id + baseCode + pre.ToUpper() + string.Format("{0:00}", rdNumber);
        }
        /// <summary>
        /// 生成流水号(雪花算法）
        /// </summary>
        /// <returns></returns>
        public long SerialSnow()
        {
            return WeiSha.Core.Request.SnowID();
        }
        /// <summary>
        /// 测试是否完成授权
        /// </summary>
        public bool IsLicense()
        {
             WeiSha.Core.License lic = WeiSha.Core.License.Value;
             return lic.IsLicense;
        }

        /// <summary>
        /// 数据库完整性测试
        /// </summary>
        /// <returns> string, string[],前者为表名，后者为字段</returns>
        public Dictionary<string, string[]> DatabaseCompleteTest()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\\bin\\";
            string assemblyName = path + "Song.Entities.dll";
            System.Reflection.Assembly assembly = Assembly.LoadFrom(assemblyName);
            Type[] ts = assembly.GetTypes();
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            //List<string> classList = new List<string>();
            foreach (Type t in ts)
            {
                //创建实体
                object obj = System.Activator.CreateInstance(t);
                if (!(obj is WeiSha.Data.Entity)) continue;
                WeiSha.Data.Entity entity = (WeiSha.Data.Entity)obj;
                if (entity == null) continue;
                //对比缺少的字段
                try
                {
                    DataSet ds = Gateway.Default.FromSql(string.Format("select top 1 * from [{0}]", t.Name)).ToDataSet();
                    List<string> fieldExist = new List<string>();
                    PropertyInfo[] propertyinfo = t.GetProperties();
                    foreach (PropertyInfo pi in propertyinfo)
                    {
                        bool isExist = false;
                        foreach (DataColumn dc in ds.Tables[0].Columns)
                        {
                            if (dc.ColumnName == pi.Name)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist) fieldExist.Add(pi.Name);

                    }
                    if (fieldExist.Count > 0)
                        dic.Add(t.Name, fieldExist.ToArray());
                }
                catch
                {
                    dic.Add(t.Name, new string[] { });
                    //classList.Add(t.Name + ":（缺少整个表）");
                }
            }
            return dic;
        }
        /// <summary>
        /// 数据库链接测试
        /// </summary>
        /// <returns>链接正确为true，否则为false</returns>
        public bool DatabaseLinkTest()
        {
            return  Gateway.IsCorrect;
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            int i = Gateway.Default.FromSql(sql).Execute();
            return i;
        }
        /// <summary>
        /// 执行sql语句，返回第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回第一行第一列的数据</returns>
        public object ScalarSql(string sql)
        {
            return Gateway.Default.FromSql(sql).ToScalar();
        }
        /// <summary>
        /// 执行sql语句，返回第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T ScalarSql<T>(string sql) where T : WeiSha.Data.Entity
        {
            return Gateway.Default.FromSql(sql).ToFirst<T>();
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回数据集</returns>
        public List<T> ForSql<T>(string sql) where T : WeiSha.Data.Entity
        {
            List<T> list = Gateway.Default.FromSql(sql).ToList<T>();
            return list;
        }
        /// <summary>
        /// 返回指定的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ForSql(string sql)
        {
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 数据库里所有的表
        /// </summary>
        /// <returns></returns>
        public List<string> DataTables()
        {
            string sql = "select name from sysobjects where type='U' order by name asc";            
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                List<string> list = new List<string>();
                while (reader.Read())               
                    list.Add(reader.GetValue<string>(0));
                reader.Close();
                reader.Dispose();
                return list;
            } 
        }
        /// <summary>
        /// 获取数据字段
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,type,length,fulltype,isnullable,primary(主键，非零为真)</returns>
        public DataTable DataFields(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"
                    SELECT name,type_name(xtype) AS type,
                    --长度，无限长为-1
                    length,
                    --是否可空，0为可空
                    isnullable as nullable
                    FROM syscolumns
                    WHERE (id = OBJECT_ID('{{tablename}}'))
                    order by name asc";
            sql = sql.Replace("{{tablename}}", tablename.Trim());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count < 1) return null;
            //获取主键
            string sql2 = @"
                SELECT distinct	
	                COLUMN_NAME=stuff((
		                SELECT '|'+COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		                where TABLE_NAME ='{{tablename}}'
                    FOR XML path('')
                    ), 1, 1, '')
                FROM
	                INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                where TABLE_NAME ='{{tablename}}'";
            sql2 = sql2.Replace("{{tablename}}", tablename.Trim());
            object primary = Gateway.Default.FromSql(sql2).ToScalar();
            //将主键加到字段的表中
            DataTable dt = ds.Tables[0];
            dt.Columns.Add(new DataColumn("primary", typeof(int)));
            DataRow parimaryDr = dt.NewRow();  //主键的行
            int parimaryIndex = -1;     //主键的行索引，用以后面与第一行交换位置
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (primary != null)
                {
                    if (dr["name"].ToString().Equals(primary.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        dr["primary"] = 1;
                        parimaryDr.ItemArray = dr.ItemArray;
                        parimaryIndex = i;
                    }
                    else
                    {
                        dr["primary"] = 0;
                    }
                }
            }
            if (parimaryIndex > 0)
            {
                dt.Rows[parimaryIndex].ItemArray = dt.Rows[0].ItemArray;
                dt.Rows[0].ItemArray = parimaryDr.ItemArray;
            }
            return dt;
        }
        public DataTable DataIndexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"SELECT
                        i.name AS IndexName,
                        OBJECT_NAME(i.object_id) AS TableName,
                        c.name AS ColumnName,
                        i.type_desc AS IndexType,
                        ic.is_descending_key AS IsDescending
                    FROM
                        sys.indexes i
                    INNER JOIN 
                        sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                    INNER JOIN 
                        sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                    WHERE
                        OBJECT_NAME(i.object_id) = '{{tablename}}'
                    ORDER BY
                        OBJECT_NAME(i.object_id), i.name, ic.key_ordinal";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 仅获取下的字段的名称，不包括类型等其它属性
        /// </summary>
        /// <param name="tablename">表</param>
        /// <returns></returns>
        public List<string> DataFieldNames(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"SELECT name FROM syscolumns WHERE id = OBJECT_ID('{{tablename}}')";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                List<string> list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetValue<string>(0));
                reader.Close();
                reader.Dispose();
                return list;
            }           
        }        
        #region IEnumerable 成员

        /// <summary>
        /// 实现代迭器的功能，可以引用时用foreach循环
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            //从缓存中读取
            List<SystemPara> list = Cache.EntitiesCache.GetList<SystemPara>();
            if (list == null) list = this.Refresh();
            for (int i = 0; i < list.Count; i++)
            {
                WeiSha.Core.Param.Method.ConvertToAnyValue p = new WeiSha.Core.Param.Method.ConvertToAnyValue();
                p.ParaKey = list[i].Sys_Key;
                p.ParaValue = list[i].Sys_Value;
                p.Unit = list[i].Sys_Unit;
                yield return p;
            }
        }

        #endregion
    }
}
