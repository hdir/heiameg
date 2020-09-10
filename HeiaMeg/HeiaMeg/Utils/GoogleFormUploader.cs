using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeiaMeg.Utils
{
    public class GoogleFormUploader
    {
        private static HttpClient CreateClient() => new HttpClient { Timeout = new TimeSpan(0, 0, 10) };

        private const string BaseUrl = "https://docs.google.com/forms/d/e";

        /// <summary>
        /// Upload entries to a google form.
        ///
        /// Throws exception
        /// </summary>
        /// <param name="formsId">string id of form</param>
        /// <param name="entries">entries to fill in</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Upload(string formsId, GoogleFormEntry[] entries)
        {
            using (var client = CreateClient())
            {
                try
                {
                    var url = $"{BaseUrl}/{formsId}/formResponse?";
                    url = entries.Aggregate(url, (current, entry) => current + $"&entry.{entry.EntryId}={entry.Text}");

                    return await client.PostAsync(url, null);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public class GoogleFormEntry
        {
            public uint EntryId { get; }
            public object Text { get; set; }

            public GoogleFormEntry(uint entryId)
            {
                EntryId = entryId;
            }

            public GoogleFormEntry(uint entryId, object text)
            {
                EntryId = entryId;
                Text = text;
            }
        }
    }
}