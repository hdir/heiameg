using System;
using System.Collections;
using System.Windows.Input;
using HeiaMeg.Utils;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class ThemeGridView : Grid
    {
        public DataTemplate ItemTemplate
        {
            get => _itemTemplate;
            set
            {
                _itemTemplate = value;
                SetupView();
            }
        }

        public ThemeGridView()
        {
            SetupView();
        }

        private void SetupView()
        {
            Children.Clear();

            if (ItemsSource == null)
                return;

            var columns = Math.Max(1, ColumnDefinitions.Count);

            for (var i = 0; i < ItemsSource.Count; i++)
            {
                var x = i % columns;
                var y = i / columns;

                if ((ItemTemplate?.CreateContent() ?? new Grid()) is View view)
                {
                    var item = ItemsSource[i];
                    view.BindingContext = item;
                    view.AddTouch((sender, args) => ItemTapped?.Execute(item));
                    Children.Add(view, x, y);
                }
            }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(ThemeGridView),
                default(IList),
                propertyChanged:
                (bindableObject, oldValue, newValue) => { ((ThemeGridView) bindableObject).SetupView(); }
            );

        private DataTemplate _itemTemplate;

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTappedProperty =
            BindableProperty.Create(
                nameof(ItemTapped),
                typeof(ICommand),
                typeof(ThemeGridView),
                default(ICommand)
            );

        public ICommand ItemTapped
        {
            get => (ICommand) GetValue(ItemTappedProperty);
            set => SetValue(ItemTappedProperty, value);
        }

    }
}
