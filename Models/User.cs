#if USE_MONGO
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace TBS.Models
{
    public class User
    {
#if USE_MONGO
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#endif
        public string Id { get; set; }

#if USE_MONGO
        [BsonElement("Username")]
#endif
        public string Username { get; set; }

#if USE_MONGO
        [BsonElement("Password")]
#endif
        public string Password { get; set; }

#if USE_MONGO
        [BsonElement("Role")]
#endif
        public int Role { get; set; }
    }
}
