using System;
using System.Collections.Generic;

namespace AssetManagement.Models
{
    public partial class User
    {
        public User()
        {
            BorrowingAssets = new HashSet<BorrowingAsset>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? PhoneNumber { get; set; }
        public string? Addrress { get; set; }
        public bool? Status { get; set; }
        public bool? Gender { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<BorrowingAsset> BorrowingAssets { get; set; }
    }
}
