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
        /// <summary>
        /// 模型引擎的名称
        /// </summary>
        public string Model() => Song.APIHub.LLM.Gatway.ApiModel;
        /// <summary>
        /// 咨询问题
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Consult(string message)
        {
            return Song.APIHub.LLM.Gatway.Consult(null, message);
        }
    }
}
