namespace TurboNasiratorClasses.Delete;

public class DeleteFolderService : IDeleteFolder
{
    public async Task DeleteFolder(string path)
    {
        if (!Directory.Exists(path)) return;

        await Task.Run(() => Directory.Delete(path, true));
        await Task.Delay(1);
    }

    public async Task DeleteFilesInFolder(string[] files)
    {
        foreach (string file in files)
        {
            File.Delete(file);
        }
        await Task.Delay(1);
    }

    public async Task DeleteFilesInFolder(string path)
    {
        string[] files = Directory.GetFiles(path);
        if (files.Length > 0) await DeleteFilesInFolder(files);
        await Task.Delay(1);
    }
}
