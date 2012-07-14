using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Nottit.Models {
    public class LoginManager {
        private NottitDb _db;

        private void AddToSession(User user) {
            HttpContext.Current.User = new NottitPrincipal(user.UserName);
        }

        private User ValidateUser(string userName, string password) {
            User user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null) {
                return null;
            }

            if (password == user.Password) {
                return user;
            }

            return null;
        }

        public LoginManager(NottitDb Db) {
            _db = Db;
        }

        public bool Login(string userName, string password, out string errorMessage) {
            User incomingUser = ValidateUser(userName, password);

            if (incomingUser != null) {
                AddToSession(incomingUser);
                FormsAuthentication.SetAuthCookie(userName, false);
                errorMessage = string.Empty;
                return true;
            } else {
                errorMessage = "Invalid email address and/or password";
                return false;
            }
        }

        public void Logout() {
            FormsAuthentication.SignOut();
        }

        public bool IsLoggedIn {
            get {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public User CurrentUser {
            get {
                var principal = HttpContext.Current.User;

                if (principal != null && principal.Identity.IsAuthenticated) {
                    var user = _db.Users.FirstOrDefault(u => u.UserName == principal.Identity.Name);
                    return user;
                }
                return null;
            }
        }
    }

    public class NottitIdentity : IIdentity {
        private string _name;

        public NottitIdentity(string name) {
            _name = name;
        }

        public string AuthenticationType {
            get { return "Forms"; }
        }

        public bool IsAuthenticated {
            get { return true; }
        }

        public string Name {
            get { return _name; }
        }
    }

    public class NottitPrincipal : IPrincipal {
        private NottitIdentity _identity;

        public NottitPrincipal(string userName) {
            _identity = new NottitIdentity(userName);
        }

        public IIdentity Identity {
            get { return _identity; }
        }

        public bool IsInRole(string role) {
            return false;
        }
    }
}