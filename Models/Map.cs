using System.Collections.Generic;
#if USE_MONGO
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace TBS.Models
{
    public class Map
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
        [BsonElement("Length")]
#endif
        public int Length { get; set; }

#if USE_MONGO
        [BsonElement("Width")]
#endif
        public int Width { get; set; }

#if USE_MONGO
        [BsonElement("Tiles")]
#endif
        public List<Tile> Tiles { get; set; }
    }

    public class Tile
    {
#if USE_MONGO
        [BsonElement("XPosition")]
#endif
        public int XPosition { get; set; }

#if USE_MONGO
        [BsonElement("YPosition")]
#endif
        public int YPosition { get; set; }

#if USE_MONGO
        [BsonElement("Height")]
#endif
        public int Height { get; set; }

#if USE_MONGO
        [BsonElement("TileName")]
#endif
        public string TileName { get; set; }

#if USE_MONGO
        [BsonElement("Impasible")]
#endif
        public bool Impasible { get; set; }

#if USE_MONGO
        [BsonElement("PropName")]
#endif
        public string PropName { get; set; }
    }
}
