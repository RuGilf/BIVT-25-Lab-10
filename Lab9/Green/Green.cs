namespace Lab9.Green;

public abstract class Green
{
    private string _text;

    protected Green(string text)
    {
        _text = text;
    }

    public string Input
    {
        get
        {
            return _text ?? "";
        }
    }

    public abstract void Review();

    public virtual void ChangeText(string text)
    {
        _text = text;
        Review();
    }
}