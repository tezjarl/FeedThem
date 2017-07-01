using System;
using System.Collections.Generic;
using FeedThem.Enums;

namespace FeedThem.Models
{
    public class User
    {
        public User(string login, string password, int? age=null, Gender sex =Gender.Unspecified )
        {
            this.Login = login;
            this.Password = password;
            this.Sex = sex;
            this.Age = age;
            this.FoodTypes = new List<PredefinedFoodType>();
        }

        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? Age { get; set; }
        public Gender Sex { get; set; }
        public List<PredefinedFoodType> FoodTypes { get; set; }
    }
}
