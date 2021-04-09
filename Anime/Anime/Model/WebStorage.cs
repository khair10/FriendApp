using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Anime
{
    public class WebStorage
    {
        
        FirebaseStorage FirebaseStorage;
        public WebStorage()
        {
            FirebaseStorage = new FirebaseStorage(Constants.STORAGE_URL);
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await FirebaseStorage
                .Child(App.Current.Properties["NAME"] as string)
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
    }
}
