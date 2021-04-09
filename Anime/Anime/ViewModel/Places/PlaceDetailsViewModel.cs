using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{
    class PlaceDetailsViewModel : INotifyPropertyChanged
    {

        public string name;
        public string info;
        public double lattitude;
        public double longitude;
        public string photoPath;

        public Place item;
        PlacesRepository repository;
        public event PropertyChangedEventHandler PropertyChanged;
        public PlaceDetailsViewModel(Place item)
        {
            this.item = item;
            repository = App.PlacesRepository;
        }

        public string Name
        {
            set
            {
                if (name != value)
                {
                    item.Name = value;
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
            get
            {
                return name;
            }
        }

        internal async void MakePhoto(string fullPath)
        {
            PhotoPath = fullPath;
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
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                item.LastUpdate = LastUpdateEnum.PHONE;
            }
            else
            {
                item.LastUpdate = LastUpdateEnum.DESKTOP;
            }
            await repository.Update(item);
        }

        public string Info
        {
            set
            {
                if (info != value)
                {
                    info = value;
                    item.Info = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Info"));
                }
            }
            get
            {
                return info;
            }
        }

        public double Lattitude
        {
            set
            {
                if (lattitude != value)
                {
                    lattitude = value;
                    item.Lattitude = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lattitude"));
                }
            }
            get
            {
                return lattitude;
            }
        }
        public double Longitude
        {
            set
            {
                if (longitude != value)
                {
                    longitude = value;
                    item.Longitude  = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Longitude"));
                }
            }
            get
            {
                return longitude;
            }
        }

        public string PhotoPath
        {
            set
            {
                if (item.PhotoPath != value)
                {
                    photoPath = value;
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

        public async Task LoadPlace()
        {
            var item = await repository.getPlace(this.item.Name);
            Name = item.Name;
            Info = item.Info;
            Longitude = item.Longitude;
            Lattitude = item.Lattitude;
            PhotoPath = item.PhotoPath;
        }
    }
}
