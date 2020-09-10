using System;
using System.IO;
using System.Threading;
using Foundation;
using HeiaMeg.Services.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(HeiaMeg.iOS.Services.ShareImageService))]

namespace HeiaMeg.iOS.Services
{
    public class ShareImageService : IShareImageService
    {
        //NOTE: Must be called on UI Thread
        public void Show(string title, string message, byte[] imageData)
        {
            if (!NSThread.Current.IsMainThread)
            {
#if DEBUG
                throw new ThreadStateException("ShareImageService must be main-thread");
#else
                return;
#endif
            }

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "screenshot.png");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                stream.Write(imageData, 0, imageData.Length);
            }

            var items = new[] {NSObject.FromObject(title), NSUrl.FromFilename(path)};
            var activityController = new UIActivityViewController(items, null);
            var vc = GetVisibleViewController();

            vc.PresentViewController(activityController, true, null);
        }

        private static UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            switch (rootController.PresentedViewController)
            {
                case null:
                    return rootController;
                case UINavigationController controller:
                    return controller.TopViewController;
                case UITabBarController controller:
                    return controller.SelectedViewController;
                default:
                    return rootController.PresentedViewController;
            }
        }
    }
}