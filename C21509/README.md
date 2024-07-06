<h1 style="center">Proyect: Store_API"</h1>

<p>Store_API is a responsive API solution for an online store that allows to an user making purchases for different products charged by a database.</p> 

<p>Below you wil find some important details about the aplication</p> 

# Store_API

Store_API is a responsive API solution for an online store that allows users to make purchases for different products charged by a database.

## Project Structure

- **src/**: Contains all the source code for the API
- **config/**: Configuration files for the application
- **tests/**: Unit and integration tests
- **docs/**: Documentation related files

## Security

Security is a crucial aspect of the Store_API. We ensure that the system is protected through various measures, including authentication, authorization, and secure data transmission. Below is a detailed explanation of how we manage security in the Store_API.

### Backend Security

The backend security is handled using JWT (JSON Web Tokens) for authentication and role-based authorization. Here’s a detailed explanation of how it is implemented:

1. **User Authentication and Authorization:**
    - We have a `UserAuth` class that stores user credentials and roles. Each user has a username, password, and a set of roles defined by claims.

    ```csharp
    public class UserAuth
    {
        public string UserName { get; }
        public string Password { get; }
        public IEnumerable<Claim> Roles { get; }

        public static readonly List<UserAuth> allUsers = new List<UserAuth>();

        public static IEnumerable<UserAuth> AllUsersData => allUsers.AsReadOnly();

        public UserAuth(string userName, string password, List<Claim> roles)
        {
            if (roles == null || roles.Count == 0)
                throw new ArgumentException("Must create roles for users");
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Null users are not allowed");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required");

            UserName = userName;
            Password = password;
            Roles = new List<Claim>(roles);

            allUsers.Add(this);
        }
    }
    ```

2. **JWT Token Generation:**
    - The `UserAuthController` class handles the login process. Upon successful login, it generates a JWT token that includes the user’s claims.

    ```csharp
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel user)
    {
        if (user == null)
            return BadRequest("Invalid client request");
        if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            return BadRequest("Invalid client request");

        var existingUser = UserAuth.AllUsersData.FirstOrDefault(u =>
            u.UserName == user.UserName && u.Password == user.Password);

        if (existingUser != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.UserName)
            };
            claims.AddRange(existingUser.Roles);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:7165",
                audience: "https://localhost:7165",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthenticatedResponse { Token = tokenString });
        }

        return Unauthorized();
    }
    ```

3. **JWT Authentication Middleware:**
    - The middleware is configured to validate JWT tokens for secured endpoints.

    ```csharp
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7165",
            ValidAudience = "https://localhost:7165",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });
    ```

### Frontend Security

The frontend security involves token management and validation to ensure that users are authenticated before accessing protected routes.

1. **Token Verification Component:**
    - The `VerifyToken` component ensures that the JWT token is valid and not expired. It checks the token every 60 seconds and redirects to the login page if the token is invalid.

    ```typescript
    import { useEffect, useState, createContext, useContext } from 'react';
    import { useRouter } from 'next/navigation';
    import { jwtDecode } from 'jwt-decode';

    const TokenContext = createContext({ isValidToken: false, isVerifying: true });

    export const useTokenContext = () => useContext(TokenContext);

    export default function VerifyToken({ children }: { children: React.ReactNode }) {
      const [isVerifying, setIsVerifying] = useState(true);
      const [isValidToken, setIsValidToken] = useState(false);
      const router = useRouter();

      const verifyToken = () => {
        const loginToken = sessionStorage.getItem("loginToken");

        if (!loginToken) {
          setIsValidToken(false);
          setIsVerifying(false);
          router.push("/../admin");
          return;
        }

        try {
          const tokenFormat: { exp: number } = jwtDecode(loginToken);
          const todayDate = Date.now() / 1000;

          if (tokenFormat.exp < todayDate) {
            setIsValidToken(false);
            sessionStorage.removeItem("loginToken");
            router.push("/../admin");
          } else {
            setIsValidToken(true);
          }
        } catch (error) {
          setIsValidToken(false);
          sessionStorage.removeItem("loginToken");
          router.push("/../admin");
        } finally {
          setIsVerifying(false);
        }
      };
    ```

2. **Login Page:**
    - The login page handles user authentication by sending the username and password to the backend and storing the JWT token in session storage upon successful authentication and validating the password format. After this, if the introduced login is correct the the user admin is redirected to an management options menu.

    ```typescript

        try {
          const response = await fetch('https://localhost:7165/api/UserAuth/login', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({ UserName: username, Password: password })
          });

          if (response.ok) {
            const data = await response.json();
            sessionStorage.setItem('loginToken', data.token);  
            setError('');
            window.location.href = '/admin/init';
          } else {
            const errorData = await response.json();
            setError(errorData.message || 'Error en la autenticación');
          }
        } catch (error) {
          setError('Error en la conexión al servidor');
        }
      };

    ```

## Sales Report

The Sales Report feature allows administrators to generate and view sales reports. These reports can be filtered by date and provide insights into daily and weekly sales. The sales data is visualized using Google Charts.

