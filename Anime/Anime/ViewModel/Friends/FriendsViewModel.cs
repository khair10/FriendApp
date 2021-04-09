using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anime
{
    public class FriendsViewModel: INotifyPropertyChanged
    {
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    RefreshData();
                });
            }
        }
        private FriendRepository repository;
        private ObservableCollection<Friend> friends = new ObservableCollection<Friend>(); 
        public ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Friends"));
                }
            }
        }

        public FriendsViewModel() {
            repository = App.Database;
            LoadData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void LoadData() {
            var items = await repository.getFriends();
            Friends = new ObservableCollection<Friend>(items);
        }

        public async void RefreshData()
        {
            IsRefreshing = true;
            var items = await repository.getFriends();
            Friends = new ObservableCollection<Friend>(items);
            IsRefreshing = false;
        }

        public async Task<string> delete(Friend item) {
            Friends.Remove(item);
            var i = await repository.DeleteFriend(item.Name);
            return i;
        }
    }
}
