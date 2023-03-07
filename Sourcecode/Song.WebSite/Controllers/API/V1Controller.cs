using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Song.ViewData;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// api接口调用，1.0版本
    /// </summary>
    public class V1Controller : ApiController
    {
        // GET api/values
        public string Get()
        {
            return GetInfo();
        }
        //public string Get(int id)
        //{
        //    return GetInfo();
        //}

        // POST api/values
        [HttpPost,HttpPut]        
        public string Post()
        {
            return GetInfo();
        }

        //public string Post()
        //{
        //    return GetInfo();
        //}
        public string Patch()
        {
            return GetInfo();
        }
        public string Options()
        {
            return GetInfo();
        }
        // PUT api/values/5
        //public string Put(int id, [FromBody]string value)
        //{
        //    return GetInfo();
        //}
        public string Put(string value)
        {
            return GetInfo();
        }
        // DELETE api/values/5
        public string Delete()
        {
            return GetInfo();
        }

        private string GetInfo()
        {
            return GetInfo(string.Empty);
        }
        private string GetInfo(string id)
        {
            Song.ViewData.Letter letter = new Song.ViewData.Letter(this.Request);
            DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(letter);
            string data = letter.ReturnType == "xml" ? result.ToXml() : result.ToJson();
            if (!letter.Encrypt) return data;
            return WeiSha.Core.DataConvert.EncryptForBase64(data);
        }
    }
}