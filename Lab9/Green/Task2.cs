using System;
using System.Linq;
using System.Text;

namespace Lab9.Green;

public class Task2 : Green
{
    private char[] _answer;

    public char[] Output => _answer;

    public Task2(string text) : base(text)
    {
        _answer = new char[0];
    }

    public override void Review()
    {
        if (string.IsNullOrEmpty(Input)) return;

        _answer = Input.ToLower()
            .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word =>
            {
                foreach (char c in word)
                {
                    if (char.IsLetter(c)) return c;
                    if (char.IsNumber(c)) break; 
                }
                return '\0';
            })
            .Where(c => c != '\0')
            .GroupBy(c => c)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => group.Key)
            .ToArray();
    }

    public override string ToString()
    {
        if (_answer == null || _answer.Length == 0) return "";

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _answer.Length; i++)
        {
            sb.Append(_answer[i]);
            if (i < _answer.Length - 1) 
                sb.Append(", ");
        }

        return sb.ToString();
    }
}