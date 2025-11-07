using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PathTalesBack;
using PathTalesBack.Data;
using PathTalesBack.Entities;

namespace PathTalesBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;
       
        public UserController(MongoDbService mongoDbService) 
        {
            _users = mongoDbService.Database?.GetCollection<User>("test");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _users.Find(FilterDefinition<User>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetById(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var user = _users.Find(filter).FirstOrDefault();
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPatch]
        public async Task<ActionResult> Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);

            await _users.ReplaceOneAsync(filter, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            await _users.DeleteOneAsync(filter);
            return Ok();
        }
    }
}
