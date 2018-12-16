using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MSLA.Server.WebAPI.Infra.ActionFilters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MSLA.Server.WebAPI.Controllers
{
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        [System.Web.Http.OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Authorizes", Name = "Authorizes")]
        public async Task<IHttpActionResult> Authorizes(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var usr = User.Identity as ClaimsIdentity;

            //ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (usr == null)
            {
                return InternalServerError();
            }

            Claim providerKeyClaim = usr.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim.Issuer != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            //var user = _repo.FindUser(context.UserName);
            //ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
            //    externalLogin.ProviderKey));

            //bool hasRegistered = user != null;

            //if (hasRegistered)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            //    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
            //       OAuthDefaults.AuthenticationType);
            //    ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
            //        CookieAuthenticationDefaults.AuthenticationType);

            //    AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
            //    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = externalLogin.GetClaims();
            //    ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            //    Authentication.SignIn(identity);
            //}


            //var authentication = HttpContext.GetOwinContext().Authentication;
            //var ticket = authentication.AuthenticateAsync("Application").Result;
            //var identity = ticket != null ? ticket.Identity : null;
            //if (identity == null)
            //{
            //    authentication.Challenge("Application");
            //    return Unauthorized();
            //}

            //var scopes = (Request.QueryString.Get("scope") ?? "").Split(' ');

            //if (Request.HttpMethod == "POST")
            //{
            //    if (!string.IsNullOrEmpty(Request.Form.Get("submit.Grant")))
            //    {
            //        identity = new ClaimsIdentity(identity.Claims, "Bearer", identity.NameClaimType, identity.RoleClaimType);
            //        foreach (var scope in scopes)
            //        {
            //            identity.AddClaim(new Claim("urn:oauth:scope", scope));
            //        }
            //        authentication.SignIn(identity);
            //    }
            //    if (!string.IsNullOrEmpty(Request.Form.Get("submit.Login")))
            //    {
            //        authentication.SignOut("Application");
            //        authentication.Challenge("Application");
            //        return new HttpUnauthorizedResult();
            //    }
            //}

            return Ok();
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
