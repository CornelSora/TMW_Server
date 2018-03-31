using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TAW_Server.Models;
using Swashbuckle.Swagger.Annotations;

namespace TAW_Server.Controllers
{
    public class JokesController : BaseController
    {

        public class JokeModel
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public int? UserID { get; set; }
            public int? NoLikes { get; set; }
            public int? NoUnlikes { get; set; }
            public int JokeID { get; set; }
        }

        public class RatingModel
        {
            public int NoLikes { get; set; }
            public int NoUnlikes { get; set; }
        }

        [Route("api/jokes")]
        public IEnumerable<JokeModel> Get()
        {
            var jokesBD = DbContext.Jokes.ToList();
            var jokes = new List<JokeModel>();
            foreach (var jokeBD in jokesBD)
            {
                var j = new JokeModel()
                {
                    Text = jokeBD.Description,
                    Title = jokeBD.Name,
                    UserID = (int)jokeBD.UserID,
                    NoLikes = jokeBD.NoLikes ?? 0,
                    NoUnlikes = jokeBD.NoUnlikes ?? 0,
                    JokeID = jokeBD.JokeID
                };
                jokes.Add(j);
            }
            return jokes;
        }

        #region Inregistrare Gluma
        // POST api/jokes
        [Route("api/jokes")]
        public IHttpActionResult Post([FromBody]JokeModel uploadedJoke, [FromUri] string secret)
        {
            try
            {
                var idString = "";
                if (secret.Length < 6)
                {
                    return Content<string>(System.Net.HttpStatusCode.Unauthorized, "The secret is incorrect!");
                }

                for (var i = 6; i < secret.Length; i++)
                {
                    idString += secret[i];
                }
                int SecretID = Int32.Parse(idString);

                var user = DbContext.Users
                    .Where(x => x.Id == SecretID).FirstOrDefault();

                if (user == null)
                {
                    return Content<string>(System.Net.HttpStatusCode.Unauthorized, "You don't have access here!");
                }

                var exists = user != null && user.Jokes != null && user.Jokes.Where(x => x.Description == uploadedJoke?.Text).FirstOrDefault() != null;

                if (exists)
                {
                    return Content<string>(System.Net.HttpStatusCode.NotAcceptable, "The joke already exists in your list");
                }

                var joke = new Joke()
                {
                    Name = user.Username + " - joke no. " + (user.Jokes.Count + 1),
                    Description = uploadedJoke.Text,
                    NoLikes = 0,
                    NoUnlikes = 0,
                    Ratings = new List<Rating>(),
                    UserID = user.Id
                };

                user.Jokes.Add(joke);
                //Statics.GetAllJokes().Add(joke);
                DbContext.Jokes.Add(joke);
                DbContext.SaveChanges();
            }
            catch (Exception)
            {
                return Content<string>(System.Net.HttpStatusCode.ServiceUnavailable, "An error has ocurred"); ;
            }

            return Content<string>(System.Net.HttpStatusCode.Created, "Joke has been created"); ;
        }
        #endregion

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public IHttpActionResult Delete([FromUri]int UserID, [FromUri] int JokeID)
        {
            var user = DbContext.Users
                    .Where(x => x.Id == UserID).FirstOrDefault();

            if (user == null)
            {
                return Content<string>(System.Net.HttpStatusCode.Unauthorized, "You don't have access here!");
            }

            var joke = user.Jokes.Where(x => x.JokeID == JokeID).FirstOrDefault();

            if (joke == null)
            {
                return Content<string>(System.Net.HttpStatusCode.NotFound, "The joke doesn't exist!");
            }

            DbContext.Jokes.Remove(joke);
            DbContext.SaveChanges();
            return Content(System.Net.HttpStatusCode.OK, "");

        }
    }
}