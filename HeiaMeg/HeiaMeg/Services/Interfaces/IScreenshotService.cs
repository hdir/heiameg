using Xamarin.Forms;

namespace HeiaMeg.Services.Interfaces
{
    public interface IScreenshotService
    {
        byte[] Capture();
        byte[] Capture(View view);
    }
}
