class AlertText
{
    public string Text, ID;
    public float Duration;

    public AlertText(string id, string text, float duration)
    {
        ID = id;
        Text = text;
        Duration = duration;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not AlertText text) return false;
        return ID.Equals(text.ID);
    }
}