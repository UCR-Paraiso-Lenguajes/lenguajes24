using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace StoreAPI.models;

    public class UserAccount{

        public string userName {get;}
        public string userPassword {get;}
        public IEnumerable<Claim> userRoles {get;}

        //Lista estatica momentanea de usuarios para el Lab
        private static List<UserAccount> allUsers = new List<UserAccount>();

        public static IEnumerable<UserAccount> allUsersData => allUsers.AsReadOnly();

        public UserAccount(string userName, string userPassword, List<Claim> userRoles){
            
            if(userRoles == null || userRoles.Count == 0) throw new ArgumentException($"{nameof(userRoles)} no puede haber un usuario sin roles");
            if(string.IsNullOrEmpty(userName)) throw new ArgumentException($"{nameof(userName)} no puede ser igual nulo ni vacío" );
            if(string.IsNullOrEmpty(userPassword)) throw new ArgumentException($"{nameof(userPassword)} no puede ser nulo ni vacío");            
            
            this.userName = userName;
            this.userPassword = userPassword;
            this.userRoles = new List<Claim>(userRoles);;

            //agregamos cualquier nueva instancia de golpe
            allUsers.Add(this);
        }
    }