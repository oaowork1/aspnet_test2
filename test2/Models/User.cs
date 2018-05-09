using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace test2.Models
{
    public enum Role
    {
        Admin, User
    }
    [DataContract]  
    public class User
    {

        [DataMember]
        public string email { get; set; }


        [IgnoreDataMember]
        public string password { get; set; }

        [DataMember]
        public string firstName { get; set; }

        [DataMember]
        public string lastName { get; set; }

        [DataMember]
        public Role role { get; set; }
    }    
}