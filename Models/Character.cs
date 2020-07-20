using System.Collections.Generic;
#if USE_MONGO
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace TBS.Models
{
    public class Character
    {
#if USE_MONGO
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#endif
        public string Id { get; set; }

#if USE_MONGO
        [BsonElement("Name")]
#endif
        public string Name { get; set; }

#if USE_MONGO
        [BsonElement("Price")]
#endif
        public int Price { get; set; }

#if USE_MONGO
        [BsonElement("SpellIds")]
#endif
        public List<string> SpellIds { get; set; }

#if USE_MONGO
        [BsonElement("MovementPoints")]
#endif
        public int MovementPoints { get; set; }

#if USE_MONGO
        [BsonElement("ActionPoints")]
#endif
        public int ActionPoints { get; set; }

#if USE_MONGO
        [BsonElement("LifePoints")]
#endif
        public int LifePoints { get; set; }

#if USE_MONGO
        [BsonElement("DamagePoints")]
#endif
        public int DamagePoints { get; set; }

#if USE_MONGO
        [BsonElement("ArmorPoints")]
#endif
        public int ArmorPoints { get; set; }

#if USE_MONGO
        [BsonElement("VisionPoints")]
#endif
        public int VisionPoints { get; set; }

#if USE_MONGO
        [BsonElement("InitiativePoints")]
#endif
        public int InitiativePoints { get; set; }
    }
}
