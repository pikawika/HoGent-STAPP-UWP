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
            string image = "https://ae01.alicdn.com/kf/HTB1dnp4RpXXXXbtapXXq6xXFXXXg/Sexy-Vintage-Mini-Korte-Jeans-Booty-Shorts-Leuke-Bikini-Denim-Korte-Hot-Vestidos-Sexy-Club-Party.jpg_640x640.jpg";

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

            // In a real app, these would be initialized with actual data
            int conversationId = 384928;

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Reply", new QueryString()
                    {
                        { "action", "Bekijken" },
                        { "conversationId", conversationId.ToString() }

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
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddDays(2);
            toast.Tag = "18365";
            toast.Group = "wallPosts";

            return toast;
        }
    }
}
