using Lab10;

namespace Lab10.Green;

public abstract class GreenFileManager : MyFileManager, ISerializer
{
    protected GreenFileManager(string name) : base(name) {}
    protected GreenFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") : base(name, folderPath, fileName, fileExtension) {}

    public override void EditFile(string content)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));

        if (string.IsNullOrWhiteSpace(FullPath)) return;

        base.EditFile(content);
    }

    public override void ChangeFileExtension(string newExtension)
    {
        if (newExtension is null) throw new ArgumentNullException(nameof(newExtension));

        if (string.IsNullOrWhiteSpace(FullPath)) return;

        base.ChangeFileExtension(newExtension);
    }

    public abstract T Deserialize<T>() where T : class;
    public abstract void Serialize<T>(T obj) where T : class;
}