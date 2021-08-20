using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacturacionService.Models
{
    public class ResetPasswordModel
    {
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}
