using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly static IReadOnlyDictionary<string, Delegate> vcdLookup = new Dictionary<string, Delegate>{

            /*
            {<command name from VCD>, (Action)(async () => {
                 <code that runs when that commmand is called>
            })}
            */

            {"ShowMerchants", (Action)(async () => {
                 Uri website = new Uri(@"https://www.bramdeconinck.com");
                 await Launcher.LaunchUriAsync(website);
             })},

        };

        // Register Custom Cortana Commands from VCD file
        public static async void RegisterVCD()
        {
            StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync(@"Cortana\CustomVoiceCommandDefinitions.xml");

            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
        }

        // Look up the spoken command and execute its corresponding action
        public static void RunCommand(VoiceCommandActivatedEventArgs cmd)
        {
            SpeechRecognitionResult result = cmd.Result;
            string commandName = result.RulePath[0];
            vcdLookup[commandName].DynamicInvoke();
        }
    }
}
