using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Token(bool authenticated = true)
        {
            if (authenticated)
            {
                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, "api"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("webapi-authentication"));
                var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(issuer: "WebApi.Api",
                                                 audience: "WebApi",
                                                 claims: claims,
                                                 signingCredentials: credenciais,
                                                 expires: DateTime.Now.AddMinutes(30));

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenString);
            }

            return Unauthorized();
        }
    }
}
