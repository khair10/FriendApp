using Firebase.Database;
using System;
using System.Collections.Generic;
using Firebase.Database.Query;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{

    
    public class RemoteStorage
    {
        FirebaseClient FirebaseClient;

        public RemoteStorage() {
            FirebaseClient = new FirebaseClient(Constants.DATABASE_URL);
        }

        public async Task<List<Friend>> GetAllFriends()
        {
            IReadOnlyCollection<FirebaseObject<Friend>> entry;
            entry = await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                .Child("Friends")
                .OnceAsync<Friend>();
            return entry.Select(item => {
                var friend = new Friend();
                friend.Name = item.Object.Name;
                friend.Hobby = item.Object.Hobby;
                friend.Bio = item.Object.Bio;
                friend.BirthDay = item.Object.BirthDay;
                friend.PhotoPath = item.Object.PhotoPath;
                friend.LastUpdate = item.Object.LastUpdate;
                return friend;
            }).ToList();
        }

        private int getLastUpdate()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone) {
                return LastUpdateEnum.PHONE;
            } else {
                return LastUpdateEnum.DESKTOP;
            }
        }

        public async Task AddFrined(Friend friend)
        {
            var toUpdatePerson = (await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
             .Child("Friends")
             .OnceAsync<Friend>())
             .Where(a => a.Object.Name == friend.Name)
             .FirstOrDefault();

            if (toUpdatePerson == null)
            {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                .Child("Friends")
                .PostAsync(friend);
            }
            else {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
                  .Child("Friends")
                  .Child(toUpdatePerson.Key)
                  .PutAsync(friend);
            }
        }

        public async Task<Friend> GetPerson(string id)
        {
            var allPersons = await GetAllFriends();
            return allPersons.FirstOrDefault(a => a.Name == id);
        }

        public async Task DeletePerson(string id)
        {
            var toDeletePerson = (await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string)
              .Child("Friends")
              .OnceAsync<Friend>()).Where(a => a.Object.Name == id).FirstOrDefault();
            if (toDeletePerson != null)
            {
                await FirebaseClient
                .Child(App.Current.Properties["NAME"] as string).Child("Friends").Child(toDeletePerson.Key).DeleteAsync();
            }
        }
    }
}
