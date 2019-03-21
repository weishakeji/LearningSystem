using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Common;
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
            try
            {
                WeiSha.Common.Cache<Song.Entities.SystemPara>.Data.Clear();
            }
            catch
            {
            }
            finally
            {
                SystemPara[] syspara = Gateway.Default.From<SystemPara>().OrderBy(SystemPara._.Sys_Key.Asc).ToArray<SystemPara>();
                foreach (Song.Entities.SystemPara p in syspara)
                    WeiSha.Common.Cache<Song.Entities.SystemPara>.Data.Add(p);
                List<SystemPara> list = WeiSha.Common.Cache<SystemPara>.Data.List;                
            }
            return WeiSha.Common.Cache<SystemPara>.Data.List;
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
            if (curr == null) return null;
            return !string.IsNullOrWhiteSpace(curr.Sys_Value) ? curr.Sys_Value.Trim() : curr.Sys_Default;
        }
        /// <summary>
        /// 根据键，获取值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public WeiSha.Common.Param.Method.ConvertToAnyValue this[string key]
        {
            get
            {
                SystemPara curr = GetSingle(key);
                if (curr == null) return new WeiSha.Common.Param.Method.ConvertToAnyValue(null);
                string val = !string.IsNullOrWhiteSpace(curr.Sys_Value) ? curr.Sys_Value.Trim() : curr.Sys_Default;
                WeiSha.Common.Param.Method.ConvertToAnyValue p = new WeiSha.Common.Param.Method.ConvertToAnyValue(val);
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
            List<SystemPara> list = WeiSha.Common.Cache<SystemPara>.Data.List;
            if (list == null) list = this.Refresh();
            if (list == null) return null;
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
        public DataTable GetAll()
        {
            DataSet ds = Gateway.Default.From<SystemPara>().OrderBy(SystemPara._.Sys_Key.Asc).ToDataSet();
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
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
        /// 测试是否完成授权
        /// </summary>
        public bool IsLicense()
        {
             WeiSha.Common.License lic = WeiSha.Common.License.Value;
             return lic.IsLicense;
        }

        /// <summary>
        /// 数据库完整性测试
        /// </summary>
        /// <returns>如果不缺少，则返回null；如果缺少，则返回"表名:字段"</returns>
        public List<string> DatabaseCompleteTest()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\\bin\\";
            string assemblyName = path + "Song.Entities.dll";               
            System.Reflection.Assembly assembly = Assembly.LoadFrom(assemblyName);  
            Type[] ts = assembly.GetTypes();  
            List<string> classList = new List<string>();
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
                    string fieldExist = "";
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
                        if (!isExist)
                        {
                            fieldExist += pi.Name + ",";
                        }
                    }
                    if (fieldExist != "")
                        classList.Add(t.Name + ":" + fieldExist);
                }
                catch
                {
                    classList.Add(t.Name + ":（缺少整个表）");
                }
            }
            return classList.Count < 1 ? null : classList;  
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
        #region IEnumerable 成员

        /// <summary>
        /// 实现代迭器的功能，可以引用时用foreach循环
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            //从缓存中读取
            List<SystemPara> list = WeiSha.Common.Cache<SystemPara>.Data.List;
            if (list == null) list = this.Refresh();
            for (int i = 0; i < list.Count; i++)
            {
                WeiSha.Common.Param.Method.ConvertToAnyValue p = new WeiSha.Common.Param.Method.ConvertToAnyValue();
                p.ParaKey = list[i].Sys_Key;
                p.ParaValue = list[i].Sys_Value;
                p.Unit = list[i].Sys_Unit;
                yield return p;
            }
        }

        #endregion
    }
}
