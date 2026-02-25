using TurboNasiratorClasses.RandomElements;

namespace TurboNasiratorClasses.Create;

public interface ICreateFolder
{
    public Task<List<string>> PreparePaths(string path, int countIncluded = 10);

    public Task CreateFolder(string path);
}
