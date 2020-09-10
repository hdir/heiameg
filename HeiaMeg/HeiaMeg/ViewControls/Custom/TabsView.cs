using System;
using System.Collections;
using System.Collections.Specialized;
using HeiaMeg.Utils;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class TabsView : StackLayout
    {
        public event EventHandler ItemSelected;

        public TabsView()
        {
            Orientation = StackOrientation.Horizontal;
        }

        private void SetupView()
        {
            Children.Clear();

            if (ItemsSource == null)
                return;

            foreach (var item in ItemsSource)
            {
                if (!(ItemTemplate?.CreateContent() is View view))
                    continue;

                view.BindingContext = item;
                view.Opacity = 0;
                Children.Add(view);

                view.AddTouch(OnItemTapped);
            }

            foreach (var view in Children)
                view.FadeTo(1, 100);
        }

        private void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is View view)
                SelectedItem = view.BindingContext;
        }

        private DataTemplate _itemTemplate;
        public DataTemplate ItemTemplate
        {
            get => _itemTemplate;
            set
            {
                _itemTemplate = value;
                SetupView();
            }
        }


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(ICollection),
                typeof(TabsView),
                default(ICollection),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TabsView)bindableObject).ItemsSourcePropertyChanged((ICollection)oldValue, (ICollection)newValue);
                }
            );

        private void ItemsSourcePropertyChanged(ICollection oldValue, ICollection newValue)
        {
            if (oldValue != null && oldValue is INotifyCollectionChanged old)
            {
                old.CollectionChanged -= CollectionChanged;
            }

            if (newValue != null && newValue is INotifyCollectionChanged @new)
            {
                @new.CollectionChanged += CollectionChanged;
            }

            CollectionChanged(this, null);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetupView();
        }

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => (object) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(TabsView),
                default(object),
                BindingMode.TwoWay,
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TabsView) bindableObject).ItemSelectedPropertyChanged((object) oldValue, (object) newValue);
                }
            );

        private void ItemSelectedPropertyChanged(object oldValue, object newValue)
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}