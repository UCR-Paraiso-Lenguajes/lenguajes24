namespace core;
public class DataConnection
{
    public string ConnectionString { get; private set; }
    public string ConnectionStringMyDb { get; private set; }

    public static DataConnection Instance;
    public static void Init(string connectionString, string connectionStringMyDb)
    {
        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(connectionStringMyDb))
            throw new ArgumentNullException("Connection strings are required.");
        Instance = new DataConnection(connectionString, connectionStringMyDb);
    }

    private DataConnection(string connectionString, string connectionStringMyDb)
    {
        ConnectionString = connectionString;
        ConnectionStringMyDb = connectionStringMyDb;
    }

}