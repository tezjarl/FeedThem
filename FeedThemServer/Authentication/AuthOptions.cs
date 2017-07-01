using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FeedThem.Authentication
{
    public class AuthOptions
    {
        public AuthOptions()
        {
            public const string ISSUER = "FeedThemServer";
            public const string AUDIENCE = "http://localhost:8080/";
            const string KEY = "feEd109836tseeekslqpwoe873g$#@";
            public const int LIFETIME = 15;

		    public static SymmetricSecurityKey GetSymmetricSecurityKey()
		    {
			  return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
    }
}
