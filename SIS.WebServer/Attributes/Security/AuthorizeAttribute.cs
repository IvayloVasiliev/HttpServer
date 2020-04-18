using SIS.MvcFramework.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.Security
{
    public class AuthorizeAttribute : Attribute
    {
        private readonly string authority;

        public AuthorizeAttribute(string authority = "authorized")
        {
            this.authority = authority;
        } 

        private bool IsLoggedIn(Principal principal)
        {
            return principal != null;
        }

        public bool IsAuthority(Principal principal)
        {
            if (!this.IsLoggedIn(principal))
            {
                return this.authority == "anonymous";
            }
            return this.authority == "authorized" || principal.Roles.Contains(this.authority.ToLower());
        }
    }
}
