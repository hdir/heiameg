using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using HeiaMeg.ViewModels.Items.Message;
using PanCardView;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{

    public class FilteredHorizontalListView : CoverFlowView
    {
        private static readonly Func<MessageViewModel, bool> _filter = model => true;

        public static readonly BindableProperty FilterProperty =
            BindableProperty.Create(
                nameof(Filter),
                typeof(Func<MessageViewModel, bool>),
                typeof(FilteredHorizontalListView),
                (_filter)
            );

        public Func<MessageViewModel, bool> Filter
        {
            get => (Func<MessageViewModel, bool>)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public new static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(ICollection),
                typeof(FilteredHorizontalListView),
                default(ICollection),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((FilteredHorizontalListView)bindableObject).ItemsSourcePropertyChanged((ICollection)oldValue, (ICollection)newValue);
                }
            );

        private void ItemsSourcePropertyChanged(ICollection oldValue, ICollection newValue)
        {
            if (oldValue != null && oldValue is INotifyCollectionChanged old)
            {
                old.CollectionChanged -= CollectionChanged;
                foreach (var item in oldValue)
                {
                    if (item is INotifyPropertyChanged notify)
                        notify.PropertyChanged -= OnItemPropertyChanged;
                }
            }

            if (newValue != null && newValue is INotifyCollectionChanged @new)
            {
                @new.CollectionChanged += CollectionChanged;
            }

            CollectionChanged(this, null);
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CollectionChanged(null, null);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e != null)
            {
                if (e.OldItems != null)
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        if (oldItem is INotifyPropertyChanged notify)
                            notify.PropertyChanged -= OnItemPropertyChanged;
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var newItem in e.NewItems)
                    {
                        if (newItem is INotifyPropertyChanged notify)
                            notify.PropertyChanged += OnItemPropertyChanged;
                    }
                }
            }

            if (ItemsSource is Collection<MessageViewModel> collection)
            {
                var favorited = collection.Where(Filter).ToList();
                base.ItemsSource = favorited;
                ItemSourceCount = favorited.Count;
                base.OnPropertyChanged(nameof(ListView.ItemsSource));
            }
        }

        public new ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemSourceCountProperty =
            BindableProperty.Create(
                nameof(ItemSourceCount),
                typeof(int),
                typeof(FilteredHorizontalListView),
                default(int),
                BindingMode.OneWayToSource
            );

        public int ItemSourceCount
        {
            get => (int) GetValue(ItemSourceCountProperty);
            set => SetValue(ItemSourceCountProperty, value);
        }
    }
}
