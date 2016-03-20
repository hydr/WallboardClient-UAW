using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Wallboard.Model;
using Wallboard.Model.Repos;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace WallboardClient
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DeviceConfiguration deviceConfiguration;

        IDeviceConfigurationRepo deviceConfigurationRepo;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            deviceConfigurationRepo = new LocalDeviceConfigurationRepo();
            deviceConfiguration = await deviceConfigurationRepo.Get();
            if (deviceConfiguration == null)
            {
                deviceConfiguration = new DeviceConfiguration();
            }
            else
            {
                ToggleMenu();
                Address.Text = deviceConfiguration.DefaulUrl;
                MyWebView.Navigate(new Uri(deviceConfiguration.DefaulUrl));
            } 
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            deviceConfiguration.DefaulUrl = Address.Text;
            await deviceConfigurationRepo.Save(deviceConfiguration);

            MyWebView.Navigate(new Uri(Address.Text));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MyWebView.Refresh();
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F11)
            {
                ToggleMenu();
            }
        }

        private void MyWebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            // check for F11 pressed
            if(e.Value == "KeyDown:122")
            {
                ToggleMenu();
            }
        }

        private void ToggleMenu()
        {
            if (MainGrid.RowDefinitions[0].Height == new GridLength(0))
            {
                MainGrid.RowDefinitions[0].Height = new GridLength(40);
                MainGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            else
            {
                MainGrid.RowDefinitions[0].Height = new GridLength(0);
                MainGrid.RowDefinitions[1].Height = new GridLength(3);
            }
        }

        private async void MyWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // Inject our Keydown handler into the page
            string inject =
                @"document.addEventListener('keydown', function(event) {
                    window.external.notify('KeyDown:' + event.which);
                });";
            await MyWebView.InvokeScriptAsync("eval", new List<string>() { inject });

        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }
    }
}
