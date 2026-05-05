using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Events;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class EventPropertyAttribute : Attribute
{
    public CageType Type { get; }
    public string? PropertyName { get; }

    public EventPropertyAttribute(CageType type, string? propertyName = null)
    {
        Type = type;
        PropertyName = propertyName;
    }
}