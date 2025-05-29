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
        /// 咨询问题，只处理一个问题
        /// </summary>
        /// <param name="character">AI的角色设定，可以为空</param>
        /// <param name="message">提出的问题</param>
        /// <returns></returns>
        public string Consult(string character, string message)
        {
            return Song.APIHub.LLM.Gatway.Consult(character, message);
        }
        /// <summary>
        /// 与AI交流问题，可以多轮沟通
        /// </summary>
        /// <param name="character">AI的角色设定，可以为空</param>
        /// <param name="messages">
        /// 由于AI并不存储沟通过程，这里是多轮沟通的内容。
        /// 例如：{"role", "user"},{"content", msg }
        /// </param>
        /// <returns></returns>
        public string Communion(string character, JArray messages)
        {
            return Song.APIHub.LLM.Gatway.Communion(character, messages);
        }
    }
}
