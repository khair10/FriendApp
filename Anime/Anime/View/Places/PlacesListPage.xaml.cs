using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlacesListPage : ContentPage
    {
        public PlacesViewModel PlacesViewModel;

        public PlacesListPage()
        {
            InitializeComponent();
            PlacesViewModel = new PlacesViewModel();
            PlacesViewModel.LoadData();
            BindingContext = PlacesViewModel;
        }

        override protected void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<MessagesService2>(this, "Upda", (sender) =>
            {
                PlacesViewModel.LoadData();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //MessagingCenter.Unsubscribe<MessagesService2>(this, "Upda");
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new PlaceDetailsPage(e.Item as Place));
            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
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
            var delete = await DisplayAlert("Delete Context Action", (mi.CommandParameter as Place).Name + " delete context action", "OK", "CANCEL");
            if (delete)
            {
                await PlacesViewModel.delete((mi.CommandParameter as Place));
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPlacePage());
        }
    }
}
