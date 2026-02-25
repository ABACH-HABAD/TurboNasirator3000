using TurboNasiratorClasses.RandomElements;

namespace TurboNasiratorClasses.Create;

public class CreateFolderService(INameGenerator nameGenerator) : ICreateFolder
{
    public async Task<List<string>> PreparePaths(string path, int countIncluded = 10)
    {
        List<string> includedFolders = [];
        for (int i = 0; i < countIncluded; i++)
        {
            includedFolders.Add(Path.Combine(path, nameGenerator.NextName()));
        }
        return includedFolders;
    }

    public async Task CreateFolder(string path)
    {
        Directory.CreateDirectory(path);
        await Task.Delay(1);
    }
}
