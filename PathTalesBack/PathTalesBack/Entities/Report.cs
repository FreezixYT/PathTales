using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace PathTalesBack.Entities
{
    public class Report
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("storyId"), BsonRepresentation(BsonType.ObjectId)]
        public ObjectId StoryId { get; set; }

        [BsonElement("reportedBy"), BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ReportedBy { get; set; }

        [BsonElement("reason"), BsonRepresentation(BsonType.String)]
        public string? Reason { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string? Status { get; set; }
    }
}
