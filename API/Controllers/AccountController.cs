using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenservice) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.username)) return BadRequest("Username is taken");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {

                UserName = registerDto.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key

            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Username= user.UserName,
                Token = tokenservice.createToken(user)
            };

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await context.Users.FirstOrDefaultAsync(x=>x.UserName== loginDto.Username.ToLower());

            if (user== null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var ComputeHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if(ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username= user.UserName,
                Token = tokenservice.createToken(user)
            
            };
        
        }

        private async Task<bool>UserExists(string username)
        {
            return await context.Users.AnyAsync(x=> x.UserName.ToLower()== username.ToLower());
        }

    }
}