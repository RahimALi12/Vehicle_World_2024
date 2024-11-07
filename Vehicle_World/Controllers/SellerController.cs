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

    [Authorize(Roles = "Admin,Seller")]

    public class SellerController : Controller
    {


        private readonly ApplicationDbContext _AppDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SellerController(ApplicationDbContext AppDb, UserManager<AppUser> userManager, IWebHostEnvironment whe)
        {
            _AppDbContext = AppDb;
            _userManager = userManager;
            _webHostEnvironment = whe;
        }

        //        public async Task<IActionResult> Index()
        //{


        //            var user = await _userManager.GetUserAsync(User);
        //            var myCars = _AppDbContext.CarDetails.Where(c => c.Seller_Id == user.Id).ToList();

        //            if (myCars == null)
        //            {
        //                myCars = new List<CarDetail>();
        //            }

        //            var cars = _AppDbContext.CarDetails
        //            .Include(c => c.BodyType)
        //            .Include(c => c.EngineType)
        //            .Include(c => c.FuelType)
        //            .Include(c => c.TransmissionType)
        //            .ToList();


        //            return View(myCars);
        //        }





        public async Task<IActionResult> SellerProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SellerProfile(AppUser model, IFormFile profilePicture)
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









        public async Task<IActionResult> SellerView()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Fetch the seller's own car listings
                var myCars = await _AppDbContext.CarDetails
                    .Where(c => c.Seller_Id == user.Id)
                    .Include(c => c.MakeType)
                    .Include(c => c.ModelType)
                    .Include(c => c.BodyType)
                    .Include(c => c.EngineType)
                    .Include(c => c.FuelType)
                    .Include(c => c.TransmissionType)
                    .Include(c => c.ConditionType)
                    .ToListAsync();

                if (myCars == null || !myCars.Any())
                {
                    myCars = new List<CarDetail>();
                }

                // Return the seller's cars to the SellerView
                return View(myCars);
            }

            // Redirect or handle cases where the user is neither an Admin nor a Seller
            return RedirectToAction("AccessDenied", "Account");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminView()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Get all users who are in the "Seller" role
                var sellers = await _userManager.GetUsersInRoleAsync("Seller");

                // Return the list of sellers to the AdminView
                return View(sellers);
            }

            // Handle cases where the user is not an Admin
            return RedirectToAction("AccessDenied", "Account");
        }


        public IActionResult AddCar()
        {
            ViewBag.MakeTypes = new SelectList(_AppDbContext.MakeTypes, "Id", "Name");
            ViewBag.ModelTypes = new SelectList(_AppDbContext.ModelTypes, "Id", "Name");
            ViewBag.ConditionTypes = new SelectList(_AppDbContext.ConditionTypes, "Id", "Name");
            ViewBag.BodyTypes = new SelectList(_AppDbContext.BodyTypes, "Id", "Name");
            ViewBag.EngineTypes = new SelectList(_AppDbContext.EngineTypes, "Id", "Name");
            ViewBag.FuelTypes = new SelectList(_AppDbContext.FuelTypes, "Id", "Name");
            ViewBag.TransmissionTypes = new SelectList(_AppDbContext.TransmissionTypes, "Id", "Name");


            return View();  
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public async Task<IActionResult> AddCar(CarDetail crd, int BodyTypeId, int EngineTypeId, int FuelTypeId, int TransmissionTypeId, int MakeTypeId, int ModelTypeId, int ConditionTypeId)
        {
            if (crd.CarImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filename = Guid.NewGuid().ToString() + "_" + crd.CarImage.FileName;
                string filepath = Path.Combine(uploadFolder, filename);

                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await crd.CarImage.CopyToAsync(fileStream);
                }

                if (crd.CarImage.Length <= 1048576000)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var makeType = await _AppDbContext.MakeTypes.FindAsync(MakeTypeId);
                    var modelType = await _AppDbContext.ModelTypes.FindAsync(ModelTypeId);
                    var conditionType = await _AppDbContext.ConditionTypes.FindAsync(ConditionTypeId);
                    var bodyType = await _AppDbContext.BodyTypes.FindAsync(BodyTypeId);
                    var engineType = await _AppDbContext.EngineTypes.FindAsync(EngineTypeId);
                    var fuelType = await _AppDbContext.FuelTypes.FindAsync(FuelTypeId);
                    var transmissionType = await _AppDbContext.TransmissionTypes.FindAsync(TransmissionTypeId);


                    CarDetail car = new CarDetail()
                    {
                        MakeType = makeType,
                        ModelType = modelType,
                        Year = crd.Year,
                        BodyType = bodyType,
                        EngineType = engineType,
                        FuelType = fuelType,
                        TransmissionType = transmissionType,
                        Color = crd.Color,
                        Mileage = crd.Mileage,
                        SeatingCapacity = crd.SeatingCapacity,
                        Price = crd.Price,
                        ConditionType = conditionType,
                        Seller_Id = user.Id,
                        Image = filename,
                        BlindspotMonitor = crd.BlindspotMonitor,
                        Adaptivecruisecontrol = crd.Adaptivecruisecontrol,
                        BackupCamera = crd.BackupCamera,
                        ForwardCollisionwarning = crd.ForwardCollisionwarning,
                        Heatedseats = crd.Heatedseats,
                        Hillassist = crd.Hillassist,
                        Sunroof = crd.Sunroof,
                        AutoPark = crd.AutoPark,
                        Automaticemergencybraking = crd.Automaticemergencybraking,
                        EvasiveSteering = crd.EvasiveSteering,
                        Leatherseats = crd.Leatherseats,
                        Remotestart = crd.Remotestart,
                        USBoutlets = crd.USBoutlets,
                        Drivercommunicationassistance = crd.Drivercommunicationassistance,
                        AirConditioning = crd.AirConditioning,
                        Battery = crd.Battery,
                        Bluetooth = crd.Bluetooth,

                        // Setting feature-related fields
                        Status = "Pending",  // For admin approval
                        IsFeatured = false,  // Initially false
                    
                    
                        BiddingStatus = "Pending",
                        IsBiddingEnabled = false,
                    
                    
                    };

                    car.CreatedDate = DateTime.Now; // Set the current date when adding a car

                    _AppDbContext.CarDetails.Add(car);
                    await _AppDbContext.SaveChangesAsync();
                    return RedirectToAction("SellerView", "Seller");
                }
            }
            return View();
        }



        [HttpPost]
        public IActionResult RequestFeatureCar(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null && car.Seller_Id == _userManager.GetUserId(User))
            {
                if (!car.IsFeatureRequested && !car.FeatureRequestPending)
                {
                    // Request feature
                    car.IsFeatureRequested = true;  // Mark as requested
                    car.FeatureRequestPending = true;  // Mark as pending

                    // Optional: Update status for admin visibility
                    car.Status = "Pending";

                    _AppDbContext.SaveChanges();
                }
            }

            return RedirectToAction("SellerView", "Seller");
        }


        [HttpPost]
        public IActionResult RemoveFeatureCar(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null && car.Seller_Id == _userManager.GetUserId(User))
            {
                // Directly remove from featured list
                car.Status = "Rejected";  // For admin approval
                car.IsFeatured = false;  // Remove car from featured
                car.FeatureRequestPending = false;  // Reset pending status
                car.IsFeatureRequested = false;  // Reset request status

                _AppDbContext.SaveChanges();
            }

            return RedirectToAction("SellerView", "Seller");
        }























        [HttpPost]
        public IActionResult RequestBidding(int carId, decimal minimumBidAmount)
        {
            var car = _AppDbContext.CarDetails.Find(carId);

            if (car != null && car.Seller_Id == _userManager.GetUserId(User))
            {
                if (!car.IsBiddingRequested && !car.BiddingRequestPending)
                {
                    // Calculate 50% of the car's price
                    decimal maxAllowedBid = car.Price / 2;

                    // Check if the submitted minimum bid amount is greater than the allowed 50%
                    if (minimumBidAmount > maxAllowedBid)
                    {
                        // If greater, set the minimum bid to 50% of the car's price
                        car.MinimumBidAmount = maxAllowedBid;
                    }
                    else
                    {
                        // Otherwise, use the amount provided by the seller
                        car.MinimumBidAmount = minimumBidAmount;
                    }

                    // Request feature
                    car.IsBiddingRequested = true;  // Mark as requested
                    car.BiddingRequestPending = true;  // Mark as pending
                    car.BiddingStatus = "Pending";

                    _AppDbContext.SaveChanges();
                }
            }

            return RedirectToAction("SellerView", "Seller");
        }



        [HttpPost]
        public IActionResult RemoveBiddingCar(int carId)
        {
            var car = _AppDbContext.CarDetails.Find(carId);
            if (car != null && car.Seller_Id == _userManager.GetUserId(User))
            {
                // Directly remove from featured list
                car.BiddingStatus = "Rejected";  // For admin approval
                car.IsBiddingEnabled = false;  // Remove car from featured
                car.BiddingRequestPending = false;  // Reset pending status
                car.IsBiddingRequested = false;  // Reset request status

                _AppDbContext.SaveChanges();
            }

            return RedirectToAction("SellerView", "Seller");
        }










        [HttpGet]
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _AppDbContext.CarDetails
                .Include(c => c.MakeType)
                .Include(c => c.ModelType)
                .Include(c => c.ConditionType)
                .Include(c => c.BodyType)
                .Include(c => c.EngineType)
                .Include(c => c.FuelType)
                .Include(c => c.TransmissionType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            ViewBag.MakeTypes = new SelectList(_AppDbContext.MakeTypes, "Id", "Name", car.MakeType.Id);
            ViewBag.ModelTypes = new SelectList(_AppDbContext.ModelTypes, "Id", "Name", car.ModelType.Id);
            ViewBag.ConditionTypes = new SelectList(_AppDbContext.ConditionTypes, "Id", "Name", car.ConditionType.Id);
            ViewBag.BodyTypes = new SelectList(_AppDbContext.BodyTypes, "Id", "Name", car.BodyType.Id);
            ViewBag.EngineTypes = new SelectList(_AppDbContext.EngineTypes, "Id", "Name", car.EngineType.Id);
            ViewBag.FuelTypes = new SelectList(_AppDbContext.FuelTypes, "Id", "Name", car.FuelType.Id);
            ViewBag.TransmissionTypes = new SelectList(_AppDbContext.TransmissionTypes, "Id", "Name", car.TransmissionType.Id);

            return View(car);
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public async Task<IActionResult> EditCar(int id, CarDetail carDetail)
        {
            if (id != carDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Retrieve the existing car detail from the database
                var existingCar = await _AppDbContext.CarDetails.FindAsync(id);

                if (existingCar == null)
                {
                    return NotFound();
                }

                // Update the car details
                existingCar.MakeTypeId = carDetail.MakeTypeId;
                existingCar.ModelTypeId = carDetail.ModelTypeId;
                existingCar.Year = carDetail.Year;
                existingCar.BodyTypeId = carDetail.BodyTypeId;
                existingCar.EngineTypeId = carDetail.EngineTypeId;
                existingCar.FuelTypeId = carDetail.FuelTypeId;
                existingCar.TransmissionTypeId = carDetail.TransmissionTypeId;
                existingCar.Color = carDetail.Color;
                existingCar.Mileage = carDetail.Mileage;
                existingCar.SeatingCapacity = carDetail.SeatingCapacity;
                existingCar.Price = carDetail.Price;
                existingCar.ConditionTypeId = carDetail.ConditionTypeId;

                // More features
                existingCar.BlindspotMonitor = carDetail.BlindspotMonitor;
                existingCar.Adaptivecruisecontrol = carDetail.Adaptivecruisecontrol;
                existingCar.BackupCamera = carDetail.BackupCamera;
                // Add other features similarly

                // Handle image upload if a new image is provided
                if (carDetail.CarImage != null)
                {
                    // Save the new image file
                    var fileName = Path.GetFileName(carDetail.CarImage.FileName);
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await carDetail.CarImage.CopyToAsync(stream);
                    }

                    // Delete the old image if necessary (optional)
                    if (!string.IsNullOrEmpty(existingCar.Image))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", existingCar.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Update the image file path in the database
                    existingCar.Image = fileName;
                }

                try
                {
                    // Save changes to the database
                    _AppDbContext.Update(existingCar);
                    await _AppDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_AppDbContext.CarDetails.Any(e => e.Id == carDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index)); // Redirect to the car listing page or another appropriate page
            }

            // If model state is not valid, return the same view with errors
            return View(carDetail);
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
                                         .Include(c => c.Seller)
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
                return RedirectToAction("SellerView", "Seller");
            }

            return NotFound();  // Return a 404 if trying to delete a non-existent car
        }















        // GET: Seller/Create
        //public IActionResult AddCar()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddCar(CarDetail model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Save the car image
        //        if (model.CarImage != null)
        //        {
        //            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "car_images");
        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }

        //            var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CarImage.FileName);
        //            var filePath = Path.Combine(uploadsFolder, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await model.CarImage.CopyToAsync(stream);
        //                model.Image = $"/images/car_images/{fileName}";
        //            }
        //        }

        //        var user = await _userManager.GetUserAsync(User);
        //        model.Seller_Id = user.Id;

        //        _AppDbContext.Add(model);
        //        await _AppDbContext.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }

        //    return View(model);
        //}



        //// GET: Seller/Index
        //public async Task<IActionResult> Index()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var cars = await _AppDbContext.CarDetails
        //        .Where(c => c.Seller_Id == user.Id)
        //        .Include(c => c.Seller) // Include seller details
        //        .ToListAsync();

        //    return View(cars);
        //}




        //// GET: Seller/Edit/5
        //public async Task<IActionResult> EditCar(int id)
        //{
        //    var carDetail = await _AppDbContext.CarDetails.FindAsync(id);

        //    if (carDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carDetail);
        //}

        //// POST: Seller/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditCar(int id, CarDetail model)
        //{
        //    if (id != model.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Update the car image if provided
        //            if (model.CarImage != null)
        //            {
        //                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "car_images");
        //                if (!Directory.Exists(uploadsFolder))
        //                {
        //                    Directory.CreateDirectory(uploadsFolder);
        //                }

        //                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CarImage.FileName);
        //                var filePath = Path.Combine(uploadsFolder, fileName);

        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await model.CarImage.CopyToAsync(stream);
        //                    model.Image = $"/images/car_images/{fileName}";
        //                }
        //            }

        //            _AppDbContext.Update(model);
        //            await _AppDbContext.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CarDetailExists(model.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return RedirectToAction("Index" , "Seller");
        //    }

        //    return View(model);
        //}


        ////public IActionResult detailsdoctor(int id)
        ////{
        ////    var docdetails = _AppDbContext.DoctorsTable.Include(x => x.Speciality).FirstOrDefault(x => x.doctorId == id);
        ////    return View(docdetails);
        ////}

        //public async Task<IActionResult> DeleteCar(int id)
        //{
        //    var carDetail = await _AppDbContext.CarDetails.FindAsync(id);

        //    if (carDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carDetail);
        //}

        //// POST: Seller/Delete/5
        //[HttpPost, ActionName("DeleteCar")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var carDetail = await _AppDbContext.CarDetails.FindAsync(id);

        //    if (carDetail != null)
        //    {
        //        _AppDbContext.CarDetails.Remove(carDetail);
        //        await _AppDbContext.SaveChangesAsync();
        //    }

        //    return RedirectToAction("Index" , "Seller");
        //}


        //private bool CarDetailExists(int id)
        //{
        //    return _AppDbContext.CarDetails.Any(e => e.Id == id);
        //}









    }
}

