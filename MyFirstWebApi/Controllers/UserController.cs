using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;

        public UserController(AppDbContext appDbContext, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _context = appDbContext;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.UserName == model.UserName
            && p.Password == model.Password);
            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Messenger = "Invalid user!"

                });

            }
            var token = await GenerateToken(user);
            //cap' token
            return Ok(new ApiResponse
            {
                Success = true,
                Messenger = "Authentication success!",
                Data = token
            });
        }

        private async  Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);//work with byte
            var tokenDesciption = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //id of this access token
                    new Claim("UserName", user.UserName),
                    new Claim("UserId", user.Id.ToString()),
                    //role....

                    new Claim("TokenId",Guid.NewGuid().ToString()),

                }),
                Expires = DateTime.UtcNow.AddSeconds(30), //expires time 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes)
                    , SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandle.CreateToken(tokenDesciption);

            var accessToken = jwtTokenHandle.WriteToken(token); // return string token
            var refreshToken = GenerateRefreshToken();

            //luu vao database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = user.Id,
                JwtId = token.Id,
                isRevoke = false,
                isUsed = false,
                ExpireAt = DateTime.UtcNow.AddHours(1),
                IssueAt = DateTime.UtcNow
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefrestToken = refreshToken     
            };


        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);//work with byte
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false //ko check expire token, vì nếu check là nhảy về catch
            };
            try
            {
                //check 1 asseces token valid format
                var tokenInverification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam,
                    out var validatedToken);
                //check 2 thuat toan alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase); //khong phan biet hoa thuong
                    if (!result)//false
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Messenger = "Invalid token"
                        });
                    }
                }

                //Check 3 asscesToken expire
                var utcExpireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnitxTimeToDateTime(utcExpireDate);
                if(expireDate>DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Messenger = "Access token has not yet expire"
                    });
                }

                //check 4 refreshToken exist in Db
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefrestToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Messenger = "Refresh token has not exist!"
                    });
                }
                //check 4 refreshToken is used/revoke ?
                if(storedToken.isUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Messenger = "Refresh token has been used!"
                    });
                }
                if (storedToken.isRevoke)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Messenger = "Refresh token has been Revoke!"
                    });
                }
                
                //check 6 access token id == jwt id in refresh token
                var jti = tokenInverification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Jti).Value;
                if(jti != storedToken.JwtId)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Messenger = "Token doesn't match"
                    });
                }

                //update token is used 
                storedToken.isRevoke = true;
                storedToken.isUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.Users.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
                var token = await GenerateToken(user);
                //cap' token
                return Ok(new ApiResponse
                {
                    Success = true,
                    Messenger = "Renew token success",
                    Data = token
                });
            }
            catch
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Messenger = "Something wrong"
                });
            }
        }

        private DateTime ConvertUnitxTimeToDateTime(long utcExpireDate)
        {
           var dateTimeInterval = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
