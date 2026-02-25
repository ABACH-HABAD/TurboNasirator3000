using TurboNasiratorClasses.Event;
using TurboNasiratorClasses.Computations;

namespace TurboNasiratorClasses.Create;

public class CreateService(ICreateFolder createFolderService, IProgressCalculation progressCalculation, IProgression progression) : ICreate
{
    public event EventHandler<ProgressChangedEventArgs> ProgressChanged = null!;

    public void OnProgressChanged(ProgressChangedEventArgs.Progresses processID, double percent, string status)
    {
        ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(processID, percent, status));
    }

    public async Task CreateFolders(string baseDirectory, Queue<string> foldersQueue)
    {
        int total = foldersQueue.Count;

        if (!Directory.Exists(baseDirectory)) Directory.CreateDirectory(baseDirectory);

        while (foldersQueue.Count > 0)
        {
            await createFolderService.CreateFolder(foldersQueue.Dequeue());
            await Task.Delay(1);
            if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.SecondCreate, progressCalculation.Percent(total - foldersQueue.Count, total), progressCalculation.ProgressCalculation(total - foldersQueue.Count, total));
        }
    }

    public async Task<Queue<string>> PrepareAllStrings(string path, int octaves = 25, int countIncluded = 10)
    {
        Queue<string> includedFolders = new();

        List<string> preparedInOctave = [];

        preparedInOctave.Add(path);

        ulong total = progression.Sum(octaves, countIncluded);
        ulong num = 0;

        FolderNode rootOfFoldersTree = FolderNode.CreateRootOfFolderTree(path);
        FolderNode workingNode = rootOfFoldersTree;

        do
        {
            FolderNode? nextNode;

            List<string> localPaths = await createFolderService.PreparePaths(workingNode.Path, countIncluded);
            foreach (string localPath in localPaths)
            {
                workingNode.CreateNode(localPath);

                await Task.Delay(1);
                includedFolders.Enqueue(localPath);
                num++;
                if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.FirstCreate, progressCalculation.Percent(num, total), progressCalculation.ProgressCalculation(num, total));
            }

            if (workingNode.Octave < octaves) nextNode = workingNode.IncludetNode();
            else nextNode = workingNode.NextOrRootNode();

            if (nextNode != null) workingNode = nextNode;
            else break;

            if (workingNode == null) break;

        }
        while (workingNode != rootOfFoldersTree);

        if (num != total) num = total;
        if (ProgressChanged != null) OnProgressChanged(ProgressChangedEventArgs.Progresses.FirstCreate, progressCalculation.Percent(num, total), progressCalculation.ProgressCalculation(num, total));

        return includedFolders;
    }
}
