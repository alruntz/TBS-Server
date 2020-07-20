using System.Collections.Generic;
#if USE_MONGO
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace TBS.Models
{

    public class Range
    {
#if USE_MONGO
        [BsonElement("Type")]
#endif
        public string Type { get; set; }

#if USE_MONGO
        [BsonElement("Size")]
#endif
        public int Size { get; set; }

#if USE_MONGO
        [BsonElement("StartDistance")]
#endif
        public int StartDistance { get; set; }
    }

    public class SpellRange : Range
    {
#if USE_MONGO
        [BsonElement("Boostable")]
#endif
        public bool Boostable { get; set; }
    }

    public class EffectRange : Range { }

    public class Spell
    {
#if USE_MONGO
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#endif
        public string Id { get; set; }

#if USE_MONGO
        [BsonElement("Price")]
#endif
        public int Price { get; set; }

#if USE_MONGO
        [BsonElement("Name")]
#endif
        public string Name { get; set; }

#if USE_MONGO
        [BsonElement("Damages")]
#endif
        public int Damages { get; set; }

#if USE_MONGO
        [BsonElement("SpellRange")]
#endif
        public SpellRange SpellRange { get; set; }

#if USE_MONGO
        [BsonElement("EffectRange")]
#endif
        public EffectRange EffectRange { get; set; }

#if USE_MONGO
        [BsonElement("ActionPointsCost")]
#endif
        public int ActionPointsCost { get; set; }

#if USE_MONGO
        [BsonElement("Model3D")]
#endif
        public string Model3D { get; set; }
    }
}
