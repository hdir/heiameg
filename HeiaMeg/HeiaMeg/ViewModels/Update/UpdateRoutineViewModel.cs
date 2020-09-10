using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HeiaMeg.Services;
using HeiaMeg.ViewModels.Base;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Update
{
    public class UpdateRoutineViewModel : ViewModel
    {
        private Status _status = App.UpdateRoutine.Status;
        public Status Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public bool StatusFailed => Status == Status.Failed;
        public bool StatusRunning => Status == Status.Running;
        public bool StatusComplete => Status == Status.Completed;

        public ObservableCollection<StepViewModel> Steps { get; } = new ObservableCollection<StepViewModel>();

        public ICommand RetryCommand { get; } = new Command(async () =>
        {
            if (await App.UpdateRoutine.RunAsyncTask() == Result.Completed)
            {
                UserSettings.LastUpdateDate = DateTime.Now;
                await MainViewModel.LoadMessagesAsync();
            }
        });

        public UpdateRoutineViewModel()
        {
            App.UpdateRoutine.StatusChanged += status =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Status = status;
                    switch (status)
                    {
                        case Status.Waiting:
                            Steps.Clear();
                            break;
                        case Status.Running:
                            Steps.Clear();
                            break;
                        case Status.Failed:
                            break;
                        case Status.Completed:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(status), status, null);
                    }
                });
            };
            App.UpdateRoutine.StepStatusChanged += (step, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (args.Status)
                    {
                        case Status.Waiting:
                            break;
                        case Status.Running:
                            var stepVm = new StepViewModel(step, args.Status);
                            stepVm.Start();
                            Steps.Add(stepVm);
                            break;
                        case Status.Failed:
                            var failedVm = Steps.LastOrDefault(m => m.Step == step);
                            failedVm?.Stop(false, args.Error);
                            break;
                        case Status.Completed:
                            var completedVm = Steps.LastOrDefault(m => m.Step == step);
                            completedVm?.Stop();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            };
        }
    }

    public class StepViewModel : ViewModel
    {
        public UpdateRoutine.Step Step { get; }

        private string _text;
        public string Text
        {
            get => _text;
            private set => SetProperty(ref _text, value);
        }

        private Status _status;
        public Status Status
        {
            get => _status;
            private set => SetProperty(ref _status, value);
        }

        private DateTime _start;
        private DateTime _stop;

        public StepViewModel(UpdateRoutine.Step step, Status status)
        {
            Step = step;
            Status = status;

            switch (step)
            {
                case UpdateRoutine.Step.Connecting:
                    Text = "kobler til server";
                    break;
                case UpdateRoutine.Step.RemoteSyncing:
                    Text = "synkroniserer meldinger";
                    break;
                case UpdateRoutine.Step.RemovingNotifications:
                    Text = "fjerner fremtidige notifikasjoner";
                    break;
                case UpdateRoutine.Step.LocalSyncing:
                    Text = "synkroniserer lokalt";
                    break;
                case UpdateRoutine.Step.LocalStoring:
                    Text = "lagrer lokalt";
                    break;
                case UpdateRoutine.Step.Scheduling:
                    Text = "setter opp fremtidige notifikasjoner";
                    break;
                default:
                    Text = "";
                    break;
            }
        }

        public TimeSpan Duration => _stop - _start;

        public void Start()
        {
            _start = DateTime.Now;
            Status = Status.Running;
        }

        public void Stop(bool success = true, string errorMessage = "")
        {
            _stop = DateTime.Now;
            Status = success ? Status.Completed : Status.Failed;

            if (!string.IsNullOrEmpty(errorMessage))
                Text = errorMessage;

            Text += $" ({Duration.TotalMilliseconds:#}ms)";
        }
    }

    public class StatusRoutineDataTemplateSelector : DataTemplateSelector
    {
        public static DataTemplate Default { get; } = new DataTemplate(() => new StackLayout()
        {
            HeightRequest = 10,
            WidthRequest = 10,
            BackgroundColor = Color.Fuchsia,
        });

        public DataTemplate WaitingTemplate { get; set; } = Default;
        public DataTemplate RunningTemplate { get; set; } = Default;
        public DataTemplate FailedTemplate { get; set; } = Default;
        public DataTemplate CompletedTemplate { get; set; } = Default;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Status status)
            {
                switch (status)
                {
                    case Status.Waiting:
                        return WaitingTemplate;
                    case Status.Running:
                        return RunningTemplate;
                    case Status.Failed:
                        return FailedTemplate;
                    case Status.Completed:
                        return CompletedTemplate;
                }
            }

            return Default;
        }
    }
}