using System;
using System.Threading.Tasks;

namespace HeiaMeg.Services
{
    public class DownloadManager
    {
        private static readonly HeiaMegWebService Client = new HeiaMegWebService(Config.ApiUrl, Config.ApiKey);

        public static async Task<IServerMeta> DownloadMetaAsync(Action<Exception> onError)
        {
            try
            {
                // Get server information
                return await Client.GetMetaAsync(onError);

            }
            catch (Exception e)
            {
                onError.Invoke(e);
                throw;
            }
        }

        public static async Task<IMessage[]> DownloadMessagesAsync(DateTime from, Action<Exception> onError)
        {
            try
            {
                // Get all messages in the future
                return await Client.GetMessagesAfterAsync(from, onError);
            }
            catch (Exception e)
            {
                onError.Invoke(e);
                return null;
            }
        }

        public static async Task<IMessage[]> DownloadModifiedMessagesAsync(DateTime from, DateTime modifiedSince, Action<Exception> onError)
        {
            try
            {
                // Get all messages in the future
                return await Client.GetModifiedMessagesAfterAsync(from, modifiedSince, onError);
            }
            catch (Exception e)
            {
                onError.Invoke(e);
                return null;
            }
        }
    }
}