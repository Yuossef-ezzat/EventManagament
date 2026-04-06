using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.RegisrationDtos
{
    public class CancleRegisrationDto
    {
        [Required]
        public int RegistrationId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
