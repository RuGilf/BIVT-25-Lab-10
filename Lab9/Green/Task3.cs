using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;

namespace Lab9.Green;

public class Task3 : Green
{
    private string[] _answer;
    private string _root;

    public string[] Output
    {
        get
        {
            return _answer;
        }
    }

    public Task3(string text, string root) : base(text)
    {
        _root = root;
        _answer = new string[0];
    }

    public override void Review()
    {
        _answer = Input
        .Split(Input.Where(c => !char.IsLetter(c)).Distinct().ToArray(), StringSplitOptions.RemoveEmptyEntries)
        .Where(word => word.Contains(_root, StringComparison.OrdinalIgnoreCase))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();
    }

    public override string ToString()
    {
        if (_answer.Length == 0 || _answer == null) return "";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < _answer.Length - 1; i++)
            sb.Append(_answer[i])
            .Append("\n");
        
        sb.Append(_answer[_answer.Length - 1]);
        

        return Convert.ToString(sb);
    }
}
