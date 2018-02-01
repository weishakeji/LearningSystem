using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Song.Extend.Html
{
    public class HtmlInfo
    {
        private Context _Context;

        public Context Context
        {
            get { return _Context; }
            set { _Context = value; }
        }        
        #region 文件的结构
        public struct HtmlFileInfo
        {
            public string Name;
            public string Path;
            public string FullName;
        }
        public HtmlFileInfo File = new HtmlFileInfo();        
        #endregion
        private HtmlInfo()
        {
           
        }

        public HtmlInfo(string fullPath)
        {
            fullPath=System.Web.HttpContext.Current.Server.MapPath(fullPath);
            System.IO.FileInfo fi = new FileInfo(fullPath);
            this.File.FullName = fi.FullName;
            this.File.Path = fi.Directory.FullName;
            this.File.Name = fi.Name;
            this._Context = new Context();
            if (fi.Exists)
            {
                System.IO.StreamReader sr = new StreamReader(fullPath, System.Text.Encoding.UTF8);
                this._Context.Text = sr.ReadToEnd();
                sr.Close();               
            }
            else
            {
                throw new System.NotImplementedException();
            }

        }
        public HtmlInfo(string path, string file)
        {
            path = System.Web.HttpContext.Current.Server.MapPath(path);
            System.IO.FileInfo fi = new FileInfo(path+"\\"+file);
            this.File.FullName = fi.FullName;
            this.File.Path = fi.Directory.FullName;
            this.File.Name = fi.Name;
            this._Context = new Context();
            if (fi.Exists)
            {
                System.IO.StreamReader sr = new StreamReader(path + "\\" + file, System.Text.Encoding.UTF8);
                this._Context.Text = sr.ReadToEnd();
                sr.Close();
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }        
        /// <summary>
        /// 存储HTML数据；1为成功，0为失败
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            System.IO.FileInfo fi = new FileInfo(this.File.FullName);
            string attr = fi.Attributes.ToString();
            if (attr.IndexOf("ReadOnly") < 0)
            {
                System.IO.StreamWriter sw = new StreamWriter(this.File.FullName, false, System.Text.Encoding.UTF8);
                sw.Write(this._Context.Text);
                sw.Close();
                return 1;
            }
            else
            {
                return 0;
            }            
        }
        /// <summary>
        /// 存储HTML数据；将当前文件另存，路径不变
        /// </summary>
        /// <returns>1为成功，0为失败</returns>
        public int Save(string file)
        {
            string fullpath = this.File.Path +"\\"+ file;
            System.IO.FileInfo fi = new FileInfo(fullpath);
            string attr = fi.Attributes.ToString();
            if (attr.IndexOf("ReadOnly") < 0)
            {
                System.IO.StreamWriter sw = new StreamWriter(fullpath, false, System.Text.Encoding.UTF8);
                sw.Write(this._Context.Text);
                sw.Close();
                return 1;
            }
            else
            {
                return 0;
            }

        } 
        public int Save(string path,string file)
        {
            string fullpath = path + file;
            System.IO.FileInfo fi = new FileInfo(fullpath);
            string attr = fi.Attributes.ToString();
            if (attr.IndexOf("ReadOnly") < 0)
            {
                System.IO.StreamWriter sw = new StreamWriter(fullpath, false, System.Text.Encoding.UTF8);
                sw.Write(this._Context.Text);
                sw.Close();
                return 1;
            }
            else
            {
                return 0;
            }

        } 
    }
}
