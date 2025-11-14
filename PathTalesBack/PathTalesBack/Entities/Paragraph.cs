using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace PathTalesBack.Entities
{
    public class Paragraph
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("text"), BsonRepresentation(BsonType.String)]
        public string? Text { get; set; }

        [BsonElement("choices"), BsonRepresentation(BsonType.Array)]
        public List<Choice>? Choices { get; set; }

        [BsonElement("endingType"), BsonRepresentation(BsonType.String)]
        public string? EndingType { get; set; }

        public class Choice
        {
            [BsonElement("text"), BsonRepresentation(BsonType.String)]
            public string? Text { get; set; }

            [BsonElement("nextParagraphId"), BsonRepresentation(BsonType.ObjectId)]
            public ObjectId? NextParagraphId { get; set; }
        }
    }
}
