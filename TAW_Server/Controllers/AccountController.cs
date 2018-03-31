using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using TAW_Server.Models;

namespace TAW_Server.Controllers
{
    public class AccountController : BaseController
    {
        #region Register
        public class UserModel
        {
            public string username { get; set; }
            public string password { get; set; }
        }
     
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="username">ABC</param>
        /// <param name="password">ABC</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Account/Register")]
        public IHttpActionResult Register([FromBody] UserModel regModel)
        {
            bool exists = DbContext.Users.Where(x => x.Username == regModel.username).FirstOrDefault() != null;
            if (exists)
            {
                return Content<string>(System.Net.HttpStatusCode.NotAcceptable, "User already registered");
            }

            try
            {
                var user = new Models.User()
                {
                    Username = regModel.username,
                    Password = SecurePasswordHasher.Hash(regModel.password),
                    Jokes = new List<Joke>()
                };


                DbContext.Users.Add(user);
                DbContext.SaveChanges();
            }

            catch (Exception ex)
            {
                return Content<string>(System.Net.HttpStatusCode.ServiceUnavailable, "An error has ocurred\n" + ex.ToString()); ;
            }

            return Content<string>(System.Net.HttpStatusCode.Created, "User has been created"); ;
        }
        #endregion

        [HttpGet]
        public IEnumerable<UserModel> GetAllUsers()
        {
            var userBD = DbContext.Users.ToList();
            var users = new List<UserModel>();
            foreach (var u in userBD)
            {
                var u1 = new UserModel()
                {
                    username = u.Username,
                    password = u.Password
                };
                users.Add(u1);
            }
            return users;
        }

        #region Login

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="username">ABC</param>
        /// <param name="password">ABC</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult Login([FromBody] UserModel loginModel)
        {
            User user = DbContext.Users.Where(x => x.Username == loginModel.username).FirstOrDefault();
            if (user == null)
            {
                return Content<string>(System.Net.HttpStatusCode.NotAcceptable, "Username doesn't exist");
            }
            else if (SecurePasswordHasher.Hash(loginModel.password).Equals(user.Username))
            {
                return Content<string>(System.Net.HttpStatusCode.NotAcceptable, "Password isn't correct");
            }

            return Content<int>(System.Net.HttpStatusCode.OK, user.Id);

        }

        #endregion

    }

}