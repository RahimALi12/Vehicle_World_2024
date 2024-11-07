using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehicle_World.Models;
using System;
using System.Drawing;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicle_World.Controllers
{
    public class WebsiteController : Controller
    {

        private readonly ApplicationDbContext _AppDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public WebsiteController(ApplicationDbContext AppDb, UserManager<AppUser> userManager, IWebHostEnvironment whe, IEmailService emailService)
        {
          

       
            _emailService = emailService;
       
        _AppDbContext = AppDb;
            _userManager = userManager;
            _webHostEnvironment = whe;
        }

        public async Task<IActionResult> Index()
        {

            var featuredCars = _AppDbContext.CarDetails
              .Where(c => c.IsFeatured == true)
              .ToList();
            //var user = await _userManager.GetUserAsync(User);



            var myCars = await _AppDbContext.CarDetails
                .OrderByDescending(c => c.CreatedDate) // Sort by newest first
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
            ViewBag.FeaturedCars = featuredCars;



            return View();

        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string message)
        {
            var senderId = _userManager.GetUserId(User);
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.UtcNow
            };



            _AppDbContext.ChatMessages.Add(chatMessage);
            await _AppDbContext.SaveChangesAsync();

            return Ok();  // You can return the message ID or success status
        }

        public IActionResult Chat(string receiverId)
        {
            // Pass the receiverId to the view using ViewBag or as part of the model
            ViewBag.ReceiverId = receiverId;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var senderId = _userManager.GetUserId(User);

            var messages = await _AppDbContext.ChatMessages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Json(messages);  // Return the list of messages as JSON
        }

        public IActionResult About()
        {
            return View();
        }



        public IActionResult Contactus()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactus(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _AppDbContext.Add(contact);
                await _AppDbContext.SaveChangesAsync();
                return RedirectToAction("Index" , "Website");
            }
            return View(contact);
        }




        public IActionResult FilterCars(int? makeTypeId, int? modelTypeId, int? bodyTypeId, int? engineTypeId, int? fuelTypeId, int? transmissionTypeId, decimal? minPrice, decimal? maxPrice,
        bool? BlindspotMonitor, bool? BackupCamera, bool? Sunroof, bool? Heatedseats)
        {
            // Get the list of cars
            var cars = _AppDbContext.CarDetails.Include(c => c.MakeType)
                                          .Include(c => c.ModelType)
                                          .Include(c => c.BodyType)
                                          .Include(c => c.EngineType)
                                          .Include(c => c.FuelType)
                                          .Include(c => c.TransmissionType)
                                          .Where(c => (bodyTypeId == null || c.MakeTypeId == makeTypeId) &&
                                                      (fuelTypeId == null || c.ModelTypeId == modelTypeId) &&
                                                      (fuelTypeId == null || c.BodyTypeId == bodyTypeId) &&
                                                      (fuelTypeId == null || c.EngineTypeId == engineTypeId) &&
                                                      (fuelTypeId == null || c.FuelTypeId == fuelTypeId) &&
                                                      (transmissionTypeId == null || c.TransmissionTypeId == transmissionTypeId) &&
                                                      (minPrice == null || c.Price >= minPrice) &&
                                                      (maxPrice == null || c.Price <= maxPrice)).ToList();

            // Handle boolean feature filters
            if (BlindspotMonitor.HasValue && BlindspotMonitor.Value)
                cars = cars.Where(c => c.BlindspotMonitor).ToList();
            if (BackupCamera.HasValue && BackupCamera.Value)
                cars = cars.Where(c => c.BackupCamera).ToList();
            if (Sunroof.HasValue && Sunroof.Value)
                cars = cars.Where(c => c.Sunroof).ToList();
            if (Heatedseats.HasValue && Heatedseats.Value)
                cars = cars.Where(c => c.Heatedseats).ToList();

            // Pass the filtered data to the view
            ViewBag.MakeTypes = _AppDbContext.MakeTypes.ToList();
            ViewBag.ModelTypes = _AppDbContext.ModelTypes.ToList();
            ViewBag.BodyTypes = _AppDbContext.BodyTypes.ToList();
            ViewBag.EngineTypes = _AppDbContext.EngineTypes.ToList();
            ViewBag.FuelTypes = _AppDbContext.FuelTypes.ToList();
            ViewBag.TransmissionTypes = _AppDbContext.TransmissionTypes.ToList();

            return View("AllCars", cars);
        }





        public IActionResult AllCars()
        {
            var cars = _AppDbContext.CarDetails.Include(c => c.MakeType)
                                               .Include(c => c.ModelType)
                                               .Include(c => c.BodyType)
                                               .Include(c => c.EngineType)
                                               .Include(c => c.FuelType)
                                               .Include(c => c.TransmissionType).ToList();

            // Populate filter options
            ViewBag.MakeTypes = _AppDbContext.MakeTypes.ToList();
            ViewBag.ModelTypes = _AppDbContext.ModelTypes.ToList();
            ViewBag.BodyTypes = _AppDbContext.BodyTypes.ToList();
            ViewBag.EngineTypes = _AppDbContext.EngineTypes.ToList();
            ViewBag.FuelTypes = _AppDbContext.FuelTypes.ToList();
            ViewBag.TransmissionTypes = _AppDbContext.TransmissionTypes.ToList();

            return View("AllCars", cars);
        }



        public IActionResult Details(int id)
        {
            // Fetch the car details including feedbacks and related buyer details
            var car = _AppDbContext.CarDetails
                .Include(c => c.BodyType)
                .Include(c => c.ModelType)
                .Include(c => c.FuelType)
                .Include(c => c.TransmissionType)
                .Include(c => c.EngineType)
                .Include(c => c.Seller)
                .Include(c => c.Feedbacks) // Include feedbacks for the car
                .ThenInclude(f => f.Buyer) // Include Buyer details for feedbacks
                .FirstOrDefault(c => c.Id == id);

            if (car == null)
            {
                return NotFound(); // Return 404 if the car is not found
            }

            // Optionally, you can fetch similar cars based on criteria like model, brand, etc.
            var similarCars = _AppDbContext.CarDetails
                .Where(c => c.ModelTypeId == car.ModelTypeId && c.Id != car.Id)
                .Take(4) // Fetch the top 4 similar cars
                .ToList();

            // Pass the similar cars to the ViewBag for display in the view
            ViewBag.SimilarCars = similarCars;

            return View(car); // Pass the car and its feedbacks to the view
        }




        [HttpPost]
        public IActionResult SubmitFeedback(int carId, int rating, string comment)
        {
            // Get the logged-in user's ID
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a new Feedback object
            var feedback = new Feedback
            {
                //CarId = carId,
                Rating = rating,
                Comment = comment,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Buyer_Id = buyerId // Set the buyer ID
            };

            // Save the feedback to the database
            _AppDbContext.Feedbacks.Add(feedback);
            _AppDbContext.SaveChanges();

            // Redirect back to the Details page to display the feedback
            return RedirectToAction("Details", new { id = carId });
        }






        //[HttpPost]
        //public async Task<IActionResult> SendMessage(string sellerEmail, string message)
        //{
        //    await _emailService.SendEmailAsync(sellerEmail, "Buyer Inquiry", message);

        //    TempData["MessageSent"] = "Your message has been sent to the seller!";
        //    return RedirectToAction("Index" , "Website");
        //}




    }
}
