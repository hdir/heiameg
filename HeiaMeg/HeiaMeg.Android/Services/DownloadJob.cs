using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using HeiaMeg.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Result = HeiaMeg.Services.Result;

namespace HeiaMeg.Droid.Services
{
    [Service(Name = "no.helsedirektoratet.baredu.DownloadJob",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class DownloadJob : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(async () =>
            {
                try
                {
                    if ((DateTime.Now - UserSettings.LastUpdateDate).Days >= Config.UpdateRoutineIntervalDays)
                    {
                        if (await App.UpdateRoutine.RunAsyncTask() == Result.Completed)
                        {
                            UserSettings.LastUpdateDate = DateTime.Now;
                        }
                        foreach (var theme in ThemesManager.EnabledThemes)
                        {
                            await Notifications.NotificationService.Instance.ScheduleNotificationsAsync(theme.Id);
                        }
                    }
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
#if DEBUG
                    Device.BeginInvokeOnMainThread(() => throw e);
#endif
                }

                //NOTE: Return whether or not we want to reschedule the job
                //NOTE: We do not, as JobInfo.Builder.setPeriodic() takes care of resheduling for us!
                JobFinished(jobParams, false);
            });

            //Continue this job until Jobfinished is called
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            //Return whether or not we want to reschedule the job
            return false;
        }
    }
}
