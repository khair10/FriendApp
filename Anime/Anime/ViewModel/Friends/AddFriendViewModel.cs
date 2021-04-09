using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Anime
{
    class AddFriendViewModel : INotifyPropertyChanged
    {
        DateTime birthDay;
        Friend item;
        FriendRepository repository;
        public ICommand SubmitCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AddFriendViewModel() {
            this.item = new Friend();
            repository = App.Database;
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
                if (item.Hobby != value)
                {
                    item.Hobby = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hobby"));
                }
            }
            get
            {
                return item.Hobby;
            }
        }

        public string Bio
        {
            set
            {
                if (item.Bio != value)
                {
                    item.Bio = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bio"));
                }
            }
            get
            {
                return item.Bio;
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

        async void Save() {
            if (Name != null && !Name.Equals(""))
            {
                string newFile;
                if (PhotoPath != Constants.DEFAULT_AVATAR_URL)
                {
                    using (var stream = File.OpenRead(PhotoPath))
                        newFile = await (new WebStorage()).UploadFile(stream, Name + Path.GetExtension(PhotoPath));
                }
                else {
                    newFile = PhotoPath;
                }
                PhotoPath = newFile;
                try
                {
                    if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                    {
                        item.LastUpdate = LastUpdateEnum.PHONE;
                    }
                    else {
                        item.LastUpdate = LastUpdateEnum.DESKTOP;
                    }
                    await repository.Save(item);
                    MessagingCenter.Send<MessagesService>(App.Obj, "Update");
                    await App.Current.MainPage.Navigation.PopAsync();
                    await App.Current.MainPage.DisplayAlert("Success", "Friend was saved", "OK");
                }
                catch (SQLite.SQLiteException ex) {
                    await App.Current.MainPage.DisplayAlert("Error", "Friend was already exist", "OK");
                }
            }
            else {
                await App.Current.MainPage.DisplayAlert("Error", "Name and SecondName should be written", "OK");
            }
        }
    }

}
