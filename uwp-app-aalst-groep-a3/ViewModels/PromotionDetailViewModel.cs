using System;
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

        private void CheckMerchantOwnsPromotion()
        {
            try
            {
                var role = UserUtils.GetUserRole();
                if (role.ToLower() == "merchant")
                {
                    // HIER CODE TOEVOEGEN OM TE ZIEN OF DE MERCHANT EIGENAAR IS

                    EditPromotionCommand = new RelayCommand(_ => EditPromotion());
                    DeletePromotionCommand = new RelayCommand(_ => DeletePromotionDialog());
                }
            }
            catch { }
        }

        private void EditPromotion() { }

        private async void DeletePromotionDialog()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "Promotie verwijderen";
            contentDialog.Content = "Bent u zeker dat u deze promotie wilt verwijderen?";
            contentDialog.PrimaryButtonText = "Ja";
            contentDialog.CloseButtonText = "Nee";

            contentDialog.PrimaryButtonCommand = new RelayCommand(_ => DeleteEvent());

            await contentDialog.ShowAsync();
        }

        private void DeleteEvent() { }

    }
}
