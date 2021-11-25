using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Travel.Identity.Settings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings;
using Microsoft.IdentityModel.Tokens;
using Travel.Application.Common.Interfaces;

namespace Travel.Identity.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthSettings _authSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AuthSettings> authSettingsOption)
        {
            _next = next;
            _authSettings = authSettingsOption.Value;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if(token != null)
            {
                AttachUserToContext(httpContext, userService, token);
            }

            await _next(httpContext);
        }

        private void AttachUserToContext(HttpContext httpContext, IUserService userService, string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(c => c.Type == "sub").Value);

                httpContext.Items["User"] = userService.GetById(userId);
            }
            catch(Exception ex)
            {
                var message = ex.Message;
            }
        }
    }
}
