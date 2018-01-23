using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VideoPlayer.DAL;
using VideoPlayer.Models;
using VideoPlayer.Utility_Classes;

namespace VideoPlayer.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        public IConfiguration Configuration { get; }
        public TokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpPost("RequestToken")]
        public IActionResult RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            if (Configuration["User:username"] == tokenRequest.Username
                && Configuration["User:email"] == tokenRequest.Email)
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.Username,
                    Configuration["Auth:JwtSecurityKey"],
                    Configuration["Auth:ValidIssuer"],
                    Configuration["Auth:ValidAudience"]);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenStr);
            }
            return Unauthorized();
        }

    }
}