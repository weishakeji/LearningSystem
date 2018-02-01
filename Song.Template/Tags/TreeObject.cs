using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Song.Template.Tags
{
    /// <summary>
    /// 树形结构数据
    /// </summary>
    public class TreeObject
    {       

        #region 属性
        public List<WeiSha.Data.Entity> DataSet { get; private set; }
        /// <summary>
        /// 当前对象
        /// </summary>
        public WeiSha.Data.Entity Current { get; private set; }
        /// <summary>
        /// 当前对象的子级对象
        /// </summary>
        public List<TreeObject> Childs { get; private set; }
        ///// <summary>
        ///// 当前对象的父级对象
        ///// </summary>
        //public WeiSha.Data.Entity Parent { get; private set; }
        ///// <summary>
        ///// 当前对象的父级集合
        ///// </summary>
        //public List<WeiSha.Data.Entity> Parents { get; private set; }       
        ///// <summary>
        ///// 第一个
        ///// </summary>
        //public TreeObject First { get; private set; }
        ///// <summary>
        ///// 最后一个
        ///// </summary>
        //public TreeObject Last { get; private set; }
        #endregion

        #region 方法
        /// <summary>
        /// 下一个
        /// </summary>
        /// <returns></returns>
        public TreeObject Next()
        {
            return null;
        }
        /// <summary>
        /// 上一个
        /// </summary>
        /// <returns></returns>
        public TreeObject Prev()
        {
            return null;
        }
        #endregion

        public TreeObject(List<WeiSha.Data.Entity> list, WeiSha.Data.Entity data)
        {
            this.DataSet = list;
            this.Current = data;
        }




        public static List<TreeObject> Bulder(WeiSha.Data.Entity[] data, string pidname,string pval, string idname)
        {
            if (data == null) return null;
            return Bulder(data.ToList<WeiSha.Data.Entity>(), pidname, pval, idname);
        }
        public static List<TreeObject> Bulder(List<WeiSha.Data.Entity> data, string pidname, string pval, string idname)
        {
            List<TreeObject> dataset = new List<TreeObject>();
            if (string.IsNullOrWhiteSpace(pval)) pval = "0";
            foreach (WeiSha.Data.Entity o in data)
            {
                string pid = GetObjectPropertyValue<WeiSha.Data.Entity>(o, pidname);
                if (pid == pval)
                    dataset.Add(new TreeObject(data, o));
            }
            foreach (TreeObject to in dataset)
            {
                BulderTree(to, pidname, idname);
            } 
            return dataset;
        }
        public static void BulderTree(TreeObject tob,string pidname,string idname)
        {
            //获取自身id
            string id = GetObjectPropertyValue<WeiSha.Data.Entity>(tob.Current, idname);
            //求下级子集
            List<TreeObject> list = new List<TreeObject>();
            foreach (WeiSha.Data.Entity to in tob.DataSet)
            {
                string pid = GetObjectPropertyValue<WeiSha.Data.Entity>(to, pidname);
                if (pid == id)
                {
                    list.Add(new TreeObject(tob.DataSet, to));
                }
            }
            tob.Childs = list;
            if (list.Count > 0)
            {
                foreach (TreeObject to in list)
                {
                    BulderTree(to, pidname, idname);
                }
            }

        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = t.GetType();
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }
    }
}
