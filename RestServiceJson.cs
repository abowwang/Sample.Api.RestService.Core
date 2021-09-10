using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace RestService
{
    public class RestServiceJson
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public String reqDomain = "";
        public int defaultTimeout {get;set;} = 60000;
        public Dictionary<string,string> fixedHeaders = new Dictionary<string, string>();

        public virtual RestServiceJson WithDomain(String request_domain)
        {
            this.reqDomain = request_domain;
            return this;
        }

        public T sendRequest<T>(string resource,string method, Dictionary<string,string>reqQuery,Dictionary<string,string>reqHeader,string body) where T : new()
        {
            T result = new T();

            try
            {
                String strURL = reqDomain + resource;
                
                bool isFst = true;
                foreach (KeyValuePair<string, string> entry in reqQuery ?? new Dictionary<string, string>())
                {
                    strURL += (isFst) ? "?" : "&";
                    strURL += entry.Key + "=" + entry.Value;
                    isFst = false;
                }
                
                WebRequest req = WebRequest.Create(strURL);
                req.Method = method;
                req.Timeout = defaultTimeout;
                req.ContentType = "application/json";
               
                foreach (KeyValuePair<string, string> entry in fixedHeaders)
                {
                    req.Headers.Add(entry.Key, entry.Value);
                }
                
                foreach (KeyValuePair<string, string> entry in reqHeader ?? new Dictionary<string, string>())
                {   
                    req.Headers.Add(entry.Key, entry.Value);
                }

                Encoding utf8 = Encoding.UTF8;
                if (!String.IsNullOrEmpty(body))
                {
                    byte[] httpBody = utf8.GetBytes(body);
                    req.ContentLength = httpBody.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(httpBody, 0, httpBody.Length);
                }
                
                printrRequestLog(new RestLogModel(){
                    method = req.Method,
                    URL = req.RequestUri.AbsolutePath,
                    headers = req.Headers.AllKeys.Select(k=>$"{k}:{req.Headers[k]}").ToList(),
                    body = body
                });

                using (WebResponse resp = req.GetResponse())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        String retValue = reader.ReadToEnd();
                        result = JsonSerializer.Deserialize<T>(retValue,new JsonSerializerOptions{IgnoreNullValues = true});
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request exception error : {e.Message}");
            }
            return result;
        }


        public async Task<T> sendRequestAsync<T>(string resource,string method, Dictionary<string,string>reqQuery,Dictionary<string,string>reqHeader,string body) where T : new()
        {
            T result = new T();

            try
            {
                String strURL = reqDomain + resource;
                
                bool isFst = true;
                foreach (KeyValuePair<string, string> entry in reqQuery ?? new Dictionary<string, string>())
                {
                    strURL += (isFst) ? "?" : "&";
                    strURL += entry.Key + "=" + entry.Value;
                    isFst = false;
                }

                WebRequest req = WebRequest.Create(strURL);
                req.Method = method;
                req.Timeout = defaultTimeout;
                req.ContentType = "application/json";

                foreach (KeyValuePair<string, string> entry in fixedHeaders)
                {
                    req.Headers.Add(entry.Key, entry.Value);
                }

                foreach (KeyValuePair<string, string> entry in reqHeader ?? new Dictionary<string, string>())
                {
                    req.Headers.Add(entry.Key, entry.Value);
                }

                Encoding utf8 = Encoding.UTF8;
                if (!String.IsNullOrEmpty(body))
                {
                    byte[] httpBody = utf8.GetBytes(body);
                    req.ContentLength = httpBody.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(httpBody, 0, httpBody.Length);
                }

                printrRequestLog(new RestLogModel(){
                    method = req.Method,
                    URL = req.RequestUri.AbsolutePath,
                    headers = req.Headers.AllKeys.Select(k=>$"{k}:{req.Headers[k]}").ToList(),
                    body = body
                });

                using (WebResponse resp = await req.GetResponseAsync())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        String retValue = reader.ReadToEnd();
                        result = JsonSerializer.Deserialize<T>(retValue,new JsonSerializerOptions{IgnoreNullValues = true});
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request exception error : {e.Message}");
            }
            return result;
        }

        public async Task<string> sendRequestAsync(string resource,string method, Dictionary<string,string>reqQuery,Dictionary<string,string>reqHeader,string body)
        {
            string result = "";
            try
            {
                String strURL = reqDomain + resource;
                
                bool isFst = true;
                foreach (KeyValuePair<string, string> entry in reqQuery ?? new Dictionary<string, string>())
                {
                    strURL += (isFst) ? "?" : "&";
                    strURL += entry.Key + "=" + entry.Value;
                    isFst = false;
                }
                
                WebRequest req = WebRequest.Create(strURL);
                req.Method = method;
                req.Timeout = defaultTimeout;
                req.ContentType = "application/json";

                foreach (KeyValuePair<string, string> entry in fixedHeaders)
                {
                    req.Headers.Add(entry.Key, entry.Value);
                }

                foreach (KeyValuePair<string, string> entry in reqHeader ?? new Dictionary<string, string>())
                {
                    req.Headers.Add(entry.Key, entry.Value);
                }

                Encoding utf8 = Encoding.UTF8;
                if (!String.IsNullOrEmpty(body))
                {
                    byte[] httpBody = utf8.GetBytes(body);
                    req.ContentLength = httpBody.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(httpBody, 0, httpBody.Length);
                }

                printrRequestLog(new RestLogModel(){
                    method = req.Method,
                    URL = req.RequestUri.AbsolutePath,
                    headers = req.Headers.AllKeys.Select(k=>$"{k}:{req.Headers[k]}").ToList(),
                    body = body
                });

                using (WebResponse resp = await req.GetResponseAsync())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request exception error : {e.Message}");
            }
            return result;
        }

        public class RestLogModel{
            public string method {get;set;} = "";  
            public string URL {get;set;} = "";
            public List<string> headers {get;set;} = new List<string>();  
            public string body {get;set;} = "";
            public string message {get;set;} = "";
        }

        public void printrRequestLog(RestLogModel req){

            logger.Trace(ConvertObj2String<RestLogModel>(req));
        }

        public string ConvertObj2String<T>(T input)
        {
            return System.Text.Json.JsonSerializer.Serialize(input, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }
    }
}
