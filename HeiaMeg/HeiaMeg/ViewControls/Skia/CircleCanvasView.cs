using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Skia
{
    public class CircleCanvasView : SKCanvasView
    {
        private SKPaint CirclePaint { get; } = new SKPaint()
        {
            IsAntialias = true,
        };

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            base.OnPaintSurface(args);

            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            var circleRadius = Math.Min(info.Width, info.Height) / 2f;
            var circleCenter = new SKPoint(info.Width / 2f, info.Height / 2f);

            canvas.Clear();
            canvas.DrawCircle(circleCenter, circleRadius, CirclePaint);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(
                nameof(Color),
                typeof(Color),
                typeof(CircleCanvasView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((CircleCanvasView) bindableObject).CirclePaint.Color = ((Color) newValue).ToSKColor();
                    ((CircleCanvasView) bindableObject).InvalidateSurface();
                }
            );

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
    }
}
