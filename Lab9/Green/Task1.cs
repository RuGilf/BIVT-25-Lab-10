using System.Data;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Channels;
using System.Text;

namespace Lab9.Green;

public class Task1 : Green
{
    private (char, double)[] _answer;

    public (char, double)[] Output
    {
        get { return _answer; }
    }

    public Task1(string text) : base(text)
    {
        _answer = new (char, double)[0];
    }

    public override void Review()
    {
        string text = Input.ToLower();
        int[] ruLettersCount = new int[33];
        string ruAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        int ruLettersTotal = 0;
        int lettersTotal = 0;
        int uniqueLetters = 0;

        foreach (char letter in text)
        {
            if (letter >= 'а' && letter <= 'я' || letter == 'ё')
            {
                ruLettersCount[ruAlphabet.IndexOf(letter)]++;
                ruLettersTotal++;
            }

            if (char.IsLetter(letter))
                lettersTotal++;
        }

        foreach (int x in ruLettersCount)
            if (x > 0)
                uniqueLetters++;
        
        _answer = new (char, double)[uniqueLetters];
        int indexOfAnswer = 0;

        for (int x = 0; x <= ruLettersCount.Length - 1; x++)
            if (ruLettersCount[x] > 0)
                _answer[indexOfAnswer++] = (ruAlphabet[x], (double) ruLettersCount[x] / lettersTotal);
    }

    public override string ToString()
    {
        if (_answer == null || _answer.Length == 0) return "";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < _answer.Length; i++)
        {
            sb.Append(_answer[i].Item1)
            .Append(":")
            .Append(_answer[i].Item2.ToString("F4"));

            if (i < _answer.Length - 1)
                sb.Append("\n");
        }

        return sb.ToString();
    }
}