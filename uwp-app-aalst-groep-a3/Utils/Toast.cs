using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.ViewModels;
using Windows.UI.Notifications;

namespace uwp_app_aalst_groep_a3.Utils
{
    class Toast
    {
        public MainPageViewModel MainPage { get; set; }

        public ToastNotification createToast(string title,string content)
        {
            // Construct the visuals of the toast
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children = {
                        new AdaptiveText()
                        {
                            Text = title
                        },

                        new AdaptiveText()
                        {
                            Text = content
                        }
                    }
                }
            };
            

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Bekijk hier!", "showsubscriptions")
                    {
                        ActivationType = ToastActivationType.Foreground
                    },
                }

            };

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    {"showsubscriptions" }

                }.ToString()
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddDays(1);
            toast.Tag = "18365";
            toast.Group = "stapp";
            
            return toast;
        }

        public async void SubscriptionAsyncWriteOnly()
        {
            NetworkAPI networkAPI = new NetworkAPI();
            User user = await networkAPI.GetUser();
            if (user.UserId != -2)
            {
                //eerst initialiseren anders null indien file niet bestaat
                List<Establishment> subs = new List<Establishment>();
                subs = await networkAPI.GetSubscriptions();
                await networkAPI.SaveSubscribedEstablishemtsAsync(subs);
            }
        }

        public async void SubscriptionToastAsync(MainPageViewModel mainPageViewModel)
        {
            MainPage = mainPageViewModel;
            NetworkAPI networkAPI = new NetworkAPI();
            User user = await networkAPI.GetUser();
            if (user.UserId != -2)
            {
                //eerst initialiseren anders null indien file niet bestaat
                List<Establishment> subs = new List<Establishment>();
                subs = await networkAPI.GetSubscriptions();

                try
                {
                    bool isEqual = await networkAPI.CheckSubbedDifferenceByJSONAsync(subs);
                    if (!isEqual)
                    {
                        ToastNotificationManager.CreateToastNotifier().Show(new Toast().createToast("Stapp", "Er zijn nieuwe promoties of evenementen toegevoegd, klik hier om ze te bekijken!"));
                        await networkAPI.SaveSubscribedEstablishemtsAsync(subs);
                    }
                }
                catch
                {
                    await networkAPI.SaveSubscribedEstablishemtsAsync(subs);
                } 
            }

        }
    }
}
