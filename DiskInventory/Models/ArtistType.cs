using System;
using System.Collections.Generic;

#nullable disable

namespace DiskInventory.Models
{
    public partial class ArtistType
    {
        public ArtistType()
        {
            Artists = new HashSet<Artist>();
        }

        public int ArtistTypeId { get; set; }
        public string ArtistTypeDescription { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
