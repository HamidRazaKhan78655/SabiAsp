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
    
    public partial class UserChat
    {
        public int ChatId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string Message { get; set; }
        public string isDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual user user { get; set; }
        public virtual vendor vendor { get; set; }
    }
}
