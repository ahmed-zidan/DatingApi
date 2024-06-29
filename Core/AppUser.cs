﻿using Microsoft.AspNetCore.Identity;


namespace Core
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }

    }
}
