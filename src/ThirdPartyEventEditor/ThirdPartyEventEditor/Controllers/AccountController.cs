using System.Web.Mvc;
using ThirdPartyEventEditor.Models;
using System.Web.Security;
using ThirdPartyEventEditor.Filters;
using System.Linq;
using System.Web.Configuration;

namespace ThirdPartyEventEditor.Controllers
{
    /// <summary>
    /// Controller for user management.
    /// </summary>
    [LoggerTimeFilter]
    public class AccountController : Controller
    {
        /// <summary>
        /// Sign-in in system.
        /// </summary>
        /// <returns>View.</returns>
        public ActionResult Login()
        {
            var actionResult = SelectRouteByRole(User.Identity.Name);
            if (actionResult != null)
            {
                return actionResult;
            }
            return View();
        }

        /// <summary>
        /// Checks if the user is registered.
        /// </summary>
        /// <param name="model">User.</param>
        /// <param name="returnUrl">Url for return.</param>
        /// <returns>View.</returns>
        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            ActionResult result = View(model);
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, false);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        result = Redirect(returnUrl);
                    }
                    else
                    {
                        var actionResult = SelectRouteByRole(model.Login);
                        if (actionResult != null)
                        {
                            result = actionResult;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password: ");
                }
            }
            return result;
        }

        /// <summary>
        /// Log of in system.
        /// </summary>
        /// <returns>View.</returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private ActionResult SelectRouteByRole(string login)
        {
            var roles = Roles.GetRolesForUser(login);
            if (roles == null || !roles.Any())
            {
                return null;
            }
            var role = roles.FirstOrDefault();

            var localUrl = WebConfigurationManager.AppSettings[role];
            return Redirect(localUrl);
        }
    }
}