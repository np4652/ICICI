using ICICI.AppCode.Interfaces;
using ICICI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICI.AppCode.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }
        //public async Task Invoke(HttpContext context, IUserService userService)
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);//attachUserToContext(context, userService ,token);

            await _next(context);
        }

        //private void attachUserToContext(HttpContext context, IUserService userService ,string token)
        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    //ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims.ToList();
                var loginResponse = new LoginResponse
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = nameof(ResponseStatus.Success),
                    IsAuthenticate = true,
                    Token = token,
                    Result = new ApplicationUser
                    {
                        Id = int.Parse(claims.First(x => x.Type == "id").Value),
                        Role = Convert.ToString(claims.First(x => x.Type == "role").Value),
                        UserName = Convert.ToString(claims.First(x => x.Type == "userName").Value)
                    }
                };
                context.Items["User"] = loginResponse;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
