using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Pack;

public class PackService(IProgressCalculation progressCalculation) : IPack
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged = null!;
    public void OnProgressChanged(ProgressChangedEventArgs.Progresses processID, double percent, string status)
    {
        ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(processID, percent, status));
    }

    public async Task PackData(string basePath, string pathToFolder, string key)
    {
        if (!Directory.Exists(basePath)) throw new ArgumentException("basePath должен указывать на папку");
        if (!Directory.Exists(pathToFolder)) throw new ArgumentException("pathToFolder должен указывать на папку");

        string searchedPath = await SearchPathForKey(basePath, key);

        await Task.Run(() => { Directory.Move(pathToFolder, Path.Combine(searchedPath, "data")); });

        await Task.Delay(10);

        if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.Pack, 100, progressCalculation.ProgressCalculation(100, 100));
    }

    public async Task<string> SearchPathForKey(string basePath, string key)
    {
        if (!int.TryParse(key, out _)) throw new ArgumentException("Key должен быть числом");
        if (!Directory.Exists(basePath)) throw new ArgumentException("basePath должен указывать на папку");

        string[] directories = Directory.GetDirectories(basePath);
        string searchedDirectory = null!;

        for (int i = 0; i < key.Length; i++)
        {
            int directoryId = int.Parse(key[i].ToString()) - 1;

            if (directoryId < directories.Length && directories[directoryId] != null && Directory.Exists(directories[directoryId]))
            {
                searchedDirectory = directories[directoryId];
                directories = Directory.GetDirectories(directories[directoryId]);
            }
            else throw new ArgumentException($"Key элемент под номером {i} указывает на несуществующую папку");

            if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.Pack, progressCalculation.Percent(i, key.Length + 5), progressCalculation.ProgressCalculation(i, key.Length + 5));
            await Task.Delay(10);
        }

        return searchedDirectory;
    }
}
