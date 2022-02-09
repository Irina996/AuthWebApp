using AuthWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApp.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());


        public async Task<ActionResult> Edit(List<string> identifiers, string actionType)
        {
            if (actionType == "delete")
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                foreach (var id in identifiers)
                {
                    User user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.DeleteAsync(user);
                    }
                }

                if (identifiers.Contains(currentUser.Id))
                {
                    return RedirectToAction("Logout", "Account");
                }
                return RedirectToAction("Index");
            }
            else if (actionType == "block")
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                foreach (var id in identifiers)
                {
                    User user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        user.IsBlocked = true;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            else if (actionType == "unblock")
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);

                foreach (var id in identifiers)
                {
                    User user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        user.IsBlocked = false;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }
    }
}
