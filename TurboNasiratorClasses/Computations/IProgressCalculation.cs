namespace TurboNasiratorClasses.Computations;

public interface IProgressCalculation
{
    public const string DOES_NOT_STARTED = "Не начато";
    public const string IN_PROGRESS = "В процессе";
    public const string DONE = "Выполнено";

    public string ProgressCalculation(int now, int total);
    public string ProgressCalculation(ulong now, ulong total);

    public double Percent(ulong now, ulong total);
    public double Percent(int now, int total);
    public double Percent(double now, double total);
}
