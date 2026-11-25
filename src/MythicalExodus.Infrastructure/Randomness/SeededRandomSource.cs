using MythicalExodus.Domain.Abstractions;

namespace MythicalExodus.Infrastructure.Randomness;

/// <summary>
/// Small deterministic SplitMix64 generator. Equal seeds produce equal sequences.
/// This is for game simulation, not cryptographic security.
/// </summary>
public sealed class SeededRandomSource : IRandomSource
{
    private ulong _state;

    public SeededRandomSource(ulong seed) => _state = seed;

    public int NextInt(int inclusiveMinimum, int exclusiveMaximum)
    {
        if (exclusiveMaximum <= inclusiveMinimum)
            throw new ArgumentOutOfRangeException(nameof(exclusiveMaximum),
                "The exclusive maximum must be greater than the inclusive minimum.");

        var range = (ulong)((long)exclusiveMaximum - inclusiveMinimum);
        var threshold = unchecked(0UL - range) % range;
        ulong sample;
        do
        {
            sample = NextUInt64();
        } while (sample < threshold);

        return checked((int)(inclusiveMinimum + (long)(sample % range)));
    }

    public double NextDouble() =>
        (NextUInt64() >> 11) * (1.0 / (1UL << 53));

    private ulong NextUInt64()
    {
        _state += 0x9E3779B97F4A7C15UL;
        var value = _state;
        value = (value ^ (value >> 30)) * 0xBF58476D1CE4E5B9UL;
        value = (value ^ (value >> 27)) * 0x94D049BB133111EBUL;
        return value ^ (value >> 31);
    }
}
