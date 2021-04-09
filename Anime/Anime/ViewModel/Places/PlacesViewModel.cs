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
    public class PlacesViewModel : INotifyPropertyChanged
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
        private PlacesRepository repository;
        private ObservableCollection<Place> places = new ObservableCollection<Place>();
        public ObservableCollection<Place> Places
        {
            get
            {
                return places;
            }
            set
            {
                places = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Places"));
                }
            }
        }

        public PlacesViewModel()
        {
            repository = App.PlacesRepository;
            LoadData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void LoadData()
        {
            var items = await repository.getPlaces();
            Places = new ObservableCollection<Place>(items);
        }

        public async void RefreshData()
        {
            IsRefreshing = true;
            var items = await repository.getPlaces();
            Places = new ObservableCollection<Place>(items);
            IsRefreshing = false;
        }

        public async Task<string> delete(Place item)
        {
            Places.Remove(item);
            var i = await repository.DeletePlace(item.Name);
            return i;
        }
    }
}
