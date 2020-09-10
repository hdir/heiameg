using System;
using HeiaMeg.Services.Interfaces;
using Xamarin.Forms;

namespace HeiaMeg.Services
{
    public class ShareManager
    {
        #region Singleton

        private static readonly Lazy<ShareManager> lazy =
            new Lazy<ShareManager>(() => new ShareManager());

        public static ShareManager Instance => lazy.Value;

        #endregion

        private readonly IScreenshotService _screenshotService;
        private readonly IShareImageService _shareService;

        private ShareManager()
        {
            _screenshotService = DependencyService.Get<IScreenshotService>();
            _shareService = DependencyService.Get<IShareImageService>();
        }

        public void ShareScreenshot(View view, string title = "", string text = "")
        {
            _shareService?.Show(title, text, _screenshotService.Capture(view));
        }
    }
}
