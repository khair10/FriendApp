using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Anime
{
    public class PlacesLocalStorage
    {
        SQLiteAsyncConnection database;
        public PlacesLocalStorage(SQLiteAsyncConnection database)
        {
            this.database = database;
            database.CreateTableAsync<Place>().Wait();
        }

        public async Task<IEnumerable<Place>> GetItemsAsync()
        {
            return await database.Table<Place>().ToListAsync();
        }
        public async Task<Place> GetItemAsync(string id)
        {
            return await database.GetAsync<Place>(id);
        }

        public async Task<IEnumerable<Place>> RefreshPlaces(List<Place> items)
        {
            await database.RunInTransactionAsync(conn =>
            {
                conn.DeleteAll<Place>();
                conn.InsertAll(items);
            }
            );
            return await database.Table<Place>().ToListAsync();
        }

        public async Task<string> DeleteItemAsync(string id)
        {
            await database.DeleteAsync<Place>(id);
            return id;
        }
        public async Task<string> SaveItemAsync(Place item)
        {
            await database.InsertAsync(item);
            return item.Name;
        }

        public async Task<string> UpdateItemAsync(Place item)
        {
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
