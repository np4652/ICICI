using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.CustomAttributes
{
    public class RestrictiveAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // In case you need an OWIN context, use the next line.
            // `OwinContext` class is defined in the `Microsoft.Owin` package.
            //var context = new OwinContext(owinEnvironment);

            return true; // or `true` to allow access
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }
    }
}
