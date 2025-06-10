using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Song.Entities;

/// <summary>
/// 大语言模型（Large Language Model, LLM）
/// </summary>
namespace Song.APIHub.LLM
{
    public class Gatway
    {
        // 设置请求 URL 和内容
        //static readonly string _api_url = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";
        static readonly string _api_url = WeiSha.Core.App.Get["LLM_aliyun_url"].String;

        #region API key
        private static string _api_key = "";
        private static readonly object _lock = new object();
        /// <summary>
        /// API Key
        /// </summary>
        public static void SetApiKey(string apikey) => _api_key = apikey;
        public static string APIKey
        {
            get
            {
                lock (_lock)
                {
                    if (string.IsNullOrWhiteSpace(_api_key))
                    {
                        SystemPara para = WeiSha.Core.Business.Do<Song.ServiceInterfaces.ISystemPara>().GetSingle("LLM_aliyun_APIKey");
                        if (para != null) _api_key = para.Sys_Value;
                        if (string.IsNullOrWhiteSpace(_api_key))
                            _api_key = WeiSha.Core.App.Get["LLM_aliyun_APIKey"].String;
                    }
                    return _api_key;
                }
            }
        }
        #endregion

        //模板的根路径
        private static string _llm_path = AppDomain.CurrentDomain.BaseDirectory + "Utilities\\LLM";
        /// <summary>
        /// LLM模型的配置项
        /// </summary>
        public static JObject AliyunConfiguration
        {
            get
            {
                string filepath = Path.Combine(_llm_path, "aliyun_model.json");
                if (!File.Exists(filepath)) return null;
                string json = File.ReadAllText(filepath);
                return JObject.Parse(json);
            }
        }

        #region 模型名称
        private static string _api_model = "";
        /// <summary>
        /// 设置大语言模型的名称。模型列表：https://help.aliyun.com/zh/model-studio/getting-started/models
        /// </summary>
        /// <param name="model"></param>
        public static void SetApiModel(string model) => _api_model = model;
        /// <summary>
        /// 获取大语言模型的代码
        /// </summary>
        public static string ModelCode
        {
            get
            {
                lock (_lock)
                {
                    if (string.IsNullOrWhiteSpace(_api_model))
                    {
                        SystemPara para = WeiSha.Core.Business.Do<Song.ServiceInterfaces.ISystemPara>().GetSingle("LLM_aliyun_model");
                        if (para != null) _api_model = para.Sys_Value;
                        if (string.IsNullOrWhiteSpace(_api_model))
                        {
                            if (AliyunConfiguration != null)
                            {
                                _api_model = AliyunConfiguration["default"].ToString();
                            }
                        }                          
                    }
                    return _api_model;
                }
            }
        }
        /// <summary>
        /// 获取大语言模型的名称
        /// </summary>
        public static string ModelName
        {
            get
            {
                string modelCode = ModelCode;
                string modelName = "";
                if (AliyunConfiguration != null)
                {
                    JArray jarr = (JArray)AliyunConfiguration["items"];
                    foreach (JObject obj in jarr)
                    {
                        string model = (string)obj["model"];
                        if (model == modelCode) return (string)obj["name"];                       
                    }
                }
                if (string.IsNullOrWhiteSpace(modelName)) return modelCode;
                return  modelName;
            }
        }
        #endregion

        #region 异步方法
        private static async Task<string> Exchange(string jsonContent)
        {
            return await _sendPostRequestAsync(_api_url, jsonContent, APIKey);
        }

        /// <summary>
        /// 咨询，只问一个问题
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<string> ConsultAsync(string message)
        {
            return await ConsultAsync(null, message);
        }
        /// <summary>
        /// 咨询，只问一个问题
        /// </summary>
        /// <param name="character">假定大语言模型的角色</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<string> ConsultAsync(string character, string message)
        {
            if (string.IsNullOrWhiteSpace(character))
                character = "You are a helpful assistant.";

            JArray messages = new JArray();
            messages.Add(new JObject(){
                    {"role", "system"},
                    {"content",character}
                });
            messages.Add(new JObject()
                {
                    {"role", "user"},
                    {"content", message}
                });
            JObject jo = new JObject();
            jo.Add("model", ModelCode);
            jo.Add("stream", false);
            jo.Add("messages", messages);
            return await _sendPostRequestAsync(_api_url, jo.ToString(), APIKey);
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonContent"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static async Task<string> _sendPostRequestAsync(string url, string jsonContent, string apiKey)
        {
            HttpClient httpClient = new HttpClient();
            using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
            {
                // 清除之前的请求头（因为HttpClient是静态的）
                httpClient.DefaultRequestHeaders.Clear();

                // 设置请求头
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // 发送请求并获取响应
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                // 处理响应
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"请求失败: {response.StatusCode}";
                }
            }
        }
        #endregion


