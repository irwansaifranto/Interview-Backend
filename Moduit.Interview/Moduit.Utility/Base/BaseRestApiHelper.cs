using Moduit.Domain.Objects;
using Moduit.Utility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moduit.Utility.Base
{
    public abstract class BaseRestApiHelper
    {
        protected SolutionConfig Config { get; private set; }
        protected string BaseUrl { get; set; }
        protected BaseRestApiHelper(SolutionConfig config)
        {
            Config = config;
        }

        public ResultType AuthGetRequest<ResultType>(string url, dynamic param = null, Action<Exception, int> onFailureAttemp = null)
        {
            return this.InternalGetRequest<ResultType>(url, false, param, onFailureAttemp);
        }

        public virtual ResultType InternalGetRequest<ResultType>(
            string url,
            bool useToken,
            string json = null,
            Action<Exception, int> onFailureAttemp = null)
        {
            HttpClient client = new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = new TimeSpan(0, 0, 2),
                ConnectTimeout = new TimeSpan(0, 10, 0)
            });

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

            client.Dispose();

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

        protected async Task<HttpResponseMessage> GetAsync(
           HttpClient client,
           string url,
           HttpContent content = null)
        {
            Random random = new Random();
            int randomSleep = random.Next(Config.TimeoutMinCallApi, Config.TimeoutMaxCallApi);

            TimeSpan timeout = new TimeSpan(0, 0, 0, 0, randomSleep);

            using (var cts = new CancellationTokenSource(timeout))
            {
                var sendTask = client.GetAsync(url);
                while (true)
                {
                    if (sendTask.IsCompleted)
                        break;
                    else
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        Thread.Sleep(10);
                    }
                }
                return await sendTask;
            }
        }
    }
}
