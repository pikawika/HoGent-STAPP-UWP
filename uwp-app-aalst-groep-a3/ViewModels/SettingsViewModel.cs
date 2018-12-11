using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<string> Developers { get; set; }

        public SettingsViewModel()
        {
            ShortDescription = InitShortDescription();
            LongDescription = InitLongDescription();
            Developers = InitDevelopers();
        }

        private string InitShortDescription()
        {
            return "Deze applicatie werd ontwikkeld in het schooljaar van 2018-2019 " +
                    "in opdracht van HoGent campus Aalst voor het vak Native Apps II - Windows. " +
                    "De lector van dit vak is Joeri van Herreweghe.";
        }

        private string InitLongDescription()
        {
            return "Stapp biedt de lokale handelaars van Aalst een nieuw kanaal aan " +
                    "om hun lokale onderneming te promoten of in de kijker te zetten. Gebruikers " +
                    "kunnen de app gebruiken als (of alvorens) ze de stad bezoeken om een overzicht " +
                    "te krijgen van de lokale ondernemingen(winkels, cafés, restaurants, scholen, …). " +
                    "Gebruikers kunnen via een zoekfunctie op zoek gaan naar bijvoorbeeld. " +
                    "een sushi restaurant.Gebruikers kunnen ook meteen zoeken / filteren op " +
                    "promoties om een overzicht te krijgen van handelszaken waar momenteel promoties " +
                    "lopen en dus zeker interessant zijn om te bezoeken.";
        }

        private List<string> InitDevelopers()
        {
            return new List<string> { "Lennert Bontinck", "Bram De Coninck", "Jodi De Loof" };
        }
    }
}
