db = new Mongo().getDB("pathTales");

// User
db.createCollection("User", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["name", "email"],
      properties: {
        name: { bsonType: "string" },
        email: { bsonType: "string" },
        password: { bsonType: "string" },
        token: { bsonType: ["string", "null"] },
        role: {
          bsonType: "string",
          enum: ["member", "admin"]
        },
        isBlocked: { bsonType: "bool" },
        blockReason: { bsonType: ["string", "null"] }
      }
    }
  }
});

// Story
db.createCollection("Story", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["title", "userId"],
      properties: {
        title: {
          bsonType: "string",
          maxLength: 150
        },
        description: {
          bsonType: "string",
          maxLength: 500
        },
        userId: {
          bsonType: "objectId"
        },
        paragraphs: {
          bsonType: "array",
          items: { bsonType: "objectId" }
        },
        status: {
          bsonType: "string",
          enum: ["published", "hidden", "deleted"]
        },
        categories: {
          bsonType: "array",
          items: { bsonType: "string" }
        },
        readCount: { bsonType: "int" },
        ratings: {
          bsonType: "array",
          items: {
            bsonType: "object",
            required: ["userId", "score"],
            properties: {
              userId: { bsonType: "objectId" },
              score: {
                bsonType: "int",
                minimum: 1,
                maximum: 5
              }
            }
          }
        }
      }
    }
  }
});

// Paragraph
db.createCollection("Paragraph", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["text"],
      properties: {
        text: {
          bsonType: "string",
          maxLength: 1000
        },
        choices: {
          bsonType: "array",
          minItems: 2,
          maxItems: 4,
          items: {
            bsonType: "object",
            required: ["text", "nextParagraphId"],
            properties: {
              text: {
                bsonType: "string",
                maxLength: 200
              },
              nextParagraphId: {
                bsonType: "objectId"
              }
            }
          }
        },
        endingType: {
          bsonType: "string",
          enum: ["victory", "defeat", "neutral"]
        }
      }
    },
    if: {
      required: ["endingType"]
    },
    then: {
      properties: {
        choices: {
          bsonType: "array",
          maxItems: 0
        }
      }
    }
  }
});

// Report
db.createCollection("Report", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["storyId", "reportedBy", "reason"],
      properties: {
        storyId: { bsonType: "objectId" },
        reportedBy: { bsonType: "objectId" },
        reason: {
          bsonType: "string",
          maxLength: 500
        },
        status: {
          bsonType: "string",
          enum: ["pending", "resolved"]
        }
      }
    }
  }
});
