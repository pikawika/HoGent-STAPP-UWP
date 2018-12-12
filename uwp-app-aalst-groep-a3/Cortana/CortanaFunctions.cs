using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.System;

namespace uwp_app_aalst_groep_a3.Cortana
{
    public class CortanaFunctions
    {
        private MainPageViewModel mainPageViewModel;

        public CortanaFunctions(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;
        }

        // Register Custom Cortana Commands from VCD file
        public static async void RegisterVCD()
        {
            StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync(@"Cortana\CustomVoiceCommandDefinitions.xml");

            try
            {
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
            }
            catch (Exception e)
            {
                //Silently fail
            }
            
        }

        // Look up the spoken command and execute its corresponding action
        public void RunCommand(string commandName)
        {
            switch(commandName)
            {
                case "ShowMerchants":
                    ShowMerchants();
                    break;
                case "ShowEvents":
                    ShowEvents();
                    break;
                case "ShowPromotions":
                    ShowPromotions();
                    break;
                case "ShowMap":
                    ShowMap();
                    break;
                default:
                    break;
            }
        }

        private void ShowMerchants() => mainPageViewModel.NavigateTo(new MerchantsViewModel(mainPageViewModel));

        private void ShowEvents() => mainPageViewModel.NavigateTo(new EventsViewModel(mainPageViewModel));

        private void ShowPromotions() => mainPageViewModel.NavigateTo(new PromotionsViewModel(mainPageViewModel));

        private void ShowMap() => mainPageViewModel.NavigateTo(new MapViewModel(mainPageViewModel));

    }
}
