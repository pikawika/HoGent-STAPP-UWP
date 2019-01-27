﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_app_aalst_groep_a3.Base;
using uwp_app_aalst_groep_a3.Models;
using uwp_app_aalst_groep_a3.Network;
using uwp_app_aalst_groep_a3.Utils;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uwp_app_aalst_groep_a3.ViewModels
{
    public class PromotionDetailViewModel : ViewModelBase
    {
        public Promotion Promotion { get; set; }
        private MainPageViewModel mainPageViewModel;
        private NetworkAPI networkAPI = new NetworkAPI();

        public RelayCommand ShowPromotionCommandClicked { get; set; }
        public RelayCommand DownloadCouponCommandClicked { get; set; }

        private Visibility _merchantVisibility = Visibility.Collapsed;
        public Visibility MerchantVisibility
        {
            get { return _merchantVisibility; }
            set { _merchantVisibility = value; RaisePropertyChanged(nameof(MerchantVisibility)); }
        }

        public RelayCommand EditPromotionCommand { get; set; }
        public RelayCommand DeletePromotionCommand { get; set; }

        public PromotionDetailViewModel(Promotion promotion, MainPageViewModel mainPageViewModel)
        {
            Promotion = promotion;
            this.mainPageViewModel = mainPageViewModel;

            ShowPromotionCommandClicked = new RelayCommand(async (object args) => await ShowPromotionAsync());
            DownloadCouponCommandClicked = new RelayCommand(async _ => await DownloadCoupon());

            EditPromotionCommand = new RelayCommand(_ => EditPromotion());
            DeletePromotionCommand = new RelayCommand(async _ => await DeletePromotionDialog());

            CheckMerchantOwnsPromotion();
        }

        private async Task ShowPromotionAsync()
        {
            NetworkAPI networkAPI = new NetworkAPI();
            Establishment establishment = await networkAPI.GetEstablishmentById(Promotion.Establishment.EstablishmentId);
            mainPageViewModel.NavigateTo(new EstablishmentDetailViewModel(establishment, mainPageViewModel));
        }

        private async Task DownloadCoupon()
        {
            try
            {
                Uri source = new Uri("https://localhost:44315/" + Promotion.Attachments[0].Path);

                StorageFile destinationFile = await DownloadsFolder.CreateFileAsync(
                    $"Stapp_Kortingsbon.pdf",
                    CreationCollisionOption.GenerateUniqueName);

                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(source, destinationFile);

                await download.StartAsync();

                await Launcher.LaunchFileAsync(destinationFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Download Error", ex.Message);
            }
        }

        private async void CheckMerchantOwnsPromotion()
        {
            try
            {
                var role = UserUtils.GetUserRole();
                if (role.ToLower() == "merchant")
                {
                    bool isOwner = await networkAPI.IsOwnerOfPromotion(Promotion.PromotionId);

                    if (isOwner)  MerchantVisibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void EditPromotion() => mainPageViewModel.NavigateTo(new MerchantEditViewModel(MerchantObjectType.PROMOTION, mainPageViewModel, null, null, Promotion));

        private async Task DeletePromotionDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Promotie verwijderen";
            contentDialog.Content = "Bent u zeker dat u deze promotie wilt verwijderen?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(async _ => await DeleteEvent());

            await contentDialog.ShowAsync();
        }

        private async Task DeleteEvent() {
            var message = await networkAPI.DeletePromotion(Promotion.PromotionId);
            await MessageUtils.ShowDialog("Promotie verwijderen", message.Item1);
            if (message.Item2)
            {
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.BackButtonPressed();
                mainPageViewModel.NavigationHistoryItems.RemoveAll(v => v.GetType() == typeof(PromotionDetailViewModel) || v.GetType() == typeof(PromotionsViewModel));
                mainPageViewModel.NavigateTo(new PromotionsViewModel(mainPageViewModel));
            }
        }

    }
}
