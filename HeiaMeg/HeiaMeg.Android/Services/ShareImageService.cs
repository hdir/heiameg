using System;
using System.IO;
using Android.Content;
using Android.Support.V4.Content;
using HeiaMeg.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(HeiaMeg.Droid.Services.ShareImageService))]
namespace HeiaMeg.Droid.Services
{
    public class ShareImageService : IShareImageService
    {
        private const string ContentType = "image/png";

        public void Show(string title, string message, byte[] imageData)
        {
            var context = MainActivity.Context;

            if (context == null)
            {
                throw new Exception("You have to set ShareImageService.Context");
            }

            var filesDir = context.FilesDir.AbsolutePath;

            //NOTE: defined in Resources/xml/filepaths to be available for fileprovider
            var path = Path.Combine(filesDir, "tempscreenshots/screenshot.png");
            var dir = Path.GetDirectoryName(path);

            Directory.CreateDirectory(dir);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                stream.Write(imageData, 0, imageData.Length);
            }
            var fileUri = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", new Java.IO.File(path));

            var intent = new Intent(Intent.ActionSend);
            intent.SetType(ContentType);
            intent.PutExtra(Intent.ExtraStream, fileUri);
            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            chooserIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
            context.StartActivity(chooserIntent);
        }
    }
}