namespace TurboNasiratorClasses.Computations;
public class ProgressCalculationService : IProgressCalculation
{
    public string ProgressCalculation(int now, int total)
    {
        if (now < 0) return IProgressCalculation.IN_PROGRESS;
        else if (now == 0) return IProgressCalculation.DOES_NOT_STARTED;
        else if (now == total) return IProgressCalculation.DONE;
        else return IProgressCalculation.IN_PROGRESS + $" ({(int)Math.Round(Percent(now, total))}%)";
    }

    public string ProgressCalculation(ulong now, ulong total)
    {
        if (now < 0) return IProgressCalculation.IN_PROGRESS;
        else if (now == 0) return IProgressCalculation.DOES_NOT_STARTED;
        else if (now == total) return IProgressCalculation.DONE;
        else return IProgressCalculation.IN_PROGRESS + $" ({(int)Math.Round(Percent(now, total))}%)";
    }

    public double Percent(ulong now, ulong total)
    {
        return Percent((double)now, (double)total);
    }

    public double Percent(int now, int total)
    {
       return Percent((double)now, (double)total);
    }

    public double Percent(double now, double total)
    {
        return (now / total) * 100;
    }
}
