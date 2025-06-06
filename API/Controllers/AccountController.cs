﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{

    public class AccountController(DataContext context, ITokenServices tokenServices) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("User name is taken");
            return Ok();
            //if (await UserExist(registerDto.Username))
            //{
            //    return BadRequest("Username is taken");
            //}
            //using var hmac = new HMACSHA512();

            //var user = new AppUser
            //{
            //    UserName = registerDto.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key

            //};

            //context.Users.Add(user);
            //await context.SaveChangesAsync();

            //return new UserDto
            //{
            //    Username = user.UserName,
            //    Token = tokenServices.CreateToken(user)
            //};
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
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }

        


            private async Task<bool> UserExist(string username)
            {
                return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
            }

        }
    }

