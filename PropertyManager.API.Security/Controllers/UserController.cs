using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyManager.API.Security.Common.Interface;
using PropertyManager.API.Security.Domain;
using PropertyManager.API.Security.Domain.Model;
using PropertyManager.API.Security.Persistence;

namespace PropertyManager.API.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService _serviceUser;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserController(UserManager<User> userManager, IConfiguration configuration,
            SignInManager<User> signInManager, IUserService serviceUser,
            ApplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this._serviceUser = serviceUser;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> Get()
        {
            var users = await context.Users.ToListAsync();
            var userModel = mapper.Map<IEnumerable<UserModel>>(users);
            return userModel;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ResponseAutenticationModel>> Registrar(
            UserCredentialModel credentialModel)
        {
            var user = new User
            {
                UserName = credentialModel.Email,
                Email = credentialModel.Email
            };

            var result = await userManager.CreateAsync(user, credentialModel.Password!);

            if (result.Succeeded)
            {
                var response = await BuildToken(credentialModel);
                return response;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return ValidationProblem();
            }
        }

        private async Task<ResponseAutenticationModel> BuildToken(
            UserCredentialModel credentialModel)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credentialModel.Email)                
            };

            var user = await userManager.FindByEmailAsync(credentialModel.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["KeyJwt"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null,
                claims: claims, expires: expiracion, signingCredentials: credenciales);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new ResponseAutenticationModel
            {
                Token = token,
                Expiracion = expiracion
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAutenticationModel>> Login(
            UserCredentialModel credentialModel)
        {
            var usuario = await userManager.FindByEmailAsync(credentialModel.Email);

            if (usuario is null)
            {
                return ReturnLoginWrong();
            }

            var result = await signInManager.CheckPasswordSignInAsync(usuario,
                credentialModel.Password!, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(credentialModel);
            }
            else
            {
                return ReturnLoginWrong();
            }
        }

        private ActionResult ReturnLoginWrong()
        {
            ModelState.AddModelError(string.Empty, "Login Wrong");
            return ValidationProblem();
        }
    }
}
