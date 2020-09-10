using System.Threading.Tasks;
using HeiaMeg.ViewControls.Base;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Custom
{
    public class AnimatedScaleView : AnimatedView
    {
        public AnimatedScaleView()
        {
            if (!Animate)
            {
                Scale = FromScale;
            }
        }

        public override async Task AnimateInAsync()
        {
            if (FadeIn)
            {
                Opacity = 0;
#pragma warning disable 4014
                this.FadeTo(1, AnimationTime / 2, Easing.Linear);
#pragma warning restore 4014
            }
            
            await this.ScaleTo(ToScale, AnimationTime, EasingIn);
        }

        public override async Task AnimateOutAsync()
        {
            if (FadeOut)
            {
#pragma warning disable 4014
                this.FadeTo(0, AnimationTime, Easing.Linear);
#pragma warning restore 4014
            }
            await this.ScaleTo(FromScale, AnimationTime, EasingOut);
        }

        public bool FadeIn { get; set; }
        public bool FadeOut { get; set; }

        public double FromScale { get; set; } = 0;
        public double ToScale { get; set; } = 1;
    }
}
