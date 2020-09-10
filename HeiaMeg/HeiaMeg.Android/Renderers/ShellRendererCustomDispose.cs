using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//BUG FIX https://github.com/xamarin/Xamarin.Forms/issues/6640
//TODO: remove with release of fix

[assembly: ExportRenderer(typeof(Shell), typeof(HeiaMeg.Droid.Renderers.ShellRendererCustomDispose))]
namespace HeiaMeg.Droid.Renderers
{
    public class ShellRendererCustomDispose : ShellRenderer
    {
        bool _disposed;

        public ShellRendererCustomDispose(Context context)
            : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Element.PropertyChanged -= OnElementPropertyChanged;
                Element.SizeChanged -= (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, "OnElementSizeChanged"); // OnElementSizeChanged is private, so use reflection
            }

            _disposed = true;
        }
    }
}
