using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OpsService.Common.ApiInvoker
{
    public class WebApiInvoker
    {
        private readonly string _apiBaseAddress;

        public WebApiInvoker(string apiBaseAddress)
        {
            _apiBaseAddress = apiBaseAddress;
        }

        /// <summary>
        /// GET请求,返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public Task<string> InvokeGetRequest(string api, Dictionary<string, string> arg)
        {
            using (var client = new HttpClient())
            {
                if (arg.Count > 0 && arg != null)
                {
                    api=api+"?";
                    foreach (var temp in arg)
                    {
                        api += temp.Key + "=" + temp.Value + "&";
                    }
                    api = api.Substring(0, api.Length - 1);
                }

                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<string>(() =>
                    {
                        return result;
                    });
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// GET请求,返回泛型对象
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public Task<TResult> InvokeGetRequest<TResult>(string api)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<TResult>(() =>
                    {
                        return JsonConvert.DeserializeObject<TResult>(result);
                    });
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Task<TResult> InvokePostRequest<TResult>(string controller, string action, Dictionary<string, string> arg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync(_apiBaseAddress + "api/" + controller + "/" + action, new FormUrlEncodedContent(arg)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<TResult>(() =>
                    {
                        return JsonConvert.DeserializeObject<TResult>(result);
                    });
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// POST请求,返回字符串
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">action名称</param>
        /// <param name="arg">参数</param>
        /// <returns></returns>
        public Task<string> InvokePostRequest(string controller, string action, Dictionary<string, string> arg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync(_apiBaseAddress + "/api/" + controller + "/" + action, new FormUrlEncodedContent(arg)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<string>(() =>
                    {
                        return result;
                    });
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// PUT请求,返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Task<string> InvokePutRequest(string api, Dictionary<string, string> arg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.PutAsync(api, new FormUrlEncodedContent(arg)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<string>(() =>
                    {
                        return result;
                    });
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// PUT请求,返回泛型对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="api"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Task<TResult> InvokePutRequest<TResult>(string api, Dictionary<string, string> arg)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.PutAsync(api, new FormUrlEncodedContent(arg)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    return Task.Factory.StartNew<TResult>(() =>
                    {
                        return JsonConvert.DeserializeObject<TResult>(result);
                    });                                       
                }
                throw new HttpResponseException(response);
            }
        }


        /// <summary>
        /// DELETE请求,返回泛型对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="api"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Task<TResult> InvokeDeleteRequest<TResult>(string api)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.DeleteAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<TResult>(() =>
                    {
                        return JsonConvert.DeserializeObject<TResult>(result);
                    });    
                }
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// DELETE请求,返回字符串
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="api"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Task<string> InvokeDeleteRequest(string api)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = client.DeleteAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    return Task.Factory.StartNew<string>(() =>
                    {
                        return result;
                    });    
                }
                throw new HttpResponseException(response);
            }
        }

    }
}