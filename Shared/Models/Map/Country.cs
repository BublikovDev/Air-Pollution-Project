using Shared.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Map
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;


        public List<Location> Locations { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
