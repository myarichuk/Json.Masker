using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Json.Masker.Abstract;

/// <summary>
/// Utility methods for extracting digits from strings while preserving their order.
/// </summary>
internal static class DigitNormalizer
{
    private static readonly int SmallInputSimdCutoff = Vector<ushort>.Count * 2;

    /// <summary>
    /// Copies digits from the input span into the destination buffer, skipping non-numeric characters.
    /// </summary>
    /// <param name="input">The source characters to inspect.</param>
    /// <param name="outputBuffer">The destination buffer that receives the digits.</param>
    /// <returns>The number of digits written to <paramref name="outputBuffer"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Normalize(ReadOnlySpan<char> input, Span<char> outputBuffer)
    {
        Debug.Assert(outputBuffer.Length >= input.Length, "output buffer is too short");

        var count = 0;

        // scalar is cheaper than using SIMD for small vectors
        if (!Vector.IsHardwareAccelerated || input.Length < SmallInputSimdCutoff)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];

                // branch-friendly digit test
                // I mean, CPUs can predict one jump so unify range check into one comparison
                if ((uint)(c - '0') <= 9u)
                {
                    outputBuffer[count++] = c;
                }
            }

            return count;
        }

        // SIMD path (doubt it will be used but its fun to try :)
        var zero = new Vector<ushort>('0');
        var nine = new Vector<ushort>('9');
        var simd = Vector<ushort>.Count;

        var iBlock = 0;

        // process blocks
        for (; iBlock <= input.Length - simd; iBlock += simd)
        {
            // Load a block of chars as ushort
            var block = new Vector<ushort>(MemoryMarshal.Cast<char, ushort>(input.Slice(iBlock, simd)));

            var geZero = Vector.GreaterThanOrEqual(block, zero);
            var leNine = Vector.LessThanOrEqual(block, nine);
            var mask = Vector.BitwiseAnd(geZero, leNine);

            for (var j = 0; j < simd; j++)
            {
                if (mask[j] != 0)
                {
                    outputBuffer[count++] = (char)block[j];
                }
            }
        }

        // remainder - can't vectorize less than vector buffer size
        for (var i = iBlock; i < input.Length; i++)
        {
            var c = input[i];
            if ((uint)(c - '0') <= 9u)
            {
                outputBuffer[count++] = c;
            }
        }

        return count;
    }
}