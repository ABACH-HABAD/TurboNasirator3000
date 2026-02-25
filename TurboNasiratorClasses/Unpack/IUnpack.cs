using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Unpack;

public interface IUnpack
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    public Task UnpackData(string basePath, string unpackPath, string key);

    public Task<string> SearchPathForKey(string basePath, string key);
}
