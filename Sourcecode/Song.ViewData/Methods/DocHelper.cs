using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using Song.ViewData.Attri;


namespace Song.ViewData.Methods
{
    public class DocHelper : ViewMethod, IViewAPI
    {

        [HttpPost]
        [HtmlClear(Not = "html")]
        public JObject ToPDF(string html)
        {
            

            JObject jo = new JObject();


            return jo;
        }

        [HttpPost]
        [HtmlClear(Not = "html")]
        public JObject ToHtml(string html)
        {
            JObject jo = new JObject();


            return jo;
        }
    }
}
