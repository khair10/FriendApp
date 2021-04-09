using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{
    class FriendDetailsViewModel: INotifyPropertyChanged
    {

        public string name;
        public string hobby;
        public string bio;

        DateTime birthDay;
        public Friend item;
        FriendRepository repository;
        public event PropertyChangedEventHandler PropertyChanged;
        public FriendDetailsViewModel(Friend item) {
            this.item = item;
            BirthDay = DateTime.Parse(item.BirthDay);
            repository = App.Database;
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

        public DateTime BirthDay
        {
            set
            {
                if (birthDay != value)
                {
                    birthDay = value;
                    item.BirthDay = birthDay.ToString();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BirthDay"));
                }
            }
            get
            {
                return birthDay;
            }
        }

        public string Hobby
        {
            set
            {
                if (hobby != value)
                {
                    hobby = value;
                    item.Hobby = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hobby"));
                }
            }
            get
            {
                return hobby;
            }
        }

        public string Bio
        {
            set
            {
                if (bio != value)
                {
                    bio = value;
                    item.Bio = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bio"));
                }
            }
            get
            {
                return bio;
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

        public async Task LoadFriend()
        {
            var item = await repository.getFriend(this.item.Name);
            Name = item.Name;
            Bio = item.Bio;
            Hobby = item.Hobby;
            BirthDay = DateTime.Parse(item.BirthDay);
            PhotoPath = item.PhotoPath;
        }
    }
}
