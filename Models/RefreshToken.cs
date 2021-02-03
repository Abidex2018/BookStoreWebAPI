using System;
using System.Collections.Generic;

namespace BookStoreWebAPI.Models
{
    public partial class RefreshToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }

        public virtual User User { get; set; }
    }
}
