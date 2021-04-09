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
    public partial class AddPlacePage : ContentPage
    {
        AddPlaceViewModel AddPlaceViewModel;
        public AddPlacePage()
        {
            InitializeComponent();
            AddPlaceViewModel = new AddPlaceViewModel();
            BindingContext = AddPlaceViewModel;
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
                AddPlaceViewModel.PhotoPath = photo.FullPath;
            }
            catch (Exception ex)
            {
            }
        }
    }
}