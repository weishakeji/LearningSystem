using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;
using System.Data;
using System.Threading.Tasks;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 大语言模型（Large Language Model, LLM）
    /// </summary>
    [HttpPost, HttpGet]
    public class LLM : ViewMethod, IViewAPI
    {
        public string Consult(string message)
        {
            string result = Song.APIHub.LLM.Gatway.Consult(null,message);
            return result;
        }
    }
}
