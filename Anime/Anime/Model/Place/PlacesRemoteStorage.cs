using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{
    public class PlacesRemoteStorage
    {

        FirebaseClient FirebaseClient;

        public PlacesRemoteStorage()
        {
            FirebaseClient = new FirebaseClient(Constants.DATABASE_URL);
        }

        public async Task<List<Place>> GetAllPlaces()
        {
            IReadOnlyCollection<FirebaseObject<Place>> entry;
            entry = await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                .Child("Places")
                .OnceAsync<Place>();
            return entry.Select(item => {
                var place = new Place();
                place.Name = item.Object.Name;
                place.Info = item.Object.Info;
                place.LastUpdate = item.Object.LastUpdate;
                place.Longitude = item.Object.Longitude;
                place.Lattitude = item.Object.Lattitude;
                place.PhotoPath = item.Object.PhotoPath;
                return place;
            }).ToList();
        }

        private int getLastUpdate()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                return LastUpdateEnum.PHONE;
            }
            else
            {
                return LastUpdateEnum.DESKTOP;
            }
        }

        public async Task AddPlace(Place place)
        {
            var toUpdatePlace = (await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
             .Child("Places")
             .OnceAsync<Place>())
             .Where(a => a.Object.Name == place.Name)
             .FirstOrDefault();

            if (toUpdatePlace == null)
            {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                .Child("Places")
                .PostAsync(place);
            }
            else
            {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                  .Child("Places")
                  .Child(toUpdatePlace.Key)
                  .PutAsync(place);
            }
        }

        public async Task<Place> GetPlace(string id)
        {
            var allPersons = await GetAllPlaces();
            return allPersons.FirstOrDefault(a => a.Name == id);
        }

        public async Task DeletePlace(string id)
        {
            var toDeletePerson = (await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
              .Child("Places")
              .OnceAsync<Place>()).Where(a => a.Object.Name == id).FirstOrDefault();
            if (toDeletePerson != null)
            {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string).Child("Places").Child(toDeletePerson.Key).DeleteAsync();
            }
        }
    }
}
