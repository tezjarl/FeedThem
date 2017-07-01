using System;
using System.Net.Http;
using FeedThem.Models;
using FeedThemServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedThem.Controllers
{
    /// <summary>
    /// User controller. Used for working with users
    /// </summary>
    public class UserController: Controller
    {
        private UserRepository repository = new UserRepository();

        public UserController()
        {
        }

        [HttpGet]
        //[Authorize]
        public User Get(string login)
        {
            return repository.GetUserByLogin(login);
        }

        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            repository.Create(user);
            return Json(user);
        }
    }
}
