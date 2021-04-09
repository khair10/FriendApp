using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage, INotifyPropertyChanged
    {

        public string name;
        public string Name {
            get {
                return name;
            }
            set {
                if (name != value)
                {
                    name = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public Page1()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Name != null && Name != "")
            {
                App.Current.Properties.Add("NAME", Name);
                await App.Current.MainPage.Navigation.PushAsync(new MainPage());
                App.Current.MainPage.Navigation.RemovePage(this);
            }
            else {
                DisplayAlert("Error", "Name is empty", "OK");
            }
        }
    }
}