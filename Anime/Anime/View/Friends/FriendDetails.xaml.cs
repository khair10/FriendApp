using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendDetails : ContentPage
    {
        FriendDetailsViewModel friendDetailsViewModel;
        public FriendDetails(Friend item)
        {
            friendDetailsViewModel = new FriendDetailsViewModel(item);
            InitializeComponent();
            BindingContext = friendDetailsViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await friendDetailsViewModel.LoadFriend();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new EditFriendPage(friendDetailsViewModel.item));
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                {
                    return;
                }
                friendDetailsViewModel.MakePhoto(photo.FullPath);
            }
            catch (Exception ex) { }
        }
    }
}