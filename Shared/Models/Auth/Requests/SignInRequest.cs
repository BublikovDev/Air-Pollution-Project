﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Auth.Requests
{
    public class SignInRequest
    {
        [Required(ErrorMessage = "Enter the username or email")]
        public string? UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string? Password { get; set; }
    }
}
