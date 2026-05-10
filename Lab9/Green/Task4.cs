using System.Runtime.CompilerServices;
using System.Text;

namespace Lab9.Green;

public class Task4 : Green
{
    private string[] _answer;
    public string[] Output => _answer;
    public Task4(string text) : base(text)
    {
        _answer = new string[0];
    }

    public override void Review()
    {
        _answer = Input.Split(", ");

        for (int i = 0; i < _answer.Length; i++)
            for (int j = 0; j < _answer.Length - 1 - i; j++)
            {
                if (IsGreater(_answer[j], _answer[j + 1]))
                    (_answer[j], _answer[j + 1]) = (_answer[j + 1], _answer[j]);
            }
    }

    public override string ToString()
    {
        if (_answer == null || _answer.Length == 0) return "";

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _answer.Length - 1; i++)
            sb.Append(_answer[i])
            .Append("\n");

        sb.Append(_answer[_answer.Length - 1]);

        return Convert.ToString(sb);
    }

    private bool IsGreater(string word1, string word2)
    {
        int compareLenght;
        if (word1.Length > word2.Length)
            compareLenght = word2.Length;
        else 
            compareLenght = word1.Length;
        
        for (int i = 0; i < compareLenght; i++)
        {
            if (word1[i] > word2[i])
                return true;
            else if (word2[i] > word1[i])
                return false;
        }

        if (compareLenght == word2.Length)
            return true;
        
        return false;
    }
}
