using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Microsoft.AppCenter.Crashes;

namespace HeiaMeg.Droid.Services
{
    public class JobStarter
    {
        private const int BACKGROUND_DOWNLOAD_JOB_ID = 1;

        public static void SetupBackgroundUpdateJob(Activity activity)
        {
            var componentName = new ComponentName(activity, Java.Lang.Class.FromType(typeof(DownloadJob)));
            var builder = new JobInfo.Builder(BACKGROUND_DOWNLOAD_JOB_ID, componentName);

            try
            {
                JobInfo backgroundJob;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    // 24+ requires minFlexMillis (1 hour)
                    backgroundJob = builder
                        .SetPeriodic(Config.DownloadInterval, JobInfo.MinFlexMillis)
                        .SetPersisted(true)
                        .Build();
                }
                else
                {
                    backgroundJob = builder
                        .SetPeriodic(Config.DownloadInterval)
                        .SetPersisted(true)
                        .Build();
                }

                var jobScheduler = (JobScheduler)activity.GetSystemService(Context.JobSchedulerService);
                jobScheduler.Schedule(backgroundJob);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}