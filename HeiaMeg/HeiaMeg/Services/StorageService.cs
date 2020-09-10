using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Models;
using Microsoft.AppCenter.Crashes;
using SQLite;
using Xamarin.Forms;

namespace HeiaMeg.Services
{
    public class StorageService
    {
        #region Singleton

        private static readonly Lazy<StorageService> Lazy =
            new Lazy<StorageService>(() => new StorageService());

        private SQLiteAsyncConnection _asyncConnection;

        public static StorageService Instance => Lazy.Value;

        #endregion

        private static string LocalPath(string dbName) => Path.Combine(
                                                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                    dbName
                                              );

        public SQLiteAsyncConnection AsyncConnection =>
            _asyncConnection ?? (_asyncConnection = new SQLiteAsyncConnection(LocalPath(Config.DatabaseName)));

        private StorageService()
        {
            AsyncConnection.CreateTableAsync<Message>().Wait();

#if DEBUG
            AsyncConnection.CreateTableAsync<DebugInfo>().Wait();
#endif
        }

        public async Task<bool> StoreMessagesAsync(IEnumerable<Message> messages, bool overwrite = true)
        {
            if (overwrite)
            {
                await AsyncConnection.InsertAllAsync(messages);
            }
            else
            {
                foreach (var message in messages)
                {
                    if (!await StoreMessageAsync(message, false))
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> StoreMessageAsync(Message message, bool overwrite)
        {
            try
            {
                if (!overwrite)
                {
                    var old = await AsyncConnection.FindAsync<Message>(message.Id);
                    if (old != null)
                    {
                        if (old.Id != message.Id)
                            throw new Exception("Mismatch between ID's");

                        message.NotifyTime = old.NotifyTime;
                        message.Opened = old.Opened;
                    }
                }

                await AsyncConnection.InsertOrReplaceAsync(message, typeof(Message));
            }
            catch (Exception e)
            {
                // NOTE: This can happen with updated sqlite library (originally sqlite 1.4.118)
                // 1.5.231 is known to be problematic 

                Crashes.TrackError(e);
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#endif
                return false;
            }
            return true;
        }

        public async Task ClearMessagesAsync()
        {
            await AsyncConnection.DropTableAsync<Message>();
            await AsyncConnection.CreateTableAsync<Message>();
        }

        public async Task UpdateMessageAsync(Message message)
        {
            await AsyncConnection.InsertOrReplaceAsync(message);
        }

        public async Task UpdateMessagesAsync(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
                await AsyncConnection.InsertOrReplaceAsync(message);
        }

        public async Task<Message> GetWelcomeMessageAsync(int themeId)
        {
            return (await GetConsecutiveMessagesAsync(themeId)).FirstOrDefault();
        }

        public async Task<Message> GetLastReleasedMessage(int themeId)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                        message.ThemeId == themeId &&
                        message.NotifyTime <= DateTime.Now &&         // in the past
                        message.Removed == null                       // hasn't been removed
                )
                .OrderBy(
                    message =>
                        message.NotifyTime
                )
                .FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetConsecutiveMessagesAsync(int themeId)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                        message.Date == null &&
                        message.ThemeId == themeId
                )
                .OrderBy(m =>
                    m.Id
                )
                .ToListAsync();
        }

        public async Task<List<Message>> GetConsecutiveMessagesAfterAsync(int themeId, DateTime date)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                     message.Date == null &&
                     message.ThemeId == themeId &&
                     (message.NotifyTime == null || message.NotifyTime > date) // schedule in the future or not at all
                )
                .OrderBy(m =>
                    m.Id
                )
                .ToListAsync();
        }

        public async Task<List<Message>> GetReleasedMessagesAsync()
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                     ((message.Text != null && message.Text != "")  || (message.Link != null && message.Link != ""))&&
                     message.NotifyTime <= DateTime.Now &&          // in the past
                     message.Removed == null                        // hasn't been removed
                )
                .OrderBy(
                    message =>
                        message.NotifyTime
                )
                .ToListAsync();
        }

        public async Task<List<Message>> GetUnreadMessagesBeforeAsync(DateTime date)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                    message.NotifyTime != null &&
                    message.NotifyTime <= date &&                   // in the past
                    message.Opened == null &&                       // unopened
                    message.FeedbackType != FeedbackType.Dislike    // not disliked
                )
                .OrderBy(
                    message =>
                        message.NotifyTime
                )
                .ToListAsync();
        }

        public async Task<List<Message>> GetScheduledThemeMessagesAfterAsync(int themeId, DateTime date)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                    message.NotifyTime != null &&           // is after date
                    message.NotifyTime > date &&            // is after date
                    message.ThemeId == themeId              // is requested theme
                )
                .OrderBy(message =>
                    message.NotifyTime
                )
                .ToListAsync();
        }

        public async Task<List<Message>> GetThemeMessagesAfterAsync(int themeId, DateTime date)
        {
            return await AsyncConnection.Table<Message>()
                .Where(message =>
                    message.Date != null &&                // has a date
                    message.Date >= date.Date &&           // is after date
                    //message.From > date.TimeOfDay &&     // is after time
                    message.ThemeId == themeId             // is requested theme
                )
                .OrderBy(message =>
                    message.NotifyTime
                )
                .ToListAsync();
        }

        public async Task<string> DumpDatabase()
        {
            var name = DateTime.Now.Ticks + ".db";
            var path = LocalPath(name);
            try
            {
                await AsyncConnection.BackupAsync(path);
            }
            catch (Exception)
            {
                return path;
            }
            return path;
        }

        public async Task<bool> RemoveEmptyMessagesAsync()
        {
            try
            {
                await AsyncConnection.Table<Message>()
                    .Where(message =>
                            (message.Text == null || message.Text == "") &&
                            (message.Link == null || message.Link == "")
                    )
                    .DeleteAsync();

                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        public async Task<bool> RemoveMessagesAfterAsync(DateTime datetime)
        {
            try
            {
                await AsyncConnection.Table<Message>()
                    .Where(message =>
                            message.Date >= datetime                    // is after date
                            || message.NotifyTime >= datetime           // is scheduled after date
                    )
                    .DeleteAsync();

                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

#if DEBUG
        public async Task<bool> StoreDebugInfoAsync(DebugInfo info)
        {
            return await AsyncConnection.InsertAsync(info, typeof(DebugInfo)) > 0;
        }
        public async Task<IEnumerable<DebugInfo>> GetDebugInfoAsync()
        {
            return await AsyncConnection.Table<DebugInfo>().ToListAsync();
        }
#endif
    }
}
