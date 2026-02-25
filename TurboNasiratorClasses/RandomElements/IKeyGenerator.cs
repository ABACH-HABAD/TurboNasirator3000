namespace TurboNasiratorClasses.RandomElements;

public interface IKeyGenerator
{
    public string GenerateKey(int keyLength, int folderCount);

    public (int keyLength, int folderCount) StringPrepare(string folderCountString);
}
