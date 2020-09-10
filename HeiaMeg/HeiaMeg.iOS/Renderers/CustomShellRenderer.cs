using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(Shell), typeof(HeiaMeg.iOS.Renderers.CustomShellRenderer))]
namespace HeiaMeg.iOS.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            var renderer = base.CreateShellSectionRenderer(shellSection);
            if (renderer is ShellSectionRenderer sectionRenderer)
                sectionRenderer.NavigationBar.Translucent = false;
            return renderer;
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem item)
        {
            var renderer = base.CreateShellItemRenderer(item);
            if (renderer is ShellItemRenderer itemRenderer)
                itemRenderer.TabBar.Translucent = false;
            return renderer;
        }
    }
}