using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class UserDto
    {
    public required string Username { get; set; }
    public required string Token { get; set; }
        
    }
}