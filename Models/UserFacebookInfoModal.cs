using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SabiAsp.Models
{
    public class UserFacebookInfoModal
    {
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
}