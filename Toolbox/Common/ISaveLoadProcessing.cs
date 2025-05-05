public interface ISaveLoadProcessing
{
    public void OnBeforeSaving();
    public void OnAfterLoading();
}

public interface ISavable
{
    public object Save();
    public void Load(string dataString);
}