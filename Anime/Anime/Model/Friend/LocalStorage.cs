using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{
    public class LocalStorage
    {
        SQLiteAsyncConnection database;
        public LocalStorage(SQLiteAsyncConnection database)
        {
            this.database = database;
            database.CreateTableAsync<Friend>().Wait();
        }

        public async Task<IEnumerable<Friend>> GetItemsAsync()
        {
            return await database.Table<Friend>().ToListAsync();
        }
        public async Task<Friend> GetItemAsync(string id)
        {
            return await database.GetAsync<Friend>(id);
        }

        public async Task<IEnumerable<Friend>> RefreshFriends(List<Friend> items)
        {
            await database.RunInTransactionAsync(conn =>
               {
                   //conn.DeleteAll<Friend>();
                   //conn.ExecuteAsync("DELETE FROM Items");
                   //conn.InsertOrReplace(items);
                   conn.DeleteAll<Friend>();
                   conn.InsertAll(items);
               }
            );
            return await database.Table<Friend>().ToListAsync();
        }

        public async Task<string> DeleteItemAsync(string id)
        {
            await database.DeleteAsync<Friend>(id);
            return id;
        }
        public async Task<string> SaveItemAsync(Friend item)
        {
            await database.InsertAsync(item);
            return item.Name;
        }

        public async Task<string> UpdateItemAsync(Friend item) {
            await database.UpdateAsync(item);
            return item.Name;
        }
        private int getLastUpdate()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                return LastUpdateEnum.DESKTOP;
            }
            else
            {
                return LastUpdateEnum.PHONE;
            }
        }

    }
}
