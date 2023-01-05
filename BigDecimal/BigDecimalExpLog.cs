using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Power, root, exponential, and logarithm methods for BigDecimal.
/// </summary>
public partial struct BigDecimal : IPowerFunctions<BigDecimal>, IRootFunctions<BigDecimal>,
    IExponentialFunctions<BigDecimal>, ILogarithmicFunctions<BigDecimal>
{
    #region Power functions

    public static BigDecimal Pow(BigDecimal x, BigDecimal y)
    {
        // Handle negative powers.
        if (y < 0)
        {
            return 1 / Pow(x, -y);
        }

        // Anything to the power of 0 (including 0) is 1.
        if (y == 0)
        {
            return 1;
        }

        // Anything to the power of 1 is itself.
        if (y == 1)
        {
            return x;
        }

        // 0 to any power other than 0 is 0.
        if (x == 0)
        {
            return 0;
        }

        // If the exponent is an integer we can computer a result quickly with exponentiation by
        // squaring.
        if (IsInteger(y))
        {
            // 10 to any power of an integer is easy.
            if (x == 10 && y >= int.MinValue && y <= int.MaxValue)
            {
                return new BigDecimal(1, (int)y);
            }

            // Even integer powers.
            if (IsEvenInteger(y))
            {
                return Pow(Sqr(x), y / 2);
            }

            // Odd integer powers.
            return x * Pow(Sqr(x), (y - 1) / 2);
        }

        // For positive x with non-integer exponent, compute the result using Exp() and Ln().
        if (x > 0)
        {
            return Exp(y * Log(x));
        }

        // For negative x with non-integer exponent, we need to get the exponent as a ratio in order
        // to determine whether or not we can compute a result.
        // Casting to BigRational will create a rational number, with no loss of precision, and
        // reduce it.
        // The only issue here is if the exponent does not exactly represent the intended value.
        // e.g. 1/3 cannot be stored exactly using a BigDecimal.
        // Thus Pow(-27, 1/3), for example, will not work. You have to use Cbrt().
        BigRational br = y;
        // To compute the result using RootN() the denominator of the fraction must be odd and in
        // the range of int.
        if (br.Denominator > int.MaxValue)
        {
            // Can't compute a real result.
            throw new ArithmeticException(
                $"Cannot compute a result as the exponent denominator ({br.Denominator}) is "
                + $"greater than {int.MaxValue}.");
        }
        if (BigInteger.IsEvenInteger(br.Denominator))
        {
            // Can't compute a real result.
            throw new ArithmeticException(
                $"Cannot compute a result as the exponent denominator ({br.Denominator}) is even.");
        }
        return Pow(RootN(x, (int)br.Denominator), br.Numerator);
    }

    public static BigDecimal Sqr(BigDecimal x) =>
        x * x;

    public static BigDecimal Cube(BigDecimal x) =>
        x * x * x;

    #endregion Power functions

    #region Root functions

    /// <inheritdoc />
    public static BigDecimal RootN(BigDecimal a, int n)
    {
        // If n is even, a negative value for a is unsupported as there will be no real results,
        // only complex ones.
        if (a < 0 && int.IsEvenInteger(n))
        {
            throw new ArithmeticException("Negative numbers have no real even roots.");
        }

        if (a == 0)
        {
            return 0;
        }

        if (a == 1)
        {
            return 1;
        }

        switch (n)
        {
            // A negative root is the inverse of the positive root.
            case < 0:
                return 1 / RootN(a, -n);

            // 0th root unsolvable.
            case 0:
                throw new ArgumentInvalidException(nameof(n),
                    "The 0th root is unsolvable since any number to the power of 0 is 1.");

            // The 1st root of a number is itself.
            case 1:
                return a;
        }

        // Get an initial estimate using double, which should be pretty fast.
        // Reduce operand to the maximum number of significant digits supported by the double type.
        BigDecimal aRound = RoundSigFigs(a, DoubleMaxSigFigs);
        double sig = double.RootN((double)aRound.Significand, n);
        BigDecimal exp = (BigDecimal)aRound.Exponent / n;
        BigDecimal x0 = (BigDecimal)sig * Exp10(exp);

        // Check if our estimate is already our result.
        if (Pow(x0, n) == a)
        {
            return x0;
        }

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        BigDecimal result;

        // Newton's method.
        // int nLoops = 0;
        while (true)
        {
            // Get the next value of y.
            BigDecimal xm = Pow(x0, n - 1);
            BigDecimal xn = xm * x0;
            BigDecimal x1 = x0 - (xn - a) / (n * xm);

            // Test for equality.
            if (x0 == x1)
            {
                result = RoundSigFigs(x0, prevMaxSigFigs);
                break;
            }

            // Test for equality post-rounding.
            BigDecimal x0Round = RoundSigFigs(x0, prevMaxSigFigs);
            BigDecimal x1Round = RoundSigFigs(x1, prevMaxSigFigs);
            if (x0Round == x1Round)
            {
                result = x0Round;
                break;
            }

            // Compare two results that differ by the smallest possible amount.
            // We need this check to prevent infinite loops that alternate between adjacent values.
            x0Round.ShiftToSigFigs(prevMaxSigFigs);
            x1Round.ShiftToSigFigs(prevMaxSigFigs);
            if (BigInteger.Abs(x0Round.Significand - x1Round.Significand) == 1)
            {
                // Test both and pick the best one.
                BigDecimal diff0 = Abs(a - Pow(x0Round, n));
                BigDecimal diff1 = Abs(a - Pow(x1Round, n));
                result = diff0 < diff1 ? x0Round : x1Round;
                break;
            }

            // Next iteration.
            x0 = x1;

            // Prevent infinite loops. Remove later, after testing.
            // nLoops++;
            // if (nLoops == 100)
            // {
            //     throw new Exception($"Problem with RootN({a}, {n}).");
            // }
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return result;
    }

    public static BigDecimal Sqrt(BigDecimal x) =>
        RootN(x, 2);

    public static BigDecimal Cbrt(BigDecimal x) =>
        RootN(x, 3);

    public static BigDecimal Hypot(BigDecimal x, BigDecimal y) =>
        Sqrt(Sqr(x) + Sqr(y));

    #endregion Root functions

    #region Exponential functions

    /// <inheritdoc />
    public static BigDecimal Exp(BigDecimal x)
    {
        // Optimizations.
        if (x == 0)
        {
            return 1;
        }

        BigDecimal sum;

        // If the exponent is negative, inverse the result of the positive exponent.
        if (x < 0)
        {
            return 1 / Exp(-x);
        }

        // Taylor/Maclaurin series.
        // https://en.wikipedia.org/wiki/Taylor_series#Exponential_function
        BigInteger n = 0;
        BigDecimal xn = 1; // x^n
        BigInteger nf = 1; // n!
        sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newSum = sum + xn / nf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            xn *= x;
            nf *= n;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal Exp2(BigDecimal x) =>
        Pow(2, x);

    /// <inheritdoc />
    public static BigDecimal Exp10(BigDecimal x) =>
        Pow(10, x);

    #endregion Exponential functions

    #region Logarithmic functions

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal a)
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
        if (a == 10 && _ln10.NumSigFigs >= MaxSigFigs)
        {
            return RoundSigFigs(_ln10);
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        int nDigits = a.Significand.NumDigits();
        int scale = nDigits + a.Exponent;
        BigDecimal x = a;
        x.Exponent = -nDigits;

        // Taylor/Newton-Mercator series.
        // https://en.wikipedia.org/wiki/Mercator_series
        x--;
        // Console.WriteLine($"a = {a}");
        // Console.WriteLine($"x = {x}");
        int n = 1;
        int sign = 1;
        BigDecimal xn = x;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        int nLoops = 0;
        while (true)
        {
            // Add the next term in the series
            BigDecimal newSum = sum + (sign * xn / n);

            // Console.WriteLine(newSum);

            // If adding the new term hasn't affected the result, we're done.
            if (newSum == sum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            sign = -sign;
            xn *= x;

            nLoops++;
            if (nLoops == 5000)
            {
                Console.WriteLine("Too many loops");
                break;
            }
        }
        // Console.WriteLine($"nLoops = {nLoops}");

        // Special handling for Log(10) to avoid infinite recursion.
        BigDecimal result = a == 10 ? -sum : sum + scale * Ln10;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        // Scale back.
        return RoundSigFigs(result);
    }

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal x, BigDecimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (x == 1 && b == 0)
        {
            return 0;
        }

        // This will throw if x <= 0 || b <= 0.
        return Log(x) / Log(b);
    }

    /// <inheritdoc />
    public static BigDecimal Log2(BigDecimal x) =>
        Log(x, 2);

    /// <inheritdoc />
    public static BigDecimal Log10(BigDecimal x) =>
        Log(x, 10);

    #endregion Logarithmic functions
}