using System;

namespace HeiaMeg
{
    public interface IServerMeta
    {
        /// <summary>
        /// Current version code
        /// </summary>
        int VersionCode { get; }

        /// <summary>
        /// Name of current version
        /// </summary>
        string VersionName { get; }

        /// <summary>
        /// Schedule for when to check for updates from spreadsheet
        /// </summary>
        string CronSchedule { get; }

        /// <summary>
        /// Time the last update was performed
        /// </summary>
        DateTime LastUpdate { get; }

        /// <summary>
        /// Time the newest item in Themes was modified
        /// </summary>
        DateTime ThemesModified { get; }

        /// <summary>
        /// Time the newest item in Messages was modified
        /// </summary>
        DateTime MessagesModified { get; }

        /// <summary>
        /// Time the newest item in Content was modified
        /// </summary>
        DateTime ContentModified { get; }

        /// <summary>
        /// Time the last update was performed
        /// </summary>
        int MinimumModifiedMinutesThreshold { get; }

        /// <summary>
        /// Time in seconds between checking if should update
        /// </summary>
        int UpdateFrequency { get; }
    }
}
