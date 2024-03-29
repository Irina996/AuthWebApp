﻿using AuthWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApp.Controllers
{
    [Authorize(Roles = "unblocked")]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.IsBlocked)
            {
                return RedirectToAction("Logout", "Account");
            }
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> Login()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.LastLogIn = DateTime.Now;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(List<string> identifiers, string actionType)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser.IsBlocked)
            {
                return RedirectToAction("Logout", "Account");
            }
            if (actionType == "delete")
            {
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
                        result = await _userManager.RemoveFromRoleAsync(user, "unblocked");
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                if (identifiers.Contains(currentUser.Id))
                {
                    return RedirectToAction("Logout", "Account");
                }
                return RedirectToAction("Index");
            }
            else if (actionType == "unblock")
            {
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
                        result = await _userManager.AddToRoleAsync(user, "unblocked");
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
