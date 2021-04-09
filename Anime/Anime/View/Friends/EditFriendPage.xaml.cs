using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFriendPage : ContentPage
    {

        EditFriendViewModel viewModel;
        public EditFriendPage(Friend item)
        {
            viewModel = new EditFriendViewModel(item);
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}