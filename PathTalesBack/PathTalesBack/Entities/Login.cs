using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PathTalesBack.Entities
{
    public class Login
    {
        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string? Password { get; set; }

    }
}
