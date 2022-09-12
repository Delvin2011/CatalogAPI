using Catalog.Configuration;
using Catalog.Dtos.Users;
using Catalog.Services.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        //private static Logger logger = LogManager.GetLogger("ItemsAPILoggerRules"); //Delete because we no long have to instantiate a new object everytime we use it. Thats the go of an singleton

        public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor) //IOptionsMonitor because we will use the configs from the startup
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponseDto()
                    {
                        Errors = new List<string>()
                            {
                                "Email already exists"
                            },
                        Success = false
                    });
                }

                var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponseDto()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponseDto()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });

                }
            }

            return BadRequest(new RegistrationResponseDto()
            {
                Errors = new List<string>()
                {
                    "Invalid Payload"
                },
                Success = false
            });
        }


        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            MyLogger.GetInstance().Info("Entering the login controller: Login Method");
            try
            {
                if (ModelState.IsValid) //check if the state is valid, like no numbers instead of letter etc
                {
                    var existingUser = await _userManager.FindByEmailAsync(user.Email);

                    if (existingUser == null)
                    {
                        MyLogger.GetInstance().Info("Login failed: User {0} does not exist", user.Email);
                        return BadRequest(new RegistrationResponseDto()
                        {
                            Errors = new List<string>()
                            {
                                "Invalid Login Request"
                            },
                            Success = false
                        });
                    }

                    var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                    if (!isCorrect)
                    {
                        MyLogger.GetInstance().Info("Login failed: Invalid password for user {0}", user.Email);
                        return BadRequest(new RegistrationResponseDto()
                        {
                            Errors = new List<string>()
                            {
                                "Invalid Login Request"
                            },
                            Success = false
                        });
                    }

                    var jwtToken = GenerateJwtToken(existingUser);

                    MyLogger.GetInstance().Info("User {0}: Succesfully logged in", user.Email);
                    return Ok(new RegistrationResponseDto()
                    {
                        Success = true,
                        Token = jwtToken
                    });

                }

                MyLogger.GetInstance().Info("Login failed: Invalid Login Request");
                return BadRequest(new RegistrationResponseDto()
                {
                    Errors = new List<string>()
                {
                    "Invalid Payload"
                },
                    Success = false
                });
            }
            catch (Exception ex)
            {
                MyLogger.GetInstance().Error("Exception: " + ex.Message);
                return BadRequest(new RegistrationResponseDto()
                {
                    Errors = new List<string>()
                {
                    "Exception: " + ex.Message
                },
                    Success = false
                });
            }


        }
        private string GenerateJwtToken(IdentityUser user)
        {
            //Process of creating the tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //getting the security key in the config
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            //Descriptor contains claims(info about our jwt within, so that we may read the email, username etc of the user)
            var tokenDescriptor = new SecurityTokenDescriptor //information we want for the token
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //jti is an id we will be using in order to use thee jwt token refresh that the jwt supports (in the evetn the user token expires whilst the user is still session)
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6), //Expire after 6hs
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //type of algorith to use to encrypt thr token
            };

            //prepare the token to be generated based on the descriptor
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;

        }



    }
}
