using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Message
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmptyMessageView : ContentView
    {
        public EmptyMessageView()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var size = width;

            BubbleCanvasView.WidthRequest = size;
            BubbleCanvasView.HeightRequest = size;

            TitleLabel.FontSize = size * 0.06f;
            TextLabel.FontSize = size * 0.045f;


            if (TextLabel.FormattedText != null)
            {
                foreach (var span in TextLabel.FormattedText.Spans)
                {
                    span.FontSize = TextLabel.FontSize;
                }
            }
        }

        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public string Text
        {
            get => TextLabel.Text;
            set => TextLabel.Text = value;
        }

        public FormattedString FormattedString
        {
            get => TextLabel.FormattedText;
            set => TextLabel.FormattedText = value;
        }
    }
}