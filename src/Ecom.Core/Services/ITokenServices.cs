﻿using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface ITokenServices
    {
        string CreateToken(AppUser appUser);
    }
}
