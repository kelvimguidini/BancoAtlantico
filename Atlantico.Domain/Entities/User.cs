using System;
using System.Collections.Generic;

namespace Atlantico.Domain
{
    public class User
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}