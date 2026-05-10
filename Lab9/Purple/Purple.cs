namespace Lab9.Purple
{
    public abstract class Purple
    {
        protected string _text;
        public string Input => _text;

        protected Purple(string text)
        {
            _text = text;
        }

        public abstract void Review();

        public virtual void ChangeText(string text)
        {
            _text = text;
            Review();
        }
        
        public abstract override string ToString();
    }
}