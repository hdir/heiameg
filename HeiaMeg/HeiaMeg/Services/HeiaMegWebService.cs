using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HeiaMeg.Models;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace HeiaMeg.Services
{
    public class HeiaMegWebService
    {
        private readonly Paths _paths;

        private static readonly Lazy<HttpClient> _lazyHttpClient =
            new Lazy<HttpClient>(() => new HttpClient()
            {
                Timeout = new TimeSpan(0, 0, 10)
            });

        public static HttpClient HttpClient => _lazyHttpClient.Value;

        /// <summary>
        /// Creates a default HTTPClient with 20 second timeout
        /// </summary>
        /// <param name="url">xms url</param>
        /// <param name="apiKey">xms api key</param>
        public HeiaMegWebService(string url, string apiKey)
        {
            if (!url.EndsWith("/"))
                url += "/";

            _paths = new Paths(url, apiKey);
        }

        public async Task<ServerMeta> GetMetaAsync(Action<Exception> failure = null)
        {
            return await RequestAsync<ServerMeta>(_paths.Config, failure);
        }

        public async Task<List<Message>> GetMessagesAsync(Action<Exception> failure = null)
        {
            return await RequestAsync<List<Message>>(_paths.Messages, failure);
        }

        public async Task<Message[]> GetMessagesAfterAsync(DateTime date, Action<Exception> failure = null)
        {
            return await RequestAsync<Message[]>(_paths.MessagesAfter(date), failure);
        }

        public async Task<Message[]> GetModifiedMessagesAfterAsync(DateTime after, DateTime modifiedSince, Action<Exception> failure = null)
        {
            return await RequestAsync<Message[]>(_paths.MessagesAfterAndModifiedSince(after, modifiedSince), failure);
        }

        private static async Task<T> RequestAsync<T>(string path, Action<Exception> failure = null)
        {
            try
            {
                var response = await HttpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<T>(json, JsonSettings);
                    return model;
                }

                throw new HttpRequestException($"{response.ReasonPhrase}",
                    new WebException($"{response.ReasonPhrase}", WebExceptionStatus.ReceiveFailure));
            }
            catch (Exception e)
            {
                failure?.Invoke(e);
                return default;
            }
        }

        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Culture = CultureInfo.InvariantCulture,
            Error = (sender, args) => { Crashes.TrackError(args.ErrorContext.Error); }
        };

        private class Paths
        {
            private string BasePath { get; }
            private string ApiKey { get; }

            public Paths(string basePath, string apiKey)
            {
                BasePath = basePath;
                ApiKey = apiKey;
            }

            private string Path(string localPath = "", params string[] args) =>
                args != null
                    ? args.Aggregate($"{BasePath}api/{localPath}?apikey={ApiKey}", (current, arg) => current + $"&{arg}")
                    : $"{BasePath}api/{localPath}?apikey={ApiKey}";

            public string Config => Path();

            public string Themes => Path("themes");

            public string Messages => Path("messages");

            public string MessagesAfter(DateTime date) => Path("messages", $"since={date.Ticks}");
            
            public string MessagesAfterAndModifiedSince(DateTime after, DateTime modifiedSince) => Path("messages", $"since={after.Ticks}&modifiedSince={modifiedSince.Ticks}");

            public string ModifiedMessages(DateTime since) => Path("messages", $"modifiedSince={since.Ticks}");

            public string Message(int messageId) => Path($"messages/{messageId}");

            public string Content => Path("content");
        }
    }
}