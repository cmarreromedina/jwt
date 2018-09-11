using System;
using WebApiJWT.Models;
namespace WebApiJWT.Classes
{
    public class LoginRequest 
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}