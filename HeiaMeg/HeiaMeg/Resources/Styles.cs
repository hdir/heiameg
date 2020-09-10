using Xamarin.Forms;

namespace HeiaMeg.Resources
{
    public static class Styles
    {
        public static ResourceDictionary MainStyle { get; } = new ResourceDictionary
        {
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter() {Property = Label.FontFamilyProperty, Value = Fonts.Normal},
                    new Setter() {Property = Label.TextColorProperty, Value = Colors.Text},
                    new Setter() {Property = Label.FontSizeProperty, Value = Sizes.TextMedium},
                    new Setter() {Property = Label.LineHeightProperty, Value = 1.1},
                },
                Triggers =
                {
                    new Trigger(typeof(Label))
                    {
                        Property = Label.FontAttributesProperty,
                        Value = FontAttributes.Bold,
                        Setters =
                        {
                            new Setter() {Property = Label.FontFamilyProperty, Value = Fonts.Bold},
                        }
                    }
                }
            },
            new Style(typeof(Span))
            {
                Setters =
                {
                    new Setter() {Property = Span.FontFamilyProperty, Value = Fonts.Normal},
                    new Setter() {Property = Span.TextColorProperty, Value = Colors.Text},
                    new Setter() {Property = Span.FontSizeProperty, Value = Sizes.TextMedium},
                    new Setter() {Property = Span.LineHeightProperty, Value = 1.1},
                },
                Triggers =
                {
                    new Trigger(typeof(Span))
                    {
                        Property = Span.FontAttributesProperty,
                        Value = FontAttributes.Bold,
                        Setters =
                        {
                            new Setter() {Property = Span.FontFamilyProperty, Value = Fonts.Bold},
                        }
                    }
                }
            },
            new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter() {Property = Entry.FontFamilyProperty, Value = Fonts.Normal},
                    new Setter() {Property = Entry.TextColorProperty, Value = Colors.Text},
                    new Setter() {Property = Entry.TextColorProperty, Value = Colors.Text},
                },
                Triggers =
                {
                    new Trigger(typeof(Entry))
                    {
                        Property = Entry.FontAttributesProperty,
                        Value = FontAttributes.Bold,
                        Setters =
                        {
                            new Setter() {Property = Entry.FontFamilyProperty, Value = Fonts.Bold},
                        }
                    }
                }
            },
        };

        public static ResourceDictionary OnboardingStyle { get; } = new ResourceDictionary
        {
            new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter() {Property = Label.FontFamilyProperty, Value = Fonts.Normal},
                    new Setter() {Property = Label.TextColorProperty, Value = Colors.OnboardingText},
                    new Setter() {Property = Label.FontSizeProperty, Value = Sizes.TextMedium},
                    new Setter() {Property = Label.LineHeightProperty, Value = 1.1},
                },
                Triggers =
                {
                    new Trigger(typeof(Label))
                    {
                        Property = Label.FontAttributesProperty,
                        Value = FontAttributes.Bold,
                        Setters =
                        {
                            new Setter() {Property = Label.FontFamilyProperty, Value = Fonts.Bold},
                            new Setter() {Property = Label.LineHeightProperty, Value = 1.15},
                        }
                    }
                }
            },
            new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter() {Property = Entry.FontFamilyProperty, Value = Fonts.Normal},
                    new Setter() {Property = Entry.TextColorProperty, Value = Colors.OnboardingText},
                    new Setter() {Property = Entry.TextColorProperty, Value = Colors.OnboardingText},
                },
                Triggers =
                {
                    new Trigger(typeof(Entry))
                    {
                        Property = Entry.FontAttributesProperty,
                        Value = FontAttributes.Bold,
                        Setters =
                        {
                            new Setter() {Property = Entry.FontFamilyProperty, Value = Fonts.Bold},
                        }
                    }
                }
            },
            new Style(typeof(Switch))
            {
                Setters =
                {
                    new Setter()
                    {
                        Property = Switch.OnColorProperty,
                        Value = Colors.NewMessageColor
                    },
                }
            },
        };
    }
}
