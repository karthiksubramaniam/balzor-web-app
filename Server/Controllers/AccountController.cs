using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using balzor_web_app.Shared.Dtos.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace balzor_web_app.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (dto.Username == "admin" && dto.Password == "123")
            {
                var response = await this.AuthenticateUser(dto);
                return Ok(response);
            }
            return BadRequest("Invalid credentials.");
        }

        private async Task<LoginResponseDto> AuthenticateUser(LoginRequestDto dto)
        {
            Claim[] userClaims =
            {
                new Claim(JwtRegisteredClaimNames.Name, dto.Username),
                new Claim(JwtRegisteredClaimNames.Typ, "admin")
            };

            DateTime expiresAt = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: "blazoer-server",
                audience: "blazor-client",
                claims: userClaims,
                expires: expiresAt,
                signingCredentials: null
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
    }
}