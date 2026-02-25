namespace TurboNasiratorClasses.Delete;

public interface IDeleteFolder
{
    public Task DeleteFolder(string path);

    public Task DeleteFilesInFolder(string[] files);
    public Task DeleteFilesInFolder(string path);
}
