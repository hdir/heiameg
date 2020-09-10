using Android.Content;
using Android.Views;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]

namespace HeiaMeg.Droid.Renderers
{
    //NOTE FIX FOR https://github.com/xamarin/Xamarin.Forms/issues/5205 
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }
    }
}