using Moduit.Domain.Objects;
using Moduit.Utility;
using Moduit.Utility.Base;
using Moduit.Utility.ErrorHandler;
using Moduit.Utility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.CallApiHelper
{
    public class CallApiModuitHelper : BaseRestApiHelper
    {
        private HttpClient client;
        public CallApiModuitHelper(SolutionConfig config, HttpClient httpClient) : base(config)
        {
            this.client = httpClient;
            this.BaseUrl = Config.ModuitAPI.BaseUrl;
        }

        public Product GetProduct()
        {
            return this.CoreAPIInternalGetRequest<Product>("backend/question/one");
        }

        public IEnumerable<Product> GetProductsFiltered()
        {
            var products = this.CoreAPIInternalGetRequest<IEnumerable<Product>>("backend/question/two");
            var result = products.Where(n => n.Description.Contains("Ergonomic") || n.Title.Contains("Ergonomic")).ToList();
            result = result.Where(n => n.Tags != null && n.Tags.Contains("Sports")).OrderByDescending(x => x.Id).ToList();

            return result.Skip(Math.Max(0, result.Count() - 3));
        }

        public IEnumerable<Product> GetProductsFlatten()
        {
            var categories = this.CoreAPIInternalGetRequest<IEnumerable<ProductCategories>>("backend/question/three");
            categories = categories.Where(n => n.Id != 0 && n.Category != 0 && n.Items != null && n.CreatedAt.HasValue).ToList();

            List<Product> result = new List<Product>();
            foreach(var category in categories)
            {
                foreach(var item in category.Items)
                {
                    result.Add(new Product
                    {
                        Id = category.Id,
                        Category = category.Category,
                        Title = item.Title,
                        Description = item.Description,
                        Footer = item.Footer,
                        CreatedAt = category.CreatedAt
                    });
                }
            }

            return result;
        }

        public ResultType CoreAPIInternalGetRequest<ResultType>(string url, dynamic param = null, IDictionary<string, string> authProps = null)
        {
            return ObjectContextExceptionHandler.ExecuteHandler<ResultType>(
                ErrorMessageConstant.ModuitAPI,
                Path.Combine(BaseUrl, url),
                null,
                null,
                () => AuthGetRequest<ResultType>(url, null));
        }

        public override ResultType InternalGetRequest<ResultType>(
            string url,
            bool useToken,
            string json = null,
            Action<Exception, int> onFailureAttemp = null)
        {
            url = this.BaseUrl.AppendUrlPath(url);
            HttpResponseMessage response = null;
            var attempHelper = new ProcessAttempHelper();

            if (!string.IsNullOrEmpty(json))
            {
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                if (onFailureAttemp != null)
                {
                    response = attempHelper.ReattempException(() =>
                    {
                        var res = GetAsync(client, url, content).Result;

                        if (!res.IsSuccessStatusCode)
                        {
                            // for Logging if any
                            res.EnsureSuccessStatusCode();
                        }

                        return res;
                    }, Config.MinSleepTimeOnRetryErrorCallAPI, Config.MaxSleepTimeOnRetryErrorCallAPI, Config.NumberOfRetryFailure, onFailureAttemp);
                }
                else
                {
                    response = GetAsync(client, url, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        // for Logging if any
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            else
            {
                if (onFailureAttemp != null)
                {
                    response = attempHelper.ReattempException(() =>
                    {
                        var res = GetAsync(client, url).Result;
                        res.EnsureSuccessStatusCode();
                        return res;
                    }, Config.MinSleepTimeOnRetryErrorCallAPI, Config.MaxSleepTimeOnRetryErrorCallAPI, Config.NumberOfRetryFailure, onFailureAttemp);
                }
                else
                {
                    response = GetAsync(client, url).Result;
                    response.EnsureSuccessStatusCode();
                }
            }

            Stream receiveStream = response.Content.ReadAsStreamAsync().Result;
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string strResult = readStream.ReadToEnd();

            if (typeof(ResultType).IsValueType)
            {
                if (typeof(ResultType) == typeof(bool))
                {
                    return (ResultType)(object)(strResult.ToUpper() == "TRUE");
                }
                else if (typeof(ResultType) == typeof(int))
                {
                    return (ResultType)(object)(int.Parse(strResult));
                }
                return (ResultType)(object)strResult;
            }
            else if (typeof(ResultType) == typeof(string))
            {
                return (ResultType)(object)strResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ResultType>(strResult);
            }
        }
    }
}
