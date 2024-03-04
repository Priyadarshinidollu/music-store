using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_store.src.user.Dtos
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}