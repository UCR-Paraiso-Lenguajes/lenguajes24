namespace Core;

public class DatabaseConfiguration
{
    public string ConnectionString { get; private set; }
    public string ConnectionStringMyDb { get; private set; }

    private static DatabaseConfiguration _instance;
    public static DatabaseConfiguration Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new InvalidOperationException("DatabaseConfiguration instance has not been initialized. Call DatabaseConfiguration.Init() first.");
            }
            return _instance;
        }
    }

    public static void Init(string connectionString)
    {
        _instance = connectionString != null ? new DatabaseConfiguration(connectionString) : throw new ArgumentNullException("Connection strings are required.");
    }


    private DatabaseConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }
}
