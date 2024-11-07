using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Security.Claims;
using Vehicle_World.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Vehicle_World.Controllers
{

    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {


        private readonly ApplicationDbContext _AppDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext AppDb, UserManager<AppUser> userManager, IWebHostEnvironment whe)
        {
            _AppDbContext = AppDb;
            _userManager = userManager;
            _webHostEnvironment = whe;
        }


        public async Task<IActionResult> Index()
        {
            var sellers = new List<AppUser>();
            var buyers = new List<AppUser>();
            var users = _userManager.Users.ToList(); // Fetch all users
            
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Seller"))
                {
                    sellers.Add(user);
                }

                if (await _userManager.IsInRoleAsync(user, "Buyer"))
                {
                    buyers.Add(user);
                }

                // You can add more roles here if needed
            }

            var myCars = await _AppDbContext.CarDetails
   .Include(c => c.MakeType)
   .Include(c => c.ModelType)
   .Include(c => c.BodyType)
   .Include(c => c.EngineType)
   .Include(c => c.FuelType)
   .Include(c => c.TransmissionType)
   .Include(c => c.ConditionType)
   .Include(c => c.Seller) // Include the seller (AppUser) information
   .ToListAsync();

            ViewBag.Cars = myCars;

            ViewBag.Sellers = sellers;
            ViewBag.Buyers = buyers;

            return View();
        }



        // Show list of cars requesting to be featured
        public IActionResult ReviewFeaturedCars()
        {
            var carsForFeatureApproval = _AppDbContext.CarDetails
     .Include(c => c.MakeType)
     .Include(c => c.ModelType)
     .Include(c => c.BodyType)
     .Include(c => c.EngineType)
     .Include(c => c.FuelType)
     .Include(c => c.TransmissionType)
     .Include(c => c.ConditionType)
     .Include(c => c.Seller)
                //.Where(c => c.IsFeatureRequested == true && c.IsFeatured == false && c.Status == "Pending")
                .Where(c => c.FeatureRequestPending == true)
                .ToList();

            return View(carsForFeatureApproval);
        }


        [HttpPost]
        public IActionResult ApproveFeaturedCar(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null)
            {
                car.IsFeatured = true;  // Approve and feature the car
                car.FeatureRequestPending = false;  // Request processed
                car.Status = "Approved";  // Optional: Set status to approved for clarity

                _AppDbContext.SaveChanges();
            }
            return RedirectToAction("ReviewFeaturedCars");
        }

        [HttpPost]
        public IActionResult RejectFeaturedCar(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null)
            {
                car.IsFeatureRequested = false;
                car.FeatureRequestPending = false;  // Reset pending status after rejection
                car.Status = "Rejected";  // Optional: Mark as rejected for tracking purposes

                _AppDbContext.SaveChanges();
            }
            return RedirectToAction("ReviewFeaturedCars");
        }








        public async Task<IActionResult> AllFeaturedCars()
        {

            var featuredCars = _AppDbContext.CarDetails
                   .Include(c => c.MakeType)
         .Include(c => c.ModelType)
         .Include(c => c.BodyType)
         .Include(c => c.EngineType)
         .Include(c => c.FuelType)
         .Include(c => c.TransmissionType)
         .Include(c => c.ConditionType)
         .Include(c => c.Seller)
              .Where(c => c.IsFeatured == true)
             .ToList();
                    
            return View(featuredCars);

        }



        [HttpPost]
        public async Task<IActionResult> UnfeatureCar(int carId)
        {
            // Find the car by its ID
            var car = await _AppDbContext.CarDetails.FindAsync(carId);

            if (car == null)
            {
                return NotFound(); // Handle case where car does not exist
            }

            // Set the car's IsFeatured property to false
            car.IsFeatured = false;

            // Reset feature request-related properties so that seller can request again
            car.IsFeatureRequested = false;
            car.FeatureRequestPending = false;  // Reset pending status after rejection
            car.Status = "Pending";  // Optional: Mark as rejected for tracking purposes

            // Save the changes to the database
            await _AppDbContext.SaveChangesAsync();

            // Redirect back to the AllFeaturedCars page
            return RedirectToAction(nameof(AllFeaturedCars));
        }












        public IActionResult ReviewBidCars()
        {
            var carsForBiddingApproval = _AppDbContext.CarDetails
     .Include(c => c.MakeType)
     .Include(c => c.ModelType)
     .Include(c => c.BodyType)
     .Include(c => c.EngineType)
     .Include(c => c.FuelType)
     .Include(c => c.TransmissionType)
     .Include(c => c.ConditionType)
     .Include(c => c.Seller)
                .Where(c => c.BiddingRequestPending == true)
                .ToList();

            return View(carsForBiddingApproval);
        }




        [HttpPost]
        public IActionResult ApproveBidding(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null)
            {
                car.IsBiddingEnabled = true;  // Approve and feature the car
                car.BiddingRequestPending = false;  // Request processed
                car.BiddingStatus = "Approved";  // Optional: Set status to approved for clarity

                //_AppDbContext.Update(car);
                _AppDbContext.SaveChanges();
            }
            return RedirectToAction("ReviewBidCars"); // Redirect to admin panel
        }

        [HttpPost]
        public IActionResult RejectBidding(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null)
            {
                car.IsBiddingRequested = false;
                car.BiddingRequestPending = false;  // Reset pending status after rejection
                car.BiddingStatus = "Rejected";  // Optional: Mark as rejected for tracking purposes

                //_AppDbContext.Update(car);
                _AppDbContext.SaveChanges();
            }
            return RedirectToAction("ReviewBidCars"); // Redirect to admin panel
        }

        [HttpPost]
        public IActionResult Unbid(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null)
            {
                // Set the car's IsFeatured property to false
                car.IsBiddingEnabled = false;

                // Reset feature request-related properties so that seller can request again
                car.IsBiddingRequested = false;
                car.BiddingRequestPending = false;  // Reset pending status after rejection
                car.BiddingStatus = "Pending";  // Optional: Mark as rejected for tracking purposes

                //_AppDbContext.Update(car);
                _AppDbContext.SaveChanges();
            }
            return RedirectToAction("ReviewBidCars"); // Redirect to admin panel
        }


        public async Task<IActionResult> AllBiddingCars()
        {

            var biddingCars = _AppDbContext.CarDetails
                   .Include(c => c.MakeType)
         .Include(c => c.ModelType)
         .Include(c => c.BodyType)
         .Include(c => c.EngineType)
         .Include(c => c.FuelType)
         .Include(c => c.TransmissionType)
         .Include(c => c.ConditionType)
         .Include(c => c.Seller)
              .Where(c => c.IsBiddingEnabled == true)
             .ToList();

            return View(biddingCars);

        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AppUser model, IFormFile profilePicture)
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
                return RedirectToAction("Index", "Admin"); // ya kisi bhi relevant page par redirect karen
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            ViewBag.ProfileImagePath = user.ProfileImage; // Preserve profile image path on error
            return View(model);
        }



        public async Task<IActionResult> ContactMessages()
        {
            var contacts = await _AppDbContext.Contacts.ToListAsync();
            return View(contacts);
        }




        // GET: Seller/Details/5
        public async Task<IActionResult> DetailsSeller(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _userManager.FindByIdAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            // Pass the seller details to the view
            return View(seller);
        }

        // GET: Seller/Create
        public IActionResult CreateSeller()
        {
            TempData["SellerMessage"] = "Seller Added Successfully";
            return View();
        }





        // GET: Seller/Edit/5
        public async Task<IActionResult> EditSeller(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _userManager.FindByIdAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Seller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSeller(string id, AppUser seller, IFormFile profilePicture/*string newPassword = null*/)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }



          
                try
                {
                    // Find the existing seller in the database
                    var existingSeller = await _userManager.FindByIdAsync(id);
                    if (existingSeller == null)
                    {
                        return NotFound();
                    }

                    var existingUser = await _userManager.FindByEmailAsync(seller.Email);
                    if (existingUser != null && existingUser.Id != existingSeller.Id)
                    {
                        ModelState.AddModelError("Email", "This email is already taken.");
                        ViewBag.ProfileImagePath = existingSeller.ProfileImage; // Preserve profile image path on error
                        return View(seller);
                    }   

                    // Handle the profile image upload if a new image is provided
                    if (profilePicture != null)
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", profilePicture.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await profilePicture.CopyToAsync(stream);
                        }
                        existingSeller.ProfileImage = "/images/" + profilePicture.FileName;
                    }

                    // Update the existing seller's details
                    existingSeller.U_Name = seller.U_Name;
                    existingSeller.Email = seller.Email;
                    existingSeller.Contact = seller.Contact;
                    existingSeller.City = seller.City;
                    existingSeller.Country = seller.Country;

                existingSeller.UserName = seller.Email; // Assign email to UserName
                existingSeller.NormalizedUserName = seller.Email.ToUpper(); // Normalize the UserName


                // Update the seller in the database
                var result = await _userManager.UpdateAsync(existingSeller);
                    if (result.Succeeded)
                    {
                    TempData["SellerMessage"] = "Seller Updated Successfully";
                    return RedirectToAction("Index", "Admin");
                    }

                    // Handle errors during the update process
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SellerExists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            
            // If the model state is not valid, return to the edit view with the current seller data
            return View(seller);
        }

        // Check if a seller exists by ID
        private async Task<bool> SellerExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }


        // GET: Seller/Delete/5
        public async Task<IActionResult> DeleteSeller(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _userManager.FindByIdAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Seller/Delete/5
        [HttpPost, ActionName("DeleteSeller")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var seller = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(seller);

            TempData["SellerMessage"] = "Seller Deleted Successfully";

            return RedirectToAction("Index" , "Admin");
        }

        private async Task<bool> SellerExistsdel(string id)
        {
            return await _userManager.Users.AnyAsync(e => e.Id == id);
        }




        public async Task<IActionResult> SellerIndex()
        {
            //var cars = _AppDbContext.CarDetails.ToList();
            //ViewBag.Cars = cars;
            var sellers = new List<AppUser>();
            var users = _userManager.Users.ToList(); // Ensure this is awaited if it's asynchronous

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Seller"))
                {
                    sellers.Add(user);
                }
            }

            return View(sellers);
            //return View();
        }















        // GET: Seller/Details/5
        public async Task<IActionResult> DetailsBuyer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _userManager.FindByIdAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }

            // Pass the seller details to the view
            return View(buyer);
        }

        // GET: Seller/Create
        public IActionResult CreateBuyer()
        {
            return View();
        }





        // GET: Seller/Edit/5
        public async Task<IActionResult> EditBuyer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _userManager.FindByIdAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return View(buyer);
        }

        // POST: Seller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBuyer(string id, AppUser buyer, IFormFile profilePicture/*string newPassword = null*/)
        {
            if (id != buyer.Id)
            {
                return NotFound();
            }




            try
            {
                // Find the existing seller in the database
                var existingBuyer = await _userManager.FindByIdAsync(id);
                if (existingBuyer == null)
                {
                    return NotFound();
                }

                var existingUser = await _userManager.FindByEmailAsync(buyer.Email);
                if (existingUser != null && existingUser.Id != existingBuyer.Id)
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                    ViewBag.ProfileImagePath = existingBuyer.ProfileImage; // Preserve profile image path on error
                    return View(buyer);
                }

                // Handle the profile image upload if a new image is provided
                if (profilePicture != null)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", profilePicture.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }
                    existingBuyer.ProfileImage = "/images/" + profilePicture.FileName;
                }

                // Update the existing seller's details
                existingBuyer.U_Name = buyer.U_Name;
                existingBuyer.Email = buyer.Email;
                existingBuyer.Contact = buyer.Contact;
                existingBuyer.City = buyer.City;
                existingBuyer.Country = buyer.Country;

                existingBuyer.UserName = buyer.Email; // Assign email to UserName
                existingBuyer.NormalizedUserName = buyer.Email.ToUpper(); // Normalize the UserName


                // Update the seller in the database
                var result = await _userManager.UpdateAsync(existingBuyer);
                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Index", "Admin");
                }

                // Handle errors during the update process
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BuyerExists(buyer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // If the model state is not valid, return to the edit view with the current seller data
            return View(buyer);
        }

        // Check if a seller exists by ID
        private async Task<bool> BuyerExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }


        // GET: Seller/Delete/5
        public async Task<IActionResult> DeleteBuyer(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _userManager.FindByIdAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // POST: Seller/Delete/5
        [HttpPost, ActionName("DeleteBuyer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyerDeleteConfirmed(string id)
        {
            var buyer = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(buyer);


            return RedirectToAction("Index", "Admin");
        }

        private async Task<bool> BuyerExistsdel(string id)
        {
            return await _userManager.Users.AnyAsync(e => e.Id == id);
        }




        public async Task<IActionResult> BuyerIndex()
        {
            //var cars = _AppDbContext.CarDetails.ToList();
            //ViewBag.Cars = cars;
            var buyer = new List<AppUser>();
            var users = _userManager.Users.ToList(); // Ensure this is awaited if it's asynchronous

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Buyer"))
                {
                    buyer.Add(user);
                }
            }

            return View(buyer);
            //return View();
        }






      








        public async Task<IActionResult> AllCars()
        {


            //var user = await _userManager.GetUserAsync(User);



            var myCars = await _AppDbContext.CarDetails
         .Include(c => c.MakeType)
         .Include(c => c.ModelType)
         .Include(c => c.BodyType)
         .Include(c => c.EngineType)
         .Include(c => c.FuelType)
         .Include(c => c.TransmissionType)
         .Include(c => c.ConditionType)
         .Include(c => c.Seller) // Include the seller (AppUser) information
         .ToListAsync();

            ViewBag.Cars = myCars;



            return View();

        }



        public IActionResult DeleteCar(int id)
        {
            var cardelete = _AppDbContext.CarDetails
                                         .Include(c => c.MakeType)
                                         .Include(c => c.ModelType)
                                         .Include(c => c.BodyType)
                                         .Include(c => c.EngineType)
                                         .Include(c => c.FuelType)
                                         .Include(c => c.TransmissionType)
                                         .Include(c => c.ConditionType)
                                         .FirstOrDefault(c => c.Id == id);

            if (cardelete == null)
            {
                return NotFound();  // Return a 404 if the car isn't found
            }

            return View(cardelete);
        }

        [HttpPost, ActionName("DeleteCar")]
        public IActionResult ConfirmDelete(int id)
        {
            var confirmdelete = _AppDbContext.CarDetails.Find(id);
            if (confirmdelete != null)
            {
                _AppDbContext.CarDetails.Remove(confirmdelete);
                _AppDbContext.SaveChanges();
                return RedirectToAction("AllCars", "Admin");
            }

            return NotFound();  // Return a 404 if trying to delete a non-existent car
        }




    }












}
