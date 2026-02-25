using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Delete;

public interface IDelete
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    public Task DeleteFolders(Stack<string> foldersStack);
    public  Task<Stack<string>> PrepareAllStrings(string path);
}
