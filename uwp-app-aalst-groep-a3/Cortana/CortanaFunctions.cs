using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.SpeechRecognition;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.System;

namespace uwp_app_aalst_groep_a3.Cortana
{
    public class CortanaFunctions
    {
        private MainPageViewModel mainPageViewModel;
        private PasswordVault passwordVault = new PasswordVault();

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
            catch
            {
                Debug.WriteLine("Er is iets fout gegaan tijdens het installeren van " +
                                "de voice commands definitions van de Stapp app. " +
                                "Mogelijks is Cortana niet geïnstalleerd op het toestel.");
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
                case "ShowSubscriptions":
                    ShowSubscriptions();
                    break;
                default:
                    break;
            }
        }

        private void ShowMerchants() => mainPageViewModel.NavigateTo(new MerchantsViewModel(mainPageViewModel));

        private void ShowEvents() => mainPageViewModel.NavigateTo(new EventsViewModel(mainPageViewModel));

        private void ShowPromotions() => mainPageViewModel.NavigateTo(new PromotionsViewModel(mainPageViewModel));

        private void ShowMap() => mainPageViewModel.NavigateTo(new MapViewModel(mainPageViewModel));

        private void ShowSubscriptions()
        {
            try
            {
                passwordVault.Retrieve("Stapp", "Token");
                mainPageViewModel.NavigateTo(new SubscriptionsViewModel(mainPageViewModel));
            }
            catch
            {
                mainPageViewModel.NavigateTo(new LoginViewModel(mainPageViewModel));
            }
        }

    }
}
