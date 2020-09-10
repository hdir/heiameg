using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HeiaMeg.Utils;
using Microsoft.AppCenter.Crashes;

namespace HeiaMeg.Services
{
    public static class FeedbackUploader
    {
        public static async Task<bool> UploadFeedbackResponseAsync(int messageId, string feedbackText,
            IEnumerable<string> toggles)
        {
            var entries = new[]
            {
                new GoogleFormUploader.GoogleFormEntry(Config.Feedback.MessageFieldId, messageId),
                new GoogleFormUploader.GoogleFormEntry(Config.Feedback.ResponseFieldId, feedbackText),
                new GoogleFormUploader.GoogleFormEntry(Config.Feedback.ToggleFieldId, string.Join(", ", toggles)),
            };

            try
            {
                var response = await GoogleFormUploader.Upload(Config.Feedback.FormsId, entries);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }
    }
}
