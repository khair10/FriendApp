using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Anime
{
    public class FriendRepository
    {
        LocalStorage localStorage;
        RemoteStorage remoteStorage;

        public FriendRepository(LocalStorage localStorage, RemoteStorage remoteStorage)
        {
            this.localStorage = localStorage;
            this.remoteStorage = remoteStorage;
        }

        public async Task<IEnumerable<Friend>> getFriends()
        {
            var items = await remoteStorage.GetAllFriends();
            var itemss = await localStorage.RefreshFriends(items);
            return itemss;
        }

        public async Task SyncFriends()
        {
            var itemsNet = await remoteStorage.GetAllFriends();
            foreach (var item in itemsNet)
            {
                await localStorage.SaveItemAsync(item);
            }
        }

        public async Task<Friend> getFriend(string id)
        {
            var item = await localStorage.GetItemAsync(id);
            return item;
        }

        public async Task<string> DeleteFriend(string id)
        {
            var index = await localStorage.DeleteItemAsync(id);
            await remoteStorage.DeletePerson(id);
            return index;
        }

        public async Task<string> Save(Friend item)
        {
            var i = await localStorage.SaveItemAsync(item);
            await remoteStorage.AddFrined(item);
            return i;
        }

        public async Task<string> Update(Friend item)
        {
            var i = await localStorage.UpdateItemAsync(item);
            await remoteStorage.AddFrined(item);
            return i;
        }
    }
}
