using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Create;

public interface ICreate
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    public Task CreateFolders(string baseDirectory, Queue<string> foldersQueue);

    public Task<Queue<string>> PrepareAllStrings(string path, int octaves = 25, int countIncluded = 10);
}
