using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RentLoanFinancialService.Utils
{
    public class WebApiInvoker
    {
        private readonly string _apiBaseAddress;

        public WebApiInvoker(string apiBaseAddress)
        {
            _apiBaseAddress = apiBaseAddress;
        }
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
    }
}
