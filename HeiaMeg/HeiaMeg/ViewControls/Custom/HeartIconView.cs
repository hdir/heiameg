using System.Windows.Input;
using FFImageLoading.Svg.Forms;
using HeiaMeg.Resources;
using HeiaMeg.Utils;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class HeartIconView : ContentView
    {
        private readonly SvgCachedImage _icon;

        private bool _isAnimating = false;

        public HeartIconView()
        {
            // add larger touch-area
            Margin = new Thickness(-10);
            Padding = new Thickness(10);

            _icon = new SvgCachedImage()
            {
                HeightRequest = 25,
                WidthRequest = 25,
                Aspect = Aspect.AspectFit,
                Source = IsActive ? Images.FavoriteFill : Images.Favorite,
                FadeAnimationEnabled = true,
                FadeAnimationForCachedImages = true,
                FadeAnimationDuration = 150,
            };

            Content = new Grid()
            {
                Children =
                {
                    _icon,
                }
            };

            this.AddTouch(async (sender, args) =>
            {
                if (_isAnimating)
                    return;

                _isAnimating = true;

                if (!IsActive)
                {
                    IsActive = true;
                    await _icon.ScaleTo(.7f, 100u, Easing.SinInOut);
                    await _icon.ScaleTo(1f, 150u, Easing.SpringOut);
                }
                else
                {
                    IsActive = false;
                    await _icon.ScaleTo(.7f, 100u, Easing.SinOut);
                    await _icon.ScaleTo(1f, 150u, Easing.SpringOut);
                    _isAnimating = false;
                }
                ClickedCommand?.Execute(null);
            });
        }

        public static readonly BindableProperty IsActiveProperty =
            BindableProperty.Create(
                nameof(IsActive),
                typeof(bool),
                typeof(HeartIconView),
                default(bool),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((HeartIconView) bindableObject)._icon.Source = (bool)newValue ? Images.FavoriteFill : Images.Favorite;
                }
            );

        public bool IsActive
        {
            get => (bool) GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static readonly BindableProperty ClickedCommandProperty =
            BindableProperty.Create(
                nameof(ClickedCommand),
                typeof(ICommand),
                typeof(HeartIconView),
                default(ICommand)
            );

        public ICommand ClickedCommand
        {
            get => (ICommand) GetValue(ClickedCommandProperty);
            set => SetValue(ClickedCommandProperty, value);
        }

    }
}