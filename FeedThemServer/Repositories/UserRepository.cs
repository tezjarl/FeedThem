using System;
using System.Collections.Generic;
using FeedThem.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;
using System.Net.Http;

namespace FeedThemServer.Repositories
{
    public class UserRepository
    {
		private List<User> users = new List<User> {
			new User("test_user", "12345"),
			new User("test_user2", "a")
		};
        public UserRepository()
        {
        }

        public User GetUserByHashedCredentials(string hashedCredentials, byte[] salt)
        {
            return users.FirstOrDefault(user => 
                                        this.GetHashedCredentials(user.Login, user.Password, salt) 
                                        == hashedCredentials); 
        }

        internal void Create(User user)
        {
            users.Add(user);
        }

        private string GetHashedCredentials(string username, string password, byte[] salt)
		{
		   return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8
			));

		}

        public User GetUserByLogin(string login) 
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://185.125.216.181:5000/");
                var request = new HttpRequestMessage(HttpMethod.Get, "/getUserById?user_id=1");
                var res = client.SendAsync(request).Result;
                var content = res.Content.ReadAsStringAsync().Result;

            }
            return users.FirstOrDefault(u => u.Login == login);
        }
    }
}
