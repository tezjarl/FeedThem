using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace FeedThem.Controllers
{
    /// <summary>
    /// Authorization controller. Users retrieve token via it
    /// </summary>
    public class AuthorizationController: ApiController
    {
        public AuthorizationController()
        {
        }

        [HttpGet]
        public async Task<string> Get(string userID)
        {
            return "test";
        } 
    }
}
