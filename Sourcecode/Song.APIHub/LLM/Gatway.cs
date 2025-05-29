using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// 大语言模型（Large Language Model, LLM）
/// </summary>
namespace Song.APIHub.LLM
{
    public class Gatway
    {
        // 设置请求 URL 和内容
        static readonly string _api_url = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";

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
                        _api_key = "sk-149b79268cc54a42ae26ef92fd567453";

                    return _api_key;
                }
            }
        }

        private static string _api_model = "deepseek-v3";
        /// <summary>
        /// 设置大语言模型的名称。模型列表：https://help.aliyun.com/zh/model-studio/getting-started/models
        /// </summary>
        /// <param name="model"></param>
        public static void SetApiModel(string model) => _api_model = model;
        /// <summary>
        /// 获取大语言模型的名称
        /// </summary>
        public static string ApiModel => _api_model;

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
            jo.Add("model", _api_model);
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
            jo.Add("model", _api_model);
            jo.Add("stream", false);
            jo.Add("messages", messages);
            string result= _sendPostRequest(_api_url, jo.ToString(), APIKey);
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
            jo.Add("model", _api_model);
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
                    return $"请求失败: {response.StatusCode}";
                }
            }
        }
        #endregion
    }
}
