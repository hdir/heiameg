using FFImageLoading.Svg.Forms;
using HeiaMeg.Resources;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class TitleView : ContentView
    {
        private readonly Label _lblTitle = new Label()
        {
            HorizontalOptions = LayoutOptions.StartAndExpand,
            VerticalOptions = LayoutOptions.End,
            VerticalTextAlignment = TextAlignment.End,
            FontAttributes = FontAttributes.Bold,
            FontSize = Sizes.Title,
            Margin = new Thickness(0, 0, 0, -5),
        };

        private readonly SvgCachedImage _icon  = new SvgCachedImage()
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.End,
            Aspect = Aspect.AspectFit,
            FadeAnimationEnabled = true,
        };

        public TitleView()
        {
            _lblTitle.Text = Title;
            _lblTitle.TextColor = TextColor;
            _icon.Source = ImageSource;

            HeightRequest = 54;
            Padding = new Thickness(20, 30, 20, 12);

            Content = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    _lblTitle,
                    _icon
                }
            };

            SizeChanged += (sender, args) =>
            {
                var size = Height - Padding.Top - Padding.Bottom;
                _icon.HeightRequest = size;
                _icon.WidthRequest = size;
            };
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(TitleView),
                default(string),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TitleView) bindableObject)._lblTitle.Text = (string) newValue;
                }
            );

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(TitleView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TitleView) bindableObject)._lblTitle.TextColor = (Color) newValue;
                }
            );

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(
                nameof(ImageSource),
                typeof(ImageSource),
                typeof(TitleView),
                default(ImageSource),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TitleView) bindableObject)._icon.Source = (ImageSource) newValue;
                }
            );

        public ImageSource ImageSource
        {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

    }
}
