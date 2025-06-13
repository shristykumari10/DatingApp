using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{

    public class AccountController(UserManager<AppUser> userManager, ITokenServices tokenServices, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExist(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }
            

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
           

           
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
           

            return new UserDto
            {
                Username = user.UserName,
                Token =  await tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.Username.ToUpper());

            if (user == null || user.UserName == null)
            {
                return Unauthorized("Invalid username");
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized();

          
                return new UserDto
                {
                    Token = await tokenServices.CreateToken(user),
                    Username = user.UserName,
                    KnownAs = user.KnownAs,
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                    Gender = user.Gender,
                };
            }

        


            private async Task<bool> UserExist(string username)
            {
                return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
            }

        }
    }

