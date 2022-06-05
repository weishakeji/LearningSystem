using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Helper
{
    public class Cache
    {
        public string Key { get; set; }
        public string Params { get; set; }
        public string Context { get; set; }
        public DateTime DateTime { get; set; }
    }
}
