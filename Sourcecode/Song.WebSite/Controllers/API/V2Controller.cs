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
    /// api接口调用，2.0版本
    /// </summary>
    public class V2Controller : ApiController
    {
        //public IHttpActionResult Get()
        //{
        //    //给当前进度一个ID
        //    long sessionid = WeiSha.Core.Request.SnowID();
        //    this.Request.Properties["SessionID"] = sessionid;
        //    System.Web.HttpContext.Current.Items["SessionID"] = sessionid;

        //    Song.ViewData.Letter letter = new Song.ViewData.Letter_v2(this.Request);
        //    //letter.SessionID = this.Request.Properties["SessionID"].ToString();
        //    LetterBox.Insert(sessionid, letter);

        //    DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(letter);
        //    if (!letter.Encrypt) return Ok(result);
           
        //    return Ok(WeiSha.Core.DataConvert.EncryptForBase64(result.ToString()));
        //}

        // GET api/values
        public System.Net.Http.HttpResponseMessage Get()
        {
            return GetInfo();
        }
        //public string Get(int id)
        //{
        //    return GetInfo();
        //}

        // POST api/values
        [HttpPost,HttpPut]        
        public System.Net.Http.HttpResponseMessage Post()
        {
            return GetInfo();
        }

        //public string Post()
        //{
        //    return GetInfo();
        //}
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
                StatusCode = System.Net.HttpStatusCode.OK,
                Version = new Version("2.0")
            };
            ////给当前进度一个ID
            //long sessionid = WeiSha.Core.Request.SnowID();
            //this.Request.Properties["SessionID"] = sessionid;
            //System.Web.HttpContext.Current.Items["SessionID"] = sessionid;

            //Song.ViewData.Letter letter = new Song.ViewData.Letter_v2(this.Request);
            //LetterBox.Insert(sessionid, letter);

            //DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(letter);
            //string resultdata = letter.ReturnType == "xml" ? result.ToXml() : result.ToJson();
            //if (!letter.Encrypt) resultdata = WeiSha.Core.DataConvert.EncryptForBase64(resultdata);

            //System.Net.Http.HttpResponseMessage httmsg = new HttpResponseMessage();
            //httmsg.Version = new Version("2.0");
            //httmsg.Content = new System.Net.Http.StringContent(resultdata);
            //if (result.Success) httmsg.StatusCode = System.Net.HttpStatusCode.OK;
            //else
            //    httmsg.StatusCode = System.Net.HttpStatusCode.NoContent;
            //return httmsg;
        }
        private string GetInfo(string id)
        {
            //给当前进度一个ID
            long sessionid= WeiSha.Core.Request.SnowID();
            this.Request.Properties["SessionID"] = sessionid;
            System.Web.HttpContext.Current.Items["SessionID"] = sessionid;

            Song.ViewData.Letter letter = new Song.ViewData.Letter_v2(this.Request);
            //letter.SessionID = this.Request.Properties["SessionID"].ToString();
            LetterBox.Insert(sessionid, letter);

            DataResult result = Song.ViewData.ExecuteMethod.ExecToResult(letter);
            string data = letter.ReturnType == "xml" ? result.ToXml() : result.ToJson();
            if (!letter.Encrypt) return data;
            return WeiSha.Core.DataConvert.EncryptForBase64(data);
        }
    }
}