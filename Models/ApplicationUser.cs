﻿using Microsoft.AspNetCore.Identity;

namespace Identity1.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string City { get; set;}
    }
}
