using jwt.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        private readonly JWTSetting setting;
        public UserController(UserDbContext userDbContext, IOptions<JWTSetting> options)
        {
            setting = options.Value;
            _userDbContext = userDbContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userDbContext.Users.ToList());
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(User _user)
        {
            var user = await _userDbContext.Users.Where(x => x.Mailid == _user.Mailid && x.Passwords == _user.Passwords).FirstOrDefaultAsync();
            if(user != null)
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(setting.SecurityKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email,user.Mailid),
                    }),
                    Expires = DateTime.Now.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenhandler.CreateToken(tokenDescriptor);
                string FinalToken = tokenhandler.WriteToken(token);

                return Ok(FinalToken);
            }
            return BadRequest();
        }
  
    }

   
}

