using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace BookStoreWebAPI.Models
{
    public class UserWithToken : User
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }


        public UserWithToken(User user)
        {
            this.EmailAddress = user.EmailAddress;
            this.UserId = user.UserId;
            this.Password = user.Password;
            this.FirstName = user.FirstName;
            this.MiddleName = user.MiddleName;
            this.HireDate = user.HireDate;
            this.LastName = user.LastName;
            this.PubId = user.PubId;
            this.JobLevel = user.JobLevel;
            this.JobId = user.JobId;
            this.Job = user.Job;
            this.Pub = user.Pub;
        }

       
       
       
        
    }
}
