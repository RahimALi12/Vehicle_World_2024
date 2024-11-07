using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Security.Claims;
using Vehicle_World.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;


namespace Vehicle_World.Controllers
{

    [Authorize(Roles = "Admin,Buyer")]

    public class BuyerController : Controller
    {


        private readonly ApplicationDbContext _AppDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

              

        public BuyerController(ApplicationDbContext AppDb, UserManager<AppUser> userManager, IWebHostEnvironment whe)
        {
            _AppDbContext = AppDb;
            _userManager = userManager;
            _webHostEnvironment = whe;
        }







        public async Task<IActionResult> BuyerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> BuyerProfile(AppUser model, IFormFile profilePicture)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Email", "This email is already taken.");
                ViewBag.ProfileImagePath = user.ProfileImage; // Preserve profile image path on error
                return View(model);
            }

            if (profilePicture != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile_pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                user.ProfileImage = $"/images/profile_pictures/{fileName}";
            }
            else
            {
                user.ProfileImage = model.ProfileImage; // Preserve existing image if no new image is uploaded
            }

            user.U_Name = model.U_Name;
            user.Email = model.Email;
            user.Contact = model.Contact;
            user.City = model.City;
            user.Country = model.Country;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Website"); // ya kisi bhi relevant page par redirect karen
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            ViewBag.ProfileImagePath = user.ProfileImage; // Preserve profile image path on error
            return View(model);
        }





       













    }























}

