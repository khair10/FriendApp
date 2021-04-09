using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anime
{
    public class PlacesRepository
    {
        PlacesLocalStorage localStorage;
        PlacesRemoteStorage remoteStorage;

        public PlacesRepository(PlacesLocalStorage localStorage, PlacesRemoteStorage remoteStorage)
        {
            this.localStorage = localStorage;
            this.remoteStorage = remoteStorage;
        }

        public async Task<IEnumerable<Place>> getPlaces()
        {
            var items = await remoteStorage.GetAllPlaces();
            var itemss = await localStorage.RefreshPlaces(items);
            return itemss;
        }
        
        public async Task<Place> getPlace(string id)
        {
            var item = await localStorage.GetItemAsync(id);
            return item;
        }

        public async Task<string> DeletePlace(string id)
        {
            var index = await localStorage.DeleteItemAsync(id);
            await remoteStorage.DeletePlace(id);
            return index;
        }

        public async Task<string> Save(Place item)
        {
            var i = await localStorage.SaveItemAsync(item);
            await remoteStorage.AddPlace(item);
            return i;
        }

        public async Task<string> Update(Place item)
        {
            var i = await localStorage.UpdateItemAsync(item);
            await remoteStorage.AddPlace(item);
            return i;
        }
    }
}
