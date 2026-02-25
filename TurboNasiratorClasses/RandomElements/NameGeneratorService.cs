using System.Text;

namespace TurboNasiratorClasses.RandomElements;

public class NameGeneratorService(Random random) : INameGenerator
{
    private const string SYMBOLS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string NextName(int count = 5)
    {
        StringBuilder builder = new(string.Empty);
        for (int i = 0; i < count; i++)
        {
            builder.Append(SYMBOLS[random.Next(0, SYMBOLS.Length)]);
        }
        return builder.ToString();
    }
}
