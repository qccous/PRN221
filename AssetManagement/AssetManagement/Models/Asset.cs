using System;
using System.Collections.Generic;

namespace AssetManagement.Models
{
    public partial class Asset
    {
        public Asset()
        {
            BorrowingAssets = new HashSet<BorrowingAsset>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? AssetName { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public string? Specification { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<BorrowingAsset> BorrowingAssets { get; set; }
    }
}
