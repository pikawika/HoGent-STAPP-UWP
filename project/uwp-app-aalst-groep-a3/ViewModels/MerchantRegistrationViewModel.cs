﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class MerchantRegistrationViewModel : ViewModelBase
    {
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string RepeatPassword { get; set; } = "";

        public RelayCommand RegisterAsMerchantCommand { get; set; }
        public RelayCommand NavigateToLoginCommand { get; set; }

        public MerchantRegistrationViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            RegisterAsMerchantCommand = new RelayCommand(async _ => await CreateAccountAsync());
            NavigateToLoginCommand = new RelayCommand(_ => NavigateToLogin());
        }

        private async Task CreateAccountAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName)
                || string.IsNullOrWhiteSpace(LastName)
                || string.IsNullOrWhiteSpace(EmailAddress)
                || string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(RepeatPassword))
            {
                await MessageUtils.ShowDialog("Handelaar account aanmaken", "Gelieve in ieder veld een waarde in te voeren.");
                return;
            }

            if (!Regex.IsMatch(EmailAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                await MessageUtils.ShowDialog("Handelaar account aanmaken", "Gelieve een geldig e-mailadres in te voeren.");
                return;
            }

            if (Password != RepeatPassword)
            {
                await MessageUtils.ShowDialog("Handelaar account aanmaken", "Wachtwoord en herhaal wachtwoord komen niet overeen.");
                return;
            }

            await ShowGDPRDialogAsync();
        }

        private async Task ShowGDPRDialogAsync()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Handelaar account aanmaken";
            contentDialog.Content = "Door op de knop 'Bevestigen' te drukken, bevestigt u dat u het privacybeleid van Stapp gelezen en goedgekeurd heeft en wordt uw handelaar account aangemaakt.";
            contentDialog.PrimaryButtonText = "Bevestigen";
            contentDialog.SecondaryButtonText = "Bekijk privacybeleid";
            contentDialog.CloseButtonText = "Annuleren";
            contentDialog.DefaultButton = ContentDialogButton.Primary;

            contentDialog.PrimaryButtonCommand = new RelayCommand(async _ => await CreateAccount());
            contentDialog.SecondaryButtonCommand = new RelayCommand(async _ => await ShowPrivacyPolicy());

            await contentDialog.ShowAsync();
        }

        private async Task CreateAccount()
        {
            var token = await networkAPI.CreateAccount(FirstName, LastName, EmailAddress, Username, Password, "merchant");

            if (string.IsNullOrWhiteSpace(token))
            {
                await MessageUtils.ShowDialog("Handelaar account aanmaken", "Er is een fout opgetreden tijdens het aanmaken van een account.");
                return;
            }

            NavigateToLogin();
            await MessageUtils.ShowDialog("Handelaar account aanmaken", "Uw account werd succesvol aangemaakt. Welkom bij Stapp!");
        }

        private async Task ShowPrivacyPolicy()
        {
            string uriToLaunch = @"https://technology-salesman-toolkit.firebaseapp.com/privacy_policy_stapp.html";
            var uri = new Uri(uriToLaunch);

            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void NavigateToLogin() => mainPageViewModel.NavigateTo(new LoginViewModel(mainPageViewModel));
    }
}
