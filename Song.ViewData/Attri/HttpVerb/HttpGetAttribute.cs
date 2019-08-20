using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    public class HttpGetAttribute : Attribute
    {
        private bool _isAllow = true;
        public bool IsAllow
        {
            get { return _isAllow; }
            set { _isAllow = value; }
        }
        public HttpGetAttribute()
        {

        }
        public HttpGetAttribute(bool isallow)
        {
            _isAllow = isallow;
        }

        public string Say()
        {
            return "这是一个测试用的属性";
        }
    }
}
