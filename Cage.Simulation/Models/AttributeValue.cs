using System.Text.Json.Serialization;

namespace Cage.Simulation.Models;

[JsonConverter(typeof(AttributeValueJsonConverter))]
public class AttributeValue
{
    public AttributeType Type { get; private set; }
    public object? Value { get; private set; }

    public AttributeValue(int value)
    {
        Type = AttributeType.Int;
        Value = value;
    }

    public AttributeValue(Entity value)
    {
        Type = AttributeType.Entity;
        Value = value;
    }

    public AttributeValue(List<Entity> value)
    {
        Type = AttributeType.EntityList;
        Value = value;
    }

    public int AsInt() => Type == AttributeType.Int && Value != null ? (int)Value : throw new InvalidCastException();
    public Entity AsEntity() => Type == AttributeType.Entity && Value != null ? (Entity)Value : throw new InvalidCastException();
    public List<Entity> AsEntityList() => Type == AttributeType.EntityList && Value != null ? (List<Entity>)Value : throw new InvalidCastException();
}