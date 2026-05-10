namespace Lab10.Green;

public class Green
{
    public GreenFileManager Manager { get; private set; }
    public Lab9.Green.Green[] Tasks { get; private set; }

    public Green()
    {
        Tasks = Array.Empty<Lab9.Green.Green>();
    }

    public Green(GreenFileManager manager, Lab9.Green.Green[] tasks)
    {
        Manager = manager;
        Tasks = tasks ?? Array.Empty<Lab9.Green.Green>();
    }

    public void Add(Lab9.Green.Green task)
    {
        var list = Tasks.ToList();
        list.Add(task);
        Tasks = list.ToArray();
    }

    public void Add(Lab9.Green.Green[] tasks)
    {
        var list = Tasks.ToList();
        list.AddRange(tasks);
        Tasks = list.ToArray();
    }

    public void Remove(Lab9.Green.Green task)
    {
        var list = Tasks.ToList();
        list.Remove(task);
        Tasks = list.ToArray();
    }

    public void Clear()
    {
        Tasks = Array.Empty<Lab9.Green.Green>();
        
        if (Manager != null && !string.IsNullOrEmpty(Manager.FolderPath))
        {
            if (Directory.Exists(Manager.FolderPath))
            {
                Directory.Delete(Manager.FolderPath, true);
            }
        }
    }

    public void SaveTasks()
    {
        if (Manager == null) return;
        for (int i = 0; i < Tasks.Length; i++)
        {
            Manager.ChangeFileName($"task{i}");
            Manager.Serialize(Tasks[i]);
        }
    }

    public void LoadTasks()
    {
        if (Manager == null) return;
        for (int i = 0; i < Tasks.Length; i++)
        {
            Manager.ChangeFileName($"task{i}");
            try {
                Tasks[i] = Manager.Deserialize<Lab9.Green.Green>();
            } catch {
                Tasks[i] = null;
            }
        }
    }

    public void ChangeManager(GreenFileManager manager)
    {
        Manager = manager;
    }
}
