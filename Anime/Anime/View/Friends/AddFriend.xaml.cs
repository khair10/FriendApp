using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFriend : ContentPage
    {

        AddFriendViewModel AddFriendViewModel;
        public AddFriend()
        {
            InitializeComponent();
            AddFriendViewModel = new AddFriendViewModel();
            BindingContext = AddFriendViewModel;
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
                AddFriendViewModel.PhotoPath = photo.FullPath;
            }
            catch (Exception ex)
            {
            }
        }
    }
}