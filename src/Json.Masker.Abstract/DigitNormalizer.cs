using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Json.Masker.Abstract;

internal static class DigitNormalizer
{
    // Tune this: ~2x SIMD width is a good default on x64 (32 chars on AVX2, 16 on SSE2).
    private static readonly int SmallInputSimdCutoff = Vector<ushort>.Count * 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Normalize(ReadOnlySpan<char> input, Span<char> outputBuffer)
    {
        if (outputBuffer.Length < input.Length)
            throw new ArgumentException("Output buffer too small", nameof(outputBuffer));

        int count = 0;

        // For short inputs (or no SIMD), scalar is cheaper than warming up vectors.
        if (!Vector.IsHardwareAccelerated || input.Length < SmallInputSimdCutoff)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if ((uint)(c - '0') <= 9u) // branch-friendly digit test
                    outputBuffer[count++] = c;
            }
            return count;
        }

        // SIMD path
        var zero = new Vector<ushort>('0');
        var nine = new Vector<ushort>('9');
        int simd = Vector<ushort>.Count;

        int iBlock = 0;
        // Process full SIMD blocks
        for (; iBlock <= input.Length - simd; iBlock += simd)
        {
            // Load a block of chars as ushort
            var block = new Vector<ushort>(MemoryMarshal.Cast<char, ushort>(input.Slice(iBlock, simd)));

            var geZero = Vector.GreaterThanOrEqual(block, zero);
            var leNine = Vector.LessThanOrEqual(block, nine);
            var mask = Vector.BitwiseAnd(geZero, leNine);

            // Scatter digits
            for (int j = 0; j < simd; j++)
            {
                if (mask[j] != 0)
                    outputBuffer[count++] = (char)block[j];
            }
        }

        // Tail (remaining < simd)
        for (int i = iBlock; i < input.Length; i++)
        {
            char c = input[i];
            if ((uint)(c - '0') <= 9u)
                outputBuffer[count++] = c;
        }

        return count;
    }
}