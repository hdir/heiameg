using System;
using System.Windows.Input;
using FFImageLoading.Svg.Forms;
using HeiaMeg.Resources;
using HeiaMeg.Utils;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class OptionImageButton : ContentView
    {
        private readonly SvgCachedImage _icon;
        private readonly Label _lbl;

        public EventHandler Tapped;

        public OptionImageButton(double contentHeight)
        {
            _icon = new SvgCachedImage(){
                Source = ImageSource,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Aspect = Aspect.AspectFit
            };
            _lbl = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                Text = Text,
                TextColor = TextColor,
                FontSize = Sizes.TextSmall - 1,
            };

            Content = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                RowSpacing = 4,
                HeightRequest = contentHeight,
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition(){Height = GridLength.Star},
                    new RowDefinition(){Height = GridLength.Auto},
                },
                Children =
                {
                    {_icon, 0, 0},
                    {_lbl, 0, 1},
                }
            };
            this.AddTouch(async (sender, args) =>
            {
                Command?.Execute(this);
                await _icon.ScaleTo(0.85, 100u, Easing.SpringOut);
                await _icon.ScaleTo(1, 150u, Easing.SpringOut);
                Tapped?.Invoke(this, null);
            });
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(OptionImageButton),
                default(ICommand)
            );

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(
                nameof(ImageSource),
                typeof(SvgImageSource),
                typeof(OptionImageButton),
                default(SvgImageSource),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((OptionImageButton) bindableObject)._icon.Source = (SvgImageSource) newValue;
                }
            );

        public SvgImageSource ImageSource
        {
            get => (SvgImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(OptionImageButton),
                default(string),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((OptionImageButton) bindableObject)._lbl.Text = (string) newValue;
                }
            );

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(OptionImageButton),
                default(Color),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((OptionImageButton) bindableObject)._lbl.TextColor = (Color) newValue;
                }
            );

        public Color TextColor
        {
            get => (Color) GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(OptionImageButton)
            );

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

    }
}
