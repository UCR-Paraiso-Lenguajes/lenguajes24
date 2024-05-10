namespace Core;

public class ConnectionDB
{
    public string ConnectionString {get;private set;}
    public static ConnectionDB Instance;
    public static void Init(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"{nameof(connectionString)} is required.");

        ConnectionDB.Instance = new ConnectionDB(connectionString);
    }
    private ConnectionDB (string connectionString)
    {
       
        ConnectionString = connectionString;
    }
}