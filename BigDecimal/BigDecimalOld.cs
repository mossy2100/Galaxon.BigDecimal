using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics;

/// <summary>
/// Old, cut code, keeping just in case.
/// </summary>
public partial struct BigDecimal
{
    public static BigDecimal LogHalleys(BigDecimal a)
    {
        // Guards.
        if (a == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of 0 is -∞, which cannot be expressed using a BigDecimal.");
        }
        if (a < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of a negative value is a complex number, which cannot be expressed using a BigDecimal.");
        }

        // Optimization.
        if (a == 1)
        {
            return 0;
        }

        // Shortcut for Log(10).
        if (a == 10 && s_ln10.NumSigFigs >= MaxSigFigs)
        {
            return RoundSigFigs(s_ln10);
        }

        // Scale the value to the range (0..1).
        int nDigits = a.Significand.NumDigits();
        int scale = nDigits + a.Exponent;
        BigDecimal x = a;
        x.Exponent = -nDigits;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Halley's method.
        BigDecimal y0 = 0;
        BigDecimal expY0 = 1;
        BigDecimal dY0 = 2 * (x - 1) / (x + 1);
        BigDecimal result = 0;

        int nLoops = 0;
        while (true)
        {
            // Get the next value.
            BigDecimal expY1 = expY0 + expY0 * (Exp(dY0) - 1);
            BigDecimal y1 = y0 + dY0;

            // Test for equality.
            if (y0 == y1)
            {
                result = y0;
                break;
            }

            // Test for equality post-rounding.
            BigDecimal y0R = RoundSigFigs(y0, prevMaxSigFigs);
            BigDecimal y1R = RoundSigFigs(y1, prevMaxSigFigs);
            if (y0R == y1R)
            {
                result = y0R;
                break;
            }

            // Compare two results that differ by the smallest possible amount.
            // We need this check to prevent infinite loops that alternate between adjacent values.
            y0R.ShiftToSigFigs(prevMaxSigFigs);
            y1R.ShiftToSigFigs(prevMaxSigFigs);
            if (BigInteger.Abs(y0R.Significand - y1R.Significand) == 1)
            {
                // Test both and pick the best one.
                BigDecimal diff0 = Abs(a - Exp(y0R));
                BigDecimal diff1 = Abs(a - Exp(y1R));
                result = diff0 < diff1 ? y0R : y1R;
                break;
            }

            // Next iteration.
            y0 = y1;
            expY0 = expY1;
            dY0 = 2 * (x - expY1) / (x + expY1);

            nLoops++;
            if (nLoops == 10000)
            {
                // Console.WriteLine("Too many loops");
                break;
            }
        }
        // Console.WriteLine($"nLoops = {nLoops}");

        // Special handling for Log(10) to avoid infinite recursion.
        result = a == 10 ? -result : result + scale * Ln10;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        // Scale back.
        return RoundSigFigs(result);
    }

    public static BigDecimal LogAgm(BigDecimal x)
    {
        // Guards.
        if (x == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Logarithm of 0 is -∞, which cannot be expressed using a BigDecimal.");
        }
        if (x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Logarithm of a negative value is a complex number, which cannot be expressed using a BigDecimal.");
        }

        // Optimization.
        if (x == 1)
        {
            return 0;
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        // int nDigits = a.Significand.NumDigits();
        // int scale = nDigits + a.Exponent;
        // BigDecimal x = a;
        // x.Exponent = -nDigits;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 3;

        // Select a value "m" such that m bits can store the required number of decimal digits.
        // A quick and easy way to find m without using Log() uses 10 bits per 3 decimal digits.
        // m must be an int to avoid recursively calling Log() when we call Exp2() below, so we
        // round up to the nearest integer.
        int m = (int)double.Ceiling((double)MaxSigFigs / 3 * 10);

        // Calculate result.
        BigDecimal s = x * Exp2(m - 2);
        BigDecimal agm = ArithmeticGeometricMean(1, 1 / s);
        BigDecimal p = Pi / (2 * agm);
        BigDecimal result = x == 2
            ? p / (1 + m)
            : p - m * LogAgm(2);

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(result);
    }

    public static float ConvertToFloatUsingMaths(BigDecimal bd)
    {
        // Check for 0.
        if (bd == 0)
        {
            return 0f;
        }

        // Check the value is within the supported range for float.
        if (bd < float.MinValue || bd > float.MaxValue)
        {
            throw new OverflowException();
        }

        // Initialize.
        BigDecimal abs = Abs(bd);
        bool isSubnormal = abs < XFloat.MinPosNormalValue;
        BigInteger exp = isSubnormal ? XFloat.MinExp : (BigInteger)Floor(Log2(abs));
        BigDecimal sig = abs / Exp2(exp);
        ulong fracBits = 0;
        byte nextBit = 0;

        // Collect 24 bits.
        for (int i = 0; i < XFloat.NumFracBits + 1; i++)
        {
            // Get the next bit.
            nextBit = (byte)(sig < 1 ? 0 : 1);

            // Add the bit.
            fracBits = (fracBits << 1) + nextBit;

            // Prepare for next iteration.
            sig = (sig - nextBit) * 2;
        }

        // Round up if necessary, using MidpointRounding.ToEven method.
        if (nextBit == 1 && sig >= 0.5 || nextBit == 0 && sig > 0.5)
        {
            // Don't go over 24 bits.
            if (fracBits == 0b11111111_11111111_11111111)
            {
                fracBits = 0b10000000_00000000_00000000;
                exp--;
            }
            else
            {
                fracBits += 1;
            }
        }

        // Get the float's parts.
        byte signBit = (byte)(bd < 0 ? 1 : 0);
        ushort expBits = 0;
        if (!isSubnormal)
        {
            // Convert exponent to stored value.
            expBits = (ushort)(exp + XFloat.MaxExp);
            // Clear bit 23 as this isn't stored in the float.
            fracBits &= 0b01111111_11111111_11111111;
        }

        // Assemble the final value.
        return XFloatingPoint.Assemble<float>(signBit, expBits, fracBits);
    }
}
