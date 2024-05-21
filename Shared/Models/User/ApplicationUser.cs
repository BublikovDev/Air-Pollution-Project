using Microsoft.AspNetCore.Identity;
using Shared.Models.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? Lastname { get; set; }

        public string? Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<Country> Countries { get; set; }


        public bool? IsDeleted { get; set; } = false;

        public bool DebugMode { get; set; } = false;
    }
}
