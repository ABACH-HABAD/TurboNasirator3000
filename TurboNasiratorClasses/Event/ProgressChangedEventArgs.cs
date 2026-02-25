namespace TurboNasiratorClasses.Event;

public class ProgressChangedEventArgs(ProgressChangedEventArgs.Progresses processID, double persentage, string status, int foldersCount = 0)
{
    public enum Progresses
    {
        FirstCreate,
        SecondCreate,

        FirstDelete,
        SecondDelete,

        Pack,
        Unpack,
    }

    public Progresses ProcessID { get; } = processID;
    public double Persentage { get; } = persentage;
    public string Status { get; } = status;
    public int FoldersCount { get; } = foldersCount;
}