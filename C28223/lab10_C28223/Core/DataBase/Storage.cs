namespace Core;

public class Storage
{
    public string ConnectionString { get; private set; }

    public static Storage Instance;
    public static void Init(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("Connection strings are required.");
        Instance = new Storage(connectionString);
    }

    private Storage(string connectionString)
    {
        ConnectionString = connectionString;
    }

}
