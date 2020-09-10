using System;
using System.Collections;
using System.Windows.Input;
using HeiaMeg.Resources;
using HeiaMeg.ViewControls.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.ViewControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsView : Grid
    {
        public EventHandler Tapped;

        public OptionsView()
        {
            InitializeComponent();
            SetupView();
        }

        private void SetupView()
        {
            Children.Clear();
            if (ItemsSource == null)
                return;

            var right = ColumnDefinitions.Count;

            Children.Add(Background(), right - ItemsSource.Count, right, 0, 1);

            var j = ItemsSource.Count - 1;
            for (var i = right-1; (i >= 0 && j >= 0); i--)
            {
                var view = OptionsButton();
                view.BindingContext = ItemsSource[j];
                view.CommandParameter = BindingContext;
                view.Tapped = Tapped;
                Children.Add(view, i, 0);

                if (j < ItemsSource.Count - 1)
                    Children.Add(Split(), i, 0);

                j--;
            }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(OptionsView),
                default(IList),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((OptionsView)bindableObject).SetupView();
                }
            );

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private OptionImageButton OptionsButton()
        {
            var view = new OptionImageButton(Sizes.OptionsHeight - 40)
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                TextColor = Colors.OptionsMessage,
            };
            view.SetBinding(OptionImageButton.TextProperty, nameof(OptionItemViewModel.ButtonText));
            view.SetBinding(OptionImageButton.ImageSourceProperty, nameof(OptionItemViewModel.ImageSource));
            view.SetBinding(OptionImageButton.CommandProperty, nameof(OptionItemViewModel.Command));

            return view;
        }

        private static View Split()
        {
            return new BoxView()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Fill,
                WidthRequest = 1,
                Color = Colors.MessageTitleSplit
            };
        }

        private static View Background()
        {
            return new Frame()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.White,
                Padding = 0,
                Margin = 0,
                HasShadow = true,
                CornerRadius = 20,
            };
        }
    }

    public class OptionItemViewModel
    {
        public string ButtonText { get; }
        public ImageSource ImageSource { get; }
        public ICommand Command { get; set; }

        public OptionItemViewModel(string buttonText, ImageSource imageSource, ICommand command)
        {
            ButtonText = buttonText;
            ImageSource = imageSource;
            Command = command;
        }
    }
}