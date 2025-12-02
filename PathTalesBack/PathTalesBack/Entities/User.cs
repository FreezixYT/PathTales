using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PathTalesBack.Entities
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string? Password { get; set; }

        [BsonElement("token"), BsonRepresentation(BsonType.String)]
        public string? Token { get; set; } = null;

        /// <summary>
        /// Role dois etre sois member sois admin sinon exeption
        /// </summary>
        [BsonElement("role"), BsonRepresentation(BsonType.String)]
        private string role = "member"; 

        public string Role
        {
            get => role;
            set
            {
                if (value == null)
                {
                    role = "member";
                }
                else if (value == "member" || value == "admin")
                {
                    role = value;
                }
                else
                {
                    throw new Exception("Erreur : rôle incorrect.");
                }
            }
        }



        [BsonElement("isBlocked"), BsonRepresentation(BsonType.Boolean)]
        public bool IsBlocked { get; set; } = false;

        [BsonElement("blockReason"), BsonRepresentation(BsonType.String)]
        public string? BlockReason { get; set; } = null;
    }
}
