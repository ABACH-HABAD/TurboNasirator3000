namespace TurboNasiratorClasses.Computations;

public class GeometricProgression : IProgression
{
    public ulong Sum(int number, int denominator)
    {
        if (denominator > 1 && number > 1) return ProgressingSum(number: number, denominator: denominator);
        else if (number == 1 && denominator != 1) return (ulong)denominator;
        else if (denominator == 1 && number != 1) return (ulong)number;
        else return 0;
    }

    public ulong ProgressingSum(int number, int denominator)
    {
        if (denominator <= 1) throw new ArgumentException("Знаменатель не может быть 1 или меньше");
        if (number <= 1) throw new ArgumentException("Количство членов не может быть 1 или меньше");

        return (
            (ulong)((ulong)denominator * ((ulong)Math.Pow(denominator, number) - 1)) /
            (ulong)(denominator - 1)
            );
    }
}
