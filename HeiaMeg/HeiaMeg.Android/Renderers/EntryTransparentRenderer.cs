using Android.Content;
using Android.Graphics.Drawables;
using BareDu.Android.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryTransparentRenderer))]
namespace BareDu.Android.Renderers
{
    public class EntryTransparentRenderer : EntryRenderer
    {
        public EntryTransparentRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                Control.Background = gd;
                Control.SetPadding(0, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
            }
        }
    }
}
