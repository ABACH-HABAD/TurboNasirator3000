namespace TurboNasiratorClasses.Create;

internal class FolderNode(string path, FolderNode rootNode, int octave)
{
    public string Path { get; init; } = path;
    public FolderNode RootNode { get; init; } = rootNode;
    public int Octave { get; init; } = octave;

    private readonly List<FolderNode> includet = [];
    public List<FolderNode> Includet => includet;

    public void CreateNode(string path)
    {
        includet.Add(new FolderNode(path, this, Octave + 1));
    }

    public FolderNode? NextOrRootNode()
    {
        if (RootNode != null)
        {
            for (int i = 0; i < RootNode.includet.Count; i++)
            {
                if (this == RootNode.Includet[i])
                {
                    if (RootNode.Includet.Count > i + 1) return RootNode.Includet[i + 1];
                    else break;
                }
            }

            FolderNode? node = RootNode.NextOrRootNode();
            if (node != null) return node;
            else return null;
        }
        else return null;
    }

    public FolderNode? IncludetNode()
    {
        if (Includet != null && Includet.Count > 0) return Includet[0];
        else return null;
    }

    public static void CreateNode(FolderNode baseNode, string newNodePath)
    {
        baseNode.Includet.Add(new FolderNode(newNodePath, baseNode, baseNode.Octave + 1));
    }

    public static FolderNode CreateRootOfFolderTree(string path)
    {
        return new(path, null!, 1);
    }

    public override string ToString() => Path;
}
