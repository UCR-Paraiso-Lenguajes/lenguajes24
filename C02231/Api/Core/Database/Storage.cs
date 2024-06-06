namespace Core;

public class Storage
{
    public string ConnectionString {get;private set;}
    public static Storage Instance;
    public static void Init(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"{nameof(connectionString)} is required.");

        Storage.Instance = new Storage(connectionString);
    }
    private Storage (string connectionString)
    {
       
        ConnectionString = connectionString;
    }
}