using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Anime
{
    class EditFriendViewModel: INotifyPropertyChanged
    {

        public DateTime birthDay;
        Friend item;
        FriendRepository repository;
        public ICommand SubmitCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public EditFriendViewModel(Friend item)
        {
            this.item = item;
            //Name = item.Name;
            //Bio = item.Bio;
            //Hobby = item.Hobby;
            BirthDay = DateTime.Parse(item.BirthDay);
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

        async void Save()
        {
            if (Name != null && !Name.Equals(""))
            {
                if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                {
                    item.LastUpdate = LastUpdateEnum.PHONE;
                }
                else
                {
                    item.LastUpdate = LastUpdateEnum.DESKTOP;
                }
                await repository.Update(item);
                await App.Current.MainPage.Navigation.PopAsync();
                await App.Current.MainPage.DisplayAlert("Success", "Friend was saved", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Name and SecondName should be written", "OK");
            }
        }
    }
}
