using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class User
    {
        public long? ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Propriedade INT do enum SystemPermissionEnum
        /// </summary>
        public int Permission { get; set; }
        //public string EmailAddress { get; set; }
    }
}
