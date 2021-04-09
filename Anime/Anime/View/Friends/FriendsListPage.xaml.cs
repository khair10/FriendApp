using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendsListPage : ContentPage
    {
        public FriendsViewModel FriendsViewModel;

        public FriendsListPage()
        {
            InitializeComponent();
            FriendsViewModel = new FriendsViewModel();
            FriendsViewModel.LoadData();
            BindingContext = FriendsViewModel;
        }

        override protected void OnAppearing() {
            base.OnAppearing();
            MessagingCenter.Subscribe<MessagesService>(this, "Update", (sender) =>
            {
                FriendsViewModel.LoadData();
            });
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            await Navigation.PushAsync(new FriendDetails(e.Item as Friend));
            ((ListView)sender).SelectedItem = null;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGray;
            }
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var delete = await DisplayAlert("Delete Context Action", (mi.CommandParameter as Friend).Name + " delete context action", "OK", "CANCEL");
            if (delete)
            {
                await FriendsViewModel.delete((mi.CommandParameter as Friend));
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFriend());
        }
    }
}
