using System;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Models;
using Microsoft.AppCenter.Crashes;

namespace HeiaMeg.Services
{
    public class UpdateRoutine
    {
        public event Action<Status> StatusChanged;

        public event Action<Step, StatusArgs> StepStatusChanged;

        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(value);
                }
            }
        }

        private static DateTime StartDate => DateTime.UtcNow;

        public async Task<Result> RunAsyncTask()
        {
            try
            {
                // Make sure not already running
                if (Status == Status.Running)
                    return Result.AlreadyStarted;

                Status = Status.Running;
                await DebugLogTask(Status, "Started");

                /* ________ Initialize ________ */

                var isFullDownload = !(UserSettings.LastUpdateDate > DateTime.MinValue);

                /* ________ Download Meta ________ */

                StepStatusChanged?.Invoke(Step.Connecting, new StatusArgs(Status.Running));
                var meta = await DownloadManager.DownloadMetaAsync(exception =>
                {
                    StepStatusChanged?.Invoke(Step.Connecting,
                        new StatusArgs(Status.Failed, ExceptionToFriendlyError(exception)));
                    Crashes.TrackError(exception);
                    DebugLogTask(Status.Failed, exception.Message).GetAwaiter().GetResult();
                });

                if (meta == null)
                {
                    Status = Status.Failed;
                    await DebugLogTask(Status.Failed, "meta null");
                    return Result.Failed;
                }

                StepStatusChanged?.Invoke(Step.Connecting, new StatusArgs(Status.Completed));

                // meta.MessagesModified is UTC
                if (meta.MessagesModified >= UserSettings.LastMessagesModified) // has Update
                {
                    /* ________ Download Messages ________ */

                    StepStatusChanged?.Invoke(Step.RemoteSyncing, new StatusArgs(Status.Running));
                    IMessage[] downloadedMessages;
                    if (!isFullDownload)
                    {
                        // download modified messages
                        downloadedMessages = await DownloadManager.DownloadModifiedMessagesAsync(StartDate,
                            UserSettings.LastUpdateDate.ToUniversalTime(),
                            exception =>
                            {
                                StepStatusChanged?.Invoke(Step.RemoteSyncing,
                                    new StatusArgs(Status.Failed, ExceptionToFriendlyError(exception)));
                                Crashes.TrackError(exception);
                                DebugLogTask(Status.Failed, exception.Message).GetAwaiter().GetResult();
                            });
                    }
                    else
                    {
                        // download all messages
                        downloadedMessages = await DownloadManager.DownloadMessagesAsync(StartDate, exception =>
                        {
                            StepStatusChanged?.Invoke(Step.RemoteSyncing,
                                new StatusArgs(Status.Failed, ExceptionToFriendlyError(exception)));
                            Crashes.TrackError(exception);
                            DebugLogTask(Status.Failed, exception.Message).GetAwaiter().GetResult();
                        });
                    }

                    if (downloadedMessages == null)
                    {
                        Status = Status.Failed;
                        return Result.Failed;
                    }

                    UserSettings.LastMessagesModified = meta.MessagesModified;

                    var messages = downloadedMessages.Cast<Message>().ToArray();
                    StepStatusChanged?.Invoke(Step.RemoteSyncing, new StatusArgs(Status.Completed));

                    /* ________ Unschedule Notifications ________ */

                    StepStatusChanged?.Invoke(Step.RemovingNotifications, new StatusArgs(Status.Running));
                    foreach (var theme in ThemesManager.EnabledThemes)
                    {
                        if (!await ScheduleManager.UnScheduleThemeAsync(theme, date: StartDate))
                        {
                            StepStatusChanged?.Invoke(Step.RemovingNotifications,
                                new StatusArgs(Status.Failed, "feil med synkronisering"));
                            Status = Status.Failed;
                            await DebugLogTask(Status.Failed, "feil med synkronisering");
                            return Result.Failed;
                        }
                    }

                    StepStatusChanged?.Invoke(Step.RemovingNotifications, new StatusArgs(Status.Completed));

                    /* ________ Remove Future Messages ________ */

                    StepStatusChanged?.Invoke(Step.LocalSyncing, new StatusArgs(Status.Running));
                    if (isFullDownload)
                    {
                        if (!await StorageService.Instance.RemoveMessagesAfterAsync(StartDate))
                        {
                            StepStatusChanged?.Invoke(Step.LocalSyncing,
                                new StatusArgs(Status.Failed, "feil med lokal synkronisering"));
                            Status = Status.Failed;
                            await DebugLogTask(Status.Failed, "feil med lokal synkronisering");
                            return Result.Failed;
                        }
                    }

                    StepStatusChanged?.Invoke(Step.LocalSyncing, new StatusArgs(Status.Completed));

                    /* ________ Store New Messages ________ */

                    StepStatusChanged?.Invoke(Step.LocalStoring, new StatusArgs(Status.Running));
                    if (!await StorageService.Instance.StoreMessagesAsync(messages, false))
                    {
                        StepStatusChanged?.Invoke(Step.LocalStoring,
                            new StatusArgs(Status.Failed, "feil med lokal lagring"));
                        Status = Status.Failed;
                        return Result.Failed;
                    }

                    // remove messages which were empty (invalid) when downloaded
                    if (!await StorageService.Instance.RemoveEmptyMessagesAsync())
                    {
                        StepStatusChanged?.Invoke(Step.LocalSyncing,
                            new StatusArgs(Status.Failed, "feil med lokal synkronisering"));
                        Status = Status.Failed;
                        await DebugLogTask(Status.Failed, "feil med lokal synkronisering");
                        return Result.Failed;
                    }

                    StepStatusChanged?.Invoke(Step.LocalStoring, new StatusArgs(Status.Completed));

                    /* ________ Schedule Notifications ________ */

                    StepStatusChanged?.Invoke(Step.Scheduling, new StatusArgs(Status.Running));
                    foreach (var theme in ThemesManager.EnabledThemes)
                    {
                        if (!await ScheduleManager.ScheduleThemeAsync(theme, date: StartDate))
                        {
                            StepStatusChanged?.Invoke(Step.Scheduling,
                                new StatusArgs(Status.Failed, "feil med notifikasjoner"));
                            Status = Status.Failed;
                            await DebugLogTask(Status.Failed, "feil med notifikasjoner");
                            return Result.Failed;
                        }
                    }

                    StepStatusChanged?.Invoke(Step.Scheduling, new StatusArgs(Status.Completed));
                    await DebugLogTask(Status.Completed,
                        $"Updated with {messages.Length} new messages, full: {isFullDownload}");
                }
                else
                {
                    await DebugLogTask(Status.Completed, $"No Update");
                }

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                Status = Status.Failed;
                return Result.Failed;
            }

            Status = Status.Completed;
            return Result.Completed;
        }

        private static async Task DebugLogTask(Status status, string message)
        {
#if DEBUG
            await StorageService.Instance.StoreDebugInfoAsync(new DebugInfo(status.ToString(), message));
#endif
        }

        private static string ExceptionToFriendlyError(Exception exception)
        {
#if DEBUG
            return exception.Message;
#else
            if (exception.Message.Contains("validation"))
                return "Valideringsproblem - er dato rett?";
            return "Nettverksproblem";
#endif
        }

        public enum Step
        {
            Connecting, 
            RemoteSyncing, 
            RemovingNotifications,
            LocalSyncing,
            LocalStoring,
            Scheduling
        }

        public class StatusArgs
        {
            public Status Status { get; }
            public string Error { get; }

            public StatusArgs(Status status)
            {
                Status = status;
            }

            public StatusArgs(Status status, string error)
            {
                Status = status;
                Error = error;
            }
        }
    }

    public enum Status
    {
        Waiting,
        Running,
        Failed,
        Completed
    }

    public enum Result
    {
        Failed,
        Completed,
        AlreadyStarted,
    }
}