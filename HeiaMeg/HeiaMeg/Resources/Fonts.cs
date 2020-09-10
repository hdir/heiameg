﻿using Xamarin.Forms;

namespace HeiaMeg.Resources
{
    public class Fonts
    {
        public static string Normal { get; } = RobotoLight;
        public static string Medium { get; } = RobotoMedium;
        public static string Bold { get; } = RobotoBold;

        private static string RobotoLight
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "Roboto-Light.ttf#Roboto-Light";
                    case Device.iOS:
                        return "Roboto-Light";
                    default:
                        return "";
                }
            }
        }

        private static string RobotoMedium
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "Roboto-Medium.ttf#Roboto-Medium";
                    case Device.iOS:
                        return "Roboto-Medium";
                    default:
                        return "";
                }
            }
        }

        private static string RobotoBold
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return "Roboto-Bold.ttf#Roboto-Bold";
                    case Device.iOS:
                        return "Roboto-Bold";
                    default:
                        return "";
                }
            }
        }
    }
}