using System.IO;
using System.Text.Json;
using Lab10;

namespace Lab10.Green;

public class GreenJsonFileManager : GreenFileManager
{
    public GreenJsonFileManager(string name) : base(name) {}

    public GreenJsonFileManager(string name, string folderPath, string fileName, string fileExtension = "json")
        : base(name, folderPath, fileName, fileExtension) {}

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

        base.ChangeFileExtension("json");
    }

    public override void Serialize<T>(T obj) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        var jsonDict = new Dictionary<string, object>();
        jsonDict["TypeName"] = obj.GetType().AssemblyQualifiedName;

        foreach (var prop in obj.GetType().GetProperties())
        {
            jsonDict[prop.Name] = prop.GetValue(obj);
        }

        var fields = new Dictionary<string, object>();
        Type baseType = obj.GetType();
        while (baseType != null)
        {
            foreach (var f in baseType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
            {
                fields[f.Name] = f.GetValue(obj);
            }
            baseType = baseType.BaseType;
        }
        jsonDict["Fields"] = fields;

        var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        string jsonString = JsonSerializer.Serialize(jsonDict, options);

        File.WriteAllText(FullPath, jsonString);
    }

    public override T Deserialize<T>() where T : class
    {
        if (!File.Exists(FullPath)) return null;

        string jsonString = File.ReadAllText(FullPath);
        
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;

        string typeName = root.GetProperty("TypeName").GetString()!;
        Type exactType = Type.GetType(typeName)!;

        object obj = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(exactType);

        if (root.TryGetProperty("Fields", out JsonElement fieldsElement))
        {
            foreach (var prop in fieldsElement.EnumerateObject())
            {
                string fieldName = prop.Name;
                Type baseType = exactType;
                while (baseType != null)
                {
                    var field = baseType.GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (field != null)
                    {
                        var options = new JsonSerializerOptions { IncludeFields = true };
                        var deserializedValue = JsonSerializer.Deserialize(prop.Value.GetRawText(), field.FieldType, options);
                        field.SetValue(obj, deserializedValue);
                        break;
                    }
                    baseType = baseType.BaseType;
                }
            }
        }

        return (T)obj;
    }
}