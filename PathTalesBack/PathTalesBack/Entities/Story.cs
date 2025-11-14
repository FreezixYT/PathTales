using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace PathTalesBack.Entities
{
    public class Story
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public string? Title { get; set; }

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string? Description { get; set; }

        [BsonElement("paragraphs"), BsonRepresentation(BsonType.Array)]
        public List<ObjectId>? Paragraphs { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string? Status { get; set; }

        [BsonElement("categories"), BsonRepresentation(BsonType.Array)]
        public List<string>? Categories { get; set; }

        [BsonElement("readCount"), BsonRepresentation(BsonType.Int32)]
        public int ReadCount { get; set; }

        [BsonElement("ratings"), BsonRepresentation(BsonType.Array)]
        public List<Rating>? Ratings { get; set; }

        public class Rating
        {
            [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
            public ObjectId UserId { get; set; }

            [BsonElement("score"), BsonRepresentation(BsonType.Int32)]
            public int Score { get; set; }
        }
    }
}
