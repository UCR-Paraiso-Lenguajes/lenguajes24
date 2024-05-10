namespace Core;

public class Storage
{
    public string ConnectionString { get; private set; }
    public string ConnectionStringMyDb { get; private set; }

    public static Storage Instance;
    public static void Init(string connectionString, string connectionStringMyDb)
    {
        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(connectionStringMyDb))
            throw new ArgumentNullException("Se necesita un string de conexion");
        Instance = new Storage(connectionString, connectionStringMyDb);
    }

    private Storage(string connectionString, string connectionStringMyDb)
    {
        ConnectionString = connectionString;
        ConnectionStringMyDb = connectionStringMyDb;
    }

}