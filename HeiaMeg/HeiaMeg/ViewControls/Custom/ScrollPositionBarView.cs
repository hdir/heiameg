using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class ScrollPositionBarView : ContentView
    {
        private readonly BoxView _current;
        private readonly BoxView _bg;

        public ScrollPositionBarView()
        {
            _current = new BoxView()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 3,
                CornerRadius = 2,
                Color = Color.Blue,
            };

            _bg = new BoxView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 1,
                Color = Color.Gray,
            };

            Content = new Grid()
            {
                Children =
                {
                    _bg,
                    _current,
                }
            };

            IsVisible = Count > 0;

            _bg.SizeChanged += (sender, args) =>
            {
                UpdateWidth();
            };
        }

        private void UpdateWidth()
        {
            if (Count != 0)
                _current.WidthRequest = (_bg.Width) / Count;
            else
                _current.WidthRequest = _bg.Width;
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private async void OnSelectedChanged()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Restart();
                return;
            }
            _stopwatch.Start();
            await this.FadeTo(1f);
            do
            {
                await Task.Delay(100);
            } while (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds < 1400);
            _stopwatch.Reset();
            await this.FadeTo(0f);
        }

        public static readonly BindableProperty CurrentProperty =
            BindableProperty.Create(
                nameof(Current),
                typeof(int),
                typeof(ScrollPositionBarView),
                default(int),
                propertyChanging:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ScrollPositionBarView) bindableObject).CurrentPropertyChanging((int) oldValue, (int) newValue);
                }
            );

        private async void CurrentPropertyChanging(int oldValue, int newValue)
        {
            await _current.TranslateTo(newValue * _current.Width, _current.Y, easing:Easing.CubicInOut);
            OnSelectedChanged();
        }

        public int Current
        {
            get => (int) GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }

        public static readonly BindableProperty CountProperty =
            BindableProperty.Create(
                nameof(Count),
                typeof(int),
                typeof(ScrollPositionBarView),
                default(int),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ScrollPositionBarView) bindableObject).CountPropertyChanged((int) oldValue, (int) newValue);
                }
            );

        private void CountPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0)
            {
                Count = 0;
                return;
            }

            IsVisible = newValue > 0;
            UpdateWidth();
        }

        public int Count
        {
            get => (int) GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static readonly BindableProperty BarColorProperty =
            BindableProperty.Create(
                nameof(BarColor),
                typeof(Color),
                typeof(ScrollPositionBarView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ScrollPositionBarView) bindableObject)._current.Color = (Color) newValue;
                }
            );

        public Color BarColor
        {
            get => (Color) GetValue(BarColorProperty);
            set => SetValue(BarColorProperty, value);
        }

        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(ScrollPositionBarView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ScrollPositionBarView) bindableObject)._bg.Color = (Color) newValue;
                }
            );

        public new Color BackgroundColor
        {
            get => (Color) GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
    }
}
