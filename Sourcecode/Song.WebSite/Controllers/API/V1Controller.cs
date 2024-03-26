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
using System.Text;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// api接口调用，1.0版本
    /// </summary>
    public class V1Controller : ApiController
    {
        // GET api/values
        public System.Net.Http.HttpResponseMessage Get()
        {
            return GetInfo();
        }

        // POST api/values
        [HttpPost,HttpPut]        
        public System.Net.Http.HttpResponseMessage Post()
        {
            return GetInfo();
        }

        public System.Net.Http.HttpResponseMessage Patch()
        {
            return GetInfo();
        }
        public System.Net.Http.HttpResponseMessage Options()
        {
            return GetInfo();
        }
        // PUT api/values/5
        //public string Put(int id, [FromBody]string value)
        //{
        //    return GetInfo();
        //}
        public System.Net.Http.HttpResponseMessage Put(string value)
        {
            return GetInfo();
        }
        // DELETE api/values/5
        public System.Net.Http.HttpResponseMessage Delete()
        {
            return GetInfo();
        }

        private System.Net.Http.HttpResponseMessage GetInfo()
        {
            return new System.Net.Http.HttpResponseMessage
            {
                Content = new System.Net.Http.StringContent(GetInfo(string.Empty), Encoding.UTF8, "application/json"),
                StatusCode = System.Net.HttpStatusCode.OK
            };         
        }
        private string GetInfo(string id)
        {
            Song.ViewData.Letter letter = new Song.ViewData.Letter_v1(this.Request);
            DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(letter);
            string data = letter.ReturnType == "xml" ? result.ToXml() : result.ToJson();
            if (!letter.Encrypt) return data;
            return WeiSha.Core.DataConvert.EncryptForBase64(data);
        }
    }
}