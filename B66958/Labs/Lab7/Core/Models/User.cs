namespace ApiLab7;

public class UserAuth
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string? Role { get; set; }

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
    public List<UserAuth> Development { get; set; }
}
