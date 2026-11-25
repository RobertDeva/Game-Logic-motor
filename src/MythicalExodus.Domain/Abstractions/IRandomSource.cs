namespace MythicalExodus.Domain.Abstractions;

/// <summary>Provides controlled randomness so simulations can be reproduced in tests.</summary>
public interface IRandomSource
{
    int NextInt(int inclusiveMinimum, int exclusiveMaximum);
    double NextDouble();
}
