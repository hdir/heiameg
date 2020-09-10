using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Skia
{
    public class MessageBubbleCanvasView : SKCanvasView
    {
        public enum MessageBubbleStyle
        {
            Default,
            Shrunk,
            Circle,
        }

        private readonly SKPaint _backgroundPaint = new SKPaint()
        {
            IsAntialias = true,
        };

        private bool _cornerIsDirty = true;
        public SKSize CornerSize { get; set; } = new SKSize(0, 0); 

        public MessageBubbleCanvasView()
        {
            _backgroundPaint.Color = Color.ToSKColor();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _cornerIsDirty = true;
            base.OnSizeAllocated(width, height);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            base.OnPaintSurface(args);

            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            if (_cornerIsDirty)
            {
                var scaledCornerRadius = CornerRadius / 100f * Math.Min(info.Height, info.Width);
                CornerSize = new SKSize(scaledCornerRadius, scaledCornerRadius);
                _cornerIsDirty = false;
            }

            switch (Style)
            {
                case MessageBubbleStyle.Default:
                    DrawDefault(info, canvas);
                    break;
                case MessageBubbleStyle.Shrunk:
                    DrawShrunk(info, canvas);
                    break;
                case MessageBubbleStyle.Circle:
                    DrawCircle(info, canvas);
                    break;
                default:
                    DrawDefault(info, canvas);
                    break;
            }

            canvas.Flush();
        }

        private void DrawDefault(SKImageInfo info, SKCanvas canvas)
        {
            var trianglePath = new SKPath();
            var paddingBottom = CornerSize.Height;

            trianglePath.MoveTo(0, info.Rect.Bottom - 2 * paddingBottom);
            trianglePath.LineTo(paddingBottom, info.Rect.Bottom - paddingBottom);
            trianglePath.LineTo(0, info.Rect.Bottom);
            trianglePath.Close();

            canvas.DrawPath(trianglePath, _backgroundPaint);
            canvas.DrawRoundRect(0, 0, info.Width, info.Height-paddingBottom, CornerSize.Width,CornerSize.Height, _backgroundPaint);
        }

        private void DrawShrunk(SKImageInfo info, SKCanvas canvas)
        {
            canvas.DrawRect(0, info.Rect.Bottom - CornerSize.Height, CornerSize.Width, CornerSize.Height, _backgroundPaint);
            canvas.DrawRoundRect(info.Rect, CornerSize, _backgroundPaint);
        }

        private void DrawCircle(SKImageInfo info, SKCanvas canvas)
        {
            var triangleHeight = info.Height * 0.08f;

            var circleRadius = Math.Min(info.Width, info.Height - triangleHeight * 2f) / 2f;

            var circleCenter = new SKPoint(info.Width / 2f, info.Height / 2f);

            const double a1 = 0.53f * Math.PI;
            const double a2 = 0.655f * Math.PI;

            var p1x = circleCenter.X + circleRadius * (float) Math.Cos(a1);
            var p1y = circleCenter.Y + circleRadius * (float) Math.Sin(a1);

            var p2x = circleCenter.X + circleRadius * (float) Math.Cos(a2);
            var p2y = circleCenter.Y + circleRadius * (float) Math.Sin(a2);

            var p3x = info.Width * 0.18f;
            var p3y = circleCenter.Y + circleRadius + triangleHeight;

            var trianglePath = new SKPath();
            trianglePath.MoveTo(p1x, p1y);
            trianglePath.LineTo(p2x, p2y);
            trianglePath.LineTo(p3x, p3y);
            trianglePath.Close();

            canvas.DrawPath(trianglePath, _backgroundPaint);
            canvas.DrawCircle(circleCenter, circleRadius, _backgroundPaint);
        }

        public new static readonly BindableProperty StyleProperty =
            BindableProperty.Create(
                nameof(Style),
                typeof(MessageBubbleStyle),
                typeof(MessageBubbleCanvasView),
                default(MessageBubbleStyle),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((MessageBubbleCanvasView) bindableObject).InvalidateSurface();
                }
            );

        public new MessageBubbleStyle Style
        {
            get => (MessageBubbleStyle) GetValue(StyleProperty);
            set => SetValue(StyleProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(float),
                typeof(MessageBubbleCanvasView),
                default(float),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((MessageBubbleCanvasView) bindableObject)._cornerIsDirty = true;
                    ((MessageBubbleCanvasView) bindableObject).InvalidateSurface();
                }
            );

        public float CornerRadius
        {
            get => (float) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(
                nameof(Color),
                typeof(Color),
                typeof(MessageBubbleCanvasView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((MessageBubbleCanvasView) bindableObject).ColorPropertyChanged((Color) oldValue, (Color) newValue);
                }
            );

        private void ColorPropertyChanged(Color oldValue, Color newValue)
        {
            _backgroundPaint.Color = newValue.ToSKColor();
            InvalidateSurface();
        }

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

    }
}
