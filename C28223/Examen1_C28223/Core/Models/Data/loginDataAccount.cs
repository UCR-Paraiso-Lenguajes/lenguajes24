using System.Security.Claims;

namespace storeApi.Models{

    public class LoginDataAccount{

        public string user {get;}
        public string userPass {get;}
        public IEnumerable<Claim> listClaims {get;}
        private static List<LoginDataAccount> usersListAccount = new List<LoginDataAccount>();

        public static IEnumerable<LoginDataAccount> listUsersData=> usersListAccount.AsReadOnly();

        public LoginDataAccount(string userLog, string passwordLog, List<Claim> listClaims){

            if(listClaims == null || listClaims.Count == 0) throw new ArgumentException($"El argumento {nameof(listClaims)} no puede ser nulo o no tener ningun claim.");
            if(string.IsNullOrEmpty(userLog)) throw new ArgumentException($"El argumento {nameof(userLog)} no puede ser nulo para crear un nuevo login." );
            if(string.IsNullOrEmpty(passwordLog)) throw new ArgumentException($"El argumento {nameof(passwordLog)} no puede ser nulo para crear un nuevo login.");            
            this.user = userLog;
            this.userPass = passwordLog;
            this.listClaims = new List<Claim>(listClaims);;
            usersListAccount.Add(this);
        }
    }
}