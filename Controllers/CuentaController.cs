using AutoMapper;
using FacturacionService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacturacionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CuentaController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public CuentaController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
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

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Registrar(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            var user = _mapper.Map<User>(userModel);
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(userModel);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmarCorreo), "Cuenta", new { token, email = user.Email }, Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
            //await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRoleAsync(user, "Administrador");
            return Ok();
        }
    }
}
