using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Lighter.Resources;
using Windows.Phone.Media.Capture;
using System.Windows.Media;

namespace Lighter
{
    public partial class MainPage : PhoneApplicationPage
    {
        protected AudioVideoCaptureDevice Device { get; set; }
        private static bool flashEnabled = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void FlashButton_Click(object sender, RoutedEventArgs e)
        {
            var sensorLocation = CameraSensorLocation.Back;

            try
            {
                if (!flashEnabled)
                {
                    if (this.Device == null)
                    {
                        // get the AudioViceoCaptureDevice
                        this.Device = await AudioVideoCaptureDevice.OpenAsync(sensorLocation,
                        AudioVideoCaptureDevice.GetAvailableCaptureResolutions(sensorLocation).First());
                    }

                    // turn flashlight on
                    var supportedCameraModes = AudioVideoCaptureDevice
                        .GetSupportedPropertyValues(sensorLocation, KnownCameraAudioVideoProperties.VideoTorchMode);
                    if (supportedCameraModes.ToList().Contains((UInt32)VideoTorchMode.On))
                    {
                        this.Device.SetProperty(KnownCameraAudioVideoProperties.VideoTorchMode, VideoTorchMode.On);

                        // set flash power to maxinum
                        this.Device.SetProperty(KnownCameraAudioVideoProperties.VideoTorchPower,
                            AudioVideoCaptureDevice.GetSupportedPropertyRange(sensorLocation, KnownCameraAudioVideoProperties.VideoTorchPower).Max);
                    }
                    else
                    {
                        turnWhiteScreen(true);
                    }
                    EllipseLight.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                    flashEnabled = true;
                }
                else
                {
                    // turn flashlight on
                    var supportedCameraModes = AudioVideoCaptureDevice
                        .GetSupportedPropertyValues(sensorLocation, KnownCameraAudioVideoProperties.VideoTorchMode);
                    if (this.Device != null && supportedCameraModes.ToList().Contains((UInt32)VideoTorchMode.Off))
                    {
                        this.Device.SetProperty(KnownCameraAudioVideoProperties.VideoTorchMode, VideoTorchMode.Off);
                    }
                    else
                    {
                        turnWhiteScreen(false);
                    }
                    flashEnabled = false;
                    EllipseLight.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
                }
            }
            catch (Exception ex)
            {
                // Flashlight isn't supported on this device, instead show a White Screen as the flash light
                turnWhiteScreen(true);
            }
        }

        private void turnWhiteScreen(bool p)
        {

            WhiteSheet.Visibility = p ? System.Windows.Visibility.Visible:System.Windows.Visibility.Collapsed;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            turnWhiteScreen(true);
        }

        private void WhiteSheet_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            turnWhiteScreen(false);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}