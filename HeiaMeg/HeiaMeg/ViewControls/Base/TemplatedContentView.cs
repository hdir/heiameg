using System;
using System.Threading.Tasks;
using Xamarin.Forms;

#if !DEBUG
using Microsoft.AppCenter.Crashes;
#endif

namespace HeiaMeg.ViewControls.Base
{
    public class TemplatedContentView : ContentView
    {
        private DataTemplateSelector _dataTemplateSelector;
        public DataTemplateSelector DataTemplateSelector
        {
            get => _dataTemplateSelector;
            set
            {
                _dataTemplateSelector = value;
                SetupView();
            }
        }

        public TemplatedContentView()
        {
            SetupView();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetupView();
        }

        private async void SetupView()
        {
            if (DataTemplateSelector == null)
                return;

            try
            {
                ViewIsBusy = true;

                var template = DataTemplateSelector.SelectTemplate(BindingContext, this);
                if (!(template?.CreateContent() is View view))
                    return;
                view.BindingContext = BindingContext;
                Content = view;
                await Task.Delay(10);
            }
            catch (Exception e)
            {
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#else
                Crashes.TrackError(e);
#endif
            }
            finally
            {
                ViewIsBusy = false;
            }
        }

        public static readonly BindableProperty ViewIsBusyProperty =
            BindableProperty.Create(
                nameof(ViewIsBusy),
                typeof(bool),
                typeof(TemplatedContentView),
                default(bool),
                BindingMode.OneWayToSource
            );

        public bool ViewIsBusy
        {
            get => (bool) GetValue(ViewIsBusyProperty);
            set => SetValue(ViewIsBusyProperty, value);
        }

    }
}
