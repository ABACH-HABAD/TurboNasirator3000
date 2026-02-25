using System.Text;

namespace TurboNasiratorClasses.RandomElements;

public class KeyGeneratorService(Random random) : IKeyGenerator
{
    public string GenerateKey(int keyLength, int folderCount)
    {
        StringBuilder keyBuilder = new(string.Empty);
        for (int i = 0; i < keyLength; i++)
        {
            keyBuilder.Append(random.Next(1, folderCount + 1));
        }
        return keyBuilder.ToString();
    }

    public (int keyLength, int folderCount) StringPrepare(string folderCountString)
    {
        int keyLength = random.Next(5, 25);

        int folderCount;
        if (int.TryParse(folderCountString, out int folderInFolder)) folderCount = folderInFolder;
        else folderCount = 10;

        if (folderCount > 10) folderCount = 10;

        return (keyLength, folderCount);
    }
}
