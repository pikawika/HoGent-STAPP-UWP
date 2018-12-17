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
                    new ToastButton("Hier Bekijken!", new QueryString()
                    {
                        { "action", "Bekijken" },
                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background, 
                        // Reference the text box's ID in order to
                        // place this button next to the text box
                        TextBoxId = "tbReply"
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
                    { "action", "viewConversation" }

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

        public async void SubscriptionToastAsync(ViewModels.MainPageViewModel mainPageViewModel)
        {
            this.MainPage = mainPageViewModel;
            NetworkAPI networkAPI = new NetworkAPI();
            User user = await networkAPI.GetUser();
            if (user.UserId != -2)
            {
                bool changed = false;
                //eerst initialiseren anders null indien file niet bestaat
                List<Establishment> subs = new List<Establishment>();
                subs = await networkAPI.GetSubscriptions();

                //List<Establishment> merc = await NetworkAPI.GetSubscribedEstablishmentsAsync();

                try
                {
                    bool isEqual = await networkAPI.CheckSubbedDifferenceByJSONAsync(subs);
                    if (!isEqual)
                    {
                        //als veranderd, dan toast tonen en wegschrijven van nieue subs
                        ToastNotificationManager.CreateToastNotifier().Show(new Toast().createToast("STAPP", "Er zijn nieuwe promoties of evenementen toegevoegd, bekijk ze hier!"));
                        await networkAPI.SaveSubscribedEstablishemtsAsync(subs);
                    }
                }
                catch
                {
                    await networkAPI.SaveSubscribedEstablishemtsAsync(subs);
                }

                
            }
            else
            {
                ToastNotificationManager.CreateToastNotifier().Show(new Toast().createToast("Welkom!", "Om alle functionaliteit van deze app te ontgrendelen, kan je hier aanmelden!"));
            }

        }
    }
}