        #region 同步方法
        /// <summary>
        /// 咨询，只问一个问题
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Consult(string message) => Consult(null, message);
        /// <summary>
        /// 咨询，只问一个问题
        /// </summary>
        /// <param name="character">假定大语言模型的角色</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Consult(string character, string message)
        {
            if (string.IsNullOrWhiteSpace(character))
                character = "You are a helpful assistant.";

            JArray messages = new JArray();
            messages.Add(new JObject(){
                    {"role", "system"},
                    {"content",character}
                });
            messages.Add(new JObject()
                {
                    {"role", "user"},
                    {"content", message}
                });
            JObject jo = new JObject();
            if (string.IsNullOrWhiteSpace(ModelCode)) throw new Exception("请设置大语言模型");
            jo.Add("model", ModelCode);
            jo.Add("stream", false);
            jo.Add("messages", messages);
            string result = _sendPostRequest(_api_url, jo.ToString(), APIKey);
            //解析为JSON对象
            dynamic json = JsonConvert.DeserializeObject(result);
            string content = (string)json["choices"][0]["message"]["content"];
            return content;
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
        public static string Communion(string character, JArray messages)
        {
            JObject jo = new JObject();
            if(string.IsNullOrWhiteSpace(ModelCode)) throw new Exception("请设置大语言模型");
            jo.Add("model", ModelCode);
            jo.Add("stream", false);
            jo.Add("messages", messages);
            string result = _sendPostRequest(_api_url, jo.ToString(), APIKey);
            //解析为JSON对象
            dynamic json = JsonConvert.DeserializeObject(result);
            string content = (string)json["choices"][0]["message"]["content"];
            return content;
        }
        /// <summary>
        /// 向云服务发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonContent"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static string _sendPostRequest(string url, string jsonContent, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new Exception("API密钥不能为空");

            HttpClient httpClient = new HttpClient();
            using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
            {
                // 清除之前的请求头（因为HttpClient是静态的）
                httpClient.DefaultRequestHeaders.Clear();

                // 设置请求头
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // 发送请求并获取响应
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                // 处理响应
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
        #endregion

        #region 相关模板内容       
        /// <summary>
        /// AI角色的文本模板
        /// </summary>
        /// <param name="path">模板所处的路径，相对于_template_path根路径</param>
        /// <returns></returns>
        public static string TemplateRole(string path) => TemplateText(path, "role.txt");
        /// <summary>
        /// 咨询内容的模板文本内容
        /// </summary>
        /// <param name="path">模板所处的路径，相对于_template_path根路径</param>
        /// <returns></returns>
        public static string TemplateMsg(string path) => TemplateText(path, "message.txt");
        /// <summary>
        /// 获取模板内容
        /// </summary>
        /// <param name="path">模板所处的路径，相对于_template_path根路径</param>
        /// <param name="file">模板文件名</param>
        /// <returns></returns>
        public static string TemplateText(string path, string file)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            path = path.Replace("/", "\\");
            string filepath = Path.Combine(_llm_path, path, file);
            if (File.Exists(filepath)) return File.ReadAllText(filepath);
            return string.Empty;
        }
        //正则表达式
        private static readonly Regex PlaceholderRegex = new Regex(@"\{\{(\w+)\}\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 模板处理
        /// </summary>
        /// <param name="text">模板文本</param>
        /// <param name="entities">数据实体</param>
        /// <returns></returns>
        public static string TemplateHandle(string text, params WeiSha.Data.Entity[] entities)
        {
            if (entities == null || entities.Length == 0) return text;
            foreach (var entity in entities) text = TemplateHandle(text, entity);
            return text;
        }
        /// <summary>
        ///  模板处理
        /// </summary>
        /// <param name="text">模板文本</param>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public static string TemplateHandle(string text, WeiSha.Data.Entity entity)
        {
            if (entity == null) return text;
            Dictionary<string, string> replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Type type = entity.GetType();
            // 获取所有公共属性
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(entity);
                replacements.Add(property.Name, value != null ? value.ToString() : string.Empty);
            }
            return _tempalte_handle(text, replacements);
        }
        /// <summary>
        /// 模版处理
        /// </summary>
        /// <param name="text">模板文本</param>
        /// <param name="tag">标签</param>
        /// <param name="val">要替换的值</param>
        /// <returns></returns>
        public static string TemplateHandle(string text, string tag, string val)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            replacements.Add(tag, val);
            return _tempalte_handle(text, replacements);
        }
        private static string _tempalte_handle(string text, Dictionary<string, string> replacements)
        {
            // 替换占位符
            string result = Regex.Replace(text, @"\{\{(\w+)\}\}", match =>
            {
                string key = match.Groups[1].Value;
                return replacements.TryGetValue(key, out string value) ? value : match.Value;
            });
            return result;
        }
        #endregion
    }
}
