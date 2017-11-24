using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace TwoWaySynonyms.Filters
{
    public class AuthorizeByPassAttribute : AuthorizeAttribute
    {
        public static string Pass
        {
            set
            {
                pass = value;
                if (!String.IsNullOrEmpty(pass))
                {
                    string simpleSalt = @"/glDaptyi+sa<<mmmgD";
                    SHA256Managed sha = new SHA256Managed();
                    StringBuilder s = new StringBuilder();
                    byte[] byteArr = sha.ComputeHash(Encoding.UTF8.GetBytes(pass + simpleSalt), 0, Encoding.UTF8.GetByteCount(pass + simpleSalt));
                    foreach (byte b in byteArr)
                    {
                        s.Append(b.ToString("x2"));
                    }
                    pass = s.ToString();
                }
            }
        }
        private string secret = ConfigurationManager.AppSettings["Secret"] ?? "-"; 
        private static string pass = "";
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (pass.Equals(secret));
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("PassError");
        }
    }
}