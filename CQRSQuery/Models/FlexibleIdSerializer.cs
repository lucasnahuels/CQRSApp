using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

public class FlexibleIdSerializer : SerializerBase<string>
{
    public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        switch (bsonType)
        {
            case BsonType.ObjectId:
                return context.Reader.ReadObjectId().ToString();
            case BsonType.Int32:
                return context.Reader.ReadInt32().ToString();
            case BsonType.String:
                return context.Reader.ReadString();
            default:
                throw new BsonSerializationException($"Cannot deserialize BsonType {bsonType} to string.");
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
    {
        if (ObjectId.TryParse(value, out var objectId))
        {
            context.Writer.WriteObjectId(objectId);
        }
        else if (int.TryParse(value, out var intValue))
        {
            context.Writer.WriteInt32(intValue);
        }
        else
        {
            context.Writer.WriteString(value);
        }
    }
}