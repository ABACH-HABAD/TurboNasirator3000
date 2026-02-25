using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Pack;

public interface IPack
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        
    public Task PackData(string basePath, string pathToFolder, string key);

    public  Task<string> SearchPathForKey(string basePath, string key);
}
