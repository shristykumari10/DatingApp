using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{

    public class AccountController(DataContext context, ITokenServices tokenServices, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExist(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }
            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            //var user = new AppUser
            //{
            //    UserName = registerDto.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key

            //};

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = tokenServices.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                //if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized(new { error = "Invalid password" });

            }
                return new UserDto
                {
                    Token = tokenServices.CreateToken(user),
                    Username = user.UserName,
                    KnownAs = user.KnownAs,
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }

        


            private async Task<bool> UserExist(string username)
            {
                return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
            }

        }
    }

