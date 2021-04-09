using SQLite;

namespace Anime
{
    [Table("Items")]
    public class Friend
    {
        //public string Id { get; set; }
        [PrimaryKey, Column("_id")]
        public string Name { get; set; }
        public string BirthDay { get; set; }
        public string Hobby { get; set; } = "Empty";
        public string Bio { get; set; } = "Empty";
        public int LastUpdate { get; set; } = LastUpdateEnum.PHONE;
        public string PhotoPath { get; set; } = Constants.DEFAULT_AVATAR_URL;
    }
}
