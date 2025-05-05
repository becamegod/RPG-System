public class Tab
{
    private TabButton button;
    internal UIAnimation content { get; }

    public Tab(TabButton button, UIAnimation content)
    {
        this.button = button;
        this.content = content;
    }

    public void Close()
    {
        button.Dehighlight();
        content.Hide();
    }

    public void Open()
    {
        button.Highlight();
        content.Show();
    }
}
