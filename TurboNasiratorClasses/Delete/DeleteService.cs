using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Create;
using TurboNasiratorClasses.Event;

namespace TurboNasiratorClasses.Delete;

public class DeleteService(IDeleteFolder deleteFolder,IProgressCalculation progressCalculation) : IDelete
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged = null!;

    public void OnProgressChanged(ProgressChangedEventArgs.Progresses processID, double percent, string status, int folderCount = 0)
    {
        ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(processID, percent, status, folderCount));
    }

    public async Task DeleteFolders(Stack<string> foldersStack)
    {
        int total = foldersStack.Count;

        while (foldersStack.Count > 0)
        {
            await deleteFolder.DeleteFolder(foldersStack.Pop());
            await Task.Delay(1);
            if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.SecondDelete, progressCalculation.Percent(total - foldersStack.Count, total), progressCalculation.ProgressCalculation(total - foldersStack.Count, total));
        }
    }

    public async Task<Stack<string>> PrepareAllStrings(string path)
    {
        Stack<string> includedFolders = new();

        List<string> preparedInOctave = [];
        
        preparedInOctave.Add(path);

        while(preparedInOctave.Count > 0)
        {
            List<string> nextOctave = [];

            foreach(string folder in preparedInOctave)
            {
                if (!Directory.Exists(folder)) continue;

                string[] nextDirectory = Directory.GetDirectories(folder);
                if (nextDirectory.Length > 0)
                {
                    foreach (string octave in nextDirectory)
                    {
                        nextOctave.Add(octave);
                    }
                }

                includedFolders.Push(folder);
                await Task.Delay(1);
                if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.FirstDelete, -1, progressCalculation.ProgressCalculation(-1, 100), includedFolders.Count);
            }

            preparedInOctave = nextOctave;
            
            await Task.Delay(1);
            if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.FirstDelete, -1, progressCalculation.ProgressCalculation(-1, 100), includedFolders.Count);
        }

        if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.FirstDelete, 100, progressCalculation.ProgressCalculation(100, 100), includedFolders.Count);
        return includedFolders;
    }
}
