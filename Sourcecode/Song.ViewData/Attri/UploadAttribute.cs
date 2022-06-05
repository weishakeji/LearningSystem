using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 上传文件的预处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UploadAttribute : WeishaAttr
    {        
        private int _maxSize = 2048;
        /// <summary>
        /// 允许的最大文件大小，单位kb，默认2048
        /// </summary>
        public int MaxSize
        {
            get { return _maxSize; }
            set
            {
                _maxSize = value;
            }
        }
        /// <summary>
        /// 限定的扩展名，例如"jpg,gif,png"，用逗号分隔，不带.
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 不可为空，如果为true,没有上传文件时，抛出异常
        /// </summary>
        public bool CannotEmpty { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static bool Verify(MemberInfo method, Letter letter)
        {
            UploadAttribute attr = WeishaAttr.GetAttr<UploadAttribute>(method);
            //如果没有特性，则返回
            if (attr == null) return true;
            //如果要求必须有上传文件
            if (attr.CannotEmpty && letter.Files.Count < 1)
            {
                throw new Exception("该方法需要提供上传的文件");
            }
            //if (letter.Files.Count > 0)
            //{

            //}
            foreach (string key in letter.Files)
            {
                HttpPostedFileBase file = letter.Files[key];
                int len = file.ContentLength / 1024; //文件大小KB
                if (len > attr.MaxSize)
                {
                    throw new Exception(string.Format("文件：{0}，大于限定的{1}（实际大小{2})，被禁止上传", 
                        file.FileName, _clacSize(attr.MaxSize), _clacSize(len)));
                }
                //验证扩展名
                if (string.IsNullOrWhiteSpace(attr.Extension)) continue;
                string ext = Path.GetExtension(file.FileName).Replace(".", "");
                bool isexist = false;
                foreach (string s in attr.Extension.Split(','))
                {
                    if (ext.Equals(s.Replace(".", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        isexist = true;
                        break;
                    }
                }
                if (!isexist)
                {
                    throw new Exception(string.Format("仅限上传“{0}”文件", attr.Extension));
                }
            }
            return true;
        }
        /// <summary>
        /// 计算文件大小的单位输出
        /// </summary>
        /// <param name="size">文件大小，单位kb</param>
        /// <returns></returns>
        private static string _clacSize(int size)
        {
            if(size<1024)return size + "Kb";
            return  Math.Floor((double)size / 1024 * 100) / 100 + "Mb";
        }
    }
}
