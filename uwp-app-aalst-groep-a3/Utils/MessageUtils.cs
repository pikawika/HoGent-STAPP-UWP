using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.Utils
{
    public static class MessageUtils {

        public static async Task ShowDialog(string title, string message)
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = title;
            contentDialog.Content = message;
            contentDialog.PrimaryButtonText = "Oké";

            await contentDialog.ShowAsync();
        }

    }
}
