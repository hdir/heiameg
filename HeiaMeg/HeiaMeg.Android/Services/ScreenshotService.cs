using System;
using System.IO;
using Android.Graphics;
using Android.Views;
using HeiaMeg.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Xamarin.Forms.View;

[assembly: Dependency(typeof(HeiaMeg.Droid.Services.ScreenshotService))]
namespace HeiaMeg.Droid.Services
{
    public class ScreenshotService : IScreenshotService
    {
        public byte[] Capture()
        {
            if (Android.App.Application.Context == null && Android.App.Application.Context.GetActivity() != null)
            {
#if DEBUG
                throw new Exception("You have to set ScreenshotService.Activity");
#else
                return new byte[0];
#endif
            }

            var view = Android.App.Application.Context.GetActivity().Window.DecorView;
#pragma warning disable 618
            view.DrawingCacheEnabled = true;
            var bitmap = view.GetDrawingCache(true);
#pragma warning restore 618

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }

        public byte[] Capture(View formsView)
        {
            if (formsView.Bounds.Width <= 0)
                return new byte[0];

            var nativeView = ConvertFormsToNative(formsView, formsView.Bounds);

            var bitmap = Bitmap.CreateBitmap(FdpToPix(formsView.Bounds.Width), FdpToPix(formsView.Bounds.Height), Bitmap.Config.Argb8888);
            var canvas = new Canvas(bitmap);
            nativeView.Draw(canvas);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }

        public static Android.Views.View ConvertFormsToNative(View view, Rectangle size)
        {
            var vRenderer = Platform.CreateRendererWithContext(view, Android.App.Application.Context);
            var viewGroup = vRenderer.View;
            vRenderer.Tracker.UpdateLayout();
            var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
            viewGroup.LayoutParameters = layoutParams;
            view.Layout(size);
            viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup;
        }

        public static int FdpToPix(double fdp)
        {
            var pixWidth = Android.App.Application.Context.Resources.DisplayMetrics.WidthPixels;
            var fdpWidth = (float)Application.Current.MainPage.Width;
            var pixPerDp = pixWidth / fdpWidth;
            return (int)(fdp * pixPerDp + 0.5f);
        }
    }
}