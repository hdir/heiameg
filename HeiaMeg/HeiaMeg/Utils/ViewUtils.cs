using System;
using Xamarin.Forms;

namespace HeiaMeg.Utils
{
    public static class ViewUtils
    {
        public static (double X, double Y) GetPosition(this VisualElement view)
        {
            var (x, y) = (0.0, 0.0);
            do
            {
                x += view.X;
                y += view.Y;
                view = view.Parent as VisualElement;
            } while(view != null);

            return (x, y);
        }

        public static void AddTouch(this View view, EventHandler tapped)
        {
            var tgr = new TapGestureRecognizer();
            tgr.Tapped += tapped;
            view.GestureRecognizers.Add(tgr);
        }

        public static void AddTouchCommand(this View view, BindingBase command, object parameter = null)
        {
            var tgr = new TapGestureRecognizer();
            tgr.SetBinding(TapGestureRecognizer.CommandProperty, command);
            if (parameter != null)
                tgr.CommandParameter = parameter;
            view.GestureRecognizers.Add(tgr);
        }

        public static void AddTouchCommand(this Span span, BindingBase command, object parameter = null)
        {
            var tgr = new TapGestureRecognizer();
            tgr.SetBinding(TapGestureRecognizer.CommandProperty, command);
            if (parameter != null)
                tgr.CommandParameter = parameter;
            span.GestureRecognizers.Add(tgr);
        }
    }
}
