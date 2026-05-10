using System.IO;
using System.Reflection; 
using Lab10;

namespace Lab10.Green;

public class GreenTxtFileManager : GreenFileManager
{
    public GreenTxtFileManager(string name) : base(name)
    {
    }

    public GreenTxtFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") 
        : base(name, folderPath, fileName, fileExtension)
    {
    }

    public override void EditFile(string content)
    {
        if (content is null) throw new ArgumentNullException(nameof(content));
        if (!File.Exists(FullPath)) return;

        var obj = Deserialize<Lab9.Green.Green>();
        if (obj is null) return;

        obj.ChangeText(content);
        Serialize(obj);
    }

    public override void ChangeFileExtension(string newExtension)
    {
        if (newExtension is null) throw new ArgumentNullException(nameof(newExtension));
        
        base.ChangeFileExtension("txt");
    }

    public override void Serialize<T>(T obj) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        Type exactType = obj.GetType();
        var lines = new List<string>
        {
            $"TypeName={exactType.AssemblyQualifiedName}"
        };

        Type baseType = exactType;
        while (baseType != null)
        {
            foreach (var field in baseType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
            {
                var value = field.GetValue(obj);
                string strVal = value == null ? "null" : value.ToString();
                
                if (value is Array arr)
                {
                    var options = new System.Text.Json.JsonSerializerOptions { IncludeFields = true };
                    strVal = System.Text.Json.JsonSerializer.Serialize(value, options);
                }

                lines.Add($"{field.Name}={strVal}");
            }
            baseType = baseType.BaseType;
        }

        File.WriteAllLines(FullPath, lines);
    }

    public override T Deserialize<T>() where T : class
    {
        if (!File.Exists(FullPath)) return null;

        string[] lines = File.ReadAllLines(FullPath);
        if (lines.Length == 0) return null;

        string typeLine = lines[0];
        string typeName = typeLine.Replace("TypeName=", "");
        Type exactType = Type.GetType(typeName)!;

        object obj = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(exactType);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('=', 2); 
            if (parts.Length == 2)
            {
                string fieldName = parts[0];
                string fieldValue = parts[1];

                Type baseType = exactType;
                while (baseType != null)
                {
                    var field = baseType.GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (field != null)
                    {
                        if (fieldValue == "null")
                        {
                            field.SetValue(obj, null);
                        }
                        else if (fieldValue.StartsWith("[") || fieldValue.StartsWith("{") || fieldValue.StartsWith("\""))
                        {
                            try {
                                var options = new System.Text.Json.JsonSerializerOptions { IncludeFields = true };
                                object deserialized = System.Text.Json.JsonSerializer.Deserialize(fieldValue, field.FieldType, options);
                                field.SetValue(obj, deserialized);
                            } catch {
                                try {
                                    object safeValue = Convert.ChangeType(fieldValue, field.FieldType);
                                    field.SetValue(obj, safeValue);
                                } catch {}
                            }
                        }
                        else
                        {
                            try {
                                object safeValue = Convert.ChangeType(fieldValue, field.FieldType);
                                field.SetValue(obj, safeValue);
                            } catch {}
                        }
                        break;
                    }
                    baseType = baseType.BaseType;
                }
            }
        }

        return (T)obj;
    }
}