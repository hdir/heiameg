using System;
using System.Threading.Tasks;
using HeiaMeg.Pages.Modals;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Theme
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeItemView : ContentView
    {
        public ThemeItemView()
        {
            InitializeComponent();
            Opacity = 0;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width < 0)
                return;

            var size = Math.Min(width, height - LinkContainer.Height);


            CheckmarkView.HeightRequest = size * 0.25;
            CheckmarkView.WidthRequest = CheckmarkView.HeightRequest;

            var topMargin = (height * 0.5f - size * 0.5f);
            var rightMargin = (width * 0.5f - size * 0.5f);
            CheckmarkView.Margin = new Thickness(0, topMargin, rightMargin, 0);

            CheckmarkSvg.HeightRequest = CheckmarkView.HeightRequest * 0.4f;
            CheckmarkSvg.WidthRequest = CheckmarkSvg.HeightRequest;

            IconView.HeightRequest = size * 0.3f;
            IconView.WidthRequest = IconView.HeightRequest;

            TitleLabelView.FontSize = size * 0.1f;

            Console.WriteLine($"size: {(height - size)}");
            Opacity = 1;
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(ThemeItemView),
                default(bool),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ThemeItemView) bindableObject).IsSelectedPropertyChanged();
                }
            );

        private bool _animating;

        private async void IsSelectedPropertyChanged()
        {
            if (_animating)
                return;

            _animating = true;
            var start = IsSelected;

            if (IsSelected)
            {
                ForegroundView.Scale = BackgroundView.Scale;
                await ForegroundView.ScaleTo(0.92f, 100, easing: Easing.SpringOut);
                CheckmarkView.Scale = 0.8f;
                await Task.WhenAll(CheckmarkView.FadeTo(1, 80),
                    CheckmarkView.ScaleTo(1f, 100, Easing.SpringOut));
            }
            else
            {
                await Task.WhenAny(CheckmarkView.FadeTo(0, 60),
                    ForegroundView.ScaleTo(1f, 80));
            }
            _animating = false;

            if (start != IsSelected)
                IsSelectedPropertyChanged();
        }

        private void ReadMore_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ThemeDetailsPopupPage()
            {
                BindingContext = BindingContext
            });
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(ThemeItemView),
                default(string),
                propertyChanged:
                (bindableObject, oldValue, newValue) => { ((ThemeItemView) bindableObject).OnTextChanged(); }
            );

        private void OnTextChanged()
        {
            var words = Text.Split(' ');

            var text = "";
            for (var i = 0; i < words.Length; i++)
            {
                text += words[i];

                if (words.Length == 3 && i == 1)
                    text += Environment.NewLine;
                else if (words.Length == 4 && i == 2)
                    text += Environment.NewLine;
                else if (i < words.Length-1)
                    text += " ";
            }

            TitleLabelView.Text = text;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(ImageSource),
                typeof(ThemeItemView),
                default(ImageSource),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ThemeItemView) bindableObject).IconView.Source = (ImageSource) newValue;
                }
            );

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty CircleColorProperty =
            BindableProperty.Create(
                nameof(CircleColor),
                typeof(Color),
                typeof(ThemeItemView),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((ThemeItemView) bindableObject).ForegroundView.Color = (Color) newValue;
                }
            );

        public Color CircleColor
        {
            get => (Color) GetValue(CircleColorProperty);
            set => SetValue(CircleColorProperty, value);
        }

    }
}