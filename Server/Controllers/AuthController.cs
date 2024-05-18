using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Auth.Requests;
using Shared.Models.Auth.Responses;
using Shared.Models.Auth.Tokens;
using Shared.Models.Auth;
using Shared.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dataContext;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail);

                if (user == null)
                {
                    return Unauthorized("Incorrect phone number/email or password");
                }

                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Unauthorized("Incorrect phone number/email or password");
                }

                if (user.IsDeleted == true)
                {
                    return Unauthorized("User is blocked/deleted");
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var authResult = await GenerateJwtToken(user, (List<string>)userRoles);

                return Ok(new SignInResponse
                {
                    AccessToken = authResult.Token,
                    RefreshToken = authResult.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occured while trying to login as {request.UsernameOrEmail}. Logs provide more information");
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                ApplicationUser? userByUserName = await _userManager!.FindByNameAsync(request.Username);

                if (userByUserName != null)
                    return BadRequest("User with the same user name already exists!");

                ApplicationUser? userByEmail = await _userManager.FindByEmailAsync(request.Email);

                if (userByEmail != null)
                    return BadRequest("User with the same email already exists!");

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    Lastname = request.Lastname,
                    Role = request.Role,
                    DebugMode = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(string.Join(".\n", result.Errors.Select(x => x.Description)));
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }

                if (request.Role == UserRoles.Admin)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                else if (request.Role == UserRoles.User)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }

                await _userManager!.UpdateSecurityStampAsync(user);

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error. Logs can to provide more details");
            }
        }

        

        #region Tokens

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(model);

                if (result == null)
                {
                    return BadRequest("Invalid tokens");
                }

                return Ok(result);
            }

            return BadRequest("Invalid payload");
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<AuthResult?> VerifyAndGenerateToken(RefreshTokenRequest refreshTokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationTokenParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };

                //Validation 1 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(refreshTokenRequest.Token, validationTokenParameters, out var validatedToken);

                //Validation 2 - Validate ecription algoritm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                //Validation 3 - validate expiry
                var ExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                DateTime expDate = GetExpiryDate(ExpiryDate);

                if (expDate > DateTime.Now)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has not yet expired"
                        }
                    };

                }

                //Validation 4 - validate existance of the token 
                var storedToken = await _dataContext.RefreshTokens!.FirstOrDefaultAsync(x => x.Token == refreshTokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token does not exist"
                        }
                    };
                }

                //Validation 5 - validate use or not

                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has been used"
                        }
                    };
                }

                //Validation 6 - validate if revoke
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token has been revoked"
                        }
                    };
                }

                //Validation 7 - validate id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Token doesn't match"
                        }
                    };
                }


                //Update current token

                storedToken.IsUsed = true;
                _dataContext.RefreshTokens!.Update(storedToken);
                await _dataContext.SaveChangesAsync();

                var dbUser = await _userManager!.FindByIdAsync(storedToken.UserId);
                var userRoles = await _userManager.GetRolesAsync(dbUser);
                return await GenerateJwtToken(dbUser, (List<string>)userRoles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private DateTime GetExpiryDate(long timeStamp)
        {
            var dateTimeVal = new DateTime();
            dateTimeVal = dateTimeVal.AddSeconds(timeStamp);
            return dateTimeVal;
        }


        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private string RandomString(int lenght)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lenght).Select(x => x[random.Next(x.Length)]).ToArray());
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<AuthResult> GenerateJwtToken(ApplicationUser user, List<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["JWT:Secret"]));

            var descriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:ValidAudience"],
                Issuer = _configuration["JWT:ValidIssuer"],
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:JwtLifeTimeMin")),
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            //var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var tokenHandler = new JwtSecurityTokenHandler();
            var securedToken = tokenHandler.CreateToken(descriptor);
            var token = tokenHandler.WriteToken(securedToken);

            var refreshToken = new RefreshToken()
            {
                JwtId = securedToken.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:RefreshTokenLifeTimeDay")),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _dataContext!.RefreshTokens.AddAsync(refreshToken);
            await _dataContext!.SaveChangesAsync();

            return new AuthResult()
            {
                Token = token,
                Success = true,
                RefreshToken = refreshToken.Token,
                Debug = user.DebugMode
            };
        }

        #endregion

    }
}
