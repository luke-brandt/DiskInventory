using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DiskInventory.Models
{
    public partial class Disk
    {
        public Disk()
        {
            DiskHasArtists = new HashSet<DiskHasArtist>();
            DiskHasBorrowers = new HashSet<DiskHasBorrower>();
        }

        public int DiskId { get; set; }
        [Required]
        public string DiskName { get; set; }
        public int GenreId { get; set; }
        public int DiskTypeId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int StatusId { get; set; }

        public virtual DiskType DiskType { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<DiskHasArtist> DiskHasArtists { get; set; }
        public virtual ICollection<DiskHasBorrower> DiskHasBorrowers { get; set; }
    }
}
