using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using HeiaMeg.Utils;
using HeiaMeg.ViewModels.Items.Message;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Base
{
    public class TemplatedListContentView : ContentView
    {
        private DataTemplate _emptyTemplate = new DataTemplate(() => new Grid() { BackgroundColor = Color.Green });
        public DataTemplate EmptyTemplate
        {
            get => _emptyTemplate;
            set
            {
                _emptyTemplate = value;
                SetupView();
            }
        }

        private DataTemplate _listViewTemplate = new DataTemplate(() => new Grid() { BackgroundColor = Color.Purple });
        public DataTemplate ListViewTemplate
        {
            get => _listViewTemplate;
            set
            {
                _listViewTemplate = value;
                SetupView();
            }
        }

        public TemplatedListContentView()
        {
            SetupView();
        }

        private DataTemplate _selected;

        private void SetupView()
        {
            var items = ItemsSource;

            if (ItemsSource is Collection<MessageViewModel> collection)
                items = collection.Where(Filter).ToList();

            var isEmpty = items.IsNullOrEmpty();
            var template = isEmpty ? EmptyTemplate : ListViewTemplate;

            if (template == _selected)
                return;

            Content = template.CreateContent() as View;
            _selected = template;
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(ICollection),
                typeof(TemplatedListContentView),
                default(ICollection),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TemplatedListContentView)bindableObject).ItemsSourcePropertyChanged((ICollection)oldValue, (ICollection)newValue);
                }
            );

        private void ItemsSourcePropertyChanged(ICollection oldValue, ICollection newValue)
        {
            if (oldValue != null && oldValue is INotifyCollectionChanged old)
            {
                foreach (var item in oldValue)
                {
                    if (item is INotifyPropertyChanged notify)
                        notify.PropertyChanged -= OnItemPropertyChanged;
                }
                old.CollectionChanged -= CollectionChanged;
            }
            if (newValue != null && newValue is INotifyCollectionChanged @new)
                @new.CollectionChanged += CollectionChanged;

            CollectionChanged(this, null);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null)
                return;

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
                foreach (var newITem in e.NewItems)
                {
                    if (newITem is INotifyPropertyChanged notify)
                        notify.PropertyChanged += OnItemPropertyChanged;
                }
            }
            SetupView();
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetupView();
        }

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static readonly Func<MessageViewModel, bool> _filter = model => true;

        public static readonly BindableProperty FilterProperty =
            BindableProperty.Create(
                nameof(Filter),
                typeof(Func<MessageViewModel, bool>),
                typeof(TemplatedListContentView),
                _filter,
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((TemplatedListContentView)bindableObject).SetupView();
                }
            );

        public Func<MessageViewModel, bool> Filter
        {
            get => (Func<MessageViewModel, bool>)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

    }
}
