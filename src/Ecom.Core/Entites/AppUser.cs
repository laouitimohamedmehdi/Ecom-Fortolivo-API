﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites
{
    public class AppUser:IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
