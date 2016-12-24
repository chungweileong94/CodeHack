using CodeHack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;

namespace CodeHack.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        public MainPage()
        {
            this.InitializeComponent();
            CodeHack.MainPage.Init(this);
            LoadApplication(new CodeHack.App());
        }

        private MobileServiceUser user;
        MobileServiceClient client = new MobileServiceClient(Constants.ApplicationURL);

        public MobileServiceAuthenticationProvider ServiceProvider { get; private set; }

        public async Task<bool> Authenticate(int Provider)
        {
            string message = string.Empty;
            var success = false;

            switch (Provider)
            {
                case 0:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Google;
                        break;
                    }
                case 1:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Twitter;
                        break;
                    }
                case 2:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.MicrosoftAccount; break;
                    }
                case 3:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Facebook;
                        break;
                    }
            }
            
            try
            {
                // Sign in with the respective Service Provider login using a server-managed flow.
                if (user == null)
                {
                    user = await client.LoginAsync(ServiceProvider);
                    if (user != null)
                    {
                        success = true;
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                    }
                }

            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

            //  Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
    }
}
