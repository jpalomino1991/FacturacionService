using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacturacionService.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Ingrese razón social")]
        public string RazonSocial { get; set; }
        [Required(ErrorMessage = "Ingrese número de documento")]
        public string NumeroDocumento { get; set; }
        [Required(ErrorMessage = "Ingrese correo electrónico")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
