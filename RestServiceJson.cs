using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace RestService
{
    public class RestServiceJson
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public String reqDomain = "";
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
                req.Timeout = 10000;
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

                using (WebResponse resp = req.GetResponse())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        String retValue = reader.ReadToEnd();
                        logger.Debug($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                        logger.Debug($"request_output : {retValue}");
                        result = JsonSerializer.Deserialize<T>(retValue,new JsonSerializerOptions{IgnoreNullValues = true});
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                logger.Error($"Exception : {e.Message}");   
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
                req.Timeout = 10000;
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

                using (WebResponse resp = await req.GetResponseAsync())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        String retValue = reader.ReadToEnd();
                        logger.Debug($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                        logger.Debug($"request_output : {retValue}");
                        result = JsonSerializer.Deserialize<T>(retValue,new JsonSerializerOptions{IgnoreNullValues = true});
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                logger.Error($"Exception : {e.Message}");   
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
                req.Timeout = 10000;
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

                using (WebResponse resp = await req.GetResponseAsync())
                {
                    using (var reader = new StreamReader(resp.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        logger.Debug($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                        logger.Debug($"request_output : {result}");
                    }
                    resp.Close();
                }
            }
            catch(Exception e)
            {
                logger.Error($"request_input - url : {resource} , request_query : {ConvertDictionary2String(reqQuery)} , request_header : {ConvertDictionary2String(reqHeader)} , request_body : {body}");
                logger.Error($"Exception : {e.Message}");   
            }
            return result;
        }

        private string ConvertDictionary2String(Dictionary<string,string> input){
            return System.Text.Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(input));
        }
    }
}
