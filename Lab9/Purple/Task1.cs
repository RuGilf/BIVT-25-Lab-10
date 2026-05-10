using System.Text;

namespace Lab9.Purple;

public class Task1 : Purple
{
    private string _output;
    public string Output => _output;

    public Task1(string text) : base(text) {}

    public override void Review()
    {
        string text = Input ?? "";
        StringBuilder result = new StringBuilder();

        List<char> reverseWord = new List<char>();

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsLetter(text[i]))
                reverseWord.Append(text[i]);
            
            else
            {
                reverseWord.Reverse();
                result.Append(reverseWord);
                result.Append(text[i]);
                reverseWord = new List<char>();
            }
        }

        _output = Convert.ToString(result) ?? "";
    }

    public override string ToString()
    {
        return _output;
    }
}