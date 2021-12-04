using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DiskInventory.Models
{
    public partial class DiskHasBorrower
    {
        public int DiskHasBorrowerId { get; set; }
        [Required]
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        [Required]
        public int DiskId { get; set; }
        [Required]
        public int BorrowerId { get; set; }

        public virtual Borrower Borrower { get; set; }
        public virtual Disk Disk { get; set; }
    }
}
