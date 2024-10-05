using Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Authentication
{
    /// <summary>
    /// Provides authentication services for generating JSON Web Tokens (JWT).
    /// </summary>
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentication"/> class.
        /// </summary>
        /// <param name="config">The configuration containing JWT settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public Authentication(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.username),
                new Claim(ClaimTypes.Email, user.email)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
