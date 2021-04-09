using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anime
{
    [Table("Places")]
    public class Place
    {
        [PrimaryKey, Column("_id")]
        public string Name { get; set; }
        public string Info { get; set; } = "Empty";
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int LastUpdate { get; set; } = LastUpdateEnum.PHONE;
        public string PhotoPath { get; set; } = Constants.DEFAULT_AVATAR_URL;
    }
}
