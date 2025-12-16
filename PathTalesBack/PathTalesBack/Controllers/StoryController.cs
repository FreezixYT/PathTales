using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PathTalesBack.Data;
using PathTalesBack.Entities;

namespace PathTalesBack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IMongoCollection<Story> _stories;

        public StoryController(MongoDbService mongoDbService)
        {
            _stories = mongoDbService.Database?.GetCollection<Story>("Story");
        }

        /// <summary>
        /// Recupere toutes les hitoirs
        /// </summary>
        /// <returns>La liste de toutes les histoires</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> Get()
        {
            var stories = await _stories.Find(_ => true).ToListAsync();
            return Ok(stories);
        }

       /// <summary>
       /// Recupere une histoire par son id
       /// </summary>
       /// <param name="id"></param>
       /// <returns>Erreur 404 ou Succes 200</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Story>> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Id invalide");

            var story = await _stories.Find(s => s.Id == id).FirstOrDefaultAsync();

            if (story == null)
                return NotFound();

            return Ok(story);
        }

        /// <summary>
        /// Chercher un histoir par mot clef
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<List<Story>>> GetByKeyWord(string keyword)
        {
            var filter = Builders<Story>.Filter.Regex(
                s => s.Title,
                new BsonRegularExpression(keyword, "i")
            );

            var stories = await _stories.Find(filter).ToListAsync();

            if (!stories.Any())
                return NotFound();

            return Ok(stories);
        }


        /// <summary>
        /// Crée une histoire
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(Story story)
        {
            await _stories.InsertOneAsync(story);
            return CreatedAtAction(nameof(GetById), new { id = story.Id }, story);
        }

        /// <summary>
        /// Modifie une histoire grace a son id
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] Story story)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Id invalide");

            story.Id = id;

            var result = await _stories.ReplaceOneAsync(s => s.Id == id, story);

            if (result.MatchedCount == 0)
                return NotFound();

            return Ok(story);
        }


        /// <summary>
        /// Supprime une histoir grace à son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Id invalide");

            var result = await _stories.DeleteOneAsync(s => s.Id == id);

            if (result.DeletedCount == 0)
                return NotFound();

            return Ok();
        }
    }
}
