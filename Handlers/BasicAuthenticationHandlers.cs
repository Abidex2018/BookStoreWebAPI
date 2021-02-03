using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BookStoreWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookStoreWebAPI.Handlers
{
    public class BasicAuthenticationHandlers : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly BookStoresDBContext _context;
        public BasicAuthenticationHandlers(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,BookStoresDBContext context): base(options,logger,encoder,clock)
        {
            _context = context;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");
            try
            {
                var auhenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var bytes = Convert.FromBase64String(auhenticationHeaderValue.Parameter);
                var credentials = Encoding.UTF8.GetString(bytes).Split(" ");

                var emailAddress = credentials[0];
                var password = credentials[1];

                var user = _context.Users.FirstOrDefault(cre => cre.EmailAddress == emailAddress && cre.Password == password);
                if (user == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
                else
                {
                    var claims = new[] {new Claim(ClaimTypes.Name, user.EmailAddress)};
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Error Has Occured");
            }

          
        }
    }
}
