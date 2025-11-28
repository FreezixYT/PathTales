using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace PathTalesBack.Entities
{
    /// <summary>
    /// Histoire
    /// </summary>
    public class Story
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        //sans paraphe par defaut
        [BsonElement("paragraphs")]
        public List<string> Paragraphs { get; set; } = new();

        [BsonElement("status")]
        public string Status { get; set; } = "hidden";

        //sans avis par defaut
        [BsonElement("categories")]
        public List<string> Categories { get; set; } = new();

        [BsonElement("readCount")]
        public int ReadCount { get; set; } = 0;

        [BsonElement("ratings")]
        public List<Rating> Ratings { get; set; } = new();

        /// <summary>
        /// Avis avec un user id et une note, de 1 par defaut
        /// </summary>
        public class Rating
        {
            [BsonElement("userId")]
            [BsonRepresentation(BsonType.ObjectId)]
            public string UserId { get; set; } = string.Empty;

            [BsonElement("score")]
            public int Score { get; set; } = 1; 
        }
    }


}
