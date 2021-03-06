using AutoMapper;
using EmailService;
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
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public CuentaController(IMapper mapper, UserManager<User> userManager, IEmailSender emailSender, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailSender = emailSender;
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
            var confirmationLink = Url.Action(nameof(Registrar), "Cuenta", new { token, email = user.Email }, Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
            //await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRoleAsync(user, "Administrador");
            return Ok();
        }

        [HttpGet]
        [Route("ConfirmarCorreo")]
        public async Task<IActionResult> ConfirmarCorreo(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok();
            else
                return NoContent();
        }

        [HttpGet]
        [Route("OlvidoContrasena")]
        public async Task<IActionResult> OlvidoContrasena(string email)
        {
            if (String.IsNullOrEmpty(email))
                return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = token.Replace("/","%2F");
            var callback = $"{Environment.GetEnvironmentVariable("WEB_URL")}recoverpass/{token}/{email}";
            var message = new Message(new string[] { user.Email }, "Recuperar Contraseña", callback);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost]
        [Route("RecuperarContrasena")]
        public async Task<IActionResult> RecuperarContrasena(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.email);

            if (user == null)
                return NotFound();

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.token, resetPasswordModel.password);

            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return NotFound();
            }

            return Ok();
        }
    }
}
