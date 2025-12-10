using Microsoft.AspNetCore.Mvc;
using Lucidez.Data;
using Lucidez.Models;
using Microsoft.EntityFrameworkCore;

namespace Lucidez.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ---------------------------
        // Main Website Pages
        // ---------------------------

        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Services() => View();

        public IActionResult Products() => View();

        public IActionResult AccountingIndia() => View();

        public IActionResult AccountingUS() => View();

        public IActionResult Wannabe() => View();

        public IActionResult LuPoll() => View();
        public IActionResult Careers() => View();
        public IActionResult Contact() => View(); 
        public IActionResult Privacy() => View();
        public IActionResult Terms() => View();
        // ---------------------------
        // CONTACT FORM SUBMISSION
        // ---------------------------

        [HttpPost]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the required fields.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            // Save Inquiry
            _db.ContactMessages.Add(model);

            // Save to Leads Table
            _db.Leads.Add(new Lead
            {
                Name = model.Name,
                Email = model.Email,
                Company = model.Company,
                Source = "Contact Form"
            });

            await _db.SaveChangesAsync();

            TempData["Success"] = "Thank you! We have received your message.";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        // ---------------------------
        // NEWSLETTER / SUBSCRIBE
        // ---------------------------

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Enter a valid email.";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            // Save lead
            _db.Leads.Add(new Lead
            {
                Name = "Subscriber",
                Email = email,
                Source = "Newsletter"
            });

            await _db.SaveChangesAsync();

            TempData["Success"] = "Subscribed successfully!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // Error Page
        public IActionResult Error() => View();
    }
}
