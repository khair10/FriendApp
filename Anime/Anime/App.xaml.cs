using SQLite;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anime
{
    public partial class App : Application
    {
        private const string DATABASE_NAME = "items.db";

        private static MessagesService obj;
        public static MessagesService Obj {
            get {
                if (obj == null) {
                    obj = new MessagesService();
                }
                return obj;
            }
        }

        private static MessagesService2 placeObj;
        public static MessagesService2 PlaceObj
        {
            get
            {
                if (placeObj == null)
                {
                    placeObj = new MessagesService2();
                }
                return placeObj;
            }
        }

        private static RemoteStorage remote;
        private static RemoteStorage Remote {
            get {
                if (remote == null)
                {
                    remote = new RemoteStorage();
                }
                return remote;
            }
        }

        private static SQLiteAsyncConnection connection;
        private static SQLiteAsyncConnection Connection
        {
            get {
                if (connection == null) 
                {
                    connection = new SQLiteAsyncConnection(Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return connection;
            }
        } 
        private static FriendRepository database;
        public static FriendRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new FriendRepository(new LocalStorage(Connection), Remote);
                }
                return database;
            }
        }

        private static PlacesRepository placesRepository;
        public static PlacesRepository PlacesRepository
        {
            get {
                if (placesRepository == null)
                {
                    placesRepository = new PlacesRepository(new PlacesLocalStorage(Connection), new PlacesRemoteStorage());
                }
                return placesRepository;
            }
        }

        public App()
        {
            InitializeComponent();
            if (App.Current.Properties.ContainsKey("NAME"))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else {
                MainPage = new NavigationPage(new Login());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
