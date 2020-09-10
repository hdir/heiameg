using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Services;
using HeiaMeg.ViewModels.Base;
using HeiaMeg.ViewModels.Items.Message;
using Xamarin.Forms;

#if !DEBUG
using Microsoft.AppCenter.Crashes;
#endif

namespace HeiaMeg.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private static TaskCompletionSource<object> _loadingTask;

        public static ObservableRangeCollection<MessageViewModel> Messages { get; } = new ObservableRangeCollection<MessageViewModel>();

        public MainViewModel()
        {
            LoadMessagesAsync().ConfigureAwait(false);
        }

        public static Task LoadMessagesAsync()
        {
            if (_loadingTask != null && !_loadingTask.Task.IsCompleted)
                return _loadingTask.Task;

            if (_loadingTask == null)
                _loadingTask = new TaskCompletionSource<object>();

            if (_loadingTask.Task.IsCompleted)
                _loadingTask = new TaskCompletionSource<object>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (App.IsForeground)
                    {
                        var query = (await StorageService.Instance.GetReleasedMessagesAsync())
                            .OrderByDescending(t => t.NotifyTime)
                            .Where(m => m.IsValid()) // double check that message is valid
                            .ToList();

                        var viewModels = query.Select(message => new MessageViewModel(message)).ToList();

                        // check if updated list has any changes
                        if (viewModels.Except(Messages).Any())
                        {
                            Messages.Clear();
                            Messages.AddRange(viewModels);

                            // mark messages as opened
                            foreach (var message in query.Where(m => !m.Opened.HasValue))
                            {
                                message.Opened = DateTime.Now;
                                await StorageService.Instance.UpdateMessageAsync(message);
                            }
                        }
                    }
                    _loadingTask.SetResult(null);
                }
                catch (Exception e)
                {
#if DEBUG
                    Device.BeginInvokeOnMainThread(() => throw e);
                    _loadingTask.SetException(e);
#else
                    Crashes.TrackError(e);
#endif
                }
            });
            return _loadingTask.Task;
        }
    }
}
