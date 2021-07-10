using FacturacionService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FacturacionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CuentaController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CuentaController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return NotFound(userModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userModel.email, userModel.password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userModel.email);
                return Ok(user);
            }
            else
            {
                return NotFound("Correo o usuario inválido");
            }
        }
    }
}
