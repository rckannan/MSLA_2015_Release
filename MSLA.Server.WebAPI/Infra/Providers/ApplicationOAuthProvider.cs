using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MSLA.Server.WebAPI.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MSLA.Server.WebAPI.Infra.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        //private readonly string _publicClientId;
        
        public ApplicationOAuthProvider()
        { 
        }

        //public ApplicationOAuthProvider(Login.ILogon iLogon)
        //{
        //    _iLogon = iLogon;
        //    //_repo = new AuthRepository();
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            //User user = null;// _repo.FindUser(context.UserName);
            var _iLogon = ActionFilters.WebContainerManager.Get<Login.ILogon>();
            var user = _iLogon.TryLogin(new Login.LogonInfo(context.UserName) );
            //User user = new User() {
            //    fldUserId = 1001,
            //    fldUserName = "kannan",
            //    fldActiveUser = true,
            //    fldEmailAddress = "kannan@mail.com",
            //    fldFullUserName = "Kanna RC"
            //};

            //add the user session
            var iSession = ActionFilters.WebContainerManager.Get<Login.ILogonService>();
            iSession.SaveLogonInfo(user);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user is not available in the context. Please check with administrators.");
                return;
            }
            //else if (!(user.fldActiveUser))
            //{
            //    context.SetError("Inactive user", "The user name is not Inactive in the context.");
            //    return;
            //}


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("UserName", user.FullUserName));
            identity.AddClaim(new Claim("Email", user.FullUserName));
            identity.AddClaim(new Claim("UserID", user.User_ID.ToString()));
            identity.AddClaim(new Claim("SessionID", user.Session_ID.ToString()));
            identity.AddClaim(new Claim("ReqIP", context.Request.RemoteIpAddress.ToString()));
           

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", context.ClientId ?? string.Empty
                    },
                    {
                        "userName", context.UserName
                    },
                    {
                        "UserID",user.User_ID.ToString()
                    },
                    {
                        "email",user.FullUserName
                    },
                    {
                        "sessionID",user.Session_ID.ToString()
                    },
                    {
                        "ReqIP",context.Request.RemoteIpAddress.ToString()
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty; 

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            var _iLogon = ActionFilters.WebContainerManager.Get<Login.ILogon>();
            //client = new WebClient() {fldWebClientId = "MSLAApp", fldActive = true,
            //                        fldAllowedOrigin = "*", fldApplicationType = 0 , fldName= "MSLAApp",
            //                        fldRefreshTokenLifeTime = 30, fldSecretCode = "AEIOU#@123"
            //};// _repo.FindWebClient(context.ClientId);

            var client = _iLogon.GetWebClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.fldApplicationType == (int)ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    //if (client.fldSecretCode != Helper.GetHash(clientSecret))
                    if (client.fldSecretCode != clientSecret)
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.fldActive)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.fldAllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.fldRefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        //public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        //{
        //    if (context.ClientId == _publicClientId)
        //    {
        //        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

        //        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
        //        {
        //            context.Validated();
        //        }
        //    }

        //    return Task.FromResult<object>(null);
        //}

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        
    }
}