using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Noclegi.Models
{
    public class CustomerTB
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
