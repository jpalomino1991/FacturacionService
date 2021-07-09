using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacturacionService.Models
{
    public class AccesoAnonimoModel
    {
        [Required(ErrorMessage = "Ingrese serie de comprobante")]
        [Display(Name = "Serie")]
        public string serie { get; set; }
        [Required(ErrorMessage = "Ingrese número de comprobante")]
        [Display(Name = "Número")]
        public int numero { get; set; }
        [Required(ErrorMessage = "Ingrese monto total")]
        [Display(Name = "Monto Total")]
        public decimal monto { get; set; }
        [Required(ErrorMessage = "Ingrese fecha de emisión")]
        [DataType(DataType.Date)]
        public string fecha { get; set; }
        public string error { get; set; }
    }
}
