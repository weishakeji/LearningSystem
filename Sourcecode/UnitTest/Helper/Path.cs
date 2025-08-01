using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UnitTest.Helper
{
    public class Path
    {
        /// <summary>
        /// 项目启动目录
        /// </summary>
        public static string StartupPath = System.AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 获取网站目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string WebSitePath()
        {
            DirectoryInfo dir = new DirectoryInfo(StartupPath);
            while (!_isExistPath(dir) && dir.Parent != null)
                dir = dir.Parent;
            return System.IO.Path.Combine(dir.FullName, "Song.WebSite");
        }
        /// <summary>
        /// 获取源码的文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static bool _isExistPath(DirectoryInfo dir)
        {
            foreach (DirectoryInfo item in dir.GetDirectories())
            {
                if (item.Name.Equals("Song.WebSite", StringComparison.CurrentCultureIgnoreCase))                
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 获取当前应用根目录的文件
        /// </summary>
        /// <returns></returns>
        public static string GetFilePath()
        {
            DirectoryInfo dir = new DirectoryInfo(StartupPath);
            while (!dir.Name.Equals("UnitTest", StringComparison.CurrentCultureIgnoreCase) && dir.Parent != null)
                dir = dir.Parent;
            return System.IO.Path.Combine(dir.FullName, "examresult.xml");
        }
        /// <summary>
        /// 获取Web站点下的文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetWebFilePath(string file)
        {
            string path = Helper.Path.WebSitePath();
            return System.IO.Path.Combine(path, file);
        }
    }
}
