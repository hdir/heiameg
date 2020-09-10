using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeiaMeg.ViewControls.Base
{
    public abstract class AnimatedView : ContentView
    {
        public event Action OnAnimateIn = () => { };
        public event Action OnAnimateInCompleted = () => { };
        public event Action OnAnimateOut = () => { };
        public event Action OnAnimateOutCompleted = () => { };

        public abstract Task AnimateInAsync();
        public abstract Task AnimateOutAsync();

        protected async void StartAnimatePropertyChanged(bool oldValue, bool newValue)
        {
            try
            {
                if (!oldValue && newValue)
                {
                    OnAnimateIn.Invoke();
                    await AnimateInAsync();
                    OnAnimateInCompleted.Invoke();
                }
                else if (oldValue && !newValue)
                {
                    OnAnimateOut.Invoke();
                    await AnimateOutAsync();
                    OnAnimateOutCompleted.Invoke();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#endif
            }
        }

        public static readonly BindableProperty AnimateProperty =
            BindableProperty.Create(
                nameof(Animate),
                typeof(bool),
                typeof(AnimatedView),
                default(bool),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((AnimatedView)bindableObject).StartAnimatePropertyChanged((bool)oldValue, (bool)newValue);
                }
            );

        public bool Animate
        {
            get => (bool)GetValue(AnimateProperty);
            set => SetValue(AnimateProperty, value);
        }

        public static readonly BindableProperty EasingInProperty =
            BindableProperty.Create(
                nameof(EasingIn),
                typeof(Easing),
                typeof(AnimatedView),
                Easing.CubicInOut
            );

        public Easing EasingIn
        {
            get => (Easing)GetValue(EasingInProperty);
            set => SetValue(EasingInProperty, value);
        }

        public static readonly BindableProperty EasingOutProperty =
            BindableProperty.Create(
                nameof(EasingOut),
                typeof(Easing),
                typeof(AnimatedView),
                Easing.CubicInOut
            );

        public Easing EasingOut
        {
            get => (Easing)GetValue(EasingOutProperty);
            set => SetValue(EasingOutProperty, value);
        }

        public static readonly BindableProperty AnimationTimeProperty =
            BindableProperty.Create(
                nameof(AnimationTime),
                typeof(uint),
                typeof(AnimatedView),
                250u
            );

        public uint AnimationTime
        {
            get => (uint)GetValue(AnimationTimeProperty);
            set => SetValue(AnimationTimeProperty, value);
        }

        protected Task<bool> ValueAnimation(IAnimatable element, string name, Func<double, float> transform, Action<float> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        public void CancelAnimation(string animationName)
        {
            this.AbortAnimation(animationName);
        }
    }
}
