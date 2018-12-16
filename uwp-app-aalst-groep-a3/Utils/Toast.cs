using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace uwp_app_aalst_groep_a3.Utils
{
    class Toast
    {
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
                        },

                        /*new AdaptiveImage()
                        {
                            Source = image
                        }*/
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
    }
}
