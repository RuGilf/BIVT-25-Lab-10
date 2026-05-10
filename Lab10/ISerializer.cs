namespace Lab10;

public interface ISerializer
{
    T Deserialize<T>() where T : class;
    void Serialize<T>(T obj) where T : class;
}
