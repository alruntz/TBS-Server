using System.Collections.Generic;

#if USE_MONGO
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace TBS.Models
{
    public class Player
    {
#if USE_MONGO
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#endif
        public string Id { get; set; }

#if USE_MONGO
        [BsonElement("UserId")]
#endif
        public string UserId { get; set; }

#if USE_MONGO
        [BsonElement("Name")]
#endif
        public string Name { get; set; }

        #if USE_MONGO
        [BsonElement("Team")]
#endif
        public Team Team { get; set; }

#if USE_MONGO
        [BsonElement("Items")]
#endif
        public List<string> Items { get; set; }
    }

    public class Team
    {
#if USE_MONGO
        [BsonElement("Characters")]
#endif
        public List<CharacterTeam> Characters { get; set; }
    }

    public class CharacterTeam 
    {
#if USE_MONGO
        [BsonElement("CharacterId")]
#endif
        public string CharacterId { get; set; }

#if USE_MONGO
        [BsonElement("Items")]
#endif
        public List<string> Items { get; set; }

#if USE_MONGO
        [BsonElement("Spells")]
#endif
        public List<string> Spells { get; set; }
    }
}
