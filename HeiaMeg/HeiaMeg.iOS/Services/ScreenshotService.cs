using System;
using CoreGraphics;
using HeiaMeg.Services.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Rectangle = Xamarin.Forms.Rectangle;

[assembly: Dependency(typeof(HeiaMeg.iOS.Services.ScreenshotService))]
namespace HeiaMeg.iOS.Services
{
    public class ScreenshotService : IScreenshotService
    {
        public byte[] Capture()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;

            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsPNG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }

        public byte[] Capture(View formsView)
        {
            if (formsView.Bounds.Width <= 0)
                return new byte[0];

            var nativeView = ConvertFormsToNative(formsView, formsView.Bounds);

            UIGraphics.BeginImageContext(nativeView.Frame.Size);
            nativeView.DrawViewHierarchy(nativeView.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsPNG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }

        public static UIView ConvertFormsToNative(View view, Rectangle rect)
        {
            var renderer = Platform.CreateRenderer(view);

            renderer.NativeView.Frame = new CGRect(0, 0, rect.Width, rect.Height);

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout(rect);

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }

    }
}