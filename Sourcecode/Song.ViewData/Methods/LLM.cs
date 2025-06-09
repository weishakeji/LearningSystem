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

        #region 设置相关参数
        //用于本地存储appkey的键值
        private static string _key = "LLM_aliyun_APIKey";

        /// <summary>
        /// 设置appkey
        /// </summary>
        [HttpPost, Admin]
        public bool SetAppkey(string apikey)
        {
            //保存
            Business.Do<ISystemPara>().Save(_key, apikey, true);
            //设置
            Song.APIHub.LLM.Gatway.SetApiKey(apikey);
            return true;
        }
        /// <summary>
        /// 获取appkey,来自数据库
        /// </summary>
        [HttpPost]
        public string GetApikey() => Business.Do<ISystemPara>().GetValue(_key);
        /// <summary>
        /// APIKey,可能来自本地存储，也可以来自web.config中设置的值
        /// </summary>
        public string APIKey() => Song.APIHub.LLM.Gatway.APIKey;
        #endregion

        #region 交流
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
        #endregion

        #region 交流记录
        /// <summary>
        /// 新增交流记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost,Student]
        public LlmRecords RecordsAdd(LlmRecords record)
        {
            Business.Do<ILargeLanguage>().RecordsAdd(record);
            return record;
        }
        /// <summary>
        /// 新增交流记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost, Student]
        public LlmRecords RecordsUpdate(LlmRecords record)
        {
            Song.Entities.LlmRecords old = Business.Do<ILargeLanguage>().RecordsSingle(record.Llr_ID);
            if (old == null) throw new Exception("Not found entity for LlmRecords！");

            old.Copy<Song.Entities.LlmRecords>(record);
            Business.Do<ILargeLanguage>().RecordsSave(old);
            return old;
        }
        /// <summary>
        /// 删除交流记录
        /// </summary>
        /// <param name="ids">交流信息的id</param>
        /// <returns></returns>
        [HttpDelete]
        public int RecordsDelete(string ids)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(ids)) return i;
            string[] arr = ids.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<ILargeLanguage>().RecordsDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;         
        }
        /// <summary>
        /// 删除学员所有交流信息
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool RecordsClear(int acid)
        {
            Business.Do<ILargeLanguage>().RecordsClear(acid);
            return true;
        }
        /// <summary>
        /// 学员所有交流记录
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public List<Song.Entities.LlmRecords> RecordsAll(int acid)
        {
            return Business.Do<ILargeLanguage>().RecordsAll(acid);
        }
        #endregion
    }
}
