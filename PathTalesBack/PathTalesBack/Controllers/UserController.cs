using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PathTalesBack;
using PathTalesBack.Data;
using PathTalesBack.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace PathTalesBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Recupere la collection user
        /// </summary>
        /// <param name="mongoDbService"></param>
        public UserController(MongoDbService mongoDbService) 
        {
            _users = mongoDbService.Database?.GetCollection<User>("User");
        }

        /// <summary>
        /// Recupere tout les user
        /// </summary>
        /// <returns>Retourn les user</returns>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _users.Find(FilterDefinition<User>.Empty).ToListAsync();
        }

        /// <summary>
        /// Recupere un user grace a son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Id invalide, 404 ou L'utilisateur corespondant</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetById(string id)
        {
            //verifie le format de l'id
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Id invalide");

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var user = _users.Find(filter).FirstOrDefault();
            return user is not null ? Ok(user) : NotFound();
        }

        /// <summary>
        /// Crée un utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Modifie un user par son id
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Son id</returns>
        [HttpPatch]
        public async Task<ActionResult> Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

            await _users.ReplaceOneAsync(filter, user);
            return Ok();
        }

        /// <summary>
        /// Supprime une histoir par son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            //verifie le format de l'id
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Id invalide");

            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            await _users.DeleteOneAsync(filter);
            return Ok();
        }
    }
}