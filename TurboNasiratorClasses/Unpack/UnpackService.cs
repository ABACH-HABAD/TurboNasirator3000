using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Unpack;

public class UnpackService(IProgressCalculation progressCalculation) : IUnpack
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged = null!;

    public void OnProgressChanged(ProgressChangedEventArgs.Progresses processID, double percent, string status)
    {
        ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(processID, percent, status));
    }

    public async Task UnpackData(string basePath, string unpackPath, string key)
    {
        if (!Directory.Exists(basePath)) throw new ArgumentException("basePath должен указывать на папку");

        string searchedPath = await SearchPathForKey(basePath, key);

        await Task.Run(() => { Directory.Move(searchedPath, unpackPath); });

        await Task.Delay(10);

        if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.Unpack, 100, progressCalculation.ProgressCalculation(100, 100));
    }

    public async Task<string> SearchPathForKey(string basePath, string key)
    {
        string path = basePath;

        for (int i = 0; i < key.Length; i++)
        {
            if (Directory.Exists(path))
            {
                int numberOfFolder = int.Parse($"{key[i]}") - 1;
                await Task.Run(() => { path = Directory.GetDirectories(path)[numberOfFolder]; });
            }
            else throw new ArgumentException($"Key элемент под номером {i} указывает на несуществующую папку");

            if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.Unpack, progressCalculation.Percent(i, key.Length + 5), progressCalculation.ProgressCalculation(i, key.Length + 5));
            await Task.Delay(10);
        }

        return Path.Combine(path, "data");
    }
}
