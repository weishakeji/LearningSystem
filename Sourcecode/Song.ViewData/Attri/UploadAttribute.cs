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
        /// <summary>
        /// 允许的最大文件大小，单位kb，
        /// </summary>
        public int MaxSize { get; set; }
        /// <summary>
        /// 限定的扩展名，例如"jpg,gif,png"，用逗号分隔，不带.
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 不可为空，如果为true,没有上传文件时，抛出异常
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 配置项，
        /// </summary>
        public string Config { get; set; }

        #region 私有方法
        /// <summary>
        /// 扩展名集合（数组）
        /// </summary>
        private string[] _extensions = null;
        /// <summary>
        /// 禁止上传的扩展名
        /// </summary>
        private static string[] _prohibit = null;
        private static readonly object _obj = new object();
        /// <summary>
        /// 初始化
        /// </summary>
        private void _initial()
        {
            lock (_obj)
            {
                if (_prohibit == null)
                {
                    _prohibit = WeiSha.Core.FileUp.Attr("prohibit").Split(',');
                }
            }
            //
            WeiSha.Core.FileUp item = WeiSha.Core.FileUp.Item(this.Config);
            if (item == null) return;
            //如果没有在程序中设置，则取配置项的值
            if (string.IsNullOrEmpty(this.Extension))
            {
                this.Extension = item.Value;
                this._extensions = item.Extensions;
            }
            else
            {
                this._extensions = this.Extension.Split(',');
            }
            if (this.MaxSize <= 0) this.MaxSize = item.MaxSize;
            if (!this.Required) this.Required = item.Required;
        }
        /// <summary>
        /// 计算文件大小的单位输出
        /// </summary>
        /// <param name="size">文件大小，单位kb</param>
        /// <returns></returns>
        private static string _clacSize(int size)
        {
            if (size < 1024) return size + "Kb";
            return Math.Floor((double)size / 1024 * 100) / 100 + "Mb";
        }
       
        #endregion


        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static bool Verify(MemberInfo method, Letter letter)
        {
            UploadAttribute attr = WeishaAttr.GetAttr<UploadAttribute>(method);
            if (attr == null) return true;
            //初始化
            if (string.IsNullOrWhiteSpace(attr.Config)) attr.Config = $"{method.ReflectedType.Name}_{method.Name}";
            attr._initial();

            //如果要求必须有上传文件
            if (attr.Required && letter.Files.Count < 1) throw new Exception("该方法需要提供上传的文件");
            foreach (string key in letter.Files)
            {
                HttpPostedFileBase file = letter.Files[key];
                //校验文件大小
                if (attr.MaxSize > 0)
                {
                    int len = file.ContentLength / 1024; //文件大小KB
                    if (len > attr.MaxSize)
                        throw new Exception(string.Format("文件：{0}，大于限定的{1}（实际大小{2})，被禁止上传",
                            file.FileName, _clacSize(attr.MaxSize), _clacSize(len)));
                }
                string ext = Path.GetExtension(file.FileName).Replace(".", "");
                //验证禁用的扩展名
                bool prohibit = UploadAttribute._prohibit.Any(s => ext.Equals(s.Replace(".", ""), StringComparison.OrdinalIgnoreCase));
                if (prohibit) throw new Exception(string.Format("文件：{0}，禁止上传", file.FileName));
                //验证允许的扩展名
                if (string.IsNullOrWhiteSpace(attr.Extension) || attr._extensions.Length < 1) continue;                
                bool permitted = attr._extensions.Any(s => ext.Equals(s.Replace(".", ""), StringComparison.OrdinalIgnoreCase));
                if (!permitted) throw new Exception(string.Format("仅限上传“{0}”文件", attr.Extension));
            }
            return true;
        }

    }
}
