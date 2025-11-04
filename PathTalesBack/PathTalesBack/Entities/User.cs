using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PathTalesBack.Entities
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // /!\ dans  la base -> int
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        //password
        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }

        //role
        [BsonElement("role"), BsonRepresentation(BsonType.String)]
        public string? Role { get; set; }

        //isBlocked
        [BsonElement("isBlocked"), BsonRepresentation(BsonType.Boolean)]
        public Boolean IsBlocked { get; set; }

        //blockReason
        [BsonElement("blockReason"), BsonRepresentation(BsonType.String)]
        public string? BlockReason { get; set; }
    }
}
