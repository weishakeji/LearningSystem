using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Song.Site
{
    /// <summary>
    /// 将要重构的文件判断
    /// </summary>
    public class Reconsi
    {
        private static string _file = "Reconsitution.text";

        private static List<string> _files = null;
        public static List<string> Files
        {
            get
            {
                if (_files == null)
                {
                    string text = System.IO.File.ReadAllText(System.Environment.CurrentDirectory + _file, Encoding.UTF8);
                    _files = new List<string>();
                    StreamReader sr = new StreamReader(System.Environment.CurrentDirectory + _file, Encoding.Default);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        _files.Add(line);
                    }
                }
                return _files;
            }
        }
    }
}