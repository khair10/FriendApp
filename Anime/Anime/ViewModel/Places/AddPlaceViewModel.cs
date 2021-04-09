using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Anime
{
    public class AddPlaceViewModel : INotifyPropertyChanged
    {
        Place item;
        PlacesRepository repository;
        public ICommand SubmitCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AddPlaceViewModel()
        {
            this.item = new Place();
            repository = App.PlacesRepository;
            SubmitCommand = new Command(
                execute: () =>
                {
                    Save();
                },
                canExecute: () =>
                {
                    return true;
                });
        }

        public string Name
        {
            set
            {
                if (item.Name != value)
                {
                    item.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
            get
            {
                return item.Name;
            }
        }

        public string Info
        {
            set
            {
                if (item.Info != value)
                {
                    item.Info = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Info"));
                }
            }
            get
            {
                return item.Info;
            }
        }

        public string PhotoPath
        {
            set
            {
                if (item.PhotoPath != value)
                {
                    item.PhotoPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PhotoPath"));
                }
            }
            get
            {
                if (item.PhotoPath == null || item.PhotoPath == "")
                {
                    return Constants.DEFAULT_AVATAR_URL;
                }
                return item.PhotoPath;
            }
        }

        async void Save()
        {
            if (Name != null && !Name.Equals(""))
            {
                string newFile;
                if (PhotoPath != Constants.DEFAULT_AVATAR_URL)
                {
                    using (var stream = File.OpenRead(PhotoPath))
                        newFile = await (new WebStorage()).UploadFile(stream, Name + Path.GetExtension(PhotoPath));
                }
                else
                {
                    newFile = PhotoPath;
                }
                PhotoPath = newFile;
                try
                {
                    if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                    {
                        item.LastUpdate = LastUpdateEnum.PHONE;
                    }
                    else
                    {
                        item.LastUpdate = LastUpdateEnum.DESKTOP;
                    }
                    var geo = await getGeolocation();
                    item.Longitude = geo.Longitude;
                    item.Lattitude = geo.Latitude;
                    await repository.Save(item);
                    MessagingCenter.Send<MessagesService2>(App.PlaceObj, "Upda");
                    await App.Current.MainPage.Navigation.PopAsync();
                    await App.Current.MainPage.DisplayAlert("Success", "Friend was saved", "OK");
                }
                catch (SQLite.SQLiteException ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Friend was already exist", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Name and SecondName should be written", "OK");
            }
        }

        private async Task<Location> getGeolocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    return location;
                }
                return new Location();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                return new Location();
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return new Location();
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return new Location();
            }
            catch (Exception ex)
            {
                // Unable to get location
                return new Location();
            }
        }
    }
}
