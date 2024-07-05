namespace ApiLab7;

public class UserAuth
{
    public string Name { get; set; }
    public string Password { get; private set; }
    public string? Role { get; set; }

    public UserAuth(string name, string password, string? role = null)
    {
        if (String.IsNullOrEmpty(name))
            throw new ArgumentNullException($"{nameof(name)} must be provided");
        if (String.IsNullOrEmpty(password))
            throw new ArgumentNullException($"{nameof(password)} must be provided");
        Name = name;
        Password = password;
        Role = role;
    }

    public static void IsPresent(UserAuth user)
    {
        if (user is null)
            throw new ArgumentNullException($"{nameof(user)} must not be null");
        if (String.IsNullOrEmpty(user.Name))
            throw new ArgumentNullException($"{nameof(user.Name)} must be provided");
        if (String.IsNullOrEmpty(user.Password))
            throw new ArgumentNullException($"{nameof(user.Password)} must be provided");
    }
}

public class CredentialOptions
{
    public IEnumerable<UserAuth> Development { get; set; }

    public CredentialOptions() { }

    public CredentialOptions(IEnumerable<UserAuth> development)
    {
        Development = development;
    }
}
