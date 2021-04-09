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
    public partial class PlaceDetailsPage : ContentPage
    {
        PlaceDetailsViewModel placeDetailsViewModel;
        public PlaceDetailsPage(Place item)
        {
            placeDetailsViewModel = new PlaceDetailsViewModel(item);
            InitializeComponent();
            BindingContext = placeDetailsViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await placeDetailsViewModel.LoadPlace();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var viewModel = placeDetailsViewModel;
            var location = new Location(viewModel.Lattitude, viewModel.Longitude);
            await Map.OpenAsync(location);
        }
    }
}