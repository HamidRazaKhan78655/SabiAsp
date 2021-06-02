//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SabiAsp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public item()
        {
            this.UserItemCards = new HashSet<UserItemCard>();
        }
    
        public int ItemId { get; set; }
        public Nullable<int> SubCategorieId { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string rating { get; set; }
        public string location { get; set; }
        public string Weight { get; set; }
        public string Time { get; set; }
        public string Price { get; set; }
        public string DeliveryFee { get; set; }
        public string minOrder { get; set; }
        public Nullable<int> itemAvailible { get; set; }
        public Nullable<int> SoldItems { get; set; }
        public string isDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Shopid { get; set; }
    
        public virtual SubCategory SubCategory { get; set; }
        public virtual Shop Shop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserItemCard> UserItemCards { get; set; }
    }
}
