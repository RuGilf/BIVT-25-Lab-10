namespace Lab10;

public abstract class MyFileManager : IFileLifeController, IFileManager
{
    private string _name;
    private string _folderPath;
    private string _fileName;
    private string _fileExtension;

    public string Name => _name;

    public MyFileManager(string name)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));

        _name = name;
        _folderPath = string.Empty;
        _fileName = string.Empty;
        _fileExtension = "txt";
    }

    public MyFileManager(string name, string folderPath, string fileName, string fileExtension = "txt")
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (folderPath is null) throw new ArgumentNullException(nameof(folderPath));
        if (fileName is null) throw new ArgumentNullException(nameof(fileName));
        if (fileExtension is null) throw new ArgumentNullException(nameof(fileExtension));

        _name = name;
        _folderPath = folderPath;
        _fileName = fileName;
        _fileExtension = fileExtension;
    }

    public string FolderPath { get => _folderPath; private set => _folderPath = value; }
    public string FileName { get => _fileName; private set => _fileName = value; }
    public string FileExtension { get => _fileExtension; private set => _fileExtension = value; }

    public string FullPath
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_folderPath) || string.IsNullOrWhiteSpace(_fileName))
                return string.Empty;
            
            string ext = _fileExtension.StartsWith(".") ? _fileExtension : $".{_fileExtension}";
            return Path.Combine(_folderPath, $"{_fileName}{ext}");
        }
    }

    public void SelectFolder(string folderPath)
    {
        if (folderPath is null) throw new ArgumentNullException(nameof(folderPath));
            _folderPath = folderPath;
    }

    public void ChangeFileName(string fileName)
    {
        if (fileName is null) throw new ArgumentNullException(nameof(fileName));
        _fileName = fileName;
    }

    public void ChangeFileFormat(string fileExtension)
    {
        if (fileExtension is null) throw new ArgumentNullException(nameof(fileExtension));
        _fileExtension = fileExtension;

        string path = FullPath;
        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(_folderPath) && !File.Exists(path))
        {
            File.Create(path).Close();
        }
    }

    public void CreateFile()
    {
        string path = FullPath;
        if (string.IsNullOrWhiteSpace(path)) return;

        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);
        
        if (!File.Exists(path))
            File.Create(path).Close();
    }

    public void DeleteFile()
    {
        string path = FullPath;
        if (File.Exists(path))
            File.Delete(path);
    }

    public virtual void EditFile(string content)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));

        string path = FullPath;
        if (string.IsNullOrWhiteSpace(path)) return;

        File.WriteAllText(path, content);
    }

    public virtual void ChangeFileExtension(string newExtension)
    {
        if (newExtension is null) throw new ArgumentNullException(nameof(newExtension));

        string oldPath = FullPath;
        bool fileExists = File.Exists(oldPath);

        string existingContent = fileExists ? File.ReadAllText(oldPath) : string.Empty;

        ChangeFileFormat(newExtension);
        string newPath = FullPath;

        if (fileExists)
        {
            File.WriteAllText(newPath, existingContent);

            if (oldPath != newPath)
            {
                File.Delete(oldPath);
            }
        }
    }
}