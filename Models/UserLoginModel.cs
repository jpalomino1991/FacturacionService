using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacturacionService.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Ingrese correo electrónico")]
        [EmailAddress]
        public string email { get; set; }
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
